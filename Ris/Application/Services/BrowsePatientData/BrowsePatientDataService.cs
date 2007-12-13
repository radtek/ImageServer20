#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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

using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Alert;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.BrowsePatientData;
using ClearCanvas.Ris.Application.Common.ModalityWorkflow;

namespace ClearCanvas.Ris.Application.Services.BrowsePatientData
{
    [ServiceImplementsContract(typeof(IBrowsePatientDataService))]
    [ExtensionOf(typeof(ApplicationServiceExtensionPoint))]
    public class BrowsePatientDataService : ApplicationServiceBase, IBrowsePatientDataService
    {
        #region IBrowsePatientDataService Members

        [ReadOperation]
        public GetDataResponse GetData(GetDataRequest request)
        {
            GetDataResponse response = new GetDataResponse();

            if (request.ListPatientProfilesRequest != null)
                response.ListPatientProfilesResponse = ListPatientProfiles(request.PatientRef, request.ListPatientProfilesRequest);

            if (request.GetPatientProfileDetailRequest != null)
                response.GetPatientProfileDetailResponse = GetPatientProfileDetail(request.PatientProfileRef, request.GetPatientProfileDetailRequest);

            if (request.ListOrdersRequest != null)
                response.ListOrdersResponse = ListOrders(request.PatientRef, request.ListOrdersRequest);

            if (request.GetOrderDetailRequest != null)
                response.GetOrderDetailResponse = GetOrderDetail(request.OrderRef, request.GetOrderDetailRequest);

            if (request.ListReportsRequest != null)
                response.ListReportsResponse = ListReports(request.PatientRef, request.ListReportsRequest);

            if (request.GetReportDetailRequest != null)
                response.GetReportDetailResponse = GetReportDetail(request.ReportRef, request.GetReportDetailRequest);

            return response;
        }

        #endregion


        private ListPatientProfilesResponse ListPatientProfiles(EntityRef patientRef, ListPatientProfilesRequest request)
        {
            Patient patient = PersistenceContext.Load<Patient>(patientRef);

            PatientProfileAssembler assembler = new PatientProfileAssembler();
            return new ListPatientProfilesResponse(
                CollectionUtils.Map<PatientProfile, PatientProfileSummary>(
                patient.Profiles,
                delegate(PatientProfile profile)
                {
                    return assembler.CreatePatientProfileSummary(profile, PersistenceContext);
                }));
        }

        private GetPatientProfileDetailResponse GetPatientProfileDetail(EntityRef profileRef, GetPatientProfileDetailRequest request)
        {
            PatientProfile profile = PersistenceContext.Load<PatientProfile>(profileRef);

            PatientProfileAssembler assembler = new PatientProfileAssembler();
            GetPatientProfileDetailResponse response = new GetPatientProfileDetailResponse();
            response.PatientProfile = assembler.CreatePatientProfileDetail(profile, this.PersistenceContext,
                                                                                 request.IncludeAddresses,
                                                                                 request.IncludeContactPersons,
                                                                                 request.IncludeEmailAddresses,
                                                                                 request.IncludeTelephoneNumbers,
                                                                                 request.IncludeNotes,
                                                                                 request.IncludeAttachments);
            if (request.IncludeAlerts)
            {
                List<IAlertNotification> alerts = new List<IAlertNotification>();
                alerts.AddRange(AlertHelper.Instance.Test(profile.Patient, this.PersistenceContext));
                alerts.AddRange(AlertHelper.Instance.Test(profile, this.PersistenceContext));

                AlertAssembler alertAssembler = new AlertAssembler();
                response.PatientAlerts =
                    CollectionUtils.Map<IAlertNotification, AlertNotificationDetail>(alerts,
                    delegate(IAlertNotification alert)
                    {
                        return alertAssembler.CreateAlertNotification(alert);
                    });
            }

            return response;
        }

        private ListOrdersResponse ListOrders(EntityRef patientRef, ListOrdersRequest request)
        {
            BrowsePatientDataAssembler assembler = new BrowsePatientDataAssembler();

            Patient patient = PersistenceContext.Load<Patient>(patientRef, EntityLoadFlags.Proxy);

            if (request.QueryDetailLevel == PatientOrdersQueryDetailLevel.Order)
            {
                return new ListOrdersResponse(
                    CollectionUtils.Map<Order, PatientOrderData>(
                        PersistenceContext.GetBroker<IPreviewBroker>().QueryOrderData(patient),
                        delegate(Order order)
                        {
                            return assembler.CreatePatientOrderData(order, this.PersistenceContext);
                        }));
            }
            else if (request.QueryDetailLevel == PatientOrdersQueryDetailLevel.RequestedProcedure)
            {
                return new ListOrdersResponse(
                    CollectionUtils.Map<RequestedProcedure, PatientOrderData>(
                        PersistenceContext.GetBroker<IPreviewBroker>().QueryRequestedProcedureData(patient),
                        delegate(RequestedProcedure rp)
                        {
                            return assembler.CreatePatientOrderData(rp, this.PersistenceContext);
                        }));
            }

            return new ListOrdersResponse(new List<PatientOrderData>());
        }

        private GetOrderDetailResponse GetOrderDetail(EntityRef orderRef, GetOrderDetailRequest request)
        {
            Order order = PersistenceContext.GetBroker<IOrderBroker>().Load(orderRef);

            GetOrderDetailResponse response = new GetOrderDetailResponse();
            OrderAssembler assembler = new OrderAssembler();
            response.Order = assembler.CreateOrderDetail(order,
                this.PersistenceContext,
                request.IncludeVisit,
                request.IncludeProcedures,
                true);

            if (request.IncludeAlerts)
            {
                List<IAlertNotification> alerts = new List<IAlertNotification>();

                AlertAssembler alertAssembler = new AlertAssembler();
                response.OrderAlerts =
                    CollectionUtils.Map<IAlertNotification, AlertNotificationDetail>(
                    AlertHelper.Instance.Test(order, this.PersistenceContext),
                    delegate(IAlertNotification alert)
                    {
                        return alertAssembler.CreateAlertNotification(alert);
                    });
            }

            return response;
        }

        private ListReportsResponse ListReports(EntityRef entityRef, ListReportsRequest listReportsRequest)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        private GetReportDetailResponse GetReportDetail(EntityRef reportRef, GetReportDetailRequest request)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }
}
