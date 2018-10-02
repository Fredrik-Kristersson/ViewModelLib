using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ViewModelLib
{
	public class DefaultDialogService : IDialogService
	{
		// TODO: Implement own error/msg dialog. 

		private Dictionary<IViewModelBase, Window> openDialogWindows = new Dictionary<IViewModelBase, Window>();

		public DefaultDialogService()
		{
		}

		public Window MainWindow { get; set; }

		public string OpenFileDialog(string filter)
		{
			OpenFileDialog dlg = new OpenFileDialog
			{
				Multiselect = false,
				Filter = filter
			};


			bool? result = dlg.ShowDialog(MainWindow);

			if (result == true)
			{
				string filename = dlg.FileName;
				return filename;
			}
			return null;
		}

		public string OpenFolderDialog(string initialDir)
		{
			var dlg = new FolderBrowserDialog();
			dlg.SelectedPath = initialDir;

			var result = dlg.ShowDialog();

			return result == DialogResult.OK ? dlg.SelectedPath : null;
		}

		public MessageBoxResult ShowMessageBox(string title, string information, Window owner = null)
		{
			return MessageBox.Show(owner ?? MainWindow, information, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public MessageBoxResult ShowMessageBox(
			string title,
			string information,
			MessageBoxButton button,
			MessageBoxImage image,
			Window owner = null)
		{
			return MessageBox.Show(owner ?? MainWindow, information, title, button, image);
		}

		public MessageBoxResult ShowMessageBox(ViewModelBase owner, string title, string information, MessageBoxButton button, MessageBoxImage image)
		{
			openDialogWindows.TryGetValue(owner, out Window ownerWindow);
			return ShowMessageBox(title, information, button, image, ownerWindow);
		}

		public bool? OpenDialogWindow(Type windowType, IViewModelBase viewModel, ViewModelBase owner = null)
		{
			if (openDialogWindows.ContainsKey(viewModel))
			{
				openDialogWindows.Remove(viewModel);
			}

			Window ownerWindow = null;
			if (owner != null)
			{
				openDialogWindows.TryGetValue(owner, out ownerWindow);
			}

			Assembly asm = Assembly.GetAssembly(windowType);
			Window dialog = (Window)asm.CreateInstance(windowType.FullName);
			dialog.Owner = ownerWindow ?? MainWindow;
			dialog.DataContext = viewModel;
			openDialogWindows.Add(viewModel, dialog);
			dialog.ShowActivated = true;
			return dialog.ShowDialog();
		}

		/// <summary>
		/// Closes the dialog corresponding with ViewModel vm.
		/// Possible "memory leak", because this method may not always be called when a window closes 
		/// (i.e. not removed from dictionary).
		/// </summary>
		/// <param name="vm">The corresponding view model</param>
		/// <param name="accepted">bool that specifies whether the activity was accepted (true) or canceled (false)</param>
		public void CloseDialog(IViewModelBase vm, bool accepted)
		{
			if (openDialogWindows.ContainsKey(vm))
			{
				Window dialog = openDialogWindows[vm];
				dialog.DialogResult = accepted;
				dialog.Close();
				openDialogWindows.Remove(vm);
			}
		}
	}
}
