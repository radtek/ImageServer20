using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.Admin;

namespace ClearCanvas.Ris.Application.Common.RegistrationWorkflow.OrderEntry
{
    [DataContract]
    public class GetOrderEntryFormDataResponse : DataContractBase
    {
        public GetOrderEntryFormDataResponse(
            List<DiagnosticServiceSummary> diagnosticServiceChoices,
            List<FacilitySummary> orderingFacilityChoices,
            List<StaffSummary> orderingPhysicianChoices,
            List<EnumValueInfo> orderPriorityChoices,
            List<DiagnosticServiceTreeItem> topLevelDiagnosticServiceTree)
        {
            this.DiagnosticServiceChoices = diagnosticServiceChoices;
            this.OrderingFacilityChoices = orderingFacilityChoices;
            this.OrderingPhysicianChoices = orderingPhysicianChoices;
            this.OrderPriorityChoices = orderPriorityChoices;
            this.TopLevelDiagnosticServiceTree = topLevelDiagnosticServiceTree;
        }

        [DataMember]
        public List<DiagnosticServiceSummary> DiagnosticServiceChoices;

        [DataMember]
        public List<FacilitySummary> OrderingFacilityChoices;

        [DataMember]
        public List<StaffSummary> OrderingPhysicianChoices;

        [DataMember]
        public List<EnumValueInfo> OrderPriorityChoices;

        [DataMember]
        public List<DiagnosticServiceTreeItem> TopLevelDiagnosticServiceTree;
    }
}
