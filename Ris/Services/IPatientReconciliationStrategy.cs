using System;
using System.Collections.Generic;

using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;

namespace ClearCanvas.Ris.Services
{
    public interface IPatientReconciliationStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        IList<PatientProfileMatch> FindReconciliationMatches(PatientProfile patient, IPatientProfileBroker broker);
    }
}
