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
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class WorkQueueUidSelectCriteria : EntitySelectCriteria
    {
        public WorkQueueUidSelectCriteria()
        : base("WorkQueueUid")
        {}
        public WorkQueueUidSelectCriteria(WorkQueueUidSelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new WorkQueueUidSelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="WorkQueueGUID")]
        public ISearchCondition<ServerEntityKey> WorkQueueKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("WorkQueueKey"))
              {
                 SubCriteria["WorkQueueKey"] = new SearchCondition<ServerEntityKey>("WorkQueueKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["WorkQueueKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Failed")]
        public ISearchCondition<Boolean> Failed
        {
            get
            {
              if (!SubCriteria.ContainsKey("Failed"))
              {
                 SubCriteria["Failed"] = new SearchCondition<Boolean>("Failed");
              }
              return (ISearchCondition<Boolean>)SubCriteria["Failed"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Duplicate")]
        public ISearchCondition<Boolean> Duplicate
        {
            get
            {
              if (!SubCriteria.ContainsKey("Duplicate"))
              {
                 SubCriteria["Duplicate"] = new SearchCondition<Boolean>("Duplicate");
              }
              return (ISearchCondition<Boolean>)SubCriteria["Duplicate"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="FailureCount")]
        public ISearchCondition<Int16> FailureCount
        {
            get
            {
              if (!SubCriteria.ContainsKey("FailureCount"))
              {
                 SubCriteria["FailureCount"] = new SearchCondition<Int16>("FailureCount");
              }
              return (ISearchCondition<Int16>)SubCriteria["FailureCount"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="GroupID")]
        public ISearchCondition<String> GroupID
        {
            get
            {
              if (!SubCriteria.ContainsKey("GroupID"))
              {
                 SubCriteria["GroupID"] = new SearchCondition<String>("GroupID");
              }
              return (ISearchCondition<String>)SubCriteria["GroupID"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="RelativePath")]
        public ISearchCondition<String> RelativePath
        {
            get
            {
              if (!SubCriteria.ContainsKey("RelativePath"))
              {
                 SubCriteria["RelativePath"] = new SearchCondition<String>("RelativePath");
              }
              return (ISearchCondition<String>)SubCriteria["RelativePath"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Extension")]
        public ISearchCondition<String> Extension
        {
            get
            {
              if (!SubCriteria.ContainsKey("Extension"))
              {
                 SubCriteria["Extension"] = new SearchCondition<String>("Extension");
              }
              return (ISearchCondition<String>)SubCriteria["Extension"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SeriesInstanceUid")]
        public ISearchCondition<String> SeriesInstanceUid
        {
            get
            {
              if (!SubCriteria.ContainsKey("SeriesInstanceUid"))
              {
                 SubCriteria["SeriesInstanceUid"] = new SearchCondition<String>("SeriesInstanceUid");
              }
              return (ISearchCondition<String>)SubCriteria["SeriesInstanceUid"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SopInstanceUid")]
        public ISearchCondition<String> SopInstanceUid
        {
            get
            {
              if (!SubCriteria.ContainsKey("SopInstanceUid"))
              {
                 SubCriteria["SopInstanceUid"] = new SearchCondition<String>("SopInstanceUid");
              }
              return (ISearchCondition<String>)SubCriteria["SopInstanceUid"];
            } 
        }
    }
}
