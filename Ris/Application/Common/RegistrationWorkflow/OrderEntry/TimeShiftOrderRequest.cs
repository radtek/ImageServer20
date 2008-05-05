using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Enterprise.Common;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.RegistrationWorkflow.OrderEntry
{
	[DataContract]
	public class TimeShiftOrderRequest : DataContractBase
	{
		public TimeShiftOrderRequest(EntityRef orderRef, int numberOfDays)
		{
			OrderRef = orderRef;
			NumberOfDays = numberOfDays;
		}

		/// <summary>
		/// Specifies the order to shift.
		/// </summary>
		[DataMember]
		public EntityRef OrderRef;

		/// <summary>
		/// Specifies the number of days by which to shift the order in time - may be positive or negative.
		/// </summary>
		[DataMember]
		public int NumberOfDays;

	}
}
