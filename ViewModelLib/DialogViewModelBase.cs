using System.Windows.Input;

namespace ViewModelLib
{
	/// <summary>
	/// A base class for ViewModels of Dialogs. 
	/// Implements the generic functionality often needed in a dialog ViewModel.
	/// Not intended for Main window to inherit from this class.
	/// </summary>
	public abstract class DialogViewModelBase : ViewModelBase
	{
		private IDialogService dlgService;

		private readonly MyCommand closeNoSaveCommand;
		private readonly MyCommand closeSaveCommand;

		protected DialogViewModelBase(IDialogService dialogService)
		{
			dlgService = dialogService;
			closeNoSaveCommand = new MyCommand(CloseNoSave);
			closeSaveCommand = new MyCommand(CloseSave, CanCloseSave);
		}

		public ICommand CloseNoSaveCommand => closeNoSaveCommand;

		private void CloseNoSave(object parameter)
		{
			DoCloseNoSave(parameter);
			dlgService.CloseDialog(this, false);
		}

		public ICommand CloseSaveCommand => closeSaveCommand;

		private void CloseSave(object parameter)
		{
			bool result = DoCloseSave();
			if (result)
			{
				dlgService.CloseDialog(this, true);
			}
			else
			{
				//Do something to fix error...
			}
		}

		/// <summary>
		/// Override this to do something before dialog closes, but! -
		/// this represents (most likely) a command that should (probably) not save values if user has selected it.
		/// E.g. cancel action.
		/// </summary>
		protected virtual void DoCloseNoSave(object parameter) { }


		/// <summary>
		/// Override this to do something before dialog closes, but! -
		/// this represents (most likely) a command that should (probably) save values if user has selected it.
		/// E.g. ok action.
		/// </summary>
		/// <returns>True if dialog can/should close, false otherwise</returns>
		protected virtual bool DoCloseSave()
		{
			return true;
		}

		protected virtual bool CanCloseSave()
		{
			return true;
		}
	}
}
