using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModelLib
{
	public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IViewModelBase
	{
		private readonly ConcurrentDictionary<string, object> propertyValues =
			new ConcurrentDictionary<string, object>();

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void Set(object value, [CallerMemberName] string memberName = "")
		{
			propertyValues.AddOrUpdate(memberName, value, (k, v) => value);
			OnPropertyChanged(memberName);
		}

		protected T Get<T>([CallerMemberName] string memberName = "")
		{
			object result;
			if (propertyValues.TryGetValue(memberName, out result))
			{
				return (T)result;
			}

			return default(T);
		}

		public virtual string Error => throw new NotImplementedException();

		public virtual string this[string columnName] => throw new NotImplementedException();
	}
}
