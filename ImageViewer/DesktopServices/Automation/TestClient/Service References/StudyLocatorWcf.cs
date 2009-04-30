﻿#region License

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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator
{
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyIdentifier))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier))]
    public partial class Identifier : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InstanceAvailabilityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RetrieveAeTitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SpecificCharacterSetField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InstanceAvailability
        {
            get
            {
                return this.InstanceAvailabilityField;
            }
            set
            {
                this.InstanceAvailabilityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RetrieveAeTitle
        {
            get
            {
                return this.RetrieveAeTitleField;
            }
            set
            {
                this.RetrieveAeTitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SpecificCharacterSet
        {
            get
            {
                return this.SpecificCharacterSetField;
            }
            set
            {
                this.SpecificCharacterSetField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class SeriesIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier
    {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModalityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfSeriesRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesDescriptionField;
        
        private string SeriesInstanceUidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SeriesNumberField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Modality
        {
            get
            {
                return this.ModalityField;
            }
            set
            {
                this.ModalityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfSeriesRelatedInstances
        {
            get
            {
                return this.NumberOfSeriesRelatedInstancesField;
            }
            set
            {
                this.NumberOfSeriesRelatedInstancesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesDescription
        {
            get
            {
                return this.SeriesDescriptionField;
            }
            set
            {
                this.SeriesDescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid
        {
            get
            {
                return this.SeriesInstanceUidField;
            }
            set
            {
                this.SeriesInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SeriesNumber
        {
            get
            {
                return this.SeriesNumberField;
            }
            set
            {
                this.SeriesNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class ImageIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier
    {
        
        private System.Nullable<int> InstanceNumberField;
        
        private string SeriesInstanceUidField;
        
        private string SopClassUidField;
        
        private string SopInstanceUidField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.Nullable<int> InstanceNumber
        {
            get
            {
                return this.InstanceNumberField;
            }
            set
            {
                this.InstanceNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SeriesInstanceUid
        {
            get
            {
                return this.SeriesInstanceUidField;
            }
            set
            {
                this.SeriesInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SopClassUid
        {
            get
            {
                return this.SopClassUidField;
            }
            set
            {
                this.SopClassUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string SopInstanceUid
        {
            get
            {
                return this.SopInstanceUidField;
            }
            set
            {
                this.SopInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier))]
    public partial class StudyIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.Identifier
    {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccessionNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.ComponentModel.BindingList<string> ModalitiesInStudyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedInstancesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumberOfStudyRelatedSeriesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyIdField;
        
        private string StudyInstanceUidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyTimeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccessionNumber
        {
            get
            {
                return this.AccessionNumberField;
            }
            set
            {
                this.AccessionNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.ComponentModel.BindingList<string> ModalitiesInStudy
        {
            get
            {
                return this.ModalitiesInStudyField;
            }
            set
            {
                this.ModalitiesInStudyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedInstances
        {
            get
            {
                return this.NumberOfStudyRelatedInstancesField;
            }
            set
            {
                this.NumberOfStudyRelatedInstancesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumberOfStudyRelatedSeries
        {
            get
            {
                return this.NumberOfStudyRelatedSeriesField;
            }
            set
            {
                this.NumberOfStudyRelatedSeriesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDate
        {
            get
            {
                return this.StudyDateField;
            }
            set
            {
                this.StudyDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyDescription
        {
            get
            {
                return this.StudyDescriptionField;
            }
            set
            {
                this.StudyDescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyId
        {
            get
            {
                return this.StudyIdField;
            }
            set
            {
                this.StudyIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUid
        {
            get
            {
                return this.StudyInstanceUidField;
            }
            set
            {
                this.StudyInstanceUidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StudyTime
        {
            get
            {
                return this.StudyTimeField;
            }
            set
            {
                this.StudyTimeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class StudyRootStudyIdentifier : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyIdentifier
    {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsBirthDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsBirthTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PatientsSexField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientId
        {
            get
            {
                return this.PatientIdField;
            }
            set
            {
                this.PatientIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthDate
        {
            get
            {
                return this.PatientsBirthDateField;
            }
            set
            {
                this.PatientsBirthDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsBirthTime
        {
            get
            {
                return this.PatientsBirthTimeField;
            }
            set
            {
                this.PatientsBirthTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsName
        {
            get
            {
                return this.PatientsNameField;
            }
            set
            {
                this.PatientsNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PatientsSex
        {
            get
            {
                return this.PatientsSexField;
            }
            set
            {
                this.PatientsSexField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class QueryFailedFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DescriptionField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query")]
    [System.SerializableAttribute()]
    public partial class DataValidationFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.clearcanvas.ca/dicom/query", ConfigurationName="ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudy" +
        "RootQuery")]
    public interface IStudyRootQuery
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryQueryFailedFaultF" +
            "ault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/StudyQueryDataValidationFau" +
            "ltFault", Name="DataValidationFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier> StudyQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier queryCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryQueryFailedFault" +
            "Fault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/SeriesQueryDataValidationFa" +
            "ultFault", Name="DataValidationFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier> SeriesQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier queryCriteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQuery", ReplyAction="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.QueryFailedFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryQueryFailedFaultF" +
            "ault", Name="QueryFailedFault")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.DataValidationFault), Action="http://www.clearcanvas.ca/dicom/query/IStudyRootQuery/ImageQueryDataValidationFau" +
            "ltFault", Name="DataValidationFault")]
        System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier> ImageQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier queryCriteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IStudyRootQueryChannel : ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class StudyRootQueryClient : System.ServiceModel.ClientBase<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery>, ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.IStudyRootQuery
    {
        
        public StudyRootQueryClient()
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public StudyRootQueryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public StudyRootQueryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier> StudyQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.StudyRootStudyIdentifier queryCriteria)
        {
            return base.Channel.StudyQuery(queryCriteria);
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier> SeriesQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.SeriesIdentifier queryCriteria)
        {
            return base.Channel.SeriesQuery(queryCriteria);
        }
        
        public System.ComponentModel.BindingList<ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier> ImageQuery(ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.StudyLocator.ImageIdentifier queryCriteria)
        {
            return base.Channel.ImageQuery(queryCriteria);
        }
    }
}
