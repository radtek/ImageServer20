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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Common.Utilities;
using ClearCanvas.ImageServer.Web.Common.WebControls.Validators;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Configure.PartitionArchive
{
    //
    // Dialog for adding new Partition.
    //
    public partial class AddEditPartitionDialog : UserControl
    {
        #region Private Members

        private bool _editMode;
        private Model.PartitionArchive _partitionArchive;

        #endregion

        #region Public Properties

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                ViewState[ClientID + "_EditMode"] = value;
            }
        }

        /// <summary>
        /// Sets/Gets the current editing partition.
        /// </summary>
        public Model.PartitionArchive PartitionArchive
        {
            set
            {
                _partitionArchive = value;
                // put into viewstate to retrieve later
                ViewState[ClientID + "_EdittedPartitionArchive"] = _partitionArchive;
            }
            get { return _partitionArchive; }
        }

        #endregion // public members

        #region Events

        /// <summary>
        /// Defines the event handler for <seealso cref="OKClicked"/>.
        /// </summary>
        /// <param name="partition">The partition being added.</param>
        public delegate void OnOKClickedEventHandler(Model.PartitionArchive partition);

        /// <summary>
        /// Occurs when users click on "OK".
        /// </summary>
        public event OnOKClickedEventHandler OKClicked;

        #endregion Events

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (ViewState[ClientID + "_EditMode"] != null)
                    _editMode = (bool) ViewState[ClientID + "_EditMode"];
            }

            ArchiveTypeDropDownList.Items.Clear();
            ArchiveTypeDropDownList.Items.Add(new ListItem("HSM Archive", "HSMArchive"));

        }


        /// <summary>
        /// Handles event when user clicks on "OK" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OKButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveData();

                if (OKClicked != null)
                    OKClicked(PartitionArchive);

                Close();
            }
            else
            {
                Show(false);
            }
        }

        /// <summary>
        /// Handles event when user clicks on "Cancel" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        

        protected void UpdateUI()
        {
            // Update the title and OK button text. Changing the image is the only way to do this, since the 
            // SkinID cannot be set dynamically after Page_PreInit.
            if (EditMode)
            {
                ModalDialog.Title = App_GlobalResources.Titles.EditPartitionArchiveTitle;
                OKButton.EnabledImageURL = ImageServerConstants.ImageURLs.UpdateButtonEnabled;
            }
            else
            {
                ModalDialog.Title = App_GlobalResources.Titles.AddPartitionArchiveTitle;
                OKButton.EnabledImageURL = ImageServerConstants.ImageURLs.AddButtonEnabled;
            }

            if (PartitionArchive == null)
            {
                //Description.Text = "Partition Archive Name";
                //ArchiveDelay.Text = "10";
                //EnabledCheckBox.Checked = true;
                //ReadOnlyCheckBox.Checked = true;
                //ConfigurationXML.Text = "<HsmArchive>\n\t<RootDir>C:\\ImageServer\\Archive</RootDir>\n</HsmArchive>";
                //ArchiveTypeDropDownList.SelectedIndex = 0;
            }
            else if (Page.IsValid)
            {
                //Description.Text = PartitionArchive.Description;
                //ArchiveDelay.Text = PartitionArchive.ArchiveDelayHours.ToString();
                //EnabledCheckBox.Checked = PartitionArchive.Enabled;
                //ReadOnlyCheckBox.Checked = PartitionArchive.ReadOnly;
                //ConfigurationXML.Text = XmlUtilities.GetXmlDocumentAsString(PartitionArchive.ConfigurationXml, false);
                //ArchiveTypeDropDownList.SelectedValue = PartitionArchive.ArchiveTypeEnum.Lookup;
            }
        }


        #region Private Methods


        private void SaveData()
        {
            if (PartitionArchive == null)
            {
                PartitionArchive = new Model.PartitionArchive();
            }

            PartitionArchive.ServerPartitionKey = ((Default)Page).ServerPartition.GetKey();
            PartitionArchive.Description = Description.Text;
            PartitionArchive.ArchiveDelayHours = int.Parse(ArchiveDelay.Text);

            PartitionArchive.ConfigurationXml = new XmlDocument();
            if (ConfigurationXML.Text != string.Empty)
            {
                PartitionArchive.ConfigurationXml.Load(new StringReader(ConfigurationXML.Text));
            }
            else
            {
                PartitionArchive.ConfigurationXml.Load(new StringReader(ImageServerConstants.DefaultConfigurationXml));                
            }
            
            PartitionArchive.Enabled = EnabledCheckBox.Checked;
            PartitionArchive.ReadOnly = ReadOnlyCheckBox.Checked;
            PartitionArchive.ArchiveTypeEnum = ArchiveTypeEnum.HsmArchive;                    
        }

        #endregion Private Methods

        #endregion Protected methods

        #region Public Methods

        /// <summary>
        /// Displays the add device dialog box.
        /// </summary>
        public void Show(bool updateUI)
        {
            if (updateUI)
                UpdateUI();

            if (Page.IsValid)
            {
//                ServerPartitionTabContainer.ActiveTabIndex = 0;
            }


            ModalDialog.Show();
        }

        /// <summary>
        /// Dismisses the dialog box.
        /// </summary>
        public void Close()
        {
            ModalDialog.Hide();   
        }

        #endregion Public methods
    }
}