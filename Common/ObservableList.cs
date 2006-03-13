using System;
using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Common
{
	public class ObservableList<TItem, TItemEventArgs> 
		: IObservableList<TItem, TItemEventArgs>
		where TItem : class
		where TItemEventArgs : CollectionEventArgs<TItem>, new()
	{
		private List<TItem> m_List = new List<TItem>();
		private event EventHandler<TItemEventArgs> m_ItemAddedEvent;
		private event EventHandler<TItemEventArgs> m_ItemRemovedEvent;

		public ObservableList()
		{

		}

		public ObservableList(ObservableList<TItem, TItemEventArgs> list)
		{
			foreach (TItem item in list)
				this.Add(item);
		}

		#region IObservableList<TItem, TItemEventArgs> Members

		public event EventHandler<TItemEventArgs> ItemAdded
		{
			add { m_ItemAddedEvent += value; }
			remove { m_ItemAddedEvent -= value;	}
		}

		public event EventHandler<TItemEventArgs> ItemRemoved
		{
			add { m_ItemRemovedEvent += value; }
			remove { m_ItemRemovedEvent -= value; }
		}

		#endregion

		#region IList<T> Members

		public int IndexOf(TItem item)
		{
			Platform.CheckForNullReference(item, "item");

			return m_List.IndexOf(item);
		}

		public void Insert(int index, TItem item)
		{
			Platform.CheckArgumentRange(index, 0, this.Count - 1, "index");

			if (m_List.Contains(item))
				return;

			m_List.Insert(index, item);

			TItemEventArgs args = new TItemEventArgs();
			args.Item = item;
			OnItemAdded(args);
		}

		public void RemoveAt(int index)
		{
			Platform.CheckArgumentRange(index, 0, this.Count - 1, "index");

			TItem itemToRemove = this[index];
			m_List.RemoveAt(index);

			TItemEventArgs args = new TItemEventArgs();
			args.Item = itemToRemove;
			OnItemRemoved(args);
		}

		public TItem this[int index]
		{
			get
			{
				Platform.CheckIndexRange(index, 0, this.Count - 1, "index");
				return m_List[index];
			}
			set
			{
				Platform.CheckIndexRange(index, 0, this.Count - 1, "index");
				m_List[index] = value;
			}
		}

		#endregion

		#region ICollection<TItem> Members

		public void Add(TItem item)
		{
			if (m_List.Contains(item))
				return;

			m_List.Add(item);

			TItemEventArgs args = new TItemEventArgs();
			args.Item = item;
			OnItemAdded(args);
		}

		public void Clear()
		{
			// If we don't have any subscribers to the ItemRemovedEvent, then
			// make it faster and just call Clear().
			if (m_ItemRemovedEvent == null)
			{
				m_List.Clear();
			}
			// But if we do, then we have to remove items one by one so that
			// subscribers are notified.
			else
			{
				while (this.Count > 0)
				{
					int lastIndex = this.Count - 1;
					RemoveAt(lastIndex);
				}
			}
		}

		public bool Contains(TItem item)
		{
			Platform.CheckForNullReference(item, "item");

			return m_List.Contains(item);
		}

		public void CopyTo(TItem[] array, int arrayIndex)
		{
			m_List.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return m_List.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(TItem item)
		{
			Platform.CheckForNullReference(item, "item");

			bool result = m_List.Remove(item);

			// Only raise event if the item was actually removed
			if (result == true)
			{
				TItemEventArgs args = new TItemEventArgs();
				args.Item = item;
				OnItemRemoved(args);
			}

			return result;
		}

		#endregion

		#region IEnumerable<TItem> Members

		public IEnumerator<TItem> GetEnumerator()
		{
			return (m_List as IEnumerable<TItem>).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion

		protected virtual void OnItemAdded(TItemEventArgs e)
		{
			EventsHelper.Fire(m_ItemAddedEvent, this, e);
		}

		protected virtual void OnItemRemoved(TItemEventArgs e)
		{
			EventsHelper.Fire(m_ItemRemovedEvent, this, e);
		}
	}
}
