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

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.ImageServer.Enterprise;

   public class WorkQueueTypePropertiesUpdateColumns : EntityUpdateColumns
   {
       public WorkQueueTypePropertiesUpdateColumns()
       : base("WorkQueueTypeProperties")
       {}
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="WorkQueueTypeEnum")]
        public WorkQueueTypeEnum WorkQueueTypeEnum
        {
            set { SubParameters["WorkQueueTypeEnum"] = new EntityUpdateColumn<WorkQueueTypeEnum>("WorkQueueTypeEnum", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="WorkQueuePriorityEnum")]
        public WorkQueuePriorityEnum WorkQueuePriorityEnum
        {
            set { SubParameters["WorkQueuePriorityEnum"] = new EntityUpdateColumn<WorkQueuePriorityEnum>("WorkQueuePriorityEnum", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MemoryLimited")]
        public Boolean MemoryLimited
        {
            set { SubParameters["MemoryLimited"] = new EntityUpdateColumn<Boolean>("MemoryLimited", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="AlertFailedWorkQueue")]
        public Boolean AlertFailedWorkQueue
        {
            set { SubParameters["AlertFailedWorkQueue"] = new EntityUpdateColumn<Boolean>("AlertFailedWorkQueue", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MaxFailureCount")]
        public Int32 MaxFailureCount
        {
            set { SubParameters["MaxFailureCount"] = new EntityUpdateColumn<Int32>("MaxFailureCount", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="ProcessDelaySeconds")]
        public Int32 ProcessDelaySeconds
        {
            set { SubParameters["ProcessDelaySeconds"] = new EntityUpdateColumn<Int32>("ProcessDelaySeconds", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="FailureDelaySeconds")]
        public Int32 FailureDelaySeconds
        {
            set { SubParameters["FailureDelaySeconds"] = new EntityUpdateColumn<Int32>("FailureDelaySeconds", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="DeleteDelaySeconds")]
        public Int32 DeleteDelaySeconds
        {
            set { SubParameters["DeleteDelaySeconds"] = new EntityUpdateColumn<Int32>("DeleteDelaySeconds", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="PostponeDelaySeconds")]
        public Int32 PostponeDelaySeconds
        {
            set { SubParameters["PostponeDelaySeconds"] = new EntityUpdateColumn<Int32>("PostponeDelaySeconds", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="ExpireDelaySeconds")]
        public Int32 ExpireDelaySeconds
        {
            set { SubParameters["ExpireDelaySeconds"] = new EntityUpdateColumn<Int32>("ExpireDelaySeconds", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="MaxBatchSize")]
        public Int32 MaxBatchSize
        {
            set { SubParameters["MaxBatchSize"] = new EntityUpdateColumn<Int32>("MaxBatchSize", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="ReadLock")]
        public Boolean ReadLock
        {
            set { SubParameters["ReadLock"] = new EntityUpdateColumn<Boolean>("ReadLock", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="WriteLock")]
        public Boolean WriteLock
        {
            set { SubParameters["WriteLock"] = new EntityUpdateColumn<Boolean>("WriteLock", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="QueueStudyStateEnum")]
        public QueueStudyStateEnum QueueStudyStateEnum
        {
            set { SubParameters["QueueStudyStateEnum"] = new EntityUpdateColumn<QueueStudyStateEnum>("QueueStudyStateEnum", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueTypeProperties", ColumnName="QueueStudyStateOrder")]
        public Int16 QueueStudyStateOrder
        {
            set { SubParameters["QueueStudyStateOrder"] = new EntityUpdateColumn<Int16>("QueueStudyStateOrder", value); }
        }
    }
}
