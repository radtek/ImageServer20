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

using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Application.Services
{
    public class RequestedProcedureAssembler
    {
        public RequestedProcedureDetail CreateRequestedProcedureDetail(RequestedProcedure rp, IPersistenceContext context)
        {
            return CreateRequestedProcedureDetail(rp, true, true, context);
        }

        public RequestedProcedureDetail CreateRequestedProcedureDetail(RequestedProcedure rp, bool includeProcedureSteps, bool includeProtocol, IPersistenceContext context)
        {
            RequestedProcedureDetail detail = new RequestedProcedureDetail();

            detail.RequestedProcedureRef = rp.GetRef();
            detail.Status = EnumUtils.GetEnumValueInfo(rp.Status, context);
            detail.Type = new RequestedProcedureTypeAssembler().CreateRequestedProcedureTypeDetail(rp.Type);

            if (includeProcedureSteps)
            {
                //TODO: what about other kinds of procedure steps ??
                ModalityProcedureStepAssembler modalityProcedureStepAssembler = new ModalityProcedureStepAssembler();
                detail.ModalityProcedureSteps = CollectionUtils.Map<ModalityProcedureStep, ModalityProcedureStepDetail>(
                    rp.ModalityProcedureSteps,
                    delegate(ModalityProcedureStep mp)
                    { return modalityProcedureStepAssembler.CreateModalityProcedureStepDetail(mp, context); });
            }

            // the Protocol may be null, if this procedure has not been protocolled
            if(includeProtocol && rp.Protocol != null)
            {
                ProtocolAssembler protocolAssembler = new ProtocolAssembler();
                detail.Protocol = protocolAssembler.CreateProtocolDetail(rp.Protocol, context);
            }

            return detail;
        }

        public RequestedProcedureSummary CreateRequestedProcedureSummary(RequestedProcedure rp, IPersistenceContext context)
        {
            RequestedProcedureTypeAssembler rptAssembler = new RequestedProcedureTypeAssembler();
            RequestedProcedureSummary summary = new RequestedProcedureSummary();

            summary.OrderRef = rp.Order.GetRef();
            summary.RequestedProcedureRef = rp.GetRef();
            summary.Index = rp.Index;
            summary.ScheduledStartTime = rp.ScheduledStartTime;
            summary.PerformingFacility = new FacilityAssembler().CreateFacilitySummary(rp.PerformingFacility);
            summary.Type = rptAssembler.CreateRequestedProcedureTypeSummary(rp.Type);

            return summary;
        }
    }
}
