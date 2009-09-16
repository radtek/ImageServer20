#region License

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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class WorkQueueTypeProperties: ServerEntity
    {
        #region Constructors
        public WorkQueueTypeProperties():base("WorkQueueTypeProperties")
        {}
        public WorkQueueTypeProperties(
             WorkQueueTypeEnum _workQueueTypeEnum_
            ,WorkQueuePriorityEnum _workQueuePriorityEnum_
            ,System.Boolean _memoryLimited_
            ,System.Boolean _alertFailedWorkQueue_
            ,System.Int32 _maxFailureCount_
            ,System.Int32 _processDelaySeconds_
            ,System.Int32 _failureDelaySeconds_
            ,System.Int32 _deleteDelaySeconds_
            ,System.Int32 _postponeDelaySeconds_
            ,System.Int32 _maxBatchSize_
            ,System.Int32 _expireDelaySeconds_
            ):base("WorkQueueTypeProperties")
        {
            _workQueueTypeEnum = _workQueueTypeEnum_;
            _workQueuePriorityEnum = _workQueuePriorityEnum_;
            _memoryLimited = _memoryLimited_;
            _alertFailedWorkQueue = _alertFailedWorkQueue_;
            _maxFailureCount = _maxFailureCount_;
            _processDelaySeconds = _processDelaySeconds_;
            _failureDelaySeconds = _failureDelaySeconds_;
            _deleteDelaySeconds = _deleteDelaySeconds_;
            _postponeDelaySeconds = _postponeDelaySeconds_;
            _maxBatchSize = _maxBatchSize_;
            _expireDelaySeconds = _expireDelaySeconds_;
        }
        #endregion

        #region Private Members
        private WorkQueueTypeEnum _workQueueTypeEnum;
        private WorkQueuePriorityEnum _workQueuePriorityEnum;
        private Boolean _memoryLimited;
        private Boolean _alertFailedWorkQueue;
        private Int32 _maxFailureCount;
        private Int32 _processDelaySeconds;
        private Int32 _failureDelaySeconds;
        private Int32 _deleteDelaySeconds;
        private Int32 _postponeDelaySeconds;
        private Int32 _maxBatchSize;
        private Int32 _expireDelaySeconds;
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="WorkQueueTypeEnum")]
        public WorkQueueTypeEnum WorkQueueTypeEnum
        {
        get { return _workQueueTypeEnum; }
        set { _workQueueTypeEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="WorkQueuePriorityEnum")]
        public WorkQueuePriorityEnum WorkQueuePriorityEnum
        {
        get { return _workQueuePriorityEnum; }
        set { _workQueuePriorityEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MemoryLimited")]
        public Boolean MemoryLimited
        {
        get { return _memoryLimited; }
        set { _memoryLimited = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="AlertFailedWorkQueue")]
        public Boolean AlertFailedWorkQueue
        {
        get { return _alertFailedWorkQueue; }
        set { _alertFailedWorkQueue = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MaxFailureCount")]
        public Int32 MaxFailureCount
        {
        get { return _maxFailureCount; }
        set { _maxFailureCount = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="ProcessDelaySeconds")]
        public Int32 ProcessDelaySeconds
        {
        get { return _processDelaySeconds; }
        set { _processDelaySeconds = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="FailureDelaySeconds")]
        public Int32 FailureDelaySeconds
        {
        get { return _failureDelaySeconds; }
        set { _failureDelaySeconds = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="DeleteDelaySeconds")]
        public Int32 DeleteDelaySeconds
        {
        get { return _deleteDelaySeconds; }
        set { _deleteDelaySeconds = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="PostponeDelaySeconds")]
        public Int32 PostponeDelaySeconds
        {
        get { return _postponeDelaySeconds; }
        set { _postponeDelaySeconds = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MaxBatchSize")]
        public Int32 MaxBatchSize
        {
        get { return _maxBatchSize; }
        set { _maxBatchSize = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="ExpireDelaySeconds")]
        public Int32 ExpireDelaySeconds
        {
        get { return _expireDelaySeconds; }
        set { _expireDelaySeconds = value; }
        }
        #endregion

        #region Static Methods
        static public WorkQueueTypeProperties Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public WorkQueueTypeProperties Load(IPersistenceContext read, ServerEntityKey key)
        {
            IWorkQueueTypePropertiesEntityBroker broker = read.GetBroker<IWorkQueueTypePropertiesEntityBroker>();
            WorkQueueTypeProperties theObject = broker.Load(key);
            return theObject;
        }
        static public WorkQueueTypeProperties Insert(WorkQueueTypeProperties entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                WorkQueueTypeProperties newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public WorkQueueTypeProperties Insert(IUpdateContext update, WorkQueueTypeProperties entity)
        {
            IWorkQueueTypePropertiesEntityBroker broker = update.GetBroker<IWorkQueueTypePropertiesEntityBroker>();
            WorkQueueTypePropertiesUpdateColumns updateColumns = new WorkQueueTypePropertiesUpdateColumns();
            updateColumns.WorkQueueTypeEnum = entity.WorkQueueTypeEnum;
            updateColumns.WorkQueuePriorityEnum = entity.WorkQueuePriorityEnum;
            updateColumns.MemoryLimited = entity.MemoryLimited;
            updateColumns.AlertFailedWorkQueue = entity.AlertFailedWorkQueue;
            updateColumns.MaxFailureCount = entity.MaxFailureCount;
            updateColumns.ProcessDelaySeconds = entity.ProcessDelaySeconds;
            updateColumns.FailureDelaySeconds = entity.FailureDelaySeconds;
            updateColumns.DeleteDelaySeconds = entity.DeleteDelaySeconds;
            updateColumns.PostponeDelaySeconds = entity.PostponeDelaySeconds;
            updateColumns.MaxBatchSize = entity.MaxBatchSize;
            updateColumns.ExpireDelaySeconds = entity.ExpireDelaySeconds;
            WorkQueueTypeProperties newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
