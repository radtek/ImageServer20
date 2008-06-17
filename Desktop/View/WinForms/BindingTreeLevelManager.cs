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
using System.Windows.Forms;
using ClearCanvas.Desktop.Trees;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Desktop.View.WinForms
{
	/// <summary>
	/// Manages a single level of a tree view, listening for changes to the underlying model and updating the tree view
	/// as required.  This class is used internally and is not intended to be used directly by application code.
	/// </summary>
	internal class BindingTreeLevelManager : IDisposable
    {
        private readonly ITree _tree;
        private readonly TreeNodeCollection _nodeCollection;
		private readonly TreeView _treeView;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodeCollection"></param>
        /// <param name="treeView"></param>
        public BindingTreeLevelManager(ITree tree, TreeNodeCollection nodeCollection, TreeView treeView)
        {
            _tree = tree;
            _tree.Items.ItemsChanged += TreeItemsChangedEventHandler;
            _nodeCollection = nodeCollection;
        	_treeView = treeView;

            BuildLevel();
        }

		public void Dispose()
		{
			_tree.Items.ItemsChanged -= TreeItemsChangedEventHandler;
		}

        /// <summary>
        /// Handles changes to the tree's items collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeItemsChangedEventHandler(object sender, ItemChangedEventArgs e)
        {
            switch (e.ChangeType)
            {
                case ItemChangeType.ItemAdded:
                    AddNode(_tree.Items[e.ItemIndex]);
                    break;
                case ItemChangeType.ItemChanged:
                    UpdateNode(e.ItemIndex, _tree.Items[e.ItemIndex]);
                    break;
                case ItemChangeType.ItemRemoved:
                    RemoveNode(e.ItemIndex);
                    break;
                case ItemChangeType.Reset:
                    BuildLevel();
                    break;
            }
        }

        /// <summary>
        /// Adds a node for the specified item
        /// </summary>
        /// <param name="item"></param>
        private void AddNode(object item)
        {
            _nodeCollection.Add(new BindingTreeNode(_tree, item, _treeView));
        }

        /// <summary>
        /// Updates the node at the specified index, with the specified item
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        private void UpdateNode(int index, object item)
        {
            BindingTreeNode node = (BindingTreeNode)_nodeCollection[index];
            node.DataBoundItem = item;
            node.UpdateDisplay();   // force update, even if it is the same item, because its properties may have changed
        }

        /// <summary>
        /// Removes the node at the specified index
        /// </summary>
        /// <param name="index"></param>
        private void RemoveNode(int index)
        {
			BindingTreeNode node = (BindingTreeNode)_nodeCollection[index];
            _nodeCollection.RemoveAt(index);
			node.Dispose();
        }

        /// <summary>
        /// Builds or rebuilds the entire level
        /// </summary>
        private void BuildLevel()
        {
			// dispose of all existing tree nodes before clearing the collection
        	foreach (TreeNode node in _nodeCollection)
        	{
        		if(node is IDisposable)
					(node as IDisposable).Dispose();
        	}

            _nodeCollection.Clear();

			// create new node for each item
            foreach (object item in _tree.Items)
            {
                BindingTreeNode node = new BindingTreeNode(_tree, item, _treeView);
                _nodeCollection.Add(node);
                node.UpdateDisplay();
            }
        }

    }
}
