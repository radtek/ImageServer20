#region License

// Copyright (c) 2006-2009, ClearCanvas Inc.
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
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.ServiceLock.FilesystemRebuildXml
{
	/// <summary>
	/// Class for processing 'FilesystemRebuildXml' <see cref="Model.ServiceLock"/> rows.
	/// </summary>
	public class FilesystemRebuildXmlItemProcessor : BaseServiceLockItemProcessor, IServiceLockItemProcessor
	{
		#region Private Members
		private IPersistentStore _store;
		private IList<ServerPartition> _partitions;
		#endregion

		#region Private Methods
		private bool GetStudyStorageLocation(ServerEntityKey partitionKey, string studyInstanceUid, out StudyStorageLocation location)
		{
			using (IReadContext context = _store.OpenReadContext())
			{
				IQueryStudyStorageLocation procedure = context.GetBroker<IQueryStudyStorageLocation>();
				StudyStorageLocationQueryParameters parms = new StudyStorageLocationQueryParameters();
				parms.ServerPartitionKey = partitionKey;
				parms.StudyInstanceUid = studyInstanceUid;
				location = procedure.FindOne(parms);

				return location != null;
			}
		}

		/// <summary>
		/// Traverse the filesystem directories for studies to rebuild the XML for.
		/// </summary>
		/// <param name="filesystem"></param>
		private void TraverseFilesystemStudies(Filesystem filesystem)
		{
			List<StudyStorageLocation> lockFailures = new List<StudyStorageLocation>();
			ServerPartition partition;

			DirectoryInfo filesystemDir = new DirectoryInfo(filesystem.FilesystemPath);

			foreach (DirectoryInfo partitionDir in filesystemDir.GetDirectories())
			{
				if (GetServerPartition(partitionDir.Name, out partition) == false)
					continue;

				foreach (DirectoryInfo dateDir in partitionDir.GetDirectories())
				{
					foreach (DirectoryInfo studyDir in dateDir.GetDirectories())
					{
						String studyInstanceUid = studyDir.Name;

						StudyStorageLocation location;
						if (false == GetStudyStorageLocation(partition.Key, studyInstanceUid, out location))
						{
							StudyStorage storage;
							if (GetStudyStorage(partition, studyInstanceUid, out storage))
							{
								Platform.Log(LogLevel.Warn, "Study {0} on filesystem partition {1} is offline {2}", studyInstanceUid,
								             partition.Description, studyDir.ToString());
							}
							else
								Platform.Log(LogLevel.Info, "Found study directory not in the database, ignoring: {0}", studyDir.ToString());
						}
						else
						{
							try
							{
								if (!location.AcquireLock())
								{
									Platform.Log(LogLevel.Warn, "Unable to lock study: {0}, delaying rebuild", location.StudyInstanceUid);
									lockFailures.Add(location);
									continue;
								}

								RebuildStudyXml(location);

								location.ReleaseLock();
							}
							catch (Exception e)
							{
								Platform.Log(LogLevel.Error, e, "Unexpected exception when rebuilding study xml for study: {0}",
								             location.StudyInstanceUid);
								lockFailures.Add(location);
							}
						}
					}
				}
			}

			// Re-do all studies that failed locks one time
			foreach (StudyStorageLocation location in lockFailures)
			{
				try
				{
					if (!location.AcquireLock())
					{
						Platform.Log(LogLevel.Warn, "Unable to lock study: {0}, skipping rebuild", location.StudyInstanceUid);
						continue;
					}

					RebuildStudyXml(location);

					location.ReleaseLock();
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e, "Unexpected exception on retry when rebuilding study xml for study: {0}",
										 location.StudyInstanceUid);
				}
			}
		}

		/// <summary>
		/// Rebuild a specific study's XML file.
		/// </summary>
		/// <param name="location"></param>
		private void RebuildStudyXml(StudyStorageLocation location)
		{
			string rootStudyPath = location.GetStudyPath();

			try
			{
				using (ServerCommandProcessor processor = new ServerCommandProcessor("Rebuild XML"))
				{
					StudyXml currentXml = LoadStudyXml(location);

					StudyXml newXml = new StudyXml(location.StudyInstanceUid);
					foreach (SeriesXml series in currentXml)
					{
						string seriesPath = Path.Combine(rootStudyPath, series.SeriesInstanceUid);
						foreach (InstanceXml instance in series)
						{
							string instancePath = Path.Combine(seriesPath, instance.SopInstanceUid + ".dcm");

							processor.AddCommand(new InsertInstanceXmlCommand(newXml, instancePath));
						}
					}

					processor.AddCommand(new SaveXmlCommand(newXml, location));

					if (!processor.Execute())
					{
						throw new ApplicationException(processor.FailureReason);
					}

					Platform.Log(LogLevel.Info, "Completed reprocessing Study XML file for study {0}", location.StudyInstanceUid);
				}
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected error when rebuilding study XML for directory: {0}", location.FilesystemPath);
				if (!ReprocessStudy(location))
				{
					Platform.Log(LogLevel.Error, "Failure attempting to reprocess study: {0}", location.StudyInstanceUid);
				}
				else
					Platform.Log(LogLevel.Error, "Inserted reprocess request for study: {0}", location.StudyInstanceUid);
			}
		}

		/// <summary>
		/// Create a Study Reprocess entry.
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		private bool ReprocessStudy(StudyStorageLocation location)
		{
			IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();
			using (IUpdateContext ctx = store.OpenUpdateContext(UpdateContextSyncMode.Flush))
			{
				// Unlock first
				ILockStudy lockStudy = ctx.GetBroker<ILockStudy>();
				LockStudyParameters lockParms = new LockStudyParameters();
				lockParms.StudyStorageKey = location.Key;
				lockParms.QueueStudyStateEnum = QueueStudyStateEnum.Idle;
				if (!lockStudy.Execute(lockParms) || !lockParms.Successful)
					return false;

				// Now relock
				lockParms.QueueStudyStateEnum = QueueStudyStateEnum.ReprocessScheduled;
				if (!lockStudy.Execute(lockParms) || !lockParms.Successful)
					return false;

				InsertWorkQueueParameters columns = new InsertWorkQueueParameters();
				columns.ScheduledTime = Platform.Time;
				columns.ServerPartitionKey = location.ServerPartitionKey;
				columns.StudyStorageKey = location.Key;
				columns.WorkQueuePriorityEnum = WorkQueuePriorityEnum.Low;
				columns.WorkQueueTypeEnum = WorkQueueTypeEnum.ReprocessStudy;
				columns.ExpirationTime = Platform.Time.Add(TimeSpan.FromMinutes(5));
				IInsertWorkQueue insertBroker = ctx.GetBroker<IInsertWorkQueue>();
				if (insertBroker.FindOne(columns) != null)
				{
					ctx.Commit();
					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Get the server partition
		/// </summary>
		/// <param name="partitionFolderName"></param>
		/// <param name="partition"></param>
		/// <returns></returns>
		private bool GetServerPartition(string partitionFolderName, out ServerPartition partition)
		{
			foreach (ServerPartition part in _partitions)
			{
				if (part.PartitionFolder == partitionFolderName)
				{
					partition = part;
					return true;
				}
			}

			partition = null;
			return false;
		}

		/// <summary>
		/// Get the study storage for a potentially nearline/offline study
		/// </summary>
		/// <param name="partition"></param>
		/// <param name="studyInstanceUid"></param>
		/// <param name="storage"></param>
		/// <returns></returns>
		private bool GetStudyStorage(ServerPartition partition, string studyInstanceUid, out StudyStorage storage)
		{
			IStudyStorageEntityBroker broker = ReadContext.GetBroker<IStudyStorageEntityBroker>();
			StudyStorageSelectCriteria criteria = new StudyStorageSelectCriteria();
			criteria.ServerPartitionKey.EqualTo(partition.GetKey());
			criteria.StudyInstanceUid.EqualTo(studyInstanceUid);
			storage = broker.FindOne(criteria);

			if (storage != null)
				return true;

			return false;
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Process the <see cref="ServiceLock"/> item.
		/// </summary>
		/// <param name="item"></param>
		public void Process(Model.ServiceLock item)
		{
			_store = PersistentStoreRegistry.GetDefaultStore();

			IServerPartitionEntityBroker broker = ReadContext.GetBroker<IServerPartitionEntityBroker>();
			ServerPartitionSelectCriteria criteria = new ServerPartitionSelectCriteria();
			criteria.AeTitle.SortAsc(0);

			_partitions = broker.Find(criteria);

			ServerFilesystemInfo info = FilesystemMonitor.Instance.GetFilesystemInfo(item.FilesystemKey);

			Platform.Log(LogLevel.Info, "Starting rebuilding of Study XML files for filesystem: {0}", info.Filesystem.Description);

			TraverseFilesystemStudies(info.Filesystem);

			item.ScheduledTime = item.ScheduledTime.AddDays(1);

			UnlockServiceLock(item, false, Platform.Time.AddDays(1));

			Platform.Log(LogLevel.Info, "Completed rebuilding of the Study XML files for filesystem: {0}", info.Filesystem.Description);
		}


		#endregion

		public new void Dispose()
		{
			base.Dispose();
		}
	}
}