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
using System.Collections.Generic;
using System.ServiceModel;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Validation;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;
using ClearCanvas.Ris.Application.Common.TranscriptionWorkflow;

namespace ClearCanvas.Ris.Client.Workflow
{
	#region TranscriptionPage definitions

	/// <summary>
	/// Defines an interface for providing custom pages to be displayed in the reporting component.
	/// </summary>
	public interface ITranscriptionPageProvider : IExtensionPageProvider<ITranscriptionPage, ITranscriptionContext>
	{
	}

	/// <summary>
	/// Defines an interface to a custom reporting page.
	/// </summary>
	public interface ITranscriptionPage : IExtensionPage
	{
	}

	/// <summary>
	/// Defines an interface for providing a custom page with access to the reporting context.
	/// </summary>
	public interface ITranscriptionContext
	{
		/// <summary>
		/// Gets the reporting worklist item.
		/// </summary>
		ReportingWorklistItem WorklistItem { get; }

		/// <summary>
		/// Occurs to indicate that the <see cref="WorklistItem"/> property has changed,
		/// meaning the entire reporting context is now focused on a different report.
		/// </summary>
		event EventHandler WorklistItemChanged;

		/// <summary>
		/// Gets the report associated with the worklist item.  Modifications made to this object
		/// will not be persisted.
		/// </summary>
		ReportDetail Report { get; }

		/// <summary>
		/// Gets the index of the active report part (the part that is being edited).
		/// </summary>
		int ActiveReportPartIndex { get; }

		/// <summary>
		/// Gets the order detail associated with the report.
		/// </summary>
		OrderDetail Order { get; }
	}

	/// <summary>
	/// Defines an extension point for adding custom pages to the reporting component.
	/// </summary>
	[ExtensionPoint]
	public class TranscriptionPageProviderExtensionPoint : ExtensionPoint<ITranscriptionPageProvider>
	{
	}

	#endregion

	#region TranscriptionEditor interfaces and extension point

	/// <summary>
	/// Defines an interface for providing a custom report editor.
	/// </summary>
	public interface ITranscriptionEditorProvider
	{
		ITranscriptionEditor GetEditor(ITranscriptionEditorContext context);
	}

	/// <summary>
	/// Defines an interface for providing a custom report editor page with access to the reporting
	/// context.
	/// </summary>
	public interface ITranscriptionEditorContext : IReportEditorContextBase<TranscriptionEditorCloseReason>
	{
		/// <summary>
		/// Gets a value indicating whether the Verify operation is enabled.
		/// </summary>
		bool CanComplete { get; }

		/// <summary>
		/// Gets a value indicating whether the Save operation is enabled.
		/// </summary>
		bool CanSaveReport { get; }

		/// <summary>
		/// Gets a value indicating whether the Reject operation is enabled.
		/// </summary>
		bool CanReject { get; }
	}

	/// <summary>
	/// Defines possible reasons that the report editor is closing.
	/// </summary>
	public enum TranscriptionEditorCloseReason
	{
		/// <summary>
		/// User has cancelled editing, leaving the report in its current state.
		/// </summary>
		CancelEditing,

		/// <summary>
		/// Transcription is saved in its current state.
		/// </summary>
		SaveDraft,

		/// <summary>
		/// Transcription is saved, completed and sent to rad.
		/// </summary>
		Complete, 

		/// <summary>
		/// Transcription is saved, completed and sent to rad with the indication that it requires corrections.
		/// </summary>
		Reject
	}

	public interface ITranscriptionEditor : IReportEditorBase<TranscriptionEditorCloseReason>
	{
	}

	/// <summary>
	/// Defines an extension point for providing a custom report editor.
	/// </summary>
	[ExtensionPoint]
	public class TranscriptionEditorProviderExtensionPoint : ExtensionPoint<ITranscriptionEditorProvider>
	{
	}

	#endregion

	/// <summary>
	/// Extension point for views onto <see cref="TranscriptionComponent"/>.
	/// </summary>
	[ExtensionPoint]
	public sealed class TranscriptionComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// TranscriptionComponent class.
	/// </summary>
	[AssociateView(typeof(TranscriptionComponentViewExtensionPoint))]
	public class TranscriptionComponent : ApplicationComponent
	{
		#region TranscriptionContext

		class TranscriptionContext : ITranscriptionContext
		{
			private readonly TranscriptionComponent _owner;

			public TranscriptionContext(TranscriptionComponent owner)
			{
				_owner = owner;
			}

			public ReportingWorklistItem WorklistItem
			{
				get { return _owner.WorklistItem; }
			}

			public event EventHandler WorklistItemChanged
			{
				add { _owner._worklistItemChanged += value; }
				remove { _owner._worklistItemChanged -= value; }
			}

			public ReportDetail Report
			{
				get { return _owner._report; }
			}

			public int ActiveReportPartIndex
			{
				get { return _owner._activeReportPartIndex; }
			}

			public OrderDetail Order
			{
				get { return _owner._orderDetail; }
			}

			protected TranscriptionComponent Owner
			{
				get { return _owner; }
			}
		}

		#endregion

		#region TranscriptionEditorContext

		class TranscriptionEditorContext : TranscriptionContext, ITranscriptionEditorContext
		{
			public TranscriptionEditorContext(TranscriptionComponent owner)
				: base(owner)
			{
			}

			public bool CanComplete
			{
				get { return this.Owner.CanComplete; }
			}

			public bool CanReject
			{
				get { return this.Owner.CanReject; }
			}

			public bool CanSaveReport
			{
				get { return this.Owner.SaveReportEnabled; }
			}

			public bool IsAddendum
			{
				get { return this.Owner._activeReportPartIndex > 0; }
			}

			public string ReportContent
			{
				get { return this.Owner.ReportContent; }
				set { this.Owner.ReportContent = value; }
			}

			public Dictionary<string, string> ExtendedProperties
			{
				get { return this.Owner._reportPartExtendedProperties; }
				set { this.Owner._reportPartExtendedProperties = value; }
			}

			//public StaffSummary Supervisor
			//{
			//    get { return Owner._supervisor; }
			//    set
			//    {
			//        Owner.SetSupervisor(value);
			//    }
			//}

			public void RequestClose(TranscriptionEditorCloseReason reason)
			{
				this.Owner.RequestClose(reason);
			}
		}

		#endregion

		private readonly TranscriptionComponentWorklistItemManager _worklistItemManager;

		private ITranscriptionEditor _transcriptionEditor;
		private ChildComponentHost _transcriptionEditorHost;
		private ChildComponentHost _bannerHost;

		private ChildComponentHost _rightHandComponentContainerHost;
		private TabComponentContainer _rightHandComponentContainer;

		private bool _canComplete;
		private bool _canReject;
		private bool _canSaveReport;

		//private EntityRef _assignedStaff;
		private ReportDetail _report;
		private OrderDetail _orderDetail;
		private int _activeReportPartIndex;
		//private ILookupHandler _supervisorLookupHandler;
		//private StaffSummary _supervisor;
		private Dictionary<string, string> _reportPartExtendedProperties;

		private ReportingOrderDetailViewComponent _orderComponent;

		private event EventHandler _worklistItemChanged;

		/// <summary>
		/// Constructor.
		/// </summary>
		public TranscriptionComponent(ReportingWorklistItem worklistItem, string folderName, EntityRef worklistRef, string worklistClassName)
		{
			_worklistItemManager = new TranscriptionComponentWorklistItemManager(folderName, worklistRef, worklistClassName);
			_worklistItemManager.Initialize(worklistItem);
			_worklistItemManager.WorklistItemChanged += OnWorklistItemChangedEvent;
		}

		/// <summary>
		/// Called by the host to initialize the application component.
		/// </summary>
		public override void Start()
		{
			//// create supervisor lookup handler, using filters supplied in application settings
			//string filters = ReportingSettings.Default.SupervisorStaffTypeFilters;
			//string[] staffTypes = string.IsNullOrEmpty(filters)
			//    ? new string[] { }
			//    : CollectionUtils.Map<string, string>(filters.Split(','), delegate(string s) { return s.Trim(); }).ToArray();
			//_supervisorLookupHandler = new StaffLookupHandler(this.Host.DesktopWindow, staffTypes);

			StartTranscribingWorklistItem();

			_bannerHost = new ChildComponentHost(this.Host, new BannerComponent(this.WorklistItem));
			_bannerHost.StartComponent();

			_rightHandComponentContainer = new TabComponentContainer();
			_rightHandComponentContainer.ValidationStrategy = new AllComponentsValidationStrategy();

			_orderComponent = new ReportingOrderDetailViewComponent(this.WorklistItem.PatientRef, this.WorklistItem.OrderRef);
			_rightHandComponentContainer.Pages.Add(new TabPage("Order", _orderComponent));

			_rightHandComponentContainerHost = new ChildComponentHost(this.Host, _rightHandComponentContainer);
			_rightHandComponentContainerHost.StartComponent();

			// check for a report editor provider.  If not found, use the default one
			ITranscriptionEditorProvider provider = CollectionUtils.FirstElement<ITranscriptionEditorProvider>(
													new TranscriptionEditorProviderExtensionPoint().CreateExtensions());

			_transcriptionEditor = provider == null 
				? new TranscriptionEditorComponent(new TranscriptionEditorContext(this))
				: provider.GetEditor(new TranscriptionEditorContext(this));
			_transcriptionEditorHost = new ChildComponentHost(this.Host, _transcriptionEditor.GetComponent());
			_transcriptionEditorHost.StartComponent();

			base.Start();
		}

		/// <summary>
		/// Called by the host when the application component is being terminated.
		/// </summary>
		public override void Stop()
		{
			// TODO prepare the component to exit the live phase
			// This is a good place to do any clean up
			base.Stop();
		}

		private ReportingWorklistItem WorklistItem
		{
			get { return _worklistItemManager.WorklistItem; }
		}

		#region Presentation Model

		public ApplicationComponentHost BannerHost
		{
			get { return _bannerHost; }
		}

		public ApplicationComponentHost TranscriptionEditorHost
		{
			get { return _transcriptionEditorHost; }
		}

		public ApplicationComponentHost RightHandComponentContainerHost
		{
			get { return _rightHandComponentContainerHost; }
		}

		public string StatusText
		{
			get { return _worklistItemManager.StatusText; }
		}

		public bool StatusTextVisible
		{
			get { return _worklistItemManager.StatusTextVisible; }
		}

		public bool TranscribeNextItem
		{
			get { return _worklistItemManager.ReportNextItem; }
			set { _worklistItemManager.ReportNextItem = value; }
		}

		public bool TranscribeNextItemEnabled
		{
			get { return _worklistItemManager.ReportNextItemEnabled; }
		}

		public string ReportContent
		{
			get
			{
				if (_reportPartExtendedProperties == null || !_reportPartExtendedProperties.ContainsKey(ReportPartDetail.ReportContentKey))
					return null;

				return _reportPartExtendedProperties[ReportPartDetail.ReportContentKey];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (_reportPartExtendedProperties != null && _reportPartExtendedProperties.ContainsKey(ReportPartDetail.ReportContentKey))
					{
						_reportPartExtendedProperties.Remove(ReportPartDetail.ReportContentKey);
					}
				}
				else
				{
					if (_reportPartExtendedProperties == null)
						_reportPartExtendedProperties = new Dictionary<string, string>();

					_reportPartExtendedProperties[ReportPartDetail.ReportContentKey] = value;
				}
			}
		}

		#region Complete

		public void Complete()
		{
			try
			{
				if (this.HasValidationErrors)
				{
					this.ShowValidation(true);
					return;
				}

				if (!_transcriptionEditor.Save(TranscriptionEditorCloseReason.Complete))
					return;

				Platform.GetService<ITranscriptionWorkflowService>(
					delegate(ITranscriptionWorkflowService service)
					{
						service.CompleteTranscription(
							new CompleteTranscriptionRequest(
							this.WorklistItem.ProcedureStepRef,
							_reportPartExtendedProperties));
					});

				// Source Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.DraftFolder));
				// Destination Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.CompletedFolder));

				_worklistItemManager.ProceedToNextWorklistItem(WorklistItemCompletedResult.Completed);
			}
			catch (Exception ex)
			{
				ExceptionHandler.Report(ex, SR.ExceptionFailedToPerformOperation, this.Host.DesktopWindow,
					delegate
					{
						this.Exit(ApplicationComponentExitCode.Error);
					});
			}
		}

		public bool CompleteEnabled
		{
			get { return this.CanComplete; }
		}

		#endregion

		#region Reject

		public void Reject()
		{
			try
			{
				if (this.HasValidationErrors)
				{
					this.ShowValidation(true);
					return;
				}

				if (!_transcriptionEditor.Save(TranscriptionEditorCloseReason.Complete))
					return;

				Platform.GetService<ITranscriptionWorkflowService>(
					delegate(ITranscriptionWorkflowService service)
					{
						service.RejectTranscription(
							new RejectTranscriptionRequest(
							this.WorklistItem.ProcedureStepRef,
							_reportPartExtendedProperties, "TODO"));
					});

				// Source Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.DraftFolder));
				// Destination Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.CompletedFolder));

				_worklistItemManager.ProceedToNextWorklistItem(WorklistItemCompletedResult.Completed);
			}
			catch (Exception ex)
			{
				ExceptionHandler.Report(ex, SR.ExceptionFailedToPerformOperation, this.Host.DesktopWindow,
					delegate
					{
						this.Exit(ApplicationComponentExitCode.Error);
					});
			}
		}

		public bool RejectEnabled
		{
			get { return this.CanReject; }
		}

		#endregion

		#region Save

		public void SaveReport()
		{
			try
			{
				if (!_transcriptionEditor.Save(TranscriptionEditorCloseReason.SaveDraft))
					return;

				Platform.GetService<ITranscriptionWorkflowService>(
					delegate(ITranscriptionWorkflowService service)
					{
						service.SaveTranscription(
							new SaveTranscriptionRequest(
							this.WorklistItem.ProcedureStepRef,
							_reportPartExtendedProperties));
					});

				// Source Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.CompletedFolder));
				// Destination Folders
				DocumentManager.InvalidateFolder(typeof(Folders.Transcription.DraftFolder));

				_worklistItemManager.ProceedToNextWorklistItem(WorklistItemCompletedResult.Completed);
			}
			catch (Exception ex)
			{
				ExceptionHandler.Report(ex, SR.ExceptionFailedToSaveReport, this.Host.DesktopWindow,
					delegate
					{
						this.Exit(ApplicationComponentExitCode.Error);
					});
			}
		}

		public bool SaveReportEnabled
		{
			get { return CanSaveReport; }
		}

		#endregion

		#region Skip

		public void Skip()
		{
			try
			{
				if (_worklistItemManager.ShouldUnclaim)
				{
					Platform.GetService<ITranscriptionWorkflowService>(
						delegate(ITranscriptionWorkflowService service)
						{
							service.DiscardTranscription(new DiscardTranscriptionRequest(this.WorklistItem.ProcedureStepRef));
						});
				}

				_worklistItemManager.ProceedToNextWorklistItem(WorklistItemCompletedResult.Skipped);
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Host.DesktopWindow);
			}
		}

		public bool SkipEnabled
		{
			get { return _worklistItemManager.CanSkipItem; }
		}

		#endregion

		#region Cancel

		public void CancelEditing()
		{
			try
			{
				if (_worklistItemManager.ShouldUnclaim)
				{
					Platform.GetService<ITranscriptionWorkflowService>(
						delegate(ITranscriptionWorkflowService service)
						{
							service.DiscardTranscription(new DiscardTranscriptionRequest(this.WorklistItem.ProcedureStepRef));
						});
				}

				this.Exit(ApplicationComponentExitCode.None);
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Host.DesktopWindow);
			}
		}

		#endregion

		#endregion

		private bool CanComplete
		{
			get { return _canComplete; }
		}

		private bool CanReject
		{
			get { return _canReject; }
		}

		private bool CanSaveReport
		{
			get { return _canSaveReport; }
		}

		private void RequestClose(TranscriptionEditorCloseReason reason)
		{
			switch (reason)
			{
				case TranscriptionEditorCloseReason.SaveDraft:
					SaveReport();
					break;
				case TranscriptionEditorCloseReason.Complete:
					Complete();
					break;
				case TranscriptionEditorCloseReason.Reject:
					Reject();
					break;
				case TranscriptionEditorCloseReason.CancelEditing:
					CancelEditing();
					break;
			}
		}

		private void OnWorklistItemChangedEvent(object sender, EventArgs args)
		{
			if (this.WorklistItem != null)
			{
				try
				{
					StartTranscribingWorklistItem();
					UpdateChildComponents();
					// notify extension pages that the worklist item has changed
					EventsHelper.Fire(_worklistItemChanged, this, EventArgs.Empty);
				}
				catch (FaultException<ConcurrentModificationException>)
				{
					this._worklistItemManager.ProceedToNextWorklistItem(WorklistItemCompletedResult.Invalid);
				}
			}
			else
			{
				this.Exit(ApplicationComponentExitCode.Accepted);
			}
		}

		private void StartTranscribingWorklistItem()
		{
			ClaimWorklistItem(this.WorklistItem);

			Platform.GetService<ITranscriptionWorkflowService>(
				delegate(ITranscriptionWorkflowService service)
				{
					GetOperationEnablementResponse enablementResponse = service.GetOperationEnablement(new GetOperationEnablementRequest(this.WorklistItem));
					_canComplete = enablementResponse.OperationEnablementDictionary["CompleteTranscription"];
					_canReject = enablementResponse.OperationEnablementDictionary["RejectTranscription"];
					_canSaveReport = enablementResponse.OperationEnablementDictionary["SaveTranscription"];

					LoadTranscriptionForEditResponse response = service.LoadTranscriptionForEdit(
						new LoadTranscriptionForEditRequest(this.WorklistItem.ProcedureStepRef));
					_report = response.Report;
					_activeReportPartIndex = response.ReportPartIndex;
					_orderDetail = response.Order;

					ReportPartDetail activePart = _report.GetPart(_activeReportPartIndex);
					_reportPartExtendedProperties = activePart == null ? null : activePart.ExtendedProperties;
					//if (activePart != null && activePart.Supervisor != null)
					//{
					//    // active part already has a supervisor assigned
					//    _supervisor = activePart.Supervisor;
					//}
					//else if (Thread.CurrentPrincipal.IsInRole(ClearCanvas.Ris.Application.Common.AuthorityTokens.Workflow.Protocol.SubmitForReview))
					//{
					//    // active part does not have a supervisor assigned
					//    // if this user has a default supervisor, retreive it, otherwise leave supervisor as null
					//    if (!String.IsNullOrEmpty(ReportingSettings.Default.SupervisorID))
					//    {
					//        object supervisor;
					//        if (_supervisorLookupHandler.Resolve(ReportingSettings.Default.SupervisorID, false, out supervisor))
					//        {
					//            _supervisor = (StaffSummary)supervisor;
					//        }
					//    }
					//}
				});
		}

		private void ClaimWorklistItem(ReportingWorklistItem item)
		{
			if (item.ActivityStatus.Code == StepState.Scheduled)
			{
				// start the interpretation step
				// note: updating only the ProcedureStepRef is hacky - the service should return an updated item
				StartTranscriptionResponse response = null;
				Platform.GetService<ITranscriptionWorkflowService>(
					delegate(ITranscriptionWorkflowService service)
					{
						response = service.StartTranscription(new StartTranscriptionRequest(item.ProcedureStepRef));
					});

				item.ProcedureStepRef = response.TranscriptionStepRef;
			}
		}
		private void UpdateChildComponents()
		{
			((BannerComponent)_bannerHost.Component).HealthcareContext = this.WorklistItem;
			_orderComponent.Context = new ReportingOrderDetailViewComponent.PatientOrderContext(this.WorklistItem.PatientRef, this.WorklistItem.OrderRef);

			this.Host.Title = ReportDocument.GetTitle(this.WorklistItem);

			NotifyPropertyChanged("StatusText");
		}
	}
}
