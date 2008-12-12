﻿#region License

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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Desktop.View.WinForms
{
	/// <summary>
	/// This control may be deprecated and removed in a future release. Use <see cref="GalleryComponent"/> instead.
	/// </summary>
	/// <remarks>
	/// It is strongly recommended that <see cref="GalleryComponent"/>-derived components be used instead of directly using this control.
	/// The <see cref="GalleryComponent"/> model and view duplicates and improves upon the functionality available using this control.
	/// As a result, this control may be deprecated and removed in a future release.
	/// </remarks>
	public partial class GalleryView : UserControl
	{
		private IBindingList _gallery;
		private ActionModelNode _toolbarModel;
		private ActionModelNode _contextMenuModel;
		private event EventHandler _selectionChanged;
		private bool _suppressGalleryChangeEvents = false;
		private bool _dragOutside = false;

		public GalleryView()
		{
			InitializeComponent();

			_listView.LargeImageList = new ImageList();
			_listView.LargeImageList.ImageSize = new Size(100, 100);
			_listView.View = System.Windows.Forms.View.LargeIcon;
			_listView.BackColor = Color.Black;
			_listView.ForeColor = Color.WhiteSmoke;
			_listView.ListViewItemSorter = new ListViewIndexComparer();
			_listView.ItemSelectionChanged += OnSelectionChanged;
			_listView.ItemDrag += OnItemDrag;
			_listView.DragEnter += OnItemDragEnter;
			_listView.DragOver += OnItemDragOver;
			_listView.DragLeave += OnItemDragLeave;
			_listView.DragDrop += OnItemDragDrop;

			_toolStrip.Visible = false;
		}

		public object DataSource
		{
			get
			{
				return _gallery;
			}
			set
			{
				if (_gallery != value)
				{
					_gallery = value as IBindingList;

					if (_gallery == null)
						throw new Exception("DataSource must be an IBindingList of IGalleryItem objects.");

					InitializeBindings();
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ActionModelNode ToolbarModel
		{
			get { return _toolbarModel; }
			set
			{
				_toolbarModel = value;

				// Turn on the toolbar only if a toolbar model exists
				_toolStrip.Visible = true;

				InitializeToolStrip();
			}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ActionModelNode ContextMenuModel
		{
			get { return _contextMenuModel; }
			set
			{
				_contextMenuModel = value;
				ToolStripBuilder.Clear(_contextMenu.Items);
				if (_contextMenuModel != null)
				{
					ToolStripBuilder.BuildMenu(_contextMenu.Items, _contextMenuModel.ChildNodes);
				}
			}
		}

		public ISelection Selection
		{
			get
			{
				List<object> selectedItems = new List<object>();

				//when an item is removed from the list view, the 'selection changed' event fires
				//before the item is removed, so the indices are out of sync and we can't rely on them
				foreach (int index in _listView.SelectedIndices)
				{
					object item = _listView.Items[index].Tag;
					selectedItems.Add(CollectionUtils.SelectFirst(_gallery,
					                                              delegate(object test) { return ((IGalleryItem) test).Item == item; }));
				}

				return new Selection(selectedItems);
			}
		}

		public event EventHandler SelectionChanged
		{
			add { _selectionChanged += value; }
			remove { _selectionChanged -= value; }
		}

		public bool DragReorder
		{
			get { return _listView.AllowDrop; }
			set { _listView.AllowDrop = value; }
		}

		public bool DragOutside
		{
			get { return _dragOutside; }
			set { _dragOutside = value; }
		}

		public bool MultiSelect
		{
			get { return _listView.MultiSelect; }
			set { _listView.MultiSelect = value; }
		}

		public bool ShowDescription
		{
			get { return _listView.View == System.Windows.Forms.View.Tile; }
			set { _listView.View = value ? System.Windows.Forms.View.Tile : System.Windows.Forms.View.LargeIcon; }
		}

		public bool HideSelection
		{
			get { return _listView.HideSelection; }
			set { _listView.HideSelection = value; }
		}

		public int MaxDescriptionLines
		{
			get { return _listView.Columns.Count - 1; }
			set
			{
				if (value >= 0)
				{
					// add and remove columns as necessary to allow the correct number of description lines to show (first column is always the label/name)
					while (_listView.Columns.Count - 1 > value)
					{
						_listView.Columns.RemoveAt(1);
					}
					while (_listView.Columns.Count - 1 < value)
					{
						_listView.Columns.Add("");
					}
				}
			}
		}

		public Size ImageSize
		{
			get
			{
				if (_listView.LargeImageList == null)
					return new Size();
				return _listView.LargeImageList.ImageSize;
			}
			set
			{
				if (_listView.LargeImageList != null)
					_listView.LargeImageList.ImageSize = value;
			}
		}

		public bool AllowRenaming
		{
			get { return _listView.LabelEdit; }
			set { _listView.LabelEdit = value; }
		}

		private void InitializeToolStrip()
		{
			ToolStripBuilder.Clear(_toolStrip.Items);

			if (_toolbarModel != null)
			{
				ToolStripBuilder.BuildToolbar(_toolStrip.Items, _toolbarModel.ChildNodes);
			}
		}

		private void InitializeBindings()
		{
			_gallery.ListChanged += OnListChanged;
			_listView.Items.Clear();

			foreach (object item in _gallery)
				AddItem(item);
		}

		private void OnListChanged(object sender, ListChangedEventArgs e)
		{
			if (_suppressGalleryChangeEvents)
				return;

			switch (e.ListChangedType)
			{
				case ListChangedType.ItemAdded:
					AddItem(_gallery[e.NewIndex]);
					break;
				case ListChangedType.ItemDeleted:
					RemoveItem(e.NewIndex);
					break;
				case ListChangedType.ItemChanged:
					UpdateItem(e.NewIndex);
					break;
				case ListChangedType.ItemMoved:
					MoveItem(e.NewIndex, e.OldIndex);
					break;
				case ListChangedType.Reset:
					ResetList();
					break;
			}
		}

		private void UpdateItem(int index)
		{
			ListViewItem lvi = _listView.Items[index];
			IGalleryItem item = (IGalleryItem) _gallery[index];
			int keyIndex = _listView.LargeImageList.Images.IndexOfKey(lvi.ImageKey);
			_listView.LargeImageList.Images[keyIndex] = item.Image;
			// update name, description
			_listView.RedrawItems(index, index, true);
		}

		private void AddItem(object item)
		{
			IGalleryItem galleryItem = CastToGalleryItem(item);

			string imageKey = Guid.NewGuid().ToString();
			_listView.LargeImageList.Images.Add(imageKey, galleryItem.Image);
			ListViewItem lvi = new ListViewItem(galleryItem.Name, imageKey);
			foreach (string line in galleryItem.Description.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
				)
			{
				lvi.SubItems.Add(line, Color.Gray, Color.Transparent, _listView.Font);
			}
			lvi.Tag = galleryItem.Item;
			_listView.Items.Add(lvi);
		}

		private void RemoveItem(int index)
		{
			ListViewItem lvi = _listView.Items[index];
			_listView.LargeImageList.Images.RemoveByKey(lvi.ImageKey);
			_listView.Items.RemoveAt(index);

			if (_listView.Items.Count > 0)
			{
				int i = Math.Min(index, _listView.Items.Count - 1);
				_listView.Items[i].Selected = true;
			}
		}

		private void MoveItem(int index, int oldIndex)
		{
			ListViewItem lvi = _listView.Items[oldIndex];
			_listView.Items.RemoveAt(oldIndex);
			_listView.Items.Insert(index, lvi);
			_listView.Items[index].Selected = true;
		}

		private void ResetList()
		{
			_listView.Items.Clear();
			_listView.LargeImageList.Images.Clear();

			foreach (object item in _gallery)
				AddItem(item);
		}

		private static IGalleryItem CastToGalleryItem(object item)
		{
			IGalleryItem galleryItem = item as IGalleryItem;

			if (galleryItem == null)
				throw new InvalidCastException("DataSource must be an IBindingList of IGalleryItem objects.");

			return galleryItem;
		}

		private void OnSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			EventsHelper.Fire(_selectionChanged, this, EventArgs.Empty);
		}

		private ListViewItem ExtractListViewItem(IDataObject dataObject)
		{
			if (dataObject.GetDataPresent(typeof (ListViewItem)))
			{
				ListViewItem item = (ListViewItem) dataObject.GetData(typeof (ListViewItem));
				if (item.ListView == _listView)
					return item;
			}

			return null;
		}

		private void OnListViewResize(object sender, EventArgs e)
		{
			// force tile sizing to fit within the control without horizontal scrolling
			const int tileSpacing = 4;
			_listView.TileSize = new Size(
				Math.Max(3*this.ImageSize.Width + tileSpacing, _listView.ClientSize.Width - 2*tileSpacing),
				this.ImageSize.Height + tileSpacing
				);
		}

		private void OnItemDrag(object sender, ItemDragEventArgs e)
		{
			// Only allow dragging of one item at a time, so deselect all other items
			foreach (ListViewItem lvi in _listView.Items)
			{
				if (lvi != e.Item)
					lvi.Selected = false;
			}

			ListViewItem draggedItem = (ListViewItem) e.Item;

			DataObject data = new DataObject();
			if (DragOutside)
				data.SetData(draggedItem.Tag);
			if (DragReorder)
				data.SetData(draggedItem);

			_listView.DoDragDrop(data, DragDropEffects.Move);
		}

		private void OnItemDragEnter(object sender, DragEventArgs e)
		{
			if (ExtractListViewItem(e.Data) != null)
				e.Effect = e.AllowedEffect;
			else
				e.Effect = DragDropEffects.None;
		}

		private void OnItemDragOver(object sender, DragEventArgs e)
		{
			// Retrieve the client coordinates of the mouse pointer.
			Point targetPoint = _listView.PointToClient(new Point(e.X, e.Y));

			int targetIndex = -1;

			ListViewItem item = ExtractListViewItem(e.Data);
			if (item != null)
			{
				// Retrieve the index of the item closest to the mouse pointer.
				targetIndex = _listView.InsertionMark.NearestIndex(targetPoint);

				// Confirm that the mouse pointer is not over the dragged item.
				if (targetIndex > -1)
				{
					// Determine whether the mouse pointer is to the left(top) or
					// the right(bottom) of the midpoint of the closest item and set
					// the InsertionMark.AppearsAfterItem property accordingly.
					Rectangle itemBounds = _listView.GetItemRect(targetIndex);
					if (_listView.View == System.Windows.Forms.View.LargeIcon)
					{
						if (targetPoint.X > itemBounds.Left + (itemBounds.Width/2))
						{
							_listView.InsertionMark.AppearsAfterItem = true;
						}
						else
						{
							_listView.InsertionMark.AppearsAfterItem = false;
						}
					}
					else
					{
						if (targetPoint.Y > itemBounds.Top + (itemBounds.Height/2))
						{
							_listView.InsertionMark.AppearsAfterItem = true;
						}
						else
						{
							_listView.InsertionMark.AppearsAfterItem = false;
						}
					}
				}
			}

			// Set the location of the insertion mark. If the mouse is
			// over the dragged item, the targetIndex value is -1 and
			// the insertion mark disappears.
			_listView.InsertionMark.Index = targetIndex;
		}

		private void OnItemDragLeave(object sender, EventArgs e)
		{
			_listView.InsertionMark.Index = -1;
		}

		private void OnItemDragDrop(object sender, DragEventArgs e)
		{
			// Retrieve the index of the insertion mark;
			int targetIndex = _listView.InsertionMark.Index;

			// If the insertion mark is not visible, exit the method.
			if (targetIndex == -1)
			{
				return;
			}

			// If the insertion mark is to the right of the item with
			// the corresponding index, increment the target index.
			if (_listView.InsertionMark.AppearsAfterItem)
			{
				targetIndex++;
			}

			ListViewItem draggedItem = ExtractListViewItem(e.Data);
			int draggedIndex = draggedItem.Index;

			// Insert a copy of the dragged item at the target index.
			// A copy must be inserted before the original item is removed
			// to preserve item index values. 
			_listView.Items.Insert(targetIndex, (ListViewItem) draggedItem.Clone());

			// Remove the original copy of the dragged item.
			_listView.Items.Remove(draggedItem);

			_suppressGalleryChangeEvents = true;

			// Alter the index if removing before inserting will change the meaning of the target index
			if (draggedIndex < targetIndex)
				targetIndex--;

			object o = _gallery[draggedIndex];

			// Move the corresponding data object by removing the original dragged item.
			_gallery.RemoveAt(draggedIndex);

			// then inserting the dragged item
			_gallery.Insert(targetIndex, o);

			_suppressGalleryChangeEvents = false;
		}

		private void OnAfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			try
			{
				((IGalleryItem) _gallery[e.Item]).Name = e.Label;
			}
			catch (Exception)
			{
				// if editing the name on the item fails, abort the label change
				e.CancelEdit = true;
			}
		}

		// Sorts ListViewItem objects by index.
		private class ListViewIndexComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((ListViewItem) x).Index - ((ListViewItem) y).Index;
			}
		}

	}
}
