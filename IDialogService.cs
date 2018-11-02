using System;
using System.Windows;

namespace ViewModelLib
{
	public interface IDialogService
	{
		Window MainWindow { get; set; }

		string OpenFileDialog(string filter);

		string OpenFolderDialog(string initialDir);

		MessageBoxResult ShowMessageBox(string title, string information, Window owner = null);

		MessageBoxResult ShowMessageBox(
			string title,
			string information,
			MessageBoxButton button,
			MessageBoxImage image,
			Window owner = null);

		MessageBoxResult ShowMessageBox(
			ViewModelBase owner,
			string title,
			string information,
			MessageBoxButton button,
			MessageBoxImage image);

		bool? OpenDialogWindow(Type windowType, IViewModelBase viewModel, ViewModelBase owner = null);

		/// <summary>
		/// Closes the dialog corresponding with ViewModel vm.
		/// Possible "memory leak", because this method may not always be called when a window closes 
		/// (i.e. not removed from dictionary).
		/// </summary>
		/// <param name="vm">The corresponding view model</param>
		/// <param name="accepted">bool that specifies whether the activity was accepted (true) or canceled (false)</param>
		void CloseDialog(IViewModelBase vm, bool accepted);
	}
}
