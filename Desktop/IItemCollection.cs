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
using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Desktop
{
    /// <summary>
    /// Enumerates the types of item changes.
    /// </summary>
    public enum ItemChangeType
    {
        /// <summary>
        /// An item was added to the table.
        /// </summary>
        ItemAdded,

        /// <summary>
        /// An item in the table was changed.
        /// </summary>
        ItemChanged,

        /// <summary>
        /// An item was removed from the table.
        /// </summary>
        ItemRemoved,

        /// <summary>
        /// All items in the table may have changed.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Provides data for the <see cref="IItemCollection.ItemsChanged"/> event.
    /// </summary>
    public class ItemChangedEventArgs : EventArgs
    {
        private readonly object _item;
        private readonly int _itemIndex;
        private readonly ItemChangeType _changeType;

        internal ItemChangedEventArgs(ItemChangeType changeType, int itemIndex, object item)
        {
            _changeType = changeType;
            _itemIndex = itemIndex;
            _item = item;
        }

        /// <summary>
        /// Gets the type of change that occured.
        /// </summary>
        public ItemChangeType ChangeType
        {
            get { return _changeType; }
        }

        /// <summary>
        /// Gets the index of the item that changed.
        /// </summary>
        public int ItemIndex
        {
            get { return _itemIndex; }
        }

        /// <summary>
        /// Gets the item that has changed.
        /// </summary>
        public object Item
        {
            get { return _item; }
        }
    }

    /// <summary>
    /// Defines the interface to the collection of items.
    /// </summary>
    public interface IItemCollection : IList
    {
        /// <summary>
        /// Occurs when an item in the collection has changed.
        /// </summary>
        event EventHandler<ItemChangedEventArgs> ItemsChanged;

        /// <summary>
        /// Notifies the table that the item at the specified index has changed in some way.
        /// </summary>
        /// <remarks>
		/// Use this method to cause the view to update itself to reflect the changed item.
		/// </remarks>
        void NotifyItemUpdated(int index);

        /// <summary>
        /// Adds all items in the specified enumeration..
        /// </summary>
        void AddRange(IEnumerable enumerable);
    }

    /// <summary>
    /// Defines the interface to the collection of items.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    public interface IItemCollection<TItem> : IItemCollection, IList<TItem>
    {
        /// <summary>
        /// Notifies the table that the specified item has changed in some way.
        /// </summary>
        /// <remarks>
		/// Use this method to cause the view to update itself to reflect the changed item.
		/// </remarks>
        void NotifyItemUpdated(TItem item);

        /// <summary>
        /// Adds all items in the specified enumeration.
        /// </summary>
        void AddRange(IEnumerable<TItem> enumerable);

        /// <summary>
        /// Sorts items in the collection using the specified <see cref="IComparer{TItem}"/>.
        /// </summary>
        void Sort(IComparer<TItem> comparer);

        /// <summary>
        /// Sets any items in the collection matching the specified constraint to the specified new value. 
        /// </summary>
        /// <param name="constraint">A predicate against which all items in the collection will be compared, and replaced with the new value.</param>
        /// <param name="newValue">The new value with which to replace all matching items in the collection.</param>
        void Replace(Predicate<TItem> constraint, TItem newValue);

        /// <summary>
        /// Searches the collection for an item that satisfies the specified constraint and returns
        /// the index of the first such item.
        /// </summary>
        /// <returns>The index of the first matching item, or -1 if no matching items are found.</returns>
        int FindIndex(Predicate<TItem> constraint);
    }
}
