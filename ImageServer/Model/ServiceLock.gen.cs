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
    public partial class ServiceLock: ServerEntity
    {
        #region Constructors
        public ServiceLock():base("ServiceLock")
        {}
        public ServiceLock(
             ServiceLockTypeEnum _serviceLockTypeEnum_
            ,Boolean _lock_
            ,DateTime _scheduledTime_
            ,Boolean _enabled_
            ,XmlDocument _state_
            ,ServerEntityKey _filesystemKey_
            ,String _processorId_
            ):base("ServiceLock")
        {
            ServiceLockTypeEnum = _serviceLockTypeEnum_;
            Lock = _lock_;
            ScheduledTime = _scheduledTime_;
            Enabled = _enabled_;
            State = _state_;
            FilesystemKey = _filesystemKey_;
            ProcessorId = _processorId_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="ServiceLockTypeEnum")]
        public ServiceLockTypeEnum ServiceLockTypeEnum
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="Lock")]
        public Boolean Lock
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="ScheduledTime")]
        public DateTime ScheduledTime
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="Enabled")]
        public Boolean Enabled
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="State")]
        public XmlDocument State
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="FilesystemGUID")]
        public ServerEntityKey FilesystemKey
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServiceLock", ColumnName="ProcessorId")]
        public String ProcessorId
        { get; set; }
        #endregion

        #region Static Methods
        static public ServiceLock Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public ServiceLock Load(IPersistenceContext read, ServerEntityKey key)
        {
            IServiceLockEntityBroker broker = read.GetBroker<IServiceLockEntityBroker>();
            ServiceLock theObject = broker.Load(key);
            return theObject;
        }
        static public ServiceLock Insert(ServiceLock entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                ServiceLock newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public ServiceLock Insert(IUpdateContext update, ServiceLock entity)
        {
            IServiceLockEntityBroker broker = update.GetBroker<IServiceLockEntityBroker>();
            ServiceLockUpdateColumns updateColumns = new ServiceLockUpdateColumns();
            updateColumns.ServiceLockTypeEnum = entity.ServiceLockTypeEnum;
            updateColumns.Lock = entity.Lock;
            updateColumns.ScheduledTime = entity.ScheduledTime;
            updateColumns.Enabled = entity.Enabled;
            updateColumns.State = entity.State;
            updateColumns.FilesystemKey = entity.FilesystemKey;
            updateColumns.ProcessorId = entity.ProcessorId;
            ServiceLock newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
