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
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Services.LocalDataStore;
using System.ComponentModel;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Services.Tools
{
	[ExtensionPoint()]
	public sealed class DicomFileImportComponentToolExtensionPoint : ExtensionPoint<ITool>
	{
	}

	public interface IDicomFileImportComponentToolContext : IToolContext
	{
		IDesktopWindow DesktopWindow { get; }

		event EventHandler SelectionUpdated;

		bool CanCancelSelected();
		bool CanClearSelected();
		bool CanClearInactive();

		void CancelSelected();
		void ClearSelected();
		void ClearInactive();
	}

	[ExtensionPoint]
	public sealed class DicomFileImportApplicationComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	[AssociateView(typeof(DicomFileImportApplicationComponentViewExtensionPoint))]
	public class DicomFileImportApplicationComponent : ApplicationComponent
	{
		public class DicomFileImportComponentToolContext : ToolContext, IDicomFileImportComponentToolContext
		{
			private DicomFileImportApplicationComponent _component;

			public DicomFileImportComponentToolContext(DicomFileImportApplicationComponent component)
			{
				_component = component;
			}

			#region IDicomFileImportComponentToolContext Members

			public event EventHandler SelectionUpdated
			{
				add { _component.SelectionUpdated += value; }
				remove { _component.SelectionUpdated -= value; }
			}

			public IDesktopWindow DesktopWindow
			{
				get { return _component.Host.DesktopWindow; }
			}

			public int NumberOfEntries
			{
				get { return _component._importTable.Items.Count; }
			}

			public bool CanCancelSelected()
			{
				return _component.SelectedCancelEnabled;
			}

			public bool CanClearSelected()
			{
				return _component.SelectedClearEnabled;
			}

			public bool CanClearInactive()
			{
				return _component.ClearInactiveEnabled;
			}

			public void CancelSelected()
			{
				_component.CancelSelected();
			}

			public void ClearSelected()
			{
				_component.ClearSelected();
			}

			public void ClearInactive()
			{
				_component.ClearInactive();
			}

			#endregion
		}

		private ToolSet _toolSet;
		private Table<ImportProgressItem> _importTable;
		private ISelection _selection;
		private ImportProgressItem _selectedProgressItem;
		private event EventHandler _selectionUpdated;

		private string _selectedStatusMessage;
		private int _selectedTotalProcessed;
		private int _selectedTotalToProcess;
		private int _selectedFailedSteps;
		private int _selectedAvailableCount;
		private bool _selectedCancelEnabled;
		private bool _selectedClearEnabled;

		public DicomFileImportApplicationComponent()
		{
		}

		public event EventHandler SelectionUpdated
		{
			add { _selectionUpdated += value; }
			remove { _selectionUpdated -= value; }
		}

		public override void Start()
		{
			base.Start();

			InitializeTable();

			_toolSet = new ToolSet(new DicomFileImportComponentToolExtensionPoint(), new DicomFileImportComponentToolContext(this));

			LocalDataStoreActivityMonitor.Instance.LostConnection += new EventHandler(OnLostConnection);
			LocalDataStoreActivityMonitor.Instance.Connected += new EventHandler(OnConnected);
			LocalDataStoreActivityMonitor.Instance.ImportProgressUpdate += new EventHandler<ItemEventArgs<ImportProgressItem>>(OnImportProgressUpdate);

			Clear();
			if (!LocalDataStoreActivityMonitor.Instance.IsConnected)
				this.SelectedStatusMessage = SR.MessageActivityMonitorServiceUnavailable;
			else
				this.SelectedStatusMessage = SR.MessageNothingSelected;
		}

		public override void Stop()
		{
			base.Stop();

			LocalDataStoreActivityMonitor.Instance.LostConnection -= new EventHandler(OnLostConnection);
			LocalDataStoreActivityMonitor.Instance.Connected -= new EventHandler(OnConnected);
			LocalDataStoreActivityMonitor.Instance.ImportProgressUpdate -= new EventHandler<ItemEventArgs<ImportProgressItem>>(OnImportProgressUpdate);
		}

		private void OnConnected(object sender, EventArgs e)
		{
			Clear();
			this.SelectedStatusMessage = SR.MessageNothingSelected;
		}

		private void OnLostConnection(object sender, EventArgs e)
		{
			this._importTable.Items.Clear();
			Clear();
			this.SelectedStatusMessage = SR.MessageActivityMonitorServiceUnavailable;
		}

		private void OnImportProgressUpdate(object sender, ItemEventArgs<ImportProgressItem> e)
		{
			int index = _importTable.Items.FindIndex(delegate(ImportProgressItem testItem)
				{
					return testItem.Identifier.Equals(e.Item.Identifier);
				});

			ImportProgressItem existingItem = null;
			if (index >= 0)
			{
				existingItem = _importTable.Items[index];

				if (e.Item.Removed)
				{
					_importTable.Items.Remove(existingItem);
				}
				else
				{
					existingItem.CopyFrom(e.Item);
					_importTable.Items.NotifyItemUpdated(index);
				}
			}
			else
			{
				existingItem = e.Item;

				if (!e.Item.Removed)
				{
					_importTable.Items.Add(e.Item);
				}
			}

			UpdateSelectedItemStats();

			if (existingItem == _selectedProgressItem)
				EventsHelper.Fire(_selectionUpdated, this, EventArgs.Empty);
		}

		private void Clear()
		{
			this.SelectedTotalProcessed = 0;
			this.SelectedAvailableCount = 0;
			this.SelectedTotalToProcess = 0;
			this.SelectedFailedSteps = 0;
			this.SelectedCancelEnabled = false;
			this.SelectedClearEnabled = false;
			this.SelectedStatusMessage = "";
		}
		
		private void UpdateSelectedItemStats()
		{
			if (_selectedProgressItem == null)
				return;

			this.SelectedTotalToProcess = _selectedProgressItem.TotalFilesToImport;
			this.SelectedAvailableCount = _selectedProgressItem.NumberOfFilesCommittedToDataStore;
			this.SelectedFailedSteps = _selectedProgressItem.TotalDataStoreCommitFailures;
			this.SelectedTotalProcessed = _selectedProgressItem.TotalImportsProcessed;
			this.SelectedStatusMessage = _selectedProgressItem.StatusMessage;
			this.SelectedCancelEnabled = (_selectedProgressItem.AllowedCancellationOperations & CancellationFlags.Cancel) == CancellationFlags.Cancel;
			this.SelectedClearEnabled = (_selectedProgressItem.AllowedCancellationOperations & CancellationFlags.Clear) == CancellationFlags.Clear;
		}

		private string FormatString(string input)
		{
			return String.IsNullOrEmpty(input) ? "" : input;
		}

		private void InitializeTable()
		{
			_importTable = new Table<ImportProgressItem>();

			TableColumnBase<ImportProgressItem> column;

			column = new TableColumn<ImportProgressItem, string>(
					SR.TitleDescription,
					delegate(ImportProgressItem item) { return FormatString(item.Description); },
					2f);

			_importTable.Columns.Add(column);

			column = new TableColumn<ImportProgressItem, string>(
					SR.TitleStartTime,
					delegate(ImportProgressItem item)
					{
						if (item.StartTime == default(DateTime))
							return "";

						return item.StartTime.ToString(Format.TimeFormat);
					},
					null,
					0.5f,
					delegate(ImportProgressItem one, ImportProgressItem two) { return one.StartTime.CompareTo(two.StartTime); });

			_importTable.Columns.Add(column);

			// Default: Sort by start time
			_importTable.Sort(new TableSortParams(column, false));

			column = new TableColumn<ImportProgressItem, string>(
					SR.TitleStatus,
					delegate(ImportProgressItem item) { return FormatString(item.StatusMessage); },
					2f);

			_importTable.Columns.Add(column);
		}

		private void InternalCancel(CancellationFlags flags)
		{
			if (_selectedProgressItem == null)
				return;

			List<Guid> progressIdentifiers = new List<Guid>();
			progressIdentifiers.Add(_selectedProgressItem.Identifier);

			CancelProgressItemInformation cancelInformation = new CancelProgressItemInformation();
			cancelInformation.CancellationFlags = flags;
			cancelInformation.ProgressItemIdentifiers = progressIdentifiers;

			LocalDataStoreActivityMonitor.Instance.Cancel(cancelInformation);
		}

		public ActionModelNode ToolbarModel
		{
			get { return ActionModelRoot.CreateModel(this.GetType().FullName, "dicom-file-import-toolbar", _toolSet.Actions); }
		}

		public override ActionModelNode ContextMenuModel
		{
			get { return ActionModelRoot.CreateModel(this.GetType().FullName, "dicom-file-import-contextmenu", _toolSet.Actions); }
		}

		public string Title
		{
			get { return SR.TitleImportActivity; }
		}

		public ITable ImportTable
		{
			get { return _importTable; }
		}

		public void SetSelection(ISelection selection)
		{
			_selection = selection;
			ImportProgressItem lastSelection = _selectedProgressItem;
			_selectedProgressItem = (ImportProgressItem)_selection.Item;

			if (lastSelection != _selectedProgressItem)
			{
				Clear();
				if (_selectedProgressItem == null)
				{
					this.SelectedStatusMessage = SR.MessageNothingSelected;
				}
				else
				{
					this.UpdateSelectedItemStats();
				}

				EventsHelper.Fire(_selectionUpdated, this, EventArgs.Empty);
			}
		}

		public string SelectedStatusMessage
		{
			get
			{
				return _selectedStatusMessage;
			}
			protected set
			{
				if (value == _selectedStatusMessage)
					return;

				_selectedStatusMessage = value;
				this.NotifyPropertyChanged("SelectedStatusMessage");
			}
		}

		public int SelectedTotalProcessed
		{
			get
			{
				return _selectedTotalProcessed;
			}
			protected set
			{
				if (value == _selectedTotalProcessed)
					return;

				_selectedTotalProcessed = value;
				this.NotifyPropertyChanged("SelectedTotalProcessed");
			}
		}

		public int SelectedTotalToProcess
		{
			get
			{
				return _selectedTotalToProcess;
			}
			protected set
			{
				if (value == _selectedTotalToProcess)
					return;

				_selectedTotalToProcess = value;
				this.NotifyPropertyChanged("SelectedTotalToProcess");
			}
		}

		public int SelectedAvailableCount
		{
			get
			{
				return _selectedAvailableCount;
			}
			protected set
			{
				if (value == _selectedAvailableCount)
					return;

				_selectedAvailableCount = value;
				this.NotifyPropertyChanged("SelectedAvailableCount");
			}
		}

		public int SelectedFailedSteps
		{
			get
			{
				return _selectedFailedSteps;
			}
			protected set
			{
				if (value == _selectedFailedSteps)
					return;

				_selectedFailedSteps = value;
				this.NotifyPropertyChanged("SelectedFailedSteps");
			}
		}

		public bool SelectedCancelEnabled
		{
			get { return _selectedCancelEnabled; }
			protected set
			{
				if (value == _selectedCancelEnabled)
					return;

				_selectedCancelEnabled = value;
				this.NotifyPropertyChanged("SelectedCancelEnabled");
			}
		}

		public bool SelectedClearEnabled
		{
			get { return _selectedClearEnabled; }
			protected set
			{
				if (value == _selectedClearEnabled)
					return;

				_selectedClearEnabled = value;
				this.NotifyPropertyChanged("SelectedClearEnabled");
			}
		}

		public bool ClearInactiveEnabled
		{
			get { return _importTable.Items.Count > 0; }
		}

		public void CancelSelected()
		{
			InternalCancel(CancellationFlags.Cancel);
		}

		public void ClearSelected()
		{
			InternalCancel(CancellationFlags.Clear);
		}

		public void ClearInactive()
		{
			LocalDataStoreActivityMonitor.Instance.ClearInactive();
		}
	}
}
