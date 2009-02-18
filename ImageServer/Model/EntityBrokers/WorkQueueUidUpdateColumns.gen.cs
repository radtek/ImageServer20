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

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using ClearCanvas.Dicom;
    using ClearCanvas.ImageServer.Enterprise;

   public class WorkQueueUidUpdateColumns : EntityUpdateColumns
   {
       public WorkQueueUidUpdateColumns()
       : base("WorkQueueUid")
       {}
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Duplicate")]
        public System.Boolean Duplicate
        {
            set { SubParameters["Duplicate"] = new EntityUpdateColumn<System.Boolean>("Duplicate", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Extension")]
        public System.String Extension
        {
            set { SubParameters["Extension"] = new EntityUpdateColumn<System.String>("Extension", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Failed")]
        public System.Boolean Failed
        {
            set { SubParameters["Failed"] = new EntityUpdateColumn<System.Boolean>("Failed", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="FailureCount")]
        public System.Int16 FailureCount
        {
            set { SubParameters["FailureCount"] = new EntityUpdateColumn<System.Int16>("FailureCount", value); }
        }
       [DicomField(DicomTags.SeriesInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SeriesInstanceUid")]
        public System.String SeriesInstanceUid
        {
            set { SubParameters["SeriesInstanceUid"] = new EntityUpdateColumn<System.String>("SeriesInstanceUid", value); }
        }
       [DicomField(DicomTags.SopInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SopInstanceUid")]
        public System.String SopInstanceUid
        {
            set { SubParameters["SopInstanceUid"] = new EntityUpdateColumn<System.String>("SopInstanceUid", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="WorkQueueGUID")]
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey WorkQueueKey
        {
            set { SubParameters["WorkQueueKey"] = new EntityUpdateColumn<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("WorkQueueKey", value); }
        }
    }
}
