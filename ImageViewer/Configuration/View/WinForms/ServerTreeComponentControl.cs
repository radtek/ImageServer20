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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.View.WinForms;
using System.Collections.Generic;
using ClearCanvas.ImageViewer.Configuration.ServerTree;
using ClearCanvas.ImageViewer.Services.ServerTree;

namespace ClearCanvas.ImageViewer.Configuration.View.WinForms
{
	public partial class ServerTreeComponentControl : UserControl
	{
		private enum CheckState
		{
			None = -1,
			Unchecked = 0,
			Partial = 1,
			Checked = 2
		}

		private readonly ServerTreeComponent _component;
		private TreeNode _lastClickedNode;
		private ActionModelNode _toolbarModel;
		private ActionModelNode _menuModel;
		private ToolStripItemDisplayStyle _toolStripItemDisplayStyle = ToolStripItemDisplayStyle.Image;
		private TreeNode _lastMouseOverNode = null;

		public ServerTreeComponentControl(ServerTreeComponent component)
		{
            Platform.CheckForNullReference(component, "component");
            InitializeComponent();

            _component = component;

			ClearCanvasStyle.SetTitleBarStyle(_titleBar);

			_titleBar.Visible = _component.ShowTitlebar;
			_serverTools.Visible = _component.ShowTools;

			_aeTreeView.AllowDrop = !_component.IsReadOnly;
            _aeTreeView.ItemDrag += TreeViewItemDrag;
            _aeTreeView.DragEnter += TreeViewDragEnter;
            _aeTreeView.DragOver += TreeViewDragOver;
            _aeTreeView.DragDrop += TreeViewDragDrop;
			_aeTreeView.MouseDown += AETreeViewClick;
			_aeTreeView.AfterSelect += AETreeViewAfterSelect;

			_component.SelectedServerChanged += new EventHandler(OnSelectedServerChanged);
			_component.ServerTree.ServerTreeUpdated += OnServerTreeUpdated;
			if (_component.ShowTools)
			{
				_toolStripItemDisplayStyle = ToolStripItemDisplayStyle.Image;
				ToolbarModel = _component.ToolbarModel;
				MenuModel = _component.ContextMenuModel;
			}

			if (_component.ShowLocalDataStoreNode)
			{
				// A bit cheap, but by doing this we can force a refresh of the tooltip text if the Dicom
				// Server WCF service hadn't quite started yet when this component was first created.
				_aeTreeView.MouseEnter += new EventHandler(OnLocalDataStoreNodeUpdated);
				_component.ServerTree.RootNode.LocalDataStoreNode.DicomServerConfigurationProvider.Changed += new EventHandler(OnLocalDataStoreNodeUpdated);
			}

			BuildServerTreeView(_aeTreeView, _component.ServerTree);
			SetInitialSelection();
		}

		#region CheckState Handling

		private void OnServerChecked(TreeNode serverNode)
		{
			SetServerCheck(serverNode, !((Server)serverNode.Tag).IsChecked);
			UpdateServerGroups();
		}

		private void OnServerGroupChecked(TreeNode serverGroupNode)
		{
			ServerGroup group = (ServerGroup)serverGroupNode.Tag;
			SetGroupCheck(serverGroupNode, !group.IsEntireGroupChecked());
			UpdateServerGroups();
		}

		private void SetServerCheck(TreeNode serverNode)
		{
			SetServerCheck(serverNode, ((IServerTreeNode)serverNode.Tag).IsChecked);
		}

		private void SetServerCheck(TreeNode serverNode, bool isChecked)
		{
			if (!_component.ShowCheckBoxes)
				return;

			Server server = (Server)serverNode.Tag;
			server.IsChecked = isChecked;
			serverNode.StateImageIndex = (int)(isChecked ? CheckState.Checked : CheckState.Unchecked);
		}

		private void SetGroupCheck(TreeNode serverGroupNode, bool isChecked)
		{
			if (!_component.ShowCheckBoxes)
				return;

			serverGroupNode.StateImageIndex = (int)(isChecked ? CheckState.Checked : CheckState.Unchecked);

			foreach (TreeNode node in serverGroupNode.Nodes)
			{
				if (node.Tag is ServerGroup)
					SetGroupCheck(node, isChecked);
				else if (node.Tag is Server) 
					SetServerCheck(node, isChecked);
			}
		}

		private void UpdateServerGroups()
		{
			if (!_component.ShowCheckBoxes)
				return;

			UpdateServerGroups(CollectionUtils.Cast<TreeNode>(_aeTreeView.Nodes));
		}

		private void UpdateServerGroups(IEnumerable<TreeNode> nodes)
		{
			if (!_component.ShowCheckBoxes)
				return;

			foreach (TreeNode node in nodes)
			{
				if (node.Tag is ServerGroup)
				{
					UpdateServerGroups(CollectionUtils.Cast<TreeNode>(node.Nodes));
					UpdateServerGroup(node);
				}
			}
		}

		private void UpdateServerGroup(TreeNode serverGroupNode)
		{
			if (!_component.ShowCheckBoxes)
				return;

			if (serverGroupNode.Tag is ServerGroup)
			{
				ServerGroup group = (ServerGroup)serverGroupNode.Tag;

				bool isEntireGroupChecked = group.IsEntireGroupChecked();
				bool isEntireGroupUnchecked = group.IsEntireGroupUnchecked();
				
				if (isEntireGroupUnchecked)
					serverGroupNode.StateImageIndex = (int)CheckState.Unchecked;
				else if (isEntireGroupChecked)  //when there are no servers, leave it unchecked.
					serverGroupNode.StateImageIndex = (int)CheckState.Checked;
				else
					serverGroupNode.StateImageIndex = (int)CheckState.Partial;
			}
		}

		#endregion

		#region Toolbar / Menu Model

		private ActionModelNode ToolbarModel
		{
			get { return _toolbarModel; }
			set
			{
				_toolbarModel = value;
				ToolStripBuilder.Clear(_serverTools.Items);
				if (_toolbarModel != null)
				{
					ToolStripBuilder.ToolStripBuilderStyle style = new ToolStripBuilder.ToolStripBuilderStyle(_toolStripItemDisplayStyle, ToolStripItemAlignment.Left, TextImageRelation.ImageBeforeText);
					ToolStripBuilder.BuildToolbar(_serverTools.Items, _toolbarModel.ChildNodes, style);

					foreach (ToolStripItem item in _serverTools.Items)
						item.DisplayStyle = _toolStripItemDisplayStyle;
				}
			}
		}

		private ActionModelNode MenuModel
		{
			get { return _menuModel; }
			set
			{
				_menuModel = value;
				ToolStripBuilder.Clear(_contextMenu.Items);
				if (_menuModel != null)
				{
					ToolStripBuilder.BuildMenu(_contextMenu.Items, _menuModel.ChildNodes);
				}
			}
		}

		#endregion

		private void SetInitialSelection()
		{
			SelectCurrentServerTreeNode();

			if (this._aeTreeView.SelectedNode == null)
			{
				if (_component.ShowLocalDataStoreNode)
					SelectLocalDataStoreNode();
				else
					SelectRootServerGroupNode();
			}
			else if (_component.ServerTree.CurrentNode is ServerGroup)
			{
				//expand if it's a group
				this._aeTreeView.SelectedNode.Expand();
			}

			_lastClickedNode = _aeTreeView.SelectedNode;
			if (_lastClickedNode != null)
				_component.SetSelection(_lastClickedNode.Tag as IServerTreeNode);
		}

		private void OnSelectedServerChanged(object sender, EventArgs e)
		{
			SelectCurrentServerTreeNode();
		}

		private void SelectLocalDataStoreNode()
		{
			SelectServerTreeNode(_component.ServerTree.RootNode.LocalDataStoreNode);
		}

		private void SelectRootServerGroupNode()
		{
			SelectServerTreeNode(_component.ServerTree.RootNode.ServerGroupNode);
		}

		private void SelectCurrentServerTreeNode()
		{
			SelectServerTreeNode(_component.ServerTree.CurrentNode);
		}

		private void SelectServerTreeNode(IServerTreeNode serverTreeNode)
		{
			TreeNode foundNode = FindNode(serverTreeNode, _aeTreeView.Nodes);
			if (foundNode != null)
				SelectNode(foundNode);
		}

		private TreeNode FindNode(IServerTreeNode findNode, TreeNodeCollection treeNodes)
		{
			foreach (TreeNode treeNode in treeNodes)
			{
				if (treeNode.Tag == findNode)
				{
					return treeNode;
				}
				else
				{
					TreeNode foundTreeNode = FindNode(findNode, treeNode.Nodes);
					if (foundTreeNode != null)
						return foundTreeNode;
				}
			}

			return null;
		}

		//It is *very* important to keep the SelectedNode of the TreeView and _lastClickNode synchronized,
		//and not only that, but _lastClickedNode must be set first, otherwise some odd behaviour can occur.
		//Please use this method to set the SelectedNode in order to avoid these issues.  Because of the 
		//sheer amount of usage of the _lastClickedNode, it is just easier to do things this way at the moment.
		private void SelectNode(TreeNode selectNode)
		{
			_aeTreeView.SelectedNode = _lastClickedNode = selectNode;
		}

		//This method keeps the _lastClickedNode and the TreeView in sync when the user navigates with the cursor keys.
		//This is another part of the reason why we need to use SelectNode to keep the 2 members synchronized.  Otherwise,
		//some very strange behaviour can occur.
		private void AETreeViewAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (_lastClickedNode != e.Node)
			{
				_lastClickedNode = _aeTreeView.SelectedNode;
				_component.SetSelection(e.Node.Tag as IServerTreeNode);
			}
		}

		private void OnLocalDataStoreNodeUpdated(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new EventHandler(OnLocalDataStoreNodeUpdated));
			}
			else
			{
				_aeTreeView.Nodes[0].ToolTipText = _component.ServerTree.RootNode.LocalDataStoreNode.ToString();
			}
		}

        private void OnServerTreeUpdated(object sender, EventArgs e)
        {
            if (_lastClickedNode == null)
                return;

			if (_component.UpdateType == 0)
				return;

			if (_component.UpdateType == (int)ServerUpdateType.Add)
            {
                IServerTreeNode dataChild = _component.ServerTree.CurrentNode;
                AddTreeNode(_lastClickedNode, dataChild);
                _lastClickedNode.Expand();
            }
            else if (_component.UpdateType == (int)ServerUpdateType.Delete)
            {
				TreeNode removeNode = _lastClickedNode;
				SelectNode(_lastClickedNode.Parent);
				removeNode.Remove();
            }
            else if (_component.UpdateType == (int)ServerUpdateType.Edit)
            {
                IServerTreeNode dataNode = _component.ServerTree.CurrentNode;
                _lastClickedNode.Text = dataNode.Name;
                _lastClickedNode.Tag = dataNode;
            	SynchronizeTooltips(_lastClickedNode);
            }

            _component.SetSelection(_lastClickedNode.Tag as IServerTreeNode);

			UpdateServerGroups();
        }

		private void SynchronizeTooltips(TreeNode startNode)
		{
			startNode.ToolTipText = startNode.Tag.ToString();

			foreach (TreeNode node in startNode.Nodes)
				SynchronizeTooltips(node);
		}

		private void AETreeViewClick(object sender, EventArgs e)
		{
			MouseEventArgs args = (MouseEventArgs) e;
			TreeViewHitTestInfo hitTest = _aeTreeView.HitTest(args.Location);
			if (hitTest == null || hitTest.Node == null)
				return;

			if (hitTest.Node.StateImageIndex >= 0 && hitTest.Location == TreeViewHitTestLocations.StateImage)
			{
				if (hitTest.Node.Tag is Server)
					OnServerChecked(hitTest.Node);
				else if (hitTest.Node.Tag is ServerGroup)
					OnServerGroupChecked(hitTest.Node);
			
				_component.ServerTree.FireServerTreeUpdatedEvent();
			}

			if (hitTest.Node == _lastClickedNode)
				return;

			if (hitTest.Location == TreeViewHitTestLocations.Label || hitTest.Location == TreeViewHitTestLocations.Image)
			{
				SelectNode(hitTest.Node);
				_component.SetSelection(_lastClickedNode.Tag as IServerTreeNode);
			}
		}

        /// <summary>
        /// Builds the root and first-level of the tree
        /// </summary>
        private void BuildServerTreeView(TreeView treeView, ClearCanvas.ImageViewer.Services.ServerTree.ServerTree dicomServerTree)
        {
            treeView.Nodes.Clear();
            treeView.ShowNodeToolTips = true;

			if (_component.ShowLocalDataStoreNode)
			{
				// build the localdatastorenode
				TreeNode localDataStoreTreeNode = new TreeNode(dicomServerTree.RootNode.LocalDataStoreNode.Name);
				localDataStoreTreeNode.Tag = dicomServerTree.RootNode.LocalDataStoreNode;
				localDataStoreTreeNode.ToolTipText = dicomServerTree.RootNode.LocalDataStoreNode.ToString();
				SetIcon(dicomServerTree.RootNode.LocalDataStoreNode, localDataStoreTreeNode);
				treeView.Nodes.Add(localDataStoreTreeNode);
			}

            // build the default server group
            TreeNode firstServerGroup = new TreeNode(dicomServerTree.RootNode.ServerGroupNode.Name);
            firstServerGroup.Tag = dicomServerTree.RootNode.ServerGroupNode;
            firstServerGroup.ToolTipText = dicomServerTree.RootNode.ServerGroupNode.ToString();
            SetIcon(dicomServerTree.RootNode.ServerGroupNode, firstServerGroup);
            treeView.Nodes.Add(firstServerGroup);
            BuildNextTreeLevel(firstServerGroup);
			
			firstServerGroup.Expand();//expand main group by default.

			UpdateServerGroups();
		}

        private void BuildNextTreeLevel(TreeNode serverGroupUITreeNode)
        {
            ServerGroup serverGroupNode = serverGroupUITreeNode.Tag as ServerGroup;
            if (null == serverGroupNode)
                return;

            foreach (ServerGroup childServerGroup in serverGroupNode.ChildGroups)
            {
                TreeNode childServerGroupUINode = AddTreeNode(serverGroupUITreeNode, childServerGroup);
                BuildNextTreeLevel(childServerGroupUINode);
            }

            foreach (Server server in serverGroupNode.ChildServers)
            {
                AddTreeNode(serverGroupUITreeNode, server);
            }
        }

        private TreeNode AddTreeNode(TreeNode treeNode, IServerTreeNode dataChild)
        {
            TreeNode treeChild = new TreeNode(dataChild.Name);
            SetIcon(dataChild, treeChild);
            treeChild.Tag = dataChild;
            treeChild.ToolTipText = dataChild.ToString();

			if (treeChild.Tag is Server)
				SetServerCheck(treeChild);

			treeNode.Nodes.Add(treeChild);
            return treeChild;
        }

        private void SetIcon(IServerTreeNode browserNode, TreeNode treeNode)
		{
            if (browserNode == null)
				return;

            if (browserNode.IsLocalDataStore)
			{
				treeNode.ImageIndex = 0;
				treeNode.SelectedImageIndex = 0;
			}
			else if (browserNode.IsServer)
			{
				treeNode.ImageIndex = 1;
				treeNode.SelectedImageIndex = 1;
			}
			else
			{
				treeNode.ImageIndex = 2;
				treeNode.SelectedImageIndex = 2;
			}
		}

        private void TreeViewItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeViewDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TreeViewDragOver(object sender, DragEventArgs e)
        {
            // precondition
            TreeNode underPointerNode = _aeTreeView.GetNodeAt(_aeTreeView.PointToClient(new Point(e.X, e.Y)));
            if (null == underPointerNode)
                return;

            IServerTreeNode underPointerDataNode = underPointerNode.Tag as IServerTreeNode;
            IServerTreeNode lastClickedDataNode = _lastClickedNode.Tag as IServerTreeNode;

            // _lastMouseOverNode was the node that I highlighted last 
            // but its highlight must be turned off
            if ((_lastMouseOverNode != null) && (_lastMouseOverNode != underPointerNode))
            {
                _lastMouseOverNode.BackColor = Color.White;
                _lastMouseOverNode.ForeColor = Color.Black;
            }

            // highlight node only if the target node is a potential place
            // for us to drop a node for moving
			if (_component.CanMoveOrAdd(underPointerDataNode, lastClickedDataNode))
            {
                underPointerNode.BackColor = Color.DarkBlue;
                underPointerNode.ForeColor = Color.White;
		
				_lastMouseOverNode = underPointerNode;
			}
        }

        private void TreeViewDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
           if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt);
                TreeNode draggingNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                IServerTreeNode draggingDataNode = draggingNode.Tag as IServerTreeNode;
                IServerTreeNode destinationDataNode = destinationNode.Tag as IServerTreeNode;

                // turn off the white-on-blue highlight of a destination node
                destinationNode.BackColor = Color.White;
                destinationNode.ForeColor = Color.Black;
                _lastMouseOverNode.BackColor = Color.White;
                _lastMouseOverNode.ForeColor = Color.Black;
 
                // detecting if a node is being moved to the same/current parent, then do nothing
                // or if we're not dragging into a server group, do nothing
                // or if you're dragging over a child group
                //if (destinationDataNode.Path == draggingDataNode.Path ||
                //    !destinationDataNode.IsServerGroup ||
                //    draggingDataNode.Path.IndexOf(destinationDataNode.Path) == -1)  // don't allow dropping a node into one of its own children

				if (!_component.CanMoveOrAdd(destinationDataNode, draggingDataNode))
                    return;

				if (!_component.NodeMoved(destinationDataNode, draggingDataNode))
                    return;

				SelectNode(destinationNode);
				draggingNode.Remove();
				
			   // build up the destination
				destinationNode.Nodes.Clear();
				BuildNextTreeLevel(_lastClickedNode);
				destinationNode.Expand();
			
			   _component.SetSelection(_lastClickedNode.Tag as IServerTreeNode);

			   UpdateServerGroups();
            }
        }

        private void TreeViewNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
			SelectNode(e.Node);
            IServerTreeNode dataNode = _lastClickedNode.Tag as IServerTreeNode;
            _component.SetSelection(dataNode);
            _component.NodeDoubleClick();
        }

    }
}
