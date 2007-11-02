#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using ClearCanvas.DicomServices.Xml;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.WorkQueue
{
    /// <summary>
    /// Base class used when implementing WorkQueue item processors.
    /// </summary>
    public abstract class BaseItemProcessor : IDisposable
    {
        #region Private Members
        private StudyStorageLocation _storageLocation;
        private IReadContext _readContext;
        private IList<WorkQueueUid> _uidList;
        #endregion

        #region Protected Properties
        protected IReadContext ReadContext
        {
            get { return _readContext; }
        }
        protected StudyStorageLocation StorageLocation
        {
            get { return _storageLocation; }
        }
        protected IList<WorkQueueUid> WorkQueueUidList
        {
            get { return _uidList; }
        }
        #endregion

        #region Contructors
        public BaseItemProcessor()
        {
            _readContext = PersistentStoreRegistry.GetDefaultStore().OpenReadContext();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Load the storage location for the WorkQueue item.
        /// </summary>
        /// <param name="item">The item to load the location for.</param>
        protected void LoadStorageLocation(Model.WorkQueue item)
        {
            IQueryStudyStorageLocation select = _readContext.GetBroker<IQueryStudyStorageLocation>();

            StudyStorageLocationQueryParameters parms = new StudyStorageLocationQueryParameters();
            parms.StudyStorageKey = item.StudyStorageKey;

            IList<StudyStorageLocation> list = select.Execute(parms);

            if (list.Count == 0)
            {
                Platform.Log(LogLevel.Error, "Unable to find storage location for WorkQueue item: {0}", item.GetKey().ToString());
                throw new ApplicationException("Unable to find storage location for WorkQueue item.");
            }

            _storageLocation = list[0];
        }

        /// <summary>
        /// Load the specific SOP Instance Uids in the database for the WorkQueue item.
        /// </summary>
        /// <param name="item">The WorkQueue item.</param>
        protected void LoadUids(Model.WorkQueue item)
        {
            IQueryWorkQueueUids select = _readContext.GetBroker<IQueryWorkQueueUids>();

            WorkQueueUidQueryParameters parms = new WorkQueueUidQueryParameters();

            parms.WorkQueueKey = item.GetKey();

            _uidList = select.Execute(parms);
        }

        /// <summary>
        /// Set a <see cref="WorkQueue"/> entry to pending.
        /// </summary>
        /// <param name="item">The <see cref="WorkQueue"/> entry to set.</param>
        /// <param name="failed">If true, the item failed and the failure retry count should be incremented.</param>
        protected static void SetWorkQueueItemPending(Model.WorkQueue item, bool failed)
        {
            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                // Update the WorkQueue item status and times.
                IUpdateWorkQueue update = updateContext.GetBroker<IUpdateWorkQueue>();

                WorkQueueUpdateParameters parms = new WorkQueueUpdateParameters();
                parms.WorkQueueKey = item.GetKey();
                parms.StudyStorageKey = item.StudyStorageKey;
                parms.ProcessorID = item.ProcessorID;

                WorkQueueSettings settings = WorkQueueSettings.Default;
                    
                if (failed)
                {
                    parms.FailureCount = item.FailureCount + 1;
                    if ((item.FailureCount + 1) > settings.WorkQueueMaxFailureCount)
                    {
                        Platform.Log(LogLevel.Error,
                                     "Failing StudyProcess WorkQueue entry ({0}), reached max retry count of {1}",
                                     item.GetKey(), item.FailureCount + 1);
                        parms.StatusEnum = StatusEnum.GetEnum("Failed");
                        parms.ScheduledTime = Platform.Time;
                        parms.ExpirationTime = Platform.Time;
                    }
                    else
                    {
                        Platform.Log(LogLevel.Error,
                                     "Resetting StudyProcess WorkQueue entry ({0}) to Pending, current retry count {1}",
                                     item.GetKey(), item.FailureCount + 1);
                        parms.StatusEnum = StatusEnum.GetEnum("Pending");
                        parms.ScheduledTime = Platform.Time.AddMinutes(settings.WorkQueueFailureDelayMinutes);
                        parms.ExpirationTime =
                            Platform.Time.AddMinutes((settings.WorkQueueMaxFailureCount-item.FailureCount) *
                                                     settings.WorkQueueFailureDelayMinutes);
                    }
                }
                else
                {
                    parms.StatusEnum = StatusEnum.GetEnum("Pending");
                    parms.FailureCount = item.FailureCount;
                    parms.ScheduledTime = Platform.Time.AddSeconds(15.0);
                    parms.ExpirationTime = Platform.Time.AddSeconds(settings.WorkQueueExpireDelaySeconds);
                }

                if (false == update.Execute(parms))
                {
                    Platform.Log(LogLevel.Error, "Unable to update StudyProcess WorkQueue GUID Status: {0}",
                                 item.GetKey().ToString());
                }

                updateContext.Commit();
            }
        }

        /// <summary>
        /// Set a <see cref="WorkQueue"/> item to complete.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This routine will set <paramref name="item"/> to Completed if 
        /// the current time is after the <see cref="WorkQueue.ExpirationTime"/>.
        /// If it is not after the <see cref="WorkQueue.ExpirationTime"/>, the 
        /// <paramref name="item"/> is set to Pending.
        /// </para>
        /// </remarks>
        /// <param name="item">The <see cref="WorkQueue"/> item to set.</param>
        protected static void SetWorkQueueItemCompleteIfExpired(Model.WorkQueue item)
        {
            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                IUpdateWorkQueue update = updateContext.GetBroker<IUpdateWorkQueue>();
                WorkQueueUpdateParameters parms = new WorkQueueUpdateParameters();
                parms.ProcessorID = item.ProcessorID;

                if (item.ExpirationTime < Platform.Time)
                {
                    parms.StatusEnum = StatusEnum.GetEnum("Completed");
                    parms.WorkQueueKey = item.GetKey();
                    parms.StudyStorageKey = item.StudyStorageKey;
                    parms.FailureCount = item.FailureCount;
                    parms.ScheduledTime = item.ScheduledTime;
                    parms.ExpirationTime = item.ExpirationTime; // Keep the same
                }
                else
                {
                    WorkQueueSettings settings = WorkQueueSettings.Default;

                    DateTime scheduledTime = Platform.Time.AddSeconds(settings.WorkQueueProcessDelaySeconds);
                    if (scheduledTime > item.ExpirationTime)
                        scheduledTime = item.ExpirationTime;

                    parms.StatusEnum = StatusEnum.GetEnum("Pending");
                    parms.WorkQueueKey = item.GetKey();
                    parms.StudyStorageKey = item.StudyStorageKey;
                    parms.ScheduledTime = scheduledTime; 
                    parms.ExpirationTime = item.ExpirationTime; // Keep the same
                    parms.FailureCount = item.FailureCount;
                }

                if (false == update.Execute(parms))
                {
                    Platform.Log(LogLevel.Error, "Unable to update StudyProcess WorkQueue GUID: {0}",
                                 item.GetKey().ToString());
                }
            }
        }

        /// <summary>
        /// Delete an entry in the <see cref="WorkQueueUid"/> table.
        /// </summary>
        /// <param name="sop">The <see cref="WorkQueueUid"/> entry to delete.</param>
        protected static void DeleteWorkQueueUid(WorkQueueUid sop)
        {
            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                IDeleteWorkQueueUid delete = updateContext.GetBroker<IDeleteWorkQueueUid>();

                WorkQueueUidDeleteParameters parms = new WorkQueueUidDeleteParameters();
                parms.WorkQueueUidKey = sop.GetKey();

                delete.Execute(parms);

                updateContext.Commit();
            }
        }

        /// <summary>
        /// Load a <see cref="StudyXml"/> file for a given <see cref="StudyStorageLocation"/>
        /// </summary>
        /// <param name="location">The location a study is stored.</param>
        /// <returns>The <see cref="StudyXml"/> instance for <paramref name="location"/></returns>
        protected static StudyXml LoadStudyXml(StudyStorageLocation location)
        {
            String streamFile = Path.Combine(location.GetStudyPath(), location.StudyInstanceUid + ".xml");

            StudyXml theXml = new StudyXml();

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
