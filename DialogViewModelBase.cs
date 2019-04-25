using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace ViewModelLib
{
	/// <summary>
	/// A base class for ViewModels of Dialogs. 
	/// Implements the generic functionality often needed in a dialog ViewModel.
	/// </summary>
	[Export(typeof(IDialogViewModelBase))]
	public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModelBase
	{
		[ImportingConstructor]
		protected DialogViewModelBase(IDialogService dialogService) : base()
		{
			DialogService = dialogService;
			OkCommand = new MyCommand(OnOk, CanExecuteOk);
		}

		public ICommand OkCommand { get; set; }

		protected IDialogService DialogService { get; }

		/// <summary>
		/// Sub classes can override this to do finishing up logic before dialog closes,
		/// or stop the closing by returning false. The sub class must map the ok command to
		/// something for this to have any effect.
		/// NOTE: This method will not be invoked when a dialog is closed by other means.
		/// </summary>
		protected virtual bool Ok()
		{
			return true;
		}

		private bool CanExecuteOk()
		{
			return IsValidated();
		}

		private void OnOk(object obj)
		{
			if (Ok())
			{
				DialogService.CloseDialog(this, true);
			}
		}

		protected void Close()
		{
			DialogService.CloseDialog(this);
		}

		protected void OpenDialog(Type windowType, IDialogViewModelBase vm)
		{
			DialogService.OpenDialogWindow(windowType, vm, this);
		}
	}
}
