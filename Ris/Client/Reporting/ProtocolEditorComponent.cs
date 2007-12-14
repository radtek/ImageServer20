using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ProtocollingWorkflow;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;

namespace ClearCanvas.Ris.Client.Reporting
{
    public enum ClaimProtocolResult
    {
        AlreadyClaimed,
        Claimed,
        Failed
    }

    /// <summary>
    /// Extension point for views onto <see cref="ProtocolEditorComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class ProtocolEditorComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// ProtocolEditorComponent class
    /// </summary>
    [AssociateView(typeof(ProtocolEditorComponentViewExtensionPoint))]
    public class ProtocolEditorComponent : ApplicationComponent
    {
        #region Private Fields

        private ReportingWorklistItem _worklistItem;
        private EntityRef _orderRef;

        private readonly ProtocolEditorProcedurePlanSummaryTable _procedurePlanSummaryTable;
        private ProtocolEditorProcedurePlanSummaryTableItem _selectedProcodurePlanSummaryTableItem;
        private event EventHandler _selectedProcedurePlanSummaryTableItemChanged;

        private readonly ProtocolCodeTable _availableProtocolCodes;
        private readonly ProtocolCodeTable _selectedProtocolCodes;

        private bool _acceptEnabled;
        private bool _rejectEnabled;
        private bool _suspendEnabled;
        private bool _saveEnabled;

        private List<ProtocolGroupSummary> _protocolGroupChoices;
        private ProtocolGroupSummary _protocolGroup;

        private bool _protocolNextItem = true;
        private readonly ProtocollingComponentMode _componentMode;
        private ProtocollingComponent _owner;
        private ApplicationComponentHost _orderNotesComponentHost;

        private event EventHandler _protocolAccepted;
        private event EventHandler _protocolRejected;
        private event EventHandler _protocolSuspended;
        private event EventHandler _protocolSaved;
        private event EventHandler _protocolSkipped;
        private event EventHandler _protocolCancelled;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ProtocolEditorComponent(ProtocollingComponent owner, ReportingWorklistItem worklistItem, ProtocollingComponentMode mode)
        {
            _owner = owner;
            _owner.WorklistItemChanged += OnWorklistItemChanged;
            _worklistItem = worklistItem;
            _componentMode = mode;

            _availableProtocolCodes = new ProtocolCodeTable();
            _selectedProtocolCodes = new ProtocolCodeTable();
            _selectedProtocolCodes.Items.ItemsChanged += SelectedProtocolCodesChanged;
            _protocolGroupChoices = new List<ProtocolGroupSummary>();
            _procedurePlanSummaryTable = new ProtocolEditorProcedurePlanSummaryTable();
        }

        ~ProtocolEditorComponent()
        {
            _owner.WorklistItemChanged -= OnWorklistItemChanged;
        }

        private void OnWorklistItemChanged(object sender, EventArgs e)
        {
            _worklistItem = _owner.WorklistItem;
            // TODO: Refresh order notes
            StartWorklistItem();
        }

        #region ApplicationComponent overrides

        public override void Start()
        {
            _orderNotesComponentHost = new ChildComponentHost(this.Host, new OrderNoteSummaryComponent());
            _orderNotesComponentHost.StartComponent();

            StartWorklistItem();
            base.Start();
        }

        private void StartWorklistItem()
        {
            Platform.GetService<IProtocollingWorkflowService>(
                delegate(IProtocollingWorkflowService service)
                    {
                        ClaimProtocolResult claimResult = ClaimProtocolResult.AlreadyClaimed;

                        // Only claim unassigned protocols
                        if (_componentMode == ProtocollingComponentMode.Assign)
                        {
                            claimResult = ClaimOrderProtocol(_worklistItem.OrderRef, service);
                        }

                        if(claimResult != ClaimProtocolResult.Failed)
                        {
                            InitializeProcedurePlanSummary(service);
                            InitializeActionEnablement(service);
                        }
                        else
                        {
                            EventsHelper.Fire(_protocolSkipped, this, EventArgs.Empty);
                        }
                    });
        }


        private ClaimProtocolResult ClaimOrderProtocol(EntityRef orderRef, IProtocollingWorkflowService service)
        {
            try
            {
                StartOrderProtocolResponse response = service.StartOrderProtocol(new StartOrderProtocolRequest(orderRef));
                return response.ProtocolClaimed ? ClaimProtocolResult.Claimed : ClaimProtocolResult.AlreadyClaimed;
            }
            catch(Exception)
            {
                return ClaimProtocolResult.Failed;
            }
        }

        #endregion

        #region Presentation Model

        public ApplicationComponentHost OrderNotesComponentHost
        {
            get { return _orderNotesComponentHost; }
        }

        public IList<string> ProtocolGroupChoices
        {
            get
            {
                return CollectionUtils.Map<ProtocolGroupSummary, string>(
                    _protocolGroupChoices,
                    delegate(ProtocolGroupSummary summary) { return summary.Name; });
            }
        }

        public string ProtocolGroup
        {
            get { return _protocolGroup == null ? "" : _protocolGroup.Name; }
            set
            {
                _protocolGroup = (value == null) 
                    ? null 
                    : CollectionUtils.SelectFirst<ProtocolGroupSummary>(
                        _protocolGroupChoices, 
                        delegate(ProtocolGroupSummary summary) { return summary.Name == value; });

                ProtocolGroupSelectionChanged();
            }
        }

        public ITable AvailableProtocolCodesTable
        {
            get { return _availableProtocolCodes; }
        }

        public ITable SelectedProtocolCodesTable
        {
            get { return _selectedProtocolCodes; }
        }

        public ITable ProcedurePlanSummaryTable
        {
            get { return _procedurePlanSummaryTable; }
        }

        public ISelection SelectedRequestedProcedure
        {
            get { return new Selection(_selectedProcodurePlanSummaryTableItem); }
            set
            {
                ProtocolEditorProcedurePlanSummaryTableItem item = (ProtocolEditorProcedurePlanSummaryTableItem)value.Item;
                ProcedureSelectionChanged(item);
            }
        }

        public event EventHandler SelectionChanged
        {
            add { _selectedProcedurePlanSummaryTableItemChanged += value; }
            remove { _selectedProcedurePlanSummaryTableItemChanged -= value; }
        }

        /// <summary>
        /// Specifies to containing <see cref="ProtocollingComponentDocument"/> if the next <see cref="ReportingWorklistItem"/> should be protocolled
        /// </summary>
        public bool ProtocolNextItem
        {
            get { return _protocolNextItem; }
            set { _protocolNextItem = value; }
        }

        public bool ProtocolNextItemEnabled
        {
            get { return _componentMode == ProtocollingComponentMode.Assign; }
        }

        #region Accept

        public void Accept()
        {
            try
            {
                Platform.GetService<IProtocollingWorkflowService>(
                    delegate(IProtocollingWorkflowService service)
                        {
                            SaveProtocols(service);
                            service.AcceptOrderProtocol(new AcceptOrderProtocolRequest(_orderRef));
                        });

                EventsHelper.Fire(_protocolAccepted, this, EventArgs.Empty);
            }
            catch(Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool AcceptEnabled
        {
            get { return _acceptEnabled; }
        }

        public event EventHandler ProtocolAccepted
        {
            add { _protocolAccepted += value; }
            remove { _protocolAccepted -= value; }
        }

        #endregion

        #region Reject

        public void Reject()
        {
            try
            {
                EnumValueInfo reason = GetRejectOrSuspendReason("Reject Reason");

                if (reason == null)
                    return;

                Platform.GetService<IProtocollingWorkflowService>(
                    delegate(IProtocollingWorkflowService service)
                        {
                            SaveProtocols(service);
                            service.RejectOrderProtocol(new RejectOrderProtocolRequest(_orderRef, reason));
                        });

                EventsHelper.Fire(_protocolRejected, this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool RejectEnabled
        {
            get { return _rejectEnabled; }
        }

        public event EventHandler ProtocolRejected
        {
            add { _protocolRejected += value; }
            remove { _protocolRejected -= value; }
        }

        #endregion

        #region Suspend

        public void Suspend()
        {
            try
            {
                EnumValueInfo reason = GetRejectOrSuspendReason("Suspend Reason");

                if(reason == null)
                    return;

                Platform.GetService<IProtocollingWorkflowService>(
                    delegate(IProtocollingWorkflowService service)
                        {
                            SaveProtocols(service);
                            service.SuspendOrderProtocol(new SuspendOrderProtocolRequest(_orderRef, reason));
                        });

                EventsHelper.Fire(_protocolSuspended, this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool SuspendEnabled
        {
            get { return _suspendEnabled;  }
        }

        public event EventHandler ProtocolSuspended
        {
            add { _protocolSuspended += value; }
            remove { _protocolSuspended -= value; }
        }

        #endregion

        #region Save

        public void Save()
        {
            try
            {
                Platform.GetService<IProtocollingWorkflowService>(
                    delegate(IProtocollingWorkflowService service)
                        {
                            SaveProtocols(service);
                        });

                EventsHelper.Fire(_protocolSaved, this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool SaveEnabled
        {
            get { return _saveEnabled; }
        }

        public event EventHandler ProtocolSaved
        {
            add { _protocolSaved += value; }
            remove { _protocolSaved -= value; }
        }

        #endregion

        #region Skip

        public void Skip()
        {
            try
            {
                // Unclaim any newly started protocols
                if (_componentMode == ProtocollingComponentMode.Assign)
                {
                    Platform.GetService<IProtocollingWorkflowService>(
                        delegate(IProtocollingWorkflowService service)
                        {
                            service.DiscardOrderProtocol(new DiscardOrderProtocolRequest(_orderRef));
                        });
                }

                EventsHelper.Fire(_protocolSkipped, this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool SkipEnabled
        {
            get { return _protocolNextItem && this.ProtocolNextItemEnabled; }
        }

        public event EventHandler ProtocolSkipped
        {
            add { _protocolSkipped += value; }
            remove { _protocolSkipped -= value; }
        }

        #endregion

        #region Close

        public void Close()
        {
            try
            {
                // Unclaim any newly started protocols
                if (_componentMode == ProtocollingComponentMode.Assign)
                {
                    Platform.GetService<IProtocollingWorkflowService>(
                        delegate(IProtocollingWorkflowService service)
                            {
                                service.DiscardOrderProtocol(new DiscardOrderProtocolRequest(_orderRef));
                            });
                }

                EventsHelper.Fire(_protocolCancelled, this, EventArgs.Empty);
            }
            catch(Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public event EventHandler ProtocolCancelled
        {
            add { _protocolCancelled += value; }
            remove { _protocolCancelled -= value; }
        }

        #endregion

        #endregion

        #region Private Methods

        private EnumValueInfo GetRejectOrSuspendReason(string title)
        {
            ProtocolReasonComponent component = new ProtocolReasonComponent();

            ApplicationComponentExitCode exitCode = LaunchAsDialog(this.Host.DesktopWindow, component, title);

            if (exitCode == ApplicationComponentExitCode.Accepted)
            {
                return component.Reason;
            }

            return null;
        }

        private void InitializeProcedurePlanSummary(IProtocollingWorkflowService service)
        {
            GetProcedurePlanForProtocollingWorklistItemRequest procedurePlanRequest = new GetProcedurePlanForProtocollingWorklistItemRequest(_worklistItem.ProcedureStepRef);
            GetProcedurePlanForProtocollingWorklistItemResponse procedurePlanResponse = service.GetProcedurePlanForProtocollingWorklistItem(procedurePlanRequest);

            if (procedurePlanResponse.ProcedurePlanSummary != null)
            {
                _orderRef = procedurePlanResponse.ProcedurePlanSummary.OrderRef;

                _procedurePlanSummaryTable.Items.Clear();

                foreach (RequestedProcedureDetail rp in procedurePlanResponse.ProcedurePlanSummary.RequestedProcedures)
                {
                    GetProcedureProtocolRequest protocolRequest = new GetProcedureProtocolRequest(rp.RequestedProcedureRef);
                    GetProcedureProtocolResponse protocolResponse = service.GetProcedureProtocol(protocolRequest);

                    if (protocolResponse.ProtocolRef != null)
                    {
                        _procedurePlanSummaryTable.Items.Add(new ProtocolEditorProcedurePlanSummaryTableItem(rp, protocolResponse.ProtocolRef, protocolResponse.ProtocolDetail));
                    }
                }
                _procedurePlanSummaryTable.Sort();
            }
        }

        private void InitializeActionEnablement(IProtocollingWorkflowService service)
        {
            GetProtocolOperationEnablementResponse response = service.GetProtocolOperationEnablement(new GetProtocolOperationEnablementRequest(_worklistItem.ProcedureStepRef));

            _acceptEnabled = response.AcceptEnabled;
            _suspendEnabled = response.SuspendEnabled;
            _rejectEnabled = response.RejectEnabled;
            _saveEnabled = response.SaveEnabled;
        }

        private void SelectedProtocolCodesChanged(object sender, ItemChangedEventArgs e)
        {
            ProtocolCodeDetail detail = (ProtocolCodeDetail) e.Item;
            switch(e.ChangeType)
            {
                case ItemChangeType.ItemAdded:
                    _selectedProcodurePlanSummaryTableItem.ProtocolDetail.Codes.Add(detail);
                    break;
                case ItemChangeType.ItemRemoved:
                    _selectedProcodurePlanSummaryTableItem.ProtocolDetail.Codes.Remove(detail);
                    break;
                default:
                    return;
            }

            this.Modified = true;
        }

        private void ProcedureSelectionChanged(ProtocolEditorProcedurePlanSummaryTableItem item)
        {
            // Same selection, do nothing
            if(item == _selectedProcodurePlanSummaryTableItem)
            {
                return;
            }

            ResetDocument();

            // Ensure something is selected
            if (item != null)
            {
                //Refresh protocol
                try
                {
                    Platform.GetService<IProtocollingWorkflowService>(
                        delegate(IProtocollingWorkflowService service)
                            {
                                // Load available protocol groups
                                ListProtocolGroupsForProcedureRequest request = new ListProtocolGroupsForProcedureRequest(item.RequestedProcedureDetail.RequestedProcedureRef);
                                ListProtocolGroupsForProcedureResponse response = service.ListProtocolGroupsForProcedure(request);

                                _protocolGroupChoices = response.ProtocolGroups;
                                _protocolGroup = response.InitialProtocolGroup;

                                RefreshAvailableProtocolCodes(item.ProtocolDetail.Codes, service);

                                // fill out selected item codes
                                _selectedProtocolCodes.Items.Clear();
                                _selectedProtocolCodes.Items.AddRange(item.ProtocolDetail.Codes);
                            });
                }
                catch (Exception e)
                {
                    ExceptionHandler.Report(e, this.Host.DesktopWindow);
                }
            }

            _selectedProcodurePlanSummaryTableItem = item;
            EventsHelper.Fire(_selectedProcedurePlanSummaryTableItemChanged, this, EventArgs.Empty);

            NotifyPropertyChanged("ProtocolGroupChoices");
        }

        private void ProtocolGroupSelectionChanged()
        {
            try
            {
                Platform.GetService<IProtocollingWorkflowService>(
                    delegate(IProtocollingWorkflowService service)
                        {
                            RefreshAvailableProtocolCodes(_selectedProcodurePlanSummaryTableItem.ProtocolDetail.Codes, service);
                        });
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        // Refresh the list of available protocol codes when the list of protocol groups is initially loaded 
        // and whenever the protocol group selection changes
        private void RefreshAvailableProtocolCodes(IEnumerable<ProtocolCodeDetail> existingSelectedCodes, IProtocollingWorkflowService service)
        {
            _availableProtocolCodes.Items.Clear();

            if (_protocolGroup != null)
            {
                GetProtocolGroupDetailRequest protocolCodesDetailRequest = new GetProtocolGroupDetailRequest(_protocolGroup);
                GetProtocolGroupDetailResponse protocolCodesDetailResponse = service.GetProtocolGroupDetail(protocolCodesDetailRequest);

                _availableProtocolCodes.Items.AddRange(protocolCodesDetailResponse.ProtocolGroup.Codes);

                // Make existing code selections unavailable
                foreach (ProtocolCodeDetail code in existingSelectedCodes)
                {
                    _availableProtocolCodes.Items.Remove(code);
                }
            }
        }

        private void ResetDocument()
        {
            _protocolGroup = null;
            _protocolGroupChoices = new List<ProtocolGroupSummary>();
            _availableProtocolCodes.Items.Clear();
            _selectedProtocolCodes.Items.Clear();
        }

        private void SaveProtocols(IProtocollingWorkflowService service)
        {
            foreach (ProtocolEditorProcedurePlanSummaryTableItem item in _procedurePlanSummaryTable.Items)
            {
                service.SaveProtocol(new SaveProtocolRequest(item.ProtocolRef, item.ProtocolDetail));
            }
            this.Modified = false;
        }

        #endregion
    }
}
