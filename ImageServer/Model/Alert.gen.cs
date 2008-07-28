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

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class Alert: ServerEntity
    {
        #region Constructors
        public Alert():base("Alert")
        {}
        #endregion

        #region Private Members
        private AlertCategoryEnum _alertCategoryEnum;
        private AlertLevelEnum _alertLevelEnum;
        private System.Xml.XmlDocument _content;
        private System.DateTime _expirationTime;
        private System.DateTime _insertTime;
        private System.String _source;
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="AlertCategoryEnum")]
        public AlertCategoryEnum AlertCategoryEnum
        {
        get { return _alertCategoryEnum; }
        set { _alertCategoryEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="AlertLevelEnum")]
        public AlertLevelEnum AlertLevelEnum
        {
        get { return _alertLevelEnum; }
        set { _alertLevelEnum = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="Content")]
        public System.Xml.XmlDocument Content
        {
        get { return _content; }
        set { _content = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="ExpirationTime")]
        public System.DateTime ExpirationTime
        {
        get { return _expirationTime; }
        set { _expirationTime = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="InsertTime")]
        public System.DateTime InsertTime
        {
        get { return _insertTime; }
        set { _insertTime = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Alert", ColumnName="Source")]
        public System.String Source
        {
        get { return _source; }
        set { _source = value; }
        }
        #endregion

        #region Static Methods
        static public Alert Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public Alert Load(IReadContext read, ServerEntityKey key)
        {
            IAlertEntityBroker broker = read.GetBroker<IAlertEntityBroker>();
            Alert theObject = broker.Load(key);
            return theObject;
        }
        #endregion
    }
}
