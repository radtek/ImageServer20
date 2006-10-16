using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Healthcare;
using ClearCanvas.Enterprise;
using ClearCanvas.Ris.Services;
using ClearCanvas.Desktop.Tables;

namespace ClearCanvas.Ris.Client.Adt
{
    public class AddressesSummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    [AssociateView(typeof(AddressesSummaryComponentViewExtensionPoint))]
    public class AddressesSummaryComponent : ApplicationComponent
    {
        private PatientProfile _patient;
        private IPatientAdminService _patientAdminService;
        private Table<Address> _addresses;
        private Address _currentAddressSelection;

        private CrudActionModel _addressActionHandler;

        private AddressTypeEnumTable _addressTypes;

        public AddressesSummaryComponent()
        {
            _addresses = new Table<Address>();
            _patientAdminService = ApplicationContext.GetService<IPatientAdminService>();
            _addressTypes = _patientAdminService.GetAddressTypeEnumTable();


            _addresses.Columns.Add(new TableColumn<Address, string>("Type", delegate(Address a) { return _addressTypes[a.Type].Value; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("Street", delegate(Address a) { return a.Street; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("City", delegate(Address a) { return a.City; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("Province", delegate(Address a) { return a.Province; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("PostalCode", delegate(Address a) { return a.PostalCode; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("Country", delegate(Address a) { return a.Country; }));
            _addresses.Columns.Add(new TableColumn<Address, string>("Valid From", delegate(Address a) { return a.ValidRange == null ? null : Format.Date(a.ValidRange.From); }));
            _addresses.Columns.Add(new TableColumn<Address, string>("Valid Until", delegate(Address a) { return a.ValidRange == null ? null : Format.Date(a.ValidRange.Until); }));

            _addressActionHandler = new CrudActionModel();
            _addressActionHandler.Add.Handler = AddAddress;
            _addressActionHandler.Edit.Handler = UpdateSelectedAddress;
            _addressActionHandler.Delete.Handler = DeleteSelectedAddress;

            _addressActionHandler.Add.Enabled = true;
        }

        public PatientProfile Subject
        {
            get { return _patient; }
            set { _patient = value; }
        }
        
        public ITable Addresses
        {
            get { return _addresses; }
        }

        public ActionModelNode AddressListActionModel
        {
            get { return _addressActionHandler; }
        }

        public Address CurrentAddressSelection
        {
            get { return _currentAddressSelection; }
            set
            {
                _currentAddressSelection = value;
                AddressSelectionChanged();
            }
        }

        public void SetSelectedAddress(ISelection selection)
        {
            this.CurrentAddressSelection = (Address)selection.Item;
        }

        private void AddressSelectionChanged()
        {
            if (_currentAddressSelection != null)
            {
                _addressActionHandler.Edit.Enabled = true;
                _addressActionHandler.Delete.Enabled = true;
            }
            else
            {
                _addressActionHandler.Edit.Enabled = false;
                _addressActionHandler.Delete.Enabled = false;
            }
        }

        public void AddAddress()
        {
            Address address = Address.New();

            // For now, hard code default values that correspond to first entry in the the _dummy*Choices string arrays in AddressesEditorComponent
            address.Province = "Ontario";
            address.Country = "Canada";

            AddressEditorComponent editor = new AddressEditorComponent(address);
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, "Add Address...");
            if (exitCode == ApplicationComponentExitCode.Normal)
            {
                _addresses.Items.Add(address);
                _patient.Addresses.Add(address);
                this.Modified = true;
            }
        }

        public void UpdateSelectedAddress()
        {
            // can occur if user double clicks while holding control
            if (_currentAddressSelection == null) return;

            Address address = Address.New();
            address.CopyFrom(_currentAddressSelection);

            AddressEditorComponent editor = new AddressEditorComponent(address);
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, "Update Address...");
            if (exitCode == ApplicationComponentExitCode.Normal)
            {
                // delete and re-insert to ensure that TableView updates correctly
                Address toBeRemoved = _currentAddressSelection;
                _addresses.Items.Remove(toBeRemoved);
                _patient.Addresses.Remove(toBeRemoved);

                _addresses.Items.Add(address);
                _patient.Addresses.Add(address);

                this.Modified = true;
            }
        }

        public void DeleteSelectedAddress()
        {
            if (this.Host.ShowMessageBox("Are you sure you want to delete this address?", MessageBoxActions.YesNo) == DialogBoxAction.Yes)
            {
                //  Must use temporary Address otherwise as a side effect TableDate.Remove() will change the current selection 
                //  resulting in the wrong Address being removed from the PatientProfile
                Address toBeRemoved  = _currentAddressSelection;
                _addresses.Items.Remove(toBeRemoved);
                _patient.Addresses.Remove(toBeRemoved);
                this.Modified = true;
            }
        }

        public void LoadAddressesTable()
        {
            if (_patient != null)
            {
                foreach (Address address in _patient.Addresses)
                {
                    _addresses.Items.Add(address);
                }
            }
        }
    }
}
