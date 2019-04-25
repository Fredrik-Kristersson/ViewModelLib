using System;
using System.Windows;

namespace ViewModelLib
{
	public interface IDialogService
	{
		Window MainWindow { get; set; }

		bool OpenFileDialog(string filter, out string filePath, string initialDirectory = "");

		bool OpenFileDialog(string filter, bool multiSelect, out string[] filePaths, string initialDirectory = "");

		string OpenFolderDialog(string initialDir);

		MessageBoxResult ShowMessageBox(string title, string information, Window owner = null);

		MessageBoxResult ShowMessageBox(
				string title,
				string information,
				MessageBoxButton button,
				MessageBoxImage image,
				Window owner = null);

		MessageBoxResult ShowMessageBox(
				IDialogViewModelBase owner,
				string title,
				string information,
				MessageBoxButton button,
				MessageBoxImage image);

		bool? OpenDialogWindow(Type windowType, IDialogViewModelBase viewModel, IDialogViewModelBase owner = null);

		void OpenWindow(Type windowType, IDialogViewModelBase viewModel, IDialogViewModelBase owner = null);

		void CloseDialog(IDialogViewModelBase vm, bool dialogResult = true);
	}
}
