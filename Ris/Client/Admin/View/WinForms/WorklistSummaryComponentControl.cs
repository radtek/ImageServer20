﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using System.Windows.Forms;
using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
	public partial class WorklistSummaryComponentControl : ApplicationComponentUserControl
	{
		private WorklistSummaryComponent _component;

		/// <summary>
		/// Constructor
		/// </summary>
		public WorklistSummaryComponentControl(WorklistSummaryComponent component)
			: base(component)
		{
			InitializeComponent();
			_component = component;

			_worklistTableView.ToolbarModel = _component.SummaryTableActionModel;
			_worklistTableView.MenuModel = _component.SummaryTableActionModel;

			_worklistTableView.Table = _component.SummaryTable;
			_worklistTableView.DataBindings.Add("Selection", _component, "SummarySelection", true, DataSourceUpdateMode.OnPropertyChanged);

			_classComboBox.DataSource = _component.WorklistClassChoices;
			_classComboBox.DataBindings.Add("Value", _component, "SelectedWorklistClass", true, DataSourceUpdateMode.OnPropertyChanged);
			_classComboBox.Format += delegate(object sender, ListControlConvertEventArgs e) { e.Value = _component.FormatWorklistClassChoicesItem(e.ListItem); };

			_name.DataBindings.Add("Value", _component, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
			_includeUserDefinedWorklists.DataBindings.Add("Checked", _component, "IncludeUserDefinedWorklists", true, DataSourceUpdateMode.OnPropertyChanged);

			_okButton.DataBindings.Add("Visible", _component, "ShowAcceptCancelButtons");
			_okButton.DataBindings.Add("Enabled", _component, "AcceptEnabled");
			_cancelButton.DataBindings.Add("Visible", _component, "ShowAcceptCancelButtons");
		}

		private void _searchButton_Click(object sender, EventArgs e)
		{
			using (new CursorManager(Cursors.WaitCursor))
			{
				_component.Search();
			}
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			_component.Accept();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			_component.Cancel();
		}

		private void _worklistTableView_ItemDoubleClicked(object sender, EventArgs e)
		{
			_component.DoubleClickSelectedItem();
		}

		private void _clearButton_Click(object sender, EventArgs e)
		{
			_component.SelectedWorklistClass = _component.NullFilter;
			_component.Name = "";
			_component.Search();
		}

		private void _field_Enter(object sender, EventArgs e)
		{
			this.AcceptButton = _searchButton;
		}

		private void _field_Leave(object sender, EventArgs e)
		{
			this.AcceptButton = _okButton;
		}
	}
}
