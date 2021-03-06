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
using System.IO;
using System.Threading;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scp;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.Helpers;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.Dicom
{
    /// <summary>
    /// Base class for all DicomScpExtensions for the ImageServer.
    /// </summary>
    public abstract class BaseScp : IDicomScp<DicomScpContext>
    {

        #region Protected Members
        protected IPersistentStore _store = PersistentStoreRegistry.GetDefaultStore();
    	private DicomScpContext _context;
        private Device _device;
        #endregion

        #region Private Methods

        #endregion

        #region Properties
        /// <summary>
		/// The <see cref="ServerPartition"/> associated with the Scp.
		/// </summary>
        protected ServerPartition Partition
        {
            get { return _context.Partition; }
        }

        protected Device Device
        {
            get { return _device; }
            set { _device = value; }
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Called during association verification.
        /// </summary>
        /// <param name="association"></param>
        /// <param name="pcid"></param>
        /// <returns></returns>
        protected abstract DicomPresContextResult OnVerifyAssociation(AssociationParameters association, byte pcid);

        #endregion

        #region Public Methods

		/// <summary>
		/// Verify a presentation context.
		/// </summary>
		/// <param name="association"></param>
		/// <param name="pcid"></param>
		/// <returns></returns>
        public DicomPresContextResult VerifyAssociation(AssociationParameters association, byte pcid)
        {
            bool isNew;

            Device = DeviceManager.LookupDevice(Partition, association, out isNew);

            // Let the subclass perform the verification
            DicomPresContextResult result = OnVerifyAssociation(association, pcid);
            if (result!=DicomPresContextResult.Accept)
            {
                Platform.Log(LogLevel.Debug, "Rejecting Presentation Context {0}:{1} in association between {2} and {3}.",
                             pcid, association.GetAbstractSyntax(pcid).Description,
                             association.CallingAE, association.CalledAE);
            }

            return result;
        }

        
        /// <summary>
        /// Helper method to load a <see cref="StudyXml"/> instance for a given study location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static StudyXml LoadStudyXml(StudyStorageLocation location)
        {
            String streamFile = Path.Combine(location.GetStudyPath(), location.StudyInstanceUid + ".xml");

            StudyXml theXml = new StudyXml();

            if (File.Exists(streamFile))
            {
                // allocate the random number generator here, in case we need it below
                Random rand = new Random();

                // Go into a retry loop, to handle if the study is being processed right now 
                for (int i = 0; ;i++ )
                    try
                    {
                        using (Stream fileStream = FileStreamOpener.OpenForRead(streamFile, FileMode.Open))
                        {
                            XmlDocument theDoc = new XmlDocument();

                            StudyXmlIo.Read(theDoc, fileStream);

                            theXml.SetMemento(theDoc);

                            fileStream.Close();

                            return theXml;
                        }
                    }
                    catch (IOException)
                    {
                        if (i < 5)
                        {
                            Thread.Sleep(rand.Next(5, 50)); // Sleep 5-50 milliseconds
                            continue;
                        }

                        throw;
                    }
            }

            return theXml;
        }

		/// <summary>
		/// Get the Status of a study.
		/// </summary>
		/// <param name="studyInstanceUid">The Study to check for.</param>
		/// <param name="studyStorage">The returned study storage object</param>
		/// <returns>true on success, false on no records found.</returns>
		public bool GetStudyStatus(string studyInstanceUid, out StudyStorage studyStorage)
		{
			using (IReadContext read = _store.OpenReadContext())
			{
				IStudyStorageEntityBroker selectBroker = read.GetBroker<IStudyStorageEntityBroker>();
				StudyStorageSelectCriteria criteria = new StudyStorageSelectCriteria();

				criteria.ServerPartitionKey.EqualTo(Partition.GetKey());
				criteria.StudyInstanceUid.EqualTo(studyInstanceUid);

				IList<StudyStorage> storageList = selectBroker.Find(criteria);

				foreach (StudyStorage studyLocation in storageList)
				{
					studyStorage = studyLocation;
					return true;
				}
				studyStorage = null;
				return false;
			}
		}

        #endregion

        #region IDicomScp Members

        public void SetContext(DicomScpContext parms)
        {
        	_context = parms;
        }
        
        public virtual bool OnReceiveRequest(DicomServer server, ServerAssociationParameters association, byte presentationID, DicomMessage message)
        {
            throw new Exception("The method or operation is not implemented.  The method must be overriden.");
        }

        public virtual IList<SupportedSop> GetSupportedSopClasses()
        {
            throw new Exception("The method or operation is not implemented.  The method must be overriden.");
        }

        #endregion
    }
}