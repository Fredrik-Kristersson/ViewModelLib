using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModelLib
{
	public abstract class ViewModelBase : IViewModelBase
	{
		private bool isDirty;
		private readonly ConcurrentDictionary<string, bool> dirtyDictionary =
				new ConcurrentDictionary<string, bool>();
		private readonly ConcurrentDictionary<string, object> propertyValues =
				new ConcurrentDictionary<string, object>();

		private readonly IDictionary<string, List<ValidationFuncMessage>> validators =
				new Dictionary<string, List<ValidationFuncMessage>>();

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsDirty
		{
			get => isDirty;
			set
			{
				if (isDirty == value)
				{
					return;
				}

				isDirty = value;
				if (value)
				{
					dirtyDictionary.Clear();
				}

				OnPropertyChanged("IsDirty");
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Send notify property changed event for all properties.
		/// </summary>
		protected void NotifyAll()
		{
			// NOTE: Dictionary does not necessarily contain all properties,
			// probably need to use reflection to be sure all properties are affected.
			foreach (var propertyValuesKey in propertyValues.Keys)
			{
				OnPropertyChanged(propertyValuesKey);
			}
		}

		protected void Set<T>(T value, [CallerMemberName] string memberName = "")
		{
			var oldValue = Get<T>(memberName);
			if (EqualityComparer<T>.Default.Equals(oldValue, value))
			{
				return;
			}

			propertyValues.AddOrUpdate(memberName, value, (k, v) => value);
			dirtyDictionary.AddOrUpdate(memberName, true, (k, v) => true);
			IsDirty = true;
			OnPropertyChanged(memberName);
		}

		protected T Get<T>([CallerMemberName] string memberName = "")
		{
			return propertyValues.TryGetValue(memberName, out var result) ? (T)result : default(T);
		}

		/// <summary>
		/// Returns true if all the validations pass, false otherwise.
		/// </summary>
		protected bool IsValidated()
		{
			return
					validators.Values.SelectMany(validator => validator)
					.All(validationFuncMessage => !validationFuncMessage.ValidationFunc());
		}

		protected void AddValidator(
				string propertyName, Func<bool> validateFunc, string validationMessage)
		{
			if (!validators.TryGetValue(propertyName, out var validatorList))
			{
				validatorList = new List<ValidationFuncMessage>();
				validators.Add(propertyName, validatorList);
			}

			validatorList.Add(
					new ValidationFuncMessage
					{
						ValidationFunc = validateFunc,
						ValidationMessage = validationMessage
					});
		}

		public virtual string this[string propertyName]
		{
			get
			{
				if (!validators.TryGetValue(propertyName, out var validatorList))
				{
					return string.Empty;
				}

				var error = validatorList.FirstOrDefault(v => v.ValidationFunc());

				return error != null && dirtyDictionary.ContainsKey(propertyName) ? error.ValidationMessage : string.Empty;
			}
		}

		public virtual string Error => throw new NotImplementedException();

		public virtual Task OnUnloadAsync()
		{
			return Task.CompletedTask;
		}

		public virtual Task OnLoadAsync()
		{
			return Task.CompletedTask;
		}

		private class ValidationFuncMessage
		{
			public Func<bool> ValidationFunc { get; set; }

			public string ValidationMessage { get; set; }
		}
	}
}
