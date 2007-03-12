using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Ris.Application.Common.Admin;
using ClearCanvas.Healthcare;

namespace ClearCanvas.Ris.Application.Services.Admin
{
    class FacilityAssembler
    {
        public FacilitySummary CreateFacilitySummary(Facility facility)
        {
            FacilitySummary summary = new FacilitySummary();
            summary.Code = facility.Code;
            summary.Name = facility.Name;
            summary.FacilityRef = facility.GetRef();
            return summary;
        }
    }
}
