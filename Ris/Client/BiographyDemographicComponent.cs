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

using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.BrowsePatientData;
using ClearCanvas.Ris.Client;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client
{
    /// <summary>
    /// Extension point for views onto <see cref="BiographyDemographicComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class BiographyDemographicComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// BiographyDemographicComponent class
    /// </summary>
    [AssociateView(typeof(BiographyDemographicComponentViewExtensionPoint))]
    public class BiographyDemographicComponent : ApplicationComponent
    {
        private readonly EntityRef _profileRef;
        private PatientProfileDetail _patientProfile;

        private PatientProfileSummary _selectedProfile;
        private List<PatientProfileSummary> _profileChoices;

        private AddressDetail _selectedAddress;
        private TelephoneDetail _selectedPhone;
        private EmailAddressDetail _selectedEmail;
        private ContactPersonDetail _selectedContact;

        private readonly AddressTable _addressTable;
        private readonly TelephoneNumberTable _phoneTable;
        private readonly EmailAddressTable _emailTable;
        private readonly ContactPersonTable _contactTable;

        private List<EnumValueInfo> _addressTypeChoices;
        private List<EnumValueInfo> _phoneTypeChoices;
        private List<EnumValueInfo> _contactPersonTypeChoices;
        private List<EnumValueInfo> _contactPersonRelationshipChoices;

        /// <summary>
        /// Constructor
        /// </summary>
        public BiographyDemographicComponent(EntityRef profileRef, PatientProfileDetail patientProfile)
        {
            _profileRef = profileRef;
            _patientProfile = patientProfile;

            _profileChoices = new List<PatientProfileSummary>();

            _addressTable = new AddressTable();
            _phoneTable = new TelephoneNumberTable();
            _emailTable = new EmailAddressTable();
            _contactTable = new ContactPersonTable();

            _addressTypeChoices = new List<EnumValueInfo>();
            _phoneTypeChoices = new List<EnumValueInfo>();
            _contactPersonTypeChoices = new List<EnumValueInfo>();
            _contactPersonRelationshipChoices = new List<EnumValueInfo>();
        }

        public override void Start()
        {
            Platform.GetService<IBrowsePatientDataService>(
                delegate(IBrowsePatientDataService service)
                {
                    GetDataRequest request = new GetDataRequest();
                    request.PatientProfileRef = _profileRef;
                    request.ListProfilesRequest = new ListProfilesRequest();
                    request.LoadPatientProfileFormDataRequest = new LoadPatientProfileFormDataRequest();
                    GetDataResponse response = service.GetData(request);

                    _profileChoices = response.ListProfilesResponse.Profiles;
                    _addressTypeChoices = response.LoadPatientProfileFormDataResponse.AddressTypeChoices;
                    _phoneTypeChoices = response.LoadPatientProfileFormDataResponse.PhoneTypeChoices;
                    _contactPersonTypeChoices = response.LoadPatientProfileFormDataResponse.ContactPersonTypeChoices;
                    _contactPersonRelationshipChoices = response.LoadPatientProfileFormDataResponse.ContactPersonRelationshipChoices;
                });

            UpdateTables();

            base.Start();
        }

        private void UpdateTables()
        {
            if (_patientProfile == null)
                return;

            _addressTable.Items.Clear();
            _phoneTable.Items.Clear();
            _emailTable.Items.Clear();
            _contactTable.Items.Clear();

            _addressTable.Items.AddRange(_patientProfile.Addresses);
            _phoneTable.Items.AddRange(_patientProfile.TelephoneNumbers);
            _emailTable.Items.AddRange(_patientProfile.EmailAddresses);
            _contactTable.Items.AddRange(_patientProfile.ContactPersons);
        }

        private static string ProfileStringConverter(PersonNameDetail name, MrnDetail mrn)
        {
            return String.Format("{0} - {1}", MrnFormat.Format(mrn), PersonNameFormat.Format(name));
        }

        private void OnSelectedProfileChanged()
        {
            if (_selectedProfile == null)
                return;

            try
            {
                Platform.GetService<IBrowsePatientDataService>(
                    delegate(IBrowsePatientDataService service)
                    {
                        GetDataRequest request = new GetDataRequest();
                        request.PatientProfileRef = _profileRef;
                        request.GetProfileDetailRequest = new GetProfileDetailRequest(true, true, true, true, true, true);
                        GetDataResponse response = service.GetData(request);

                        _patientProfile = response.GetProfileDetailResponse.PatientProfileDetail;
                    });

                UpdateTables();

                NotifyAllPropertiesChanged();       
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        #region Presentation Model

        public List<string> ProfileChoices
        {
            get
            {
                List<string> profileStrings = new List<string>();
                if (_profileChoices.Count > 0)
                {
                    profileStrings.AddRange(
                        CollectionUtils.Map<PatientProfileSummary, string>(
                            _profileChoices, delegate(PatientProfileSummary pp) { return ProfileStringConverter(pp.Name, pp.Mrn); }));
                }

                return profileStrings;
            }
        }

        public string SelectedProfile
        {
            get { return _selectedProfile == null ? "" : ProfileStringConverter(_selectedProfile.Name, _selectedProfile.Mrn); }
            set
            {
                _selectedProfile = (value == "") ? null :
                    CollectionUtils.SelectFirst<PatientProfileSummary>(_profileChoices,
                        delegate(PatientProfileSummary pp) { return ProfileStringConverter(pp.Name, pp.Mrn) == value; });

                OnSelectedProfileChanged();
            }
        }

        public ITable Addresses
        {
            get { return _addressTable; }
        }

        public ISelection SelectedAddress
        {
            get { return new Selection(_selectedAddress); }
            set { _selectedAddress = (AddressDetail)value.Item; }
        }

        public ITable PhoneNumbers
        {
            get { return _phoneTable; }
        }

        public ISelection SelectedPhone
        {
            get { return new Selection(_selectedPhone); }
            set { _selectedPhone = (TelephoneDetail)value.Item; }
        }

        public ITable EmailAddresses
        {
            get { return _emailTable; }
        }

        public ISelection SelectedEmail
        {
            get { return new Selection(_selectedEmail); }
            set { _selectedEmail = (EmailAddressDetail)value.Item; }
        }

        public ITable ContactPersons
        {
            get { return _contactTable; }
        }

        public ISelection SelectedContact
        {
            get { return new Selection(_selectedContact); }
            set { _selectedContact = (ContactPersonDetail)value.Item; }
        }

        public string FamilyName
        {
            get { return _patientProfile.Name.FamilyName; }
        }

        public string GivenName
        {
            get { return _patientProfile.Name.GivenName; }
        }

        public string MiddleName
        {
            get { return _patientProfile.Name.MiddleName; }
        }

        public string Prefix
        {
            get { return _patientProfile.Name.Prefix; }
        }

        public string Suffix
        {
            get { return _patientProfile.Name.Suffix; }
        }

        public string Degree
        {
            get { return _patientProfile.Name.Degree; }
        }

        public string Sex
        {
            get { return _patientProfile.Sex.Value; }
        }

        public string DateOfBirth
        {
            get { return Format.Date(_patientProfile.DateOfBirth); }
        }

        public string TimeOfDeath
        {
            get { return Format.DateTime(_patientProfile.TimeOfDeath); }
        }

        public string Religion
        {
            get { return _patientProfile.Religion.Value; }
        }

        public string PrimaryLanguage
        {
            get { return _patientProfile.PrimaryLanguage.Value; }
        }

        public string Mrn
        {
            get { return _patientProfile.Mrn.Id; }
        }

        public string MrnSite
        {
            get { return _patientProfile.Mrn.AssigningAuthority; }
        }

        public string Healthcard
        {
            get { return _patientProfile.Healthcard.Id; }
        }

        public string HealthcardProvince
        {
            get { return _patientProfile.Healthcard.AssigningAuthority; }
        }

        public string HealthcardVersionCode
        {
            get { return _patientProfile.Healthcard.VersionCode; }
        }

        public string HealthcardExpiry
        {
            get { return Format.Date(_patientProfile.Healthcard.ExpiryDate); }
        }

        public void ShowSelectedAddress()
        {
            if (_selectedAddress == null)
                return;

            AddressEditorComponent editor = new AddressEditorComponent(_selectedAddress, _addressTypeChoices);
            LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleAddresses);
        }

        public void ShowSelectedPhone()
        {
            if (_selectedPhone == null)
                return;

            PhoneNumberEditorComponent editor = new PhoneNumberEditorComponent(_selectedPhone, _phoneTypeChoices);
            LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitlePhoneNumbers);
        }

        public void ShowSelectedEmail()
        {
            if (_selectedEmail == null)
                return;

            EmailAddressEditorComponent editor = new EmailAddressEditorComponent(_selectedEmail);
            LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleEmailAddresses);
        }

        public void ShowSelectedContact()
        {
            if (_selectedContact == null)
                return;

            ContactPersonEditorComponent editor = new ContactPersonEditorComponent(_selectedContact, _contactPersonTypeChoices, _contactPersonRelationshipChoices);
            LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleContactPersons);
        }

        #endregion
    }
}
