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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class RestoreQueue: ServerEntity
    {
        #region Constructors
        public RestoreQueue():base("RestoreQueue")
        {}
        #endregion

        #region Private Members
        private ClearCanvas.ImageServer.Enterprise.ServerEntityKey _archiveStudyStorageKey;
        private System.String _processorId;
        private RestoreQueueStatusEnum _restoreQueueStatusEnum;
        private System.DateTime _scheduledTime;
        private ClearCanvas.ImageServer.Enterprise.ServerEntityKey _studyStorageKey;
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="RestoreQueue", ColumnName="ArchiveStudyStorageGUID")]
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey ArchiveStudyStorageKey
        {
        get { return _archiveStudyStorageKey; }
        set { _archiveStudyStorageKey = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="RestoreQueue", ColumnName="ProcessorId")]
        public System.String ProcessorId
        {
        get { return _processorId; }
        set { _processorId = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="RestoreQueue", ColumnName="RestoreQueueStatusEnum")]
        public RestoreQueueStatusEnum RestoreQueueStatusEnum
        {
        get { return _restoreQueueStatusEnum; }
        set { _restoreQueueStatusEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="RestoreQueue", ColumnName="ScheduledTime")]
        public System.DateTime ScheduledTime
        {
        get { return _scheduledTime; }
        set { _scheduledTime = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="RestoreQueue", ColumnName="StudyStorageGUID")]
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey StudyStorageKey
        {
        get { return _studyStorageKey; }
        set { _studyStorageKey = value; }
        }
        #endregion

        #region Static Methods
        static public RestoreQueue Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public RestoreQueue Load(IReadContext read, ServerEntityKey key)
        {
            IRestoreQueueEntityBroker broker = read.GetBroker<IRestoreQueueEntityBroker>();
            RestoreQueue theObject = broker.Load(key);
            return theObject;
        }
        #endregion
    }
}
