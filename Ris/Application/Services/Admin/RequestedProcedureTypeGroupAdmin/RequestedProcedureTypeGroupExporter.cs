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

using System;
using System.Collections.Generic;
using System.Xml;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;

namespace ClearCanvas.Ris.Application.Services.Admin.RequestedProcedureTypeGroupAdmin
{
    [ExtensionOf(typeof(DataExporterExtensionPoint))]
    [ExtensionOf(typeof(ApplicationRootExtensionPoint))]
    public class RequestedProcedureTypeGroupExporter : DataExporterBase
    {
        private const string tagRequestedProcedureTypeGroups = "requested-procedure-type-groups";

        private const string tagRequestedProcedureTypeGroup = "requested-procedure-type-group";
        private const string attrName = "name";
        private const string attrCategory = "category";
        private const string attrDescription = "description";

        private const string tagRequestedProcedureType = "requested-procedure-type";

        #region DateExporter overrides

        public override bool SupportsXml
        {
            get { return true; }
        }

        public override void ExportXml(XmlWriter writer, IReadContext context)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(tagRequestedProcedureTypeGroups);

            IList<RequestedProcedureTypeGroup> groups = context.GetBroker<IRequestedProcedureTypeGroupBroker>().FindAll();
            CollectionUtils.ForEach<RequestedProcedureTypeGroup>(groups,
                delegate(RequestedProcedureTypeGroup group) { WriteRequestedProcedureTypeGroupXml(group, writer, context); });

            writer.WriteEndElement();
        }

        #endregion

        #region Private methods
        
        private void WriteRequestedProcedureTypeGroupXml(RequestedProcedureTypeGroup group, XmlWriter writer, IReadContext context)
        {
            writer.WriteStartElement(tagRequestedProcedureTypeGroup);
            writer.WriteAttributeString(attrName, group.Name);
            writer.WriteAttributeString(attrCategory, group.Category.ToString());
            writer.WriteAttributeString(attrDescription, group.Description);

            CollectionUtils.ForEach<RequestedProcedureType>(group.RequestedProcedureTypes,
                delegate(RequestedProcedureType type) { WriteRequestedProcedureTypeXml(type, writer); });

            writer.WriteEndElement();
        }

        private void WriteRequestedProcedureTypeXml(RequestedProcedureType type, XmlWriter writer)
        {
            writer.WriteStartElement(tagRequestedProcedureType);
            writer.WriteAttributeString(attrName, type.Name);
            writer.WriteEndElement();
        }

        #endregion
    }
}
