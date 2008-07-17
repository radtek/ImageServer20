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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class ArchiveStudyStorageSelectCriteria : EntitySelectCriteria
    {
        public ArchiveStudyStorageSelectCriteria()
        : base("ArchiveStudyStorage")
        {}
        public ISearchCondition<System.DateTime> ArchiveTime
        {
            get
            {
              if (!SubCriteria.ContainsKey("ArchiveTime"))
              {
                 SubCriteria["ArchiveTime"] = new SearchCondition<System.DateTime>("ArchiveTime");
              }
              return (ISearchCondition<System.DateTime>)SubCriteria["ArchiveTime"];
            } 
        }
        public ISearchCondition<System.Xml.XmlDocument> ArchiveXml
        {
            get
            {
              if (!SubCriteria.ContainsKey("ArchiveXml"))
              {
                 SubCriteria["ArchiveXml"] = new SearchCondition<System.Xml.XmlDocument>("ArchiveXml");
              }
              return (ISearchCondition<System.Xml.XmlDocument>)SubCriteria["ArchiveXml"];
            } 
        }
        public ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey> PartitionArchiveKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("PartitionArchiveKey"))
              {
                 SubCriteria["PartitionArchiveKey"] = new SearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("PartitionArchiveKey");
              }
              return (ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>)SubCriteria["PartitionArchiveKey"];
            } 
        }
        public ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey> ServerTransferSyntaxKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerTransferSyntaxKey"))
              {
                 SubCriteria["ServerTransferSyntaxKey"] = new SearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("ServerTransferSyntaxKey");
              }
              return (ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>)SubCriteria["ServerTransferSyntaxKey"];
            } 
        }
        public ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey> StudyStorageKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyStorageKey"))
              {
                 SubCriteria["StudyStorageKey"] = new SearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("StudyStorageKey");
              }
              return (ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>)SubCriteria["StudyStorageKey"];
            } 
        }
    }
}
