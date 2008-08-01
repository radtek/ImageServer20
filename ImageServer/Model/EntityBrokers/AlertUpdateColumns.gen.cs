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
    using ClearCanvas.ImageServer.Enterprise;

   public class AlertUpdateColumns : EntityUpdateColumns
   {
       public AlertUpdateColumns()
       : base("Alert")
       {}
        public AlertCategoryEnum AlertCategoryEnum
        {
            set { SubParameters["AlertCategoryEnum"] = new EntityUpdateColumn<AlertCategoryEnum>("AlertCategoryEnum", value); }
        }
        public AlertLevelEnum AlertLevelEnum
        {
            set { SubParameters["AlertLevelEnum"] = new EntityUpdateColumn<AlertLevelEnum>("AlertLevelEnum", value); }
        }
        public System.String Component
        {
            set { SubParameters["Component"] = new EntityUpdateColumn<System.String>("Component", value); }
        }
        public System.Xml.XmlDocument Content
        {
            set { SubParameters["Content"] = new EntityUpdateColumn<System.Xml.XmlDocument>("Content", value); }
        }
        public System.DateTime InsertTime
        {
            set { SubParameters["InsertTime"] = new EntityUpdateColumn<System.DateTime>("InsertTime", value); }
        }
        public System.String Source
        {
            set { SubParameters["Source"] = new EntityUpdateColumn<System.String>("Source", value); }
        }
        public System.Int32 TypeCode
        {
            set { SubParameters["TypeCode"] = new EntityUpdateColumn<System.Int32>("TypeCode", value); }
        }
    }
}
