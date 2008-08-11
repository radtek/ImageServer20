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

    public partial class ServiceLockSelectCriteria : EntitySelectCriteria
    {
        public ServiceLockSelectCriteria()
        : base("ServiceLock")
        {}
        public ISearchCondition<System.Boolean> Enabled
        {
            get
            {
              if (!SubCriteria.ContainsKey("Enabled"))
              {
                 SubCriteria["Enabled"] = new SearchCondition<System.Boolean>("Enabled");
              }
              return (ISearchCondition<System.Boolean>)SubCriteria["Enabled"];
            } 
        }
        public ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey> FilesystemKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("FilesystemKey"))
              {
                 SubCriteria["FilesystemKey"] = new SearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("FilesystemKey");
              }
              return (ISearchCondition<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>)SubCriteria["FilesystemKey"];
            } 
        }
        public ISearchCondition<System.Boolean> Lock
        {
            get
            {
              if (!SubCriteria.ContainsKey("Lock"))
              {
                 SubCriteria["Lock"] = new SearchCondition<System.Boolean>("Lock");
              }
              return (ISearchCondition<System.Boolean>)SubCriteria["Lock"];
            } 
        }
        public ISearchCondition<System.String> ProcessorId
        {
            get
            {
              if (!SubCriteria.ContainsKey("ProcessorId"))
              {
                 SubCriteria["ProcessorId"] = new SearchCondition<System.String>("ProcessorId");
              }
              return (ISearchCondition<System.String>)SubCriteria["ProcessorId"];
            } 
        }
        public ISearchCondition<System.DateTime> ScheduledTime
        {
            get
            {
              if (!SubCriteria.ContainsKey("ScheduledTime"))
              {
                 SubCriteria["ScheduledTime"] = new SearchCondition<System.DateTime>("ScheduledTime");
              }
              return (ISearchCondition<System.DateTime>)SubCriteria["ScheduledTime"];
            } 
        }
        public ISearchCondition<ServiceLockTypeEnum> ServiceLockTypeEnum
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServiceLockTypeEnum"))
              {
                 SubCriteria["ServiceLockTypeEnum"] = new SearchCondition<ServiceLockTypeEnum>("ServiceLockTypeEnum");
              }
              return (ISearchCondition<ServiceLockTypeEnum>)SubCriteria["ServiceLockTypeEnum"];
            } 
        }
        public ISearchCondition<System.Xml.XmlDocument> State
        {
            get
            {
              if (!SubCriteria.ContainsKey("State"))
              {
                 SubCriteria["State"] = new SearchCondition<System.Xml.XmlDocument>("State");
              }
              return (ISearchCondition<System.Xml.XmlDocument>)SubCriteria["State"];
            } 
        }
    }
}
