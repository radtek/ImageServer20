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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Dicom;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class StudyStorage: ServerEntity
    {
        #region Constructors
        public StudyStorage():base("StudyStorage")
        {}
        public StudyStorage(
             System.DateTime _insertTime_
            ,System.DateTime _lastAccessedTime_
            ,System.Boolean _lock_
            ,QueueStudyStateEnum _queueStudyStateEnum_
            ,ClearCanvas.ImageServer.Enterprise.ServerEntityKey _serverPartitionKey_
            ,System.String _studyInstanceUid_
            ,StudyStatusEnum _studyStatusEnum_
            ):base("StudyStorage")
        {
            _insertTime = _insertTime_;
            _lastAccessedTime = _lastAccessedTime_;
            _lock = _lock_;
            _queueStudyStateEnum = _queueStudyStateEnum_;
            _serverPartitionKey = _serverPartitionKey_;
            _studyInstanceUid = _studyInstanceUid_;
            _studyStatusEnum = _studyStatusEnum_;
        }
        #endregion

        #region Private Members
        private DateTime _insertTime;
        private DateTime _lastAccessedTime;
        private Boolean _lock;
        private QueueStudyStateEnum _queueStudyStateEnum;
        private ServerEntityKey _serverPartitionKey;
        private String _studyInstanceUid;
        private StudyStatusEnum _studyStatusEnum;
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="InsertTime")]
        public DateTime InsertTime
        {
        get { return _insertTime; }
        set { _insertTime = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="LastAccessedTime")]
        public DateTime LastAccessedTime
        {
        get { return _lastAccessedTime; }
        set { _lastAccessedTime = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="Lock")]
        public Boolean Lock
        {
        get { return _lock; }
        set { _lock = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="QueueStudyStateEnum")]
        public QueueStudyStateEnum QueueStudyStateEnum
        {
        get { return _queueStudyStateEnum; }
        set { _queueStudyStateEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="ServerPartitionGUID")]
        public ServerEntityKey ServerPartitionKey
        {
        get { return _serverPartitionKey; }
        set { _serverPartitionKey = value; }
        }
        [DicomField(DicomTags.StudyInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="StudyInstanceUid")]
        public String StudyInstanceUid
        {
        get { return _studyInstanceUid; }
        set { _studyInstanceUid = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyStorage", ColumnName="StudyStatusEnum")]
        public StudyStatusEnum StudyStatusEnum
        {
        get { return _studyStatusEnum; }
        set { _studyStatusEnum = value; }
        }
        #endregion

        #region Static Methods
        static public StudyStorage Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public StudyStorage Load(IPersistenceContext read, ServerEntityKey key)
        {
            IStudyStorageEntityBroker broker = read.GetBroker<IStudyStorageEntityBroker>();
            StudyStorage theObject = broker.Load(key);
            return theObject;
        }
        static public StudyStorage Insert(StudyStorage entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                StudyStorage newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public StudyStorage Insert(IUpdateContext update, StudyStorage entity)
        {
            IStudyStorageEntityBroker broker = update.GetBroker<IStudyStorageEntityBroker>();
            StudyStorageUpdateColumns updateColumns = new StudyStorageUpdateColumns();
            updateColumns.InsertTime = entity.InsertTime;
            updateColumns.LastAccessedTime = entity.LastAccessedTime;
            updateColumns.Lock = entity.Lock;
            updateColumns.QueueStudyStateEnum = entity.QueueStudyStateEnum;
            updateColumns.ServerPartitionKey = entity.ServerPartitionKey;
            updateColumns.StudyInstanceUid = entity.StudyInstanceUid;
            updateColumns.StudyStatusEnum = entity.StudyStatusEnum;
            StudyStorage newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
