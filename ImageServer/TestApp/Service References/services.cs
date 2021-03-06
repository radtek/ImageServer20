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
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageServer.TestApp.services
{
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="HeaderStreamingParameters", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
    [System.SerializableAttribute()]
    public partial class HeaderStreamingParameters : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ReferenceIDField;
        
        private string ServerAETitleField;
        
        private string StudyInstanceUIDField;
        
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
        public string ReferenceID
        {
            get
            {
                return this.ReferenceIDField;
            }
            set
            {
                this.ReferenceIDField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string ServerAETitle
        {
            get
            {
                return this.ServerAETitleField;
            }
            set
            {
                this.ServerAETitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string StudyInstanceUID
        {
            get
            {
                return this.StudyInstanceUIDField;
            }
            set
            {
                this.StudyInstanceUIDField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudyNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
    [System.SerializableAttribute()]
    public partial class StudyNotFoundFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
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
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudyIsInUseFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
    [System.SerializableAttribute()]
    public partial class StudyIsInUseFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StudyStateField;
        
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
        public string StudyState
        {
            get
            {
                return this.StudyStateField;
            }
            set
            {
                this.StudyStateField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudyIsNearlineFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
    [System.SerializableAttribute()]
    public partial class StudyIsNearlineFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ClearCanvas.ImageServer.TestApp.services.IHeaderStreamingService")]
    public interface IHeaderStreamingService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHeaderStreamingService/GetStudyHeader", ReplyAction="http://tempuri.org/IHeaderStreamingService/GetStudyHeaderResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageServer.TestApp.services.StudyNotFoundFault), Action="http://tempuri.org/IHeaderStreamingService/GetStudyHeaderStudyNotFoundFaultFault", Name="StudyNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageServer.TestApp.services.StudyIsInUseFault), Action="http://tempuri.org/IHeaderStreamingService/GetStudyHeaderStudyIsInUseFaultFault", Name="StudyIsInUseFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClearCanvas.ImageServer.TestApp.services.StudyIsNearlineFault), Action="http://tempuri.org/IHeaderStreamingService/GetStudyHeaderStudyIsNearlineFaultFaul" +
            "t", Name="StudyIsNearlineFault", Namespace="http://schemas.datacontract.org/2004/07/ClearCanvas.Dicom.ServiceModel.Streaming")]
        System.IO.Stream GetStudyHeader(string callingAETitle, ClearCanvas.ImageServer.TestApp.services.HeaderStreamingParameters parameters);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IHeaderStreamingServiceChannel : ClearCanvas.ImageServer.TestApp.services.IHeaderStreamingService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class HeaderStreamingServiceClient : System.ServiceModel.ClientBase<ClearCanvas.ImageServer.TestApp.services.IHeaderStreamingService>, ClearCanvas.ImageServer.TestApp.services.IHeaderStreamingService
    {
        
        public HeaderStreamingServiceClient()
        {
        }
        
        public HeaderStreamingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public HeaderStreamingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public HeaderStreamingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public HeaderStreamingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.IO.Stream GetStudyHeader(string callingAETitle, ClearCanvas.ImageServer.TestApp.services.HeaderStreamingParameters parameters)
        {
            return base.Channel.GetStudyHeader(callingAETitle, parameters);
        }
    }
}
