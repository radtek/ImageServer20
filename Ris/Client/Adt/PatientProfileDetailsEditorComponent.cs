using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Enterprise;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Healthcare;

using Iesi.Collections;
using ClearCanvas.Ris.Services;
using ClearCanvas.Desktop.Validation;

namespace ClearCanvas.Ris.Client.Adt
{
    [ExtensionPoint()]
    public class PatientEditorComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    [AssociateView(typeof(PatientEditorComponentViewExtensionPoint))]
    public class PatientProfileDetailsEditorComponent : ApplicationComponent
    {
        private PatientProfile _patient;
        private IPatientAdminService _patientAdminService;
        private SexEnumTable _sexChoices;

        private string[] _dummyProvinceChoices = new string[] { "Ontario", "Alberta", "British Columbia", "Manitoba", "New Brunswick", "Newfoundland", "Nova Scotia", "PEI", "Quebec", "Saskatchewan" };
        private string[] _dummySiteChoices = new string[] { "UHN", "MSH", "SiteA", "SiteB", "SiteC", "SiteD", "SiteE", "SiteF" };

        public PatientProfileDetailsEditorComponent()
        {
            _patient = new PatientProfile();
        }

        /// <summary>
        /// Gets or sets the subject (e.g PatientProfile) that this editor operates on.  This property
        /// should never be used by the view.
        /// </summary>
        public PatientProfile Subject
        {
            get { return _patient; }
            set { _patient = value; }
        }

        public override void Start()
        {
            base.Start();
            _patientAdminService = ApplicationContext.GetService<IPatientAdminService>();
            _sexChoices = _patientAdminService.GetSexEnumTable();
        }

        #region Presentation Model

        public string MrnID
        {
            get { return _patient.Mrn.Id; }
            set
            {
                _patient.Mrn.Id = value;
                this.Modified = true;
            }
        }

        public string MrnSite
        {
            get { return _patient.Mrn.AssigningAuthority; }
            set
            {
                _patient.Mrn.AssigningAuthority = value;
                this.Modified = true;
            }
        }

        public string FamilyName
        {
            get { return _patient.Name.FamilyName; }
            set { 
                _patient.Name.FamilyName = value;
                this.Modified = true;
            }
        }

        public string GivenName
        {
            get { return _patient.Name.GivenName; }
            set { 
                _patient.Name.GivenName = value;
                this.Modified = true;
            }
        }

        public string MiddleName
        {
            get { return _patient.Name.MiddleName; }
            set { 
                _patient.Name.MiddleName = value;
                this.Modified = true;
            }
        }

        public string Sex
        {
            get { return _sexChoices[_patient.Sex].Value; }
            set {
                _patient.Sex = _sexChoices[value].Code;
                this.Modified = true;
            }
        }

        public string[] SexChoices
        {
            get { return _sexChoices.Values; }
        }

        public DateTime DateOfBirth
        {
            get { return _patient.DateOfBirth; }
            set { 
                _patient.DateOfBirth = new DateTime(value.Year, value.Month, value.Day);
                this.Modified = true;
            }
        }

        public bool DeathIndicator
        {
            get { return _patient.DeathIndicator; }
            set { 
                _patient.DeathIndicator = value;
                this.Modified = true;
            }
        }

        public DateTime? TimeOfDeath
        {
            get { return _patient.TimeOfDeath; }
            set
            {
                _patient.TimeOfDeath = value;
                this.Modified = true;
            }
        }

        public string[] MrnSiteChoices
        {
            get { return _dummySiteChoices;  }
        }

        public string HealthcardID
        {
            get { return _patient.Healthcard.Id; }
            set
            {
                _patient.Healthcard.Id = value;
                this.Modified = true;
            }
        }

        public string HealthcardProvince
        {
            get { return _patient.Healthcard.AssigningAuthority; }
            set
            {
                _patient.Healthcard.AssigningAuthority = value;
                this.Modified = true;
            }
        }

        public string[] HealthcardProvinceChoices
        {
            get { return _dummyProvinceChoices;  }
        }

        public string HealthcardVersionCode
        {
            get { return _patient.Healthcard.VersionCode; }
            set
            {
                _patient.Healthcard.VersionCode= value;
                this.Modified = true;
            }
        }

        public DateTime? HealthcardExpiryDate
        {
            get { return _patient.Healthcard.ExpiryDate; }
            set
            {
                _patient.Healthcard.ExpiryDate = value;
                this.Modified = true;
            }
        }

        #endregion
    }
}
