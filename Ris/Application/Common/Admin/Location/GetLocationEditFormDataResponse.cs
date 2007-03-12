using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.Admin.Location
{
    [DataContract]
    public class GetLocationEditFormDataResponse : DataContractBase
    {
        public GetLocationEditFormDataResponse(List<FacilitySummary> facilityChoices)
        {
            this.FacilityChoices = facilityChoices;
        }

        [DataMember]
        public List<FacilitySummary> FacilityChoices;
    }


}
