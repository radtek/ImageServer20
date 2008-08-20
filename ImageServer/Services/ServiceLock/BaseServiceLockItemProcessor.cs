#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.ServiceLock
{
    /// <summary>
    /// Base class with common routines for processors of <see cref="Model.ServiceLock"/> entries.
    /// </summary>
    public class BaseServiceLockItemProcessor : IDisposable
    {
        private IReadContext _readContext;

        #region Protected Properties
        protected IReadContext ReadContext
        {
            get { return _readContext; }
        }
        #endregion

        #region Contructors
        public BaseServiceLockItemProcessor()
        {
            _readContext = PersistentStoreRegistry.GetDefaultStore().OpenReadContext();
        }
        #endregion

        #region Protected Methods
        protected IList<FilesystemQueue> GetFilesystemQueueCandidates(Model.ServiceLock item, DateTime scheduledTime, FilesystemQueueTypeEnum type)
        {
            IFilesystemQueueEntityBroker broker = ReadContext.GetBroker<IFilesystemQueueEntityBroker>();
            FilesystemQueueSelectCriteria fsQueueCriteria = new FilesystemQueueSelectCriteria();

            fsQueueCriteria.FilesystemKey.EqualTo(item.FilesystemKey);
            fsQueueCriteria.ScheduledTime.LessThanOrEqualTo(scheduledTime);
            fsQueueCriteria.FilesystemQueueTypeEnum.EqualTo(type);

            WorkQueueSelectCriteria workQueueSearchCriteria = new WorkQueueSelectCriteria();
            workQueueSearchCriteria.WorkQueueStatusEnum.In(new WorkQueueStatusEnum[] { WorkQueueStatusEnum.Idle, WorkQueueStatusEnum.InProgress, WorkQueueStatusEnum.Pending });
            fsQueueCriteria.WorkQueue.NotExists(workQueueSearchCriteria); // no work queue item exists for the same studies
            fsQueueCriteria.ScheduledTime.SortAsc(0);

            IList<FilesystemQueue> list = broker.Find(fsQueueCriteria, 0, ServiceLockSettings.Default.FilesystemQueueResultCount);

            return list;
        }

        /// <summary>
        /// Load the storage location for a Study and partition.
        /// </summary>
        protected StudyStorageLocation LoadStorageLocation(ServerEntityKey serverPartitionKey, String studyInstanceUid)
        {
            IQueryStudyStorageLocation select = _readContext.GetBroker<IQueryStudyStorageLocation>();

            StudyStorageLocationQueryParameters parms = new StudyStorageLocationQueryParameters();
            parms.StudyInstanceUid = studyInstanceUid;
            parms.ServerPartitionKey = serverPartitionKey;

            StudyStorageLocation storageLocation = select.FindOne(parms);

            if (storageLocation == null)
            {
                string error = String.Format("Unable to find storage location for study {0} on partition {1}",studyInstanceUid, serverPartitionKey);
                Platform.Log(LogLevel.Error, error);
                throw new ApplicationException(error);
            }
            return storageLocation;
        }

		/// <summary>
		/// Load a <see cref="StudyXml"/> file for a given <see cref="StudyStorageLocation"/>
		/// </summary>
		/// <param name="location">The location a study is stored.</param>
		/// <returns>The <see cref="StudyXml"/> instance for <paramref name="location"/></returns>
		protected virtual StudyXml LoadStudyXml(StudyStorageLocation location)
		{
			StudyXml theXml = new StudyXml();

			String streamFile = Path.Combine(location.GetStudyPath(), location.StudyInstanceUid + ".xml");
			if (File.Exists(streamFile))
			{
				using (Stream fileStream = new FileStream(streamFile, FileMode.Open))
				{
					XmlDocument theDoc = new XmlDocument();

					StudyXmlIo.Read(theDoc, fileStream);

					theXml.SetMemento(theDoc);

					fileStream.Close();
				}
			}

			return theXml;
		}

    	/// <summary>
		/// Load all of the instances in a given <see cref="StudyXml"/> file into the component for sending.
		/// </summary>
		/// <param name="studyXml">The <see cref="StudyXml"/> file to load from</param>
		/// <param name="context"></param>
		/// <param name="workQueueKey"></param>
		protected void InsertWorkQueueUidFromStudyXml(StudyXml studyXml, IUpdateContext context, ServerEntityKey workQueueKey)
		{
			foreach (SeriesXml seriesXml in studyXml)
			{
				foreach (InstanceXml instanceXml in seriesXml)
				{
					WorkQueueUidUpdateColumns updateColumns = new WorkQueueUidUpdateColumns();
					updateColumns.Duplicate = false;
					updateColumns.Failed = false;
					updateColumns.SeriesInstanceUid = seriesXml.SeriesInstanceUid;
					updateColumns.SopInstanceUid = instanceXml.SopInstanceUid;
					updateColumns.WorkQueueKey = workQueueKey;

					IWorkQueueUidEntityBroker broker = context.GetBroker<IWorkQueueUidEntityBroker>();

					broker.Insert(updateColumns);
				}
			}
		}


        #endregion

        #region Static Methods
        protected static float CalculateFolderSize(string folder)
        {
            float folderSize = 0.0f;
            try
            {
                //Checks if the path is valid or not
                if (!Directory.Exists(folder))
                    return folderSize;
                else
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                            if (File.Exists(file))
                            {
                                FileInfo finfo = new FileInfo(file);
                                folderSize += finfo.Length;
                            }
                        }

                        foreach (string dir in Directory.GetDirectories(folder))
                            folderSize += CalculateFolderSize(dir);
                    }
                    catch (NotSupportedException e)
                    {
                        Platform.Log(LogLevel.Error, e, "Unable to calculate folder size");
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Platform.Log(LogLevel.Error, e, "Unable to calculate folder size");
            }
            return folderSize;
        }

        /// <summary>
        /// Set a <see cref="ServiceLock"/> entry to pending.
        /// </summary>
        /// <param name="item">The <see cref="ServiceLock"/> entry to set.</param>
        /// <param name="scheduledTime"></param>
        /// <param name="enabled">Bool telling if the ServiceLock entry should be enabled after unlock.</param>
        protected static void UnlockServiceLock(Model.ServiceLock item, bool enabled, DateTime scheduledTime)
        {
            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                // Update the WorkQueue item status and times.
                IUpdateServiceLock update = updateContext.GetBroker<IUpdateServiceLock>();

                ServiceLockUpdateParameters parms = new ServiceLockUpdateParameters();
                parms.ServiceLockKey = item.GetKey();
                parms.Lock = false;
                parms.ScheduledTime = scheduledTime;
                parms.ProcessorId = item.ProcessorId;
                parms.Enabled = enabled;

                if (false == update.Execute(parms))
                {
                    Platform.Log(LogLevel.Error, "Unable to update StudyLock GUID Status: {0}",
                                 item.GetKey().ToString());
                }

                updateContext.Commit();
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose of any native resources.
        /// </summary>
        public void Dispose()
        {
            if (_readContext != null)
            {
                _readContext.Dispose();
                _readContext = null;
            }
        }
        #endregion
    }
}
