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

using System.Collections;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Alert;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Healthcare.Workflow.Registration;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow.OrderEntry;
using System;
using ClearCanvas.Healthcare.Workflow;

namespace ClearCanvas.Ris.Application.Services.RegistrationWorkflow
{
    [ServiceImplementsContract(typeof(IRegistrationWorkflowService))]
    [ExtensionOf(typeof(ApplicationServiceExtensionPoint))]
    public class RegistrationWorkflowService : WorkflowServiceBase, IRegistrationWorkflowService
    {
        public RegistrationWorkflowService()
        {
            _worklistExtPoint = new RegistrationWorklistExtensionPoint();
        }

        #region IRegistrationWorkflowService Members

        [ReadOperation]
        public SearchResponse Search(SearchRequest request)
        {
            RegistrationWorkflowAssembler assembler = new RegistrationWorkflowAssembler();

            IList<WorklistItem> result = PersistenceContext.GetBroker<IRegistrationWorklistBroker>().Search(
                request.SearchData.MrnID,
                request.SearchData.HealthcardID,
                request.SearchData.FamilyName,
                request.SearchData.GivenName,
                request.SearchData.AccessionNumber,
                request.SearchData.ShowActiveOnly);

            return new SearchResponse(CollectionUtils.Map<WorklistItem, RegistrationWorklistItem, List<RegistrationWorklistItem>>(result,
                delegate(WorklistItem item)
                {
                    return assembler.CreateRegistrationWorklistItem(item, this.PersistenceContext);
                }));
        }

        [ReadOperation]
        public SearchPatientResponse SearchPatient(SearchPatientRequest request)
        {
            PatientProfileSearchCriteria criteria = new PatientProfileSearchCriteria();
            if (!String.IsNullOrEmpty(request.MrnID))
                criteria.Mrn.Id.StartsWith(request.MrnID);

            if (!String.IsNullOrEmpty(request.MrnAssigningAuthority))
                criteria.Mrn.AssigningAuthority.StartsWith(request.MrnAssigningAuthority);

            if (!String.IsNullOrEmpty(request.HealthcardID))
                criteria.Mrn.Id.StartsWith(request.HealthcardID);

            if (!String.IsNullOrEmpty(request.FamilyName))
                criteria.Name.FamilyName.StartsWith(request.FamilyName);

            if (!String.IsNullOrEmpty(request.GivenName))
                criteria.Name.GivenName.StartsWith(request.GivenName);

            if (request.Sex != null)
                criteria.Sex.EqualTo(EnumUtils.GetEnumValue<Sex>(request.Sex));

            if (request.DateOfBirth != null)
            {
                DateTime start = ((DateTime)request.DateOfBirth).Date;
                DateTime end = start + new TimeSpan(23, 59, 59);
                criteria.DateOfBirth.Between(start, end);
            }

            IList<PatientProfile> result = PersistenceContext.GetBroker<IPatientProfileBroker>().Find(criteria);

            PatientProfileAssembler assembler = new PatientProfileAssembler();
            return new SearchPatientResponse(
                CollectionUtils.Map<PatientProfile, PatientProfileSummary, List<PatientProfileSummary>>(result,
                delegate(PatientProfile profile)
                {
                    return assembler.CreatePatientProfileSummary(profile, this.PersistenceContext);
                }));
        }

        [ReadOperation]
        public LoadSearchPatientFormDataResponse LoadSearchPatientFormData(LoadSearchPatientFormDataRequest request)
        {
            return new LoadSearchPatientFormDataResponse(
                EnumUtils.GetEnumValueList<SexEnum>(PersistenceContext));
        }

        [ReadOperation]
        public ListWorklistsResponse ListWorklists(ListWorklistsRequest request)
        {
            WorklistAssembler assembler = new WorklistAssembler();
            return new ListWorklistsResponse(
                CollectionUtils.Map<Worklist, WorklistSummary, List<WorklistSummary>>(
                    this.PersistenceContext.GetBroker<IWorklistBroker>().FindAllRegistrationWorklists(this.CurrentUser),
                    delegate(Worklist worklist)
                    {
                        return assembler.GetWorklistSummary(worklist);
                    }));
        }

        [ReadOperation]
        public GetWorklistResponse GetWorklist(GetWorklistRequest request)
        {
            RegistrationWorkflowAssembler assembler = new RegistrationWorkflowAssembler();

            IList items = request.WorklistRef == null
                              ? GetWorklist(request.WorklistClassName)
                              : GetWorklist(request.WorklistRef);

            return new GetWorklistResponse(
                CollectionUtils.Map<WorklistItem, RegistrationWorklistItem, List<RegistrationWorklistItem>>(
                    items,
                    delegate(WorklistItem item)
                    {
                        return assembler.CreateRegistrationWorklistItem(item, this.PersistenceContext);
                    }));
        }

        [ReadOperation]
        public GetWorklistCountResponse GetWorklistCount(GetWorklistCountRequest request)
        {
            int count = request.WorklistRef == null
                            ? GetWorklistCount(request.WorklistClassName)
                            : GetWorklistCount(request.WorklistRef);

            return new GetWorklistCountResponse(count);
        }

        [ReadOperation]
        public LoadWorklistPreviewResponse LoadWorklistPreview(LoadWorklistPreviewRequest request)
        {
            PatientProfile profile = PersistenceContext.Load<PatientProfile>(request.WorklistItem.PatientProfileRef);
            Order order = PersistenceContext.Load<Order>(request.WorklistItem.OrderRef);

            List<IAlertNotification> alerts = new List<IAlertNotification>();
            alerts.AddRange(AlertHelper.Instance.Test(profile.Patient, this.PersistenceContext));
            alerts.AddRange(AlertHelper.Instance.Test(profile, this.PersistenceContext));
            alerts.AddRange(AlertHelper.Instance.Test(order, this.PersistenceContext));

            AlertAssembler alertAssembler = new AlertAssembler();
            List<AlertNotificationDetail> alertNotifications =
                CollectionUtils.Map<IAlertNotification, AlertNotificationDetail>(alerts,
                delegate(IAlertNotification alert)
                {
                    return alertAssembler.CreateAlertNotification(alert);
                });

            RegistrationWorkflowAssembler assembler = new RegistrationWorkflowAssembler();
            return new LoadWorklistPreviewResponse(assembler.CreateRegistrationWorklistPreview(
                request.WorklistItem,
                alertNotifications,
                this.PersistenceContext));
        }

        [ReadOperation]
        public GetOperationEnablementResponse GetOperationEnablement(GetOperationEnablementRequest request)
        {
            return new GetOperationEnablementResponse(GetOperationEnablement(new WorklistItemKey(request.OrderRef, request.PatientProfileRef)));
        }

        [ReadOperation]
        public GetDataForCheckInTableResponse GetDataForCheckInTable(GetDataForCheckInTableRequest request)
        {
            IPatientProfileBroker profileBroker = PersistenceContext.GetBroker<IPatientProfileBroker>();
            PatientProfile profile = profileBroker.Load(request.PatientProfileRef, EntityLoadFlags.Proxy);

            return new GetDataForCheckInTableResponse(
                CollectionUtils.Map<Order, CheckInTableItem, List<CheckInTableItem>>(
                    PersistenceContext.GetBroker<IRegistrationWorklistBroker>().GetOrdersForCheckIn(profile.Patient),
                    delegate(Order o)
                    {
                        CheckInTableItem item = new CheckInTableItem();
                        item.OrderRef = o.GetRef();
                        item.RequestedProcedureNames = StringUtilities.Combine(
                            CollectionUtils.Map<RequestedProcedure, string>(o.RequestedProcedures, 
                                delegate(RequestedProcedure rp) 
                                { 
                                    return rp.Type.Name; 
                                }),
                            "/");
                        item.SchedulingDate = o.SchedulingRequestDateTime;
                        item.OrderingFacility = o.OrderingFacility.Name;

                        return item;
                    }));
        }

        [ReadOperation]
        public GetDataForCancelOrderTableResponse GetDataForCancelOrderTable(GetDataForCancelOrderTableRequest request)
        {
            IPatientProfileBroker patientProfileBroker = PersistenceContext.GetBroker<IPatientProfileBroker>();
            PatientProfile profile = patientProfileBroker.Load(request.PatientProfileRef, EntityLoadFlags.Proxy);

            return new GetDataForCancelOrderTableResponse(
                CollectionUtils.Map<Order, CancelOrderTableItem, List<CancelOrderTableItem>>(
                    PersistenceContext.GetBroker<IRegistrationWorklistBroker>().GetOrdersForCancel(profile.Patient),
                    delegate(Order o)
                    {
                        CancelOrderTableItem item = new CancelOrderTableItem();
                        item.OrderRef = o.GetRef();
                        item.AccessionNumber = o.AccessionNumber;
                        item.SchedulingRequestDate = o.SchedulingRequestDateTime;
                        item.Priority = EnumUtils.GetEnumValueInfo(o.Priority, PersistenceContext);
                        item.RequestedProcedureNames = StringUtilities.Combine(
                            CollectionUtils.Map<RequestedProcedure, string>(o.RequestedProcedures,
                                delegate(RequestedProcedure rp)
                                {
                                    return rp.Type.Name;
                                }),
                            "/");

                        return item;
                    }),
                    EnumUtils.GetEnumValueList<OrderCancelReasonEnum>(PersistenceContext)
                    );
        }

        [UpdateOperation]
        [OperationEnablement("CanCheckInProcedure")]
        public CheckInProcedureResponse CheckInProcedure(CheckInProcedureRequest request)
        {
            IOrderBroker broker = PersistenceContext.GetBroker<IOrderBroker>();
            Operations.CheckIn op = new Operations.CheckIn();
            foreach (EntityRef orderRef in request.Orders)
            {
                Order o = broker.Load(orderRef, EntityLoadFlags.CheckVersion);
                op.Execute(o, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));
            }

            return new CheckInProcedureResponse();
        }

        [UpdateOperation]
        [OperationEnablement("CanCancelOrder")]
        public CancelOrderResponse CancelOrder(CancelOrderRequest request)
        {
            IOrderBroker broker = PersistenceContext.GetBroker<IOrderBroker>();
            OrderCancelReasonEnum reason = EnumUtils.GetEnumValue<OrderCancelReasonEnum>(request.CancelReason, PersistenceContext);

            CancelOrderOperation op = new CancelOrderOperation();
            foreach (EntityRef orderRef in request.CancelledOrders)
            {
                Order order = broker.Load(orderRef, EntityLoadFlags.CheckVersion);
                op.Execute(order, reason);
            }

            return new CancelOrderResponse();
        }

        #endregion

        public bool CanCheckInProcedure(IWorklistItemKey itemKey)
        {
            IPatientProfileBroker profileBroker = this.PersistenceContext.GetBroker<IPatientProfileBroker>();
            IRegistrationWorklistBroker broker = this.PersistenceContext.GetBroker<IRegistrationWorklistBroker>();

            PatientProfile profile = profileBroker.Load(((WorklistItemKey)itemKey).ProfileRef, EntityLoadFlags.Proxy);
            return broker.GetOrdersForCheckInCount(profile.Patient) > 0;
        }

        public bool CanCancelOrder(IWorklistItemKey itemKey)
        {
            IPatientProfileBroker profileBroker = this.PersistenceContext.GetBroker<IPatientProfileBroker>();
            IRegistrationWorklistBroker broker = this.PersistenceContext.GetBroker<IRegistrationWorklistBroker>();

            PatientProfile profile = profileBroker.Load(((WorklistItemKey)itemKey).ProfileRef, EntityLoadFlags.Proxy);
            return broker.GetOrdersForCancelCount(profile.Patient) > 0;
        }
    }
}
