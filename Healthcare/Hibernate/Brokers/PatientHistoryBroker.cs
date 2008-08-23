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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Enterprise.Hibernate;
using ClearCanvas.Healthcare.Brokers;
using NHibernate;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Healthcare.Hibernate.Brokers
{
    [ExtensionOf(typeof(BrokerExtensionPoint))]
    public class PatientHistoryBroker : Broker, IPatientHistoryBroker
    {
    	/// <summary>
    	/// Obtains the set of all orders for the specified patient.
    	/// </summary>
    	/// <param name="patient"></param>
    	/// <returns></returns>
    	public IList<Order> GetOrderHistory(Patient patient)
        {
			NHibernate.IQuery q = this.Context.GetNamedHqlQuery("orderHistory");
			q.SetParameter(0, patient);

			// uniquefy the results in case fetch joins added additional lines
			return CollectionUtils.Unique(q.List<Order>());
        }

    	/// <summary>
    	/// Obtains the set of all procedures for the specified patient.
    	/// </summary>
    	/// <param name="patient"></param>
    	/// <returns></returns>
    	public IList<Procedure> GetProcedureHistory(Patient patient)
        {
			NHibernate.IQuery q = this.Context.GetNamedHqlQuery("procedureHistory");
			q.SetParameter(0, patient);

			// uniquefy the results in case fetch joins added additional lines
			return CollectionUtils.Unique(q.List<Procedure>());
		}

		/// <summary>
		/// Obtains the set of all reports for the specified patient.
		/// </summary>
		/// <param name="patient"></param>
		/// <returns></returns>
		public IList<Report> GetReportHistory(Patient patient)
		{
			NHibernate.IQuery q = this.Context.GetNamedHqlQuery("reportHistory");
			q.SetParameter(0, patient);

			// uniquefy the results in case fetch joins added additional lines
			return CollectionUtils.Unique(q.List<Report>());
		}

		/// <summary>
		/// Obtains the set of all reports for the specified order.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public IList<Report> GetReportsForOrder(Order order)
		{
			NHibernate.IQuery q = this.Context.GetNamedHqlQuery("reportsForOrder");
			q.SetParameter(0, order);

			// uniquefy the results in case fetch joins added additional lines
			return CollectionUtils.Unique(q.List<Report>());
		}
	}

}