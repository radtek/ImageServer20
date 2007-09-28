using System.Collections.Generic;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.ModalityWorkflow.TechnologistDocumentation
{
    [DataContract]
    public class CompleteModalityProcedureStepsResponse : DataContractBase
    {
        [DataMember]
        public List<RequestedProcedureDetail> RequestedProcedures;

        [DataMember]
        public EntityRef OrderRef;
    }
}