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

   public class PatientUpdateColumns : EntityUpdateColumns
   {
       public PatientUpdateColumns()
       : base("Patient")
       {}
        public System.String IssuerOfPatientId
        {
            set { SubParameters["IssuerOfPatientId"] = new EntityUpdateColumn<System.String>("IssuerOfPatientId", value); }
        }
        public System.Int32 NumberOfPatientRelatedInstances
        {
            set { SubParameters["NumberOfPatientRelatedInstances"] = new EntityUpdateColumn<System.Int32>("NumberOfPatientRelatedInstances", value); }
        }
        public System.Int32 NumberOfPatientRelatedSeries
        {
            set { SubParameters["NumberOfPatientRelatedSeries"] = new EntityUpdateColumn<System.Int32>("NumberOfPatientRelatedSeries", value); }
        }
        public System.Int32 NumberOfPatientRelatedStudies
        {
            set { SubParameters["NumberOfPatientRelatedStudies"] = new EntityUpdateColumn<System.Int32>("NumberOfPatientRelatedStudies", value); }
        }
        public System.String PatientId
        {
            set { SubParameters["PatientId"] = new EntityUpdateColumn<System.String>("PatientId", value); }
        }
        public System.String PatientsName
        {
            set { SubParameters["PatientsName"] = new EntityUpdateColumn<System.String>("PatientsName", value); }
        }
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey ServerPartitionKey
        {
            set { SubParameters["ServerPartitionKey"] = new EntityUpdateColumn<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("ServerPartitionKey", value); }
        }
        public System.String SpecificCharacterSet
        {
            set { SubParameters["SpecificCharacterSet"] = new EntityUpdateColumn<System.String>("SpecificCharacterSet", value); }
        }
    }
}
