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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Adt.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="OrderEntryComponent"/>
    /// </summary>
    public partial class OrderEntryComponentControl : ApplicationComponentUserControl
    {
        private OrderEntryComponent _component;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderEntryComponentControl(OrderEntryComponent component)
            :base(component)
        {
            InitializeComponent();

            _component = component;

            _diagnosticService.LookupHandler = _component.DiagnosticServiceLookupHandler;
            _diagnosticService.DataBindings.Add("Value", _component, "SelectedDiagnosticService", true,
                                                DataSourceUpdateMode.OnPropertyChanged);

            _indication.DataBindings.Add("Value", _component, "Indication", true, DataSourceUpdateMode.OnPropertyChanged);

            _proceduresTableView.Table = _component.Procedures;
            _proceduresTableView.MenuModel = _component.ProceduresActionModel;
            _proceduresTableView.ToolbarModel = _component.ProceduresActionModel;
            _proceduresTableView.DataBindings.Add("Selection", _component, "SelectedProcedure", true, DataSourceUpdateMode.OnPropertyChanged);

            _consultantsTableView.Table = _component.Consultants;
            _consultantsTableView.MenuModel = _component.ConsultantsActionModel;
            _consultantsTableView.ToolbarModel = _component.ConsultantsActionModel;
            _consultantsTableView.DataBindings.Add("Selection", _component, "SelectedConsultant", true, DataSourceUpdateMode.OnPropertyChanged);
            _addConsultantButton.DataBindings.Add("Enabled", _component.ConsultantsActionModel.Add, "Enabled");

            _consultantLookup.LookupHandler = _component.ConsultantsLookupHandler;
            _consultantLookup.DataBindings.Add("Value", _component, "ConsultantToAdd", true, DataSourceUpdateMode.OnPropertyChanged);

            _visit.DataSource = _component.ActiveVisits;
            _visit.DataBindings.Add("Value", _component, "SelectedVisit", true, DataSourceUpdateMode.OnPropertyChanged);
            _visit.Format += delegate(object source, ListControlConvertEventArgs e) { e.Value = _component.FormatVisit(e.ListItem); };

            _priority.DataSource = _component.PriorityChoices;
            _priority.DataBindings.Add("Value", _component, "SelectedPriority", true, DataSourceUpdateMode.OnPropertyChanged);

            _orderingFacility.DataSource = _component.FacilityChoices;
            _orderingFacility.DataBindings.Add("Value", _component, "SelectedFacility", true, DataSourceUpdateMode.OnPropertyChanged);
            _orderingFacility.Format += delegate(object source, ListControlConvertEventArgs e) { e.Value = _component.FormatFacility(e.ListItem); };

            _orderingPractitioner.LookupHandler = _component.OrderingPractitionerLookupHandler;
            _orderingPractitioner.DataBindings.Add("Value", _component, "SelectedOrderingPractitioner", true, DataSourceUpdateMode.OnPropertyChanged);

            // bind date and time to same property
            _schedulingRequestDate.DataBindings.Add("Value", _component, "SchedulingRequestTime", true, DataSourceUpdateMode.OnPropertyChanged);
            _schedulingRequestTime.DataBindings.Add("Value", _component, "SchedulingRequestTime", true, DataSourceUpdateMode.OnPropertyChanged);

            // bind date and time to same property
            _scheduledDate.DataBindings.Add("Value", _component, "ScheduledTime", true, DataSourceUpdateMode.OnPropertyChanged);
            _scheduledTime.DataBindings.Add("Value", _component, "ScheduledTime", true, DataSourceUpdateMode.OnPropertyChanged);

            _reorderReason.DataSource = _component.CancelReasonChoices;
            _reorderReason.DataBindings.Add("Value", _component, "SelectedCancelReason", true, DataSourceUpdateMode.OnPropertyChanged);
            _reorderReason.DataBindings.Add("Visible", _component, "IsReplaceOrder");
        }

        private void DiagnosticServiceChangedEventHandler(object sender, EventArgs e)
        {
        }

        private void _placeOrderButton_Click(object sender, EventArgs e)
        {
            using (new CursorManager(Cursors.WaitCursor))
            {
                _component.Accept();
            }
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            _component.Cancel();
        }

        private void _addConsultantButton_Click(object sender, EventArgs e)
        {
            _component.AddConsultant();
        }
    }
}
