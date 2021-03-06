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

using System;
using System.Runtime.Serialization;

using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.Admin;

namespace ClearCanvas.Ris.Application.Common
{
    [DataContract]
    public class PatientNoteCategorySummary : DataContractBase, ICloneable, IEquatable<PatientNoteCategorySummary>
    {
        public PatientNoteCategorySummary(EntityRef noteCategoryRef, string name, string description, EnumValueInfo severity, bool deactivated)
        {
            this.NoteCategoryRef = noteCategoryRef;
            this.Name = name;
            this.Description = description;
            this.Severity = severity;
        	this.Deactivated = deactivated;
        }

        public PatientNoteCategorySummary()
        {
        }

        [DataMember]
        public EntityRef NoteCategoryRef;

        [DataMember]
        public string Name;

        [DataMember]
        public string Description;

        [DataMember]
        public EnumValueInfo Severity;

		[DataMember]
		public bool Deactivated;


        #region ICloneable Members

        public object Clone()
        {
			return new PatientNoteCategorySummary(this.NoteCategoryRef, this.Name, this.Description, this.Severity, this.Deactivated);
        }

        #endregion

        public bool Equals(PatientNoteCategorySummary patientNoteCategorySummary)
        {
            if (patientNoteCategorySummary == null) return false;
            return Equals(NoteCategoryRef, patientNoteCategorySummary.NoteCategoryRef);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PatientNoteCategorySummary);
        }

        public override int GetHashCode()
        {
            return NoteCategoryRef.GetHashCode();
        }
    }
}
