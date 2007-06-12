using System;
using System.Collections.Generic;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Tables;

using ClearCanvas.Ris.Application.Common.Admin;
using ClearCanvas.Ris.Application.Common.Admin.FacilityAdmin;

namespace ClearCanvas.Ris.Client.Admin
{
    [MenuAction("launch", "global-menus/Admin/Facilities")]
    [ClickHandler("launch", "Launch")]
    [ActionPermission("launch", ClearCanvas.Ris.Application.Common.AuthorityTokens.FacilityAdmin)]

    [ExtensionOf(typeof(DesktopToolExtensionPoint))]
    public class FacilitySummaryTool : Tool<IDesktopToolContext>
    {
        private IWorkspace _workspace;

        public void Launch()
        {
            if (_workspace == null)
            {
                FacilitySummaryComponent component = new FacilitySummaryComponent();

                _workspace = ApplicationComponent.LaunchAsWorkspace(
                    this.Context.DesktopWindow,
                    component,
                    SR.TitleFacilities,
                    delegate(IApplicationComponent c) { _workspace = null; });
            }
            else
            {
                _workspace.Activate();
            }
        }
    }
    
    /// <summary>
    /// Extension point for views onto <see cref="FacilitySummaryComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class FacilitySummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// FacilitySummaryComponent class
    /// </summary>
    [AssociateView(typeof(FacilitySummaryComponentViewExtensionPoint))]
    public class FacilitySummaryComponent : ApplicationComponent
    {
        private FacilitySummary _selectedFacility;
        private FacilityTable _facilityTable;
        private CrudActionModel _facilityActionHandler;

        private PagingController<FacilitySummary> _pagingController;
        private PagingActionModel<FacilitySummary> _pagingActionHandler;

        private ListAllFacilitiesRequest _listRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public FacilitySummaryComponent()
        {
        }

        public override void Start()
        {
            //_facilityAdminService.FacilityChanged += FacilityChangedEventHandler;

            _facilityTable = new FacilityTable();
            _facilityActionHandler = new CrudActionModel();
            _facilityActionHandler.Add.SetClickHandler(AddFacility);
            _facilityActionHandler.Edit.SetClickHandler(UpdateSelectedFacility);
            _facilityActionHandler.Add.Enabled = true;
            _facilityActionHandler.Delete.Enabled = false;

            InitialisePaging();
            _facilityActionHandler.Merge(_pagingActionHandler);

            base.Start();
        }

        private void InitialisePaging()
        {
            _pagingController = new PagingController<FacilitySummary>(
                delegate(int firstRow, int maxRows)
                {
                    IList<FacilitySummary> facilities = null;

                    try
                    {
                        ListAllFacilitiesResponse listResponse = null;

                        Platform.GetService<IFacilityAdminService>(
                            delegate(IFacilityAdminService service)
                            {
                                ListAllFacilitiesRequest listRequest = _listRequest;
                                listRequest.PageRequest.FirstRow = firstRow;
                                listRequest.PageRequest.MaxRows = maxRows;

                                listResponse = service.ListAllFacilities(listRequest);
                            });

                        facilities = listResponse.Facilities;
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.Report(e, this.Host.DesktopWindow);
                    }

                    return facilities;
                }
            );

            _pagingActionHandler = new PagingActionModel<FacilitySummary>(_pagingController, _facilityTable, Host.DesktopWindow);
        }

        public override void Stop()
        {
            //_facilityAdminService.FacilityChanged -= FacilityChangedEventHandler; 
            
            base.Stop();
        }

        //TODO: FacilityChangedEventHandler
        //private void FacilityChangedEventHandler(object sender, EntityChangeEventArgs e)
        //{
        //    // check if the facility with this oid is in the list
        //    int index = _facilityTable.Items.FindIndex(delegate(Facility f) { return e.EntityRef.RefersTo(f); });
        //    if (index > -1)
        //    {
        //        if (e.ChangeType == EntityChangeType.Update)
        //        {
        //            Facility f = _facilityAdminService.LoadFacility((EntityRef<Facility>)e.EntityRef);
        //            _facilityTable.Items[index] = f;
        //        }
        //        else if (e.ChangeType == EntityChangeType.Delete)
        //        {
        //            _facilityTable.Items.RemoveAt(index);
        //        }
        //    }
        //    else
        //    {
        //        if (e.ChangeType == EntityChangeType.Create)
        //        {
        //            Facility f = _facilityAdminService.LoadFacility((EntityRef<Facility>)e.EntityRef);
        //            if (f != null)
        //                _facilityTable.Items.Add(f);
        //        }
        //    }
        //}

        #region Presentation Model

        public ITable Facilities
        {
            get { return _facilityTable; }
        }

        public ActionModelNode FacilityListActionModel
        {
            get { return _facilityActionHandler; }
        }

        public ISelection SelectedFacility
        {
            get { return _selectedFacility == null ? Selection.Empty : new Selection(_selectedFacility); }
            set
            {
                _selectedFacility = (FacilitySummary)value.Item;
                FacilitySelectionChanged();
            }
        }

        public void AddFacility()
        {
            FacilityEditorComponent editor = new FacilityEditorComponent();
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                this.Host.DesktopWindow, editor, SR.TitleAddFacility);
            if (exitCode == ApplicationComponentExitCode.Normal)
            {
                _facilityTable.Items.Add(_selectedFacility = editor.FacilitySummary);
                FacilitySelectionChanged();
            }
        }

        public void UpdateSelectedFacility()
        {
            // can occur if user double clicks while holding control
            if (_selectedFacility == null) return;

            FacilityEditorComponent editor = new FacilityEditorComponent(_selectedFacility.FacilityRef);
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                this.Host.DesktopWindow, editor, SR.TitleUpdateFacility);
            if (exitCode == ApplicationComponentExitCode.Normal)
            {
                _facilityTable.Items.Replace(delegate(FacilitySummary f) { return f.FacilityRef.Equals(editor.FacilitySummary.FacilityRef); }, editor.FacilitySummary);
            }
        }

        public void LoadFacilityTable()
        {
            _listRequest = new ListAllFacilitiesRequest();
            _facilityTable.Items.Clear();
            _facilityTable.Items.AddRange(_pagingController.GetFirst());
        }

        #endregion

        private void FacilitySelectionChanged()
        {
            if (_selectedFacility != null)
                _facilityActionHandler.Edit.Enabled = true;
            else
                _facilityActionHandler.Edit.Enabled = false;

            NotifyPropertyChanged("SelectedFacility");
        }
    }
}
