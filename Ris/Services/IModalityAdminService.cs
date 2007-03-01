using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Healthcare;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Services
{
    public interface IModalityAdminService : IHealthcareServiceLayer
    {
        /// <summary>
        /// Return all modality options
        /// </summary>
        /// <returns></returns>
        IList<Modality> GetAllModalities();

        /// <summary>
        /// Add the specified modality
        /// </summary>
        /// <param name="modality"></param>
        void AddModality(Modality modality);

        /// <summary>
        /// Update the specified modality
        /// </summary>
        /// <param name="modality"></param>
        void UpdateModality(Modality modality);

        /// <summary>
        /// Loads the modality for the specified modality reference
        /// </summary>
        /// <param name="modalityRef"></param>
        /// <returns></returns>
        Modality LoadModality(EntityRef modalityRef);
    }
}
