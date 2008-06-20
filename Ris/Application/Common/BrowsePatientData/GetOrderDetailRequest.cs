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

using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;
using System.Collections.Generic;

namespace ClearCanvas.Ris.Application.Common.BrowsePatientData
{
	[DataContract]
	public class GetOrderDetailRequest : DataContractBase
	{
		public GetOrderDetailRequest(EntityRef orderRef,
			bool includeVisit,
			bool includeProcedures,
			bool includeAlerts,
			bool includeNotes,
			bool includeAttachments,
			bool includeResultRecipients)
		{
			this.OrderRef = orderRef;
			this.IncludeVisit = includeVisit;
			this.IncludeProcedures = includeProcedures;
			this.IncludeAlerts = includeAlerts;
			this.IncludeNotes = includeNotes;
			this.IncludeAttachments = includeAttachments;
			this.includeResultRecipients = includeResultRecipients;
		}

		public GetOrderDetailRequest()
		{
		}

		[DataMember]
		public EntityRef OrderRef;

		/// <summary>
		/// Include order alerts.
		/// </summary>
		[DataMember]
		public bool IncludeAlerts;

		/// <summary>
		/// Include visit information.
		/// </summary>
		[DataMember]
		public bool IncludeVisit;

		/// <summary>
		/// Include detailed procedure information.
		/// </summary>
		[DataMember]
		public bool IncludeProcedures;

		/// <summary>
		/// Include order notes.
		/// </summary>
		[DataMember]
		public bool IncludeNotes;

		/// <summary>
		/// Include order attachments.
		/// </summary>
		[DataMember]
		public bool IncludeAttachments;

		/// <summary>
		/// Include order result recipients.
		/// </summary>
		[DataMember]
		public bool includeResultRecipients;

		/// <summary>
		/// A list of filters that determine which categories of order notes are returned. Optional, defaults to all.
		/// Ignored if <see cref="IncludeNotes"/> is false.
		/// </summary>
		[DataMember]
		public List<string> NoteCategoriesFilter;
	}
}
