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

    public partial class FilesystemSelectCriteria : EntitySelectCriteria
    {
        public FilesystemSelectCriteria()
        : base("Filesystem")
        {}
        public ISearchCondition<System.String> Description
        {
            get
            {
              if (!SubCriteria.ContainsKey("Description"))
              {
                 SubCriteria["Description"] = new SearchCondition<System.String>("Description");
              }
              return (ISearchCondition<System.String>)SubCriteria["Description"];
            } 
        }
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
        public ISearchCondition<System.String> FilesystemPath
        {
            get
            {
              if (!SubCriteria.ContainsKey("FilesystemPath"))
              {
                 SubCriteria["FilesystemPath"] = new SearchCondition<System.String>("FilesystemPath");
              }
              return (ISearchCondition<System.String>)SubCriteria["FilesystemPath"];
            } 
        }
        public ISearchCondition<FilesystemTierEnum> FilesystemTierEnum
        {
            get
            {
              if (!SubCriteria.ContainsKey("FilesystemTierEnum"))
              {
                 SubCriteria["FilesystemTierEnum"] = new SearchCondition<FilesystemTierEnum>("FilesystemTierEnum");
              }
              return (ISearchCondition<FilesystemTierEnum>)SubCriteria["FilesystemTierEnum"];
            } 
        }
        public ISearchCondition<System.Decimal> HighWatermark
        {
            get
            {
              if (!SubCriteria.ContainsKey("HighWatermark"))
              {
                 SubCriteria["HighWatermark"] = new SearchCondition<System.Decimal>("HighWatermark");
              }
              return (ISearchCondition<System.Decimal>)SubCriteria["HighWatermark"];
            } 
        }
        public ISearchCondition<System.Decimal> LowWatermark
        {
            get
            {
              if (!SubCriteria.ContainsKey("LowWatermark"))
              {
                 SubCriteria["LowWatermark"] = new SearchCondition<System.Decimal>("LowWatermark");
              }
              return (ISearchCondition<System.Decimal>)SubCriteria["LowWatermark"];
            } 
        }
        public ISearchCondition<System.Decimal> PercentFull
        {
            get
            {
              if (!SubCriteria.ContainsKey("PercentFull"))
              {
                 SubCriteria["PercentFull"] = new SearchCondition<System.Decimal>("PercentFull");
              }
              return (ISearchCondition<System.Decimal>)SubCriteria["PercentFull"];
            } 
        }
        public ISearchCondition<System.Boolean> ReadOnly
        {
            get
            {
              if (!SubCriteria.ContainsKey("ReadOnly"))
              {
                 SubCriteria["ReadOnly"] = new SearchCondition<System.Boolean>("ReadOnly");
              }
              return (ISearchCondition<System.Boolean>)SubCriteria["ReadOnly"];
            } 
        }
        public ISearchCondition<System.Boolean> WriteOnly
        {
            get
            {
              if (!SubCriteria.ContainsKey("WriteOnly"))
              {
                 SubCriteria["WriteOnly"] = new SearchCondition<System.Boolean>("WriteOnly");
              }
              return (ISearchCondition<System.Boolean>)SubCriteria["WriteOnly"];
            } 
        }
    }
}
