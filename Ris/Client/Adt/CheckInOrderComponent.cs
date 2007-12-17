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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;

namespace ClearCanvas.Ris.Client.Adt
{
    /// <summary>
    /// Extension point for views onto <see cref="CheckInOrderComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class CheckInOrderComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// CheckInOrderComponent class
    /// </summary>
    [AssociateView(typeof(CheckInOrderComponentViewExtensionPoint))]
    public class CheckInOrderComponent : ApplicationComponent
    {
        private readonly RegistrationWorklistItem _worklistItem;
        private CheckInOrderTable _checkInOrderTable;
        private List<EntityRef> _selectedRequestedProcedures;
        private bool _acceptEnabled;
        private event EventHandler _acceptEnabledChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public CheckInOrderComponent(RegistrationWorklistItem item)
        {
            _worklistItem = item;
        }

        public override void Start()
        {
            _selectedRequestedProcedures = new List<EntityRef>();
            _checkInOrderTable = new CheckInOrderTable();

            Platform.GetService<IRegistrationWorkflowService>(
                delegate(IRegistrationWorkflowService service)
                {
                    GetDataForCheckInTableResponse response = service.GetDataForCheckInTable(new GetDataForCheckInTableRequest(_worklistItem.OrderRef));
                    _checkInOrderTable.Items.AddRange(
                        CollectionUtils.Map<RequestedProcedureSummary, CheckInOrderTableEntry>(response.RequestedProcedures,
                                delegate(RequestedProcedureSummary item)
                                {
                                    CheckInOrderTableEntry entry = new CheckInOrderTableEntry(item);
                                    entry.CheckedChanged += OrderCheckedStateChangedEventHandler;
                                    return entry;
                                }));
                });

            UpdateAcceptStatus();

            base.Start();
        }

        #region Presentation Model

        public ITable OrderTable
        {
            get { return _checkInOrderTable; }
        }

        public List<EntityRef> SelectedRequestedProcedures
        {
            get { return _selectedRequestedProcedures; }
        }

        #endregion

        public void Accept()
        {
            // Get the list of Order EntityRef from the table
            foreach (CheckInOrderTableEntry entry in _checkInOrderTable.Items)
            {
                if (entry.Checked)
                    _selectedRequestedProcedures.Add(entry.RequestedProcedure.RequestedProcedureRef);
            }      

            this.Exit(ApplicationComponentExitCode.Accepted);
        }

        public void Cancel()
        {
            this.Exit(ApplicationComponentExitCode.None);
        }

        public bool AcceptEnabled
        {
            get { return _acceptEnabled; }
        }

        public event EventHandler AcceptEnabledChanged
        {
            add { _acceptEnabledChanged += value; }
            remove { _acceptEnabledChanged -= value; }
        }

        private void OrderCheckedStateChangedEventHandler(object sender, EventArgs e)
        {
            UpdateAcceptStatus();
        }

        private void UpdateAcceptStatus()
        {
            bool entryChecked = CollectionUtils.Contains(
                _checkInOrderTable.Items,
                delegate(CheckInOrderTableEntry entry)
                {
                    return entry.Checked;
                });

            if (_acceptEnabled != entryChecked)
            {
                _acceptEnabled = entryChecked;
                EventsHelper.Fire(_acceptEnabledChanged, this, EventArgs.Empty);
            }
        }

    }
}
