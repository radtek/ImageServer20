using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Common.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="WorklistExplorerComponent"/>
    /// </summary>
    public partial class FolderExplorerComponentControl : CustomUserControl
    {
        private FolderExplorerComponent _component;

        /// <summary>
        /// Constructor
        /// </summary>
        public FolderExplorerComponentControl(FolderExplorerComponent component)
        {
            InitializeComponent();

            _component = component;

            // TODO add .NET databindings to _component
            _folderTreeView.Tree = _component.FolderTree;
            _folderTreeView.DataBindings.Add("Selection", _component, "SelectedFolder", true, DataSourceUpdateMode.OnPropertyChanged);

            _folderContentsTableView.Table = _component.FolderContentsTable;
            _component.SelectedFolderChanged += new EventHandler(_component_FolderContentsTableChanged);

            //_folderContentsTableView.DataBindings.Add("Table", _component, "FolderContentsTable", true, DataSourceUpdateMode.Never);
            _folderContentsTableView.DataBindings.Add("Selection", _component, "SelectedItems", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void _component_FolderContentsTableChanged(object sender, EventArgs e)
        {
            _folderContentsTableView.Table = _component.FolderContentsTable;
        }

        /// <summary>
        /// Handle the item drag event from table view, to begin a drag-drop operation when a user
        /// starts dragging items from the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _folderContentsTableView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _folderContentsTableView.DoDragDrop(e.Item, DragDropEffects.All);
        }
    }
}
