using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ViewModelLib
{
	public class RangeObservableCollection<T> : ObservableCollection<T>
	{
		private bool suppressNotification = false;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!suppressNotification)
				base.OnCollectionChanged(e);
		}

		public void AddRange(IEnumerable<T> list)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			suppressNotification = true;

			foreach (T item in list)
			{
				Add(item);
			}
			suppressNotification = false;
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void RemoveRange(IEnumerable<T> list)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			suppressNotification = true;

			var removeCopy = list.ToArray();
			foreach (T item in removeCopy)
			{
				Remove(item);
			}
			suppressNotification = false;
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}