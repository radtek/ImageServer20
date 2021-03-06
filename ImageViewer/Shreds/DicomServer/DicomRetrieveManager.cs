﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.ImageViewer.Services;
using ClearCanvas.ImageViewer.Services.Auditing;
using ClearCanvas.ImageViewer.Services.DicomServer;
using ClearCanvas.ImageViewer.Services.LocalDataStore;

namespace ClearCanvas.ImageViewer.Shreds.DicomServer
{
	#region Retrieve Service Definition

	internal class RetrieveStudiesRequest
	{
		public RetrieveStudiesRequest(AEInformation remoteAEInfo, IEnumerable<StudyInformation> studiesToRetrieve)
		{
			this.RemoteAEInfo = remoteAEInfo;
			this.StudiesToRetrieve = studiesToRetrieve;
		}

		public readonly AEInformation RemoteAEInfo;
		public readonly IEnumerable<StudyInformation> StudiesToRetrieve;
	}

	internal class RetrieveSeriesRequest
	{
		public RetrieveSeriesRequest(AEInformation remoteAEInfo, StudyInformation studyInformation, IEnumerable<string> seriesInstanceUids)
		{
			this.RemoteAEInfo = remoteAEInfo;
			StudyInformation = studyInformation;
			this.SeriesInstanceUids = seriesInstanceUids;
		}

		public readonly AEInformation RemoteAEInfo;
		public readonly StudyInformation StudyInformation;
		public readonly IEnumerable<string> SeriesInstanceUids;
	}

	//TODO: later, remove IDicomServerService, extend this to support series and image retrievals, and add a callback interface.
	internal interface IRetrieveService
	{
		void RetrieveStudies(RetrieveStudiesRequest request);

		void RetrieveSeries(RetrieveSeriesRequest request);
	}

	#endregion

	internal class DicomRetrieveManager : IRetrieveService
	{
		public static readonly DicomRetrieveManager Instance = new DicomRetrieveManager();

		#region Private Fields

		private readonly object _syncLock = new object();
		private readonly List<RetrieveScu> _scus = new List<RetrieveScu>();
		private bool _active;

		#endregion

		private DicomRetrieveManager()
		{
			_active = false;
		}

		#region RetrieveScu class

		private class RetrieveScu : StudyRootMoveScu
		{
			#region Private Fields

			private readonly Thread _thread;
			private readonly IEnumerable<StudyInformation> _studiesToRetrieve;
			private readonly IEnumerable<string> _seriesInstanceUids;

			#endregion

			public RetrieveScu(string localAETitle, AEInformation remoteAEInfo, IEnumerable<StudyInformation> studiesToRetrieve)
				:base(localAETitle, remoteAEInfo.AETitle, remoteAEInfo.HostName, remoteAEInfo.Port, localAETitle)
			{
				Platform.CheckForEmptyString(localAETitle, "localAETitle");
				Platform.CheckForEmptyString(remoteAEInfo.AETitle, "remoteAEInfo.AETitle");
				Platform.CheckForEmptyString(remoteAEInfo.HostName, "remoteAEInfo.HostName");
				Platform.CheckForNullReference(studiesToRetrieve, "studiesToRetrieve");

				_studiesToRetrieve = studiesToRetrieve;

				_thread = new Thread(RetrieveInternal);
				_thread.Name = String.Format("Retrieve from {0}/{1}:{2}", 
					remoteAEInfo.HostName, remoteAEInfo.AETitle, remoteAEInfo.Port);
			}

			public RetrieveScu(string localAETitle, AEInformation remoteAEInfo, StudyInformation studyInformation, IEnumerable<string> seriesInstanceUids)
				: this(localAETitle, remoteAEInfo, Enumerate(studyInformation))
			{
				_seriesInstanceUids = seriesInstanceUids;
				_thread = new Thread(RetrieveInternal);
				_thread.Name = String.Format("Retrieve from {0}/{1}:{2}",
					remoteAEInfo.HostName, remoteAEInfo.AETitle, remoteAEInfo.Port);
			}

			private static IEnumerable<T> Enumerate<T>(T item)
			{
				yield return item;
			}

			public override void OnReceiveResponseMessage(ClearCanvas.Dicom.Network.DicomClient client, ClearCanvas.Dicom.Network.ClientAssociationParameters association, byte presentationID, ClearCanvas.Dicom.DicomMessage message)
			{
				base.OnReceiveResponseMessage(client, association, presentationID, message);

				if (message.Status.Status == DicomState.Warning)
				{
					string msg = String.Format("Remote server returned a warning status ({0}: {1}).",
						RemoteAE, message.Status.Description);
					OnRetrieveError(msg);
				}
			}

			#region Private Methods

			private void RetrieveInternal()
			{
				try
				{
					OnBeginRetrieve();

					Move();

					Join(new TimeSpan(0, 0, 0, 0, 1000));

					if (base.Status == ScuOperationStatus.Canceled)
					{
						OnRetrieveError(String.Format("The Move operation was cancelled ({0}).", RemoteAE));
					}
					else if (base.Status == ScuOperationStatus.ConnectFailed)
					{
						OnRetrieveError(String.Format("Unable to connect to remote server ({0}: {1}).",
							RemoteAE, base.FailureDescription ?? "no failure description provided"));
					}
					else if (base.Status == ScuOperationStatus.AssociationRejected)
					{
						OnRetrieveError(String.Format("Association rejected ({0}: {1}).",
							RemoteAE, base.FailureDescription ?? "no failure description provided"));
					}
					else if (base.Status == ScuOperationStatus.Failed)
					{
						OnRetrieveError(String.Format("The Move operation failed ({0}: {1}).",
							RemoteAE, base.FailureDescription ?? "no failure description provided"));
					}
					else if (base.Status == ScuOperationStatus.TimeoutExpired)
					{
						//ignore, because this is the scu, we don't want users to think an error has occurred
						//in retrieving.
					}
					else if (base.Status == ScuOperationStatus.UnexpectedMessage)
					{
						//ignore, because this is the scu, we don't want users to think an error has occurred
						//in retrieving.
					}
					else if (base.Status == ScuOperationStatus.NetworkError)
					{
						//ignore, because this is the scu, we don't want users to think an error has occurred
						//in retrieving.
					}

					AuditRetrieveOperation(true);
				}
				catch (Exception e)
				{
					if (base.Status == ScuOperationStatus.ConnectFailed)
					{
						OnRetrieveError(String.Format("Unable to connect to remote server ({0}: {1}).",
							RemoteAE, base.FailureDescription ?? "no failure description provided"));
					}
					else
					{
						OnRetrieveError(String.Format("An unexpected error has occurred in the Move Scu: {0}:{1}:{2} -> {3}; {4}",
						                              base.RemoteAE, base.RemoteHost, base.RemotePort, base.ClientAETitle, e.Message));
					}

					AuditRetrieveOperation(false);
				}
				finally
				{
					Instance.OnRetrieveComplete(this);
				}
			}

			private void AuditRetrieveOperation(bool noExceptions)
			{
				AuditedInstances receivedInstances = new AuditedInstances();
				foreach (StudyInformation instance in this._studiesToRetrieve)
					receivedInstances.AddInstance(instance.PatientId, instance.PatientsName, instance.StudyInstanceUid);
				if (noExceptions)
					AuditHelper.LogReceivedInstances(this.RemoteAE, this.RemoteHost, receivedInstances, EventSource.CurrentProcess, EventResult.Success, EventReceiptAction.ActionUnknown);
				else
					AuditHelper.LogReceivedInstances(this.RemoteAE, this.RemoteHost, receivedInstances, EventSource.CurrentProcess, EventResult.MajorFailure, EventReceiptAction.ActionUnknown);
			}

			private void OnBeginRetrieve()
			{
					foreach (StudyInformation info in _studiesToRetrieve)
					{
						RetrieveStudyInformation retrieveInfo = new RetrieveStudyInformation();
						retrieveInfo.FromAETitle = base.RemoteAE;
						retrieveInfo.StudyInformation = info;
						LocalDataStoreEventPublisher.Instance.RetrieveStarted(retrieveInfo);
					}
				}

			private void OnRetrieveError(string message)
			{
					foreach (StudyInformation info in _studiesToRetrieve)
					{
						ReceiveErrorInformation receiveError = new ReceiveErrorInformation();
						receiveError.FromAETitle = base.RemoteAE;
						receiveError.StudyInformation = info;
						receiveError.ErrorMessage = message;
						LocalDataStoreEventPublisher.Instance.ReceiveError(receiveError);
					}
				}

			#endregion

			#region Public Methods

			public new void Cancel()
			{
				if (_thread.IsAlive)
				{
					base.Cancel();
					_thread.Join();
				}
			}

			public void Retrieve()
			{
				foreach (StudyInformation info in _studiesToRetrieve)
					AddStudyInstanceUid(info.StudyInstanceUid);

				if (_seriesInstanceUids != null)
				{
					foreach (string seriesInstanceUid in _seriesInstanceUids)
						AddSeriesInstanceUid(seriesInstanceUid);
				}

				//do this rather than use BeginSend b/c it uses thread pool threads which can be exhausted.
				_thread.Start();
			}

			#endregion
		}

		#endregion

		#region Private Methods

		private void OnRetrieveComplete(RetrieveScu scu)
		{
			lock (_syncLock)
			{
				_scus.Remove(scu);
				scu.Dispose();
			}
		}

		#endregion

		#region Public Methods

		public void Start()
		{
			lock (_syncLock)
			{
				_active = true;
			}
		}

		public void Stop()
		{
			List<RetrieveScu> scus;

			lock (_syncLock)
			{
				_active = false;
				scus = new List<RetrieveScu>(_scus);
			}

			scus.ForEach(delegate(RetrieveScu scu) { scu.Cancel(); });
		}
		
		#endregion

		#region IRetrieveService Members

		public void RetrieveStudies(RetrieveStudiesRequest request)
		{
			lock (_syncLock)
			{
				if (!_active)
					throw new InvalidOperationException("The Retrieve service is not running.");

				DicomServerConfiguration configuration = DicomServerManager.Instance.GetServerConfiguration();
				foreach (StudyInformation study in request.StudiesToRetrieve)
				{
					//Some servers seems to have a problem with C-MOVE-RQs that have more than one study uid,
					//so we'll just do them one at a time.
					List<StudyInformation> retrieveStudy = new List<StudyInformation>();
					retrieveStudy.Add(study);
					RetrieveScu scu = new RetrieveScu(configuration.AETitle, request.RemoteAEInfo, retrieveStudy);

					_scus.Add(scu);
					//don't block the calling thread to do this.
					ThreadPool.QueueUserWorkItem(delegate(object theScu) { ((RetrieveScu)theScu).Retrieve(); }, scu);
				}
			}
		}

		public void RetrieveSeries(RetrieveSeriesRequest request)
		{
			lock (_syncLock)
			{
				if (!_active)
					throw new InvalidOperationException("The Retrieve service is not running.");

				DicomServerConfiguration configuration = DicomServerManager.Instance.GetServerConfiguration();
				RetrieveScu scu = new RetrieveScu(configuration.AETitle, request.RemoteAEInfo, request.StudyInformation, request.SeriesInstanceUids);

				_scus.Add(scu);
				//don't block the calling thread to do this.
				ThreadPool.QueueUserWorkItem(delegate(object theScu) { ((RetrieveScu)theScu).Retrieve(); }, scu);
			}
		}

		#endregion
	}
}
