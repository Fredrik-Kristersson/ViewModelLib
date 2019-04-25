using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ViewModelLib
{
	[Export(typeof(IDialogService))]
	public class DefaultDialogService : IDialogService
	{
		private readonly ILogger logger;
		// TODO: Implement own error/msg dialog. 

		private readonly Dictionary<IDialogViewModelBase, Window> openDialogWindows =
				new Dictionary<IDialogViewModelBase, Window>();

		private ILog log;

		public DefaultDialogService() : this(new YeOldeLogger())
		{

		}

		[ImportingConstructor]
		public DefaultDialogService(ILogger logger)
		{
			this.logger = logger;
			log = logger.GetLogger(GetType());
		}

		public Window MainWindow { get; set; }

		public bool OpenFileDialog(string filter, out string filePath, string initialDirectory = "")
		{
			string[] filePaths;
			filePath = null;
			var result = OpenFileDialog(filter, false, out filePaths, initialDirectory);
			if (result)
			{
				filePath = filePaths[0];
			}

			return result;
		}

		public bool OpenFileDialog(string filter, bool multiSelect, out string[] filePaths, string initialDirectory = "")
		{
			filePaths = new string[] { };
			OpenFileDialog dlg = new OpenFileDialog
			{
				Multiselect = multiSelect,
				Filter = filter,
				InitialDirectory = initialDirectory
			};

			bool? result = dlg.ShowDialog(MainWindow);

			if (result == true)
			{
				var filenames = dlg.FileNames;
				filePaths = filenames;
			}
			return result ?? false;
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

		public MessageBoxResult ShowMessageBox(IDialogViewModelBase owner, string title, string information, MessageBoxButton button, MessageBoxImage image)
		{
			openDialogWindows.TryGetValue(owner, out Window ownerWindow);
			return ShowMessageBox(title, information, button, image, ownerWindow);
		}

		public bool? OpenDialogWindow(Type windowType, IDialogViewModelBase viewModel, IDialogViewModelBase owner = null)
		{
			return ConfigureOpenWindow(windowType, viewModel, owner).ShowDialog();
		}

		public void OpenWindow(Type windowType, IDialogViewModelBase viewModel, IDialogViewModelBase owner = null)
		{
			ConfigureOpenWindow(windowType, viewModel, owner).Show();
		}

		private Window ConfigureOpenWindow(Type windowType, IDialogViewModelBase viewModel, IDialogViewModelBase owner = null)
		{
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
			dialog.Closed += DialogOnClosed;
			dialog.ShowActivated = true;
			if (MainWindow == null)
			{
				MainWindow = dialog;
			}

			return dialog;
		}

		public void CloseDialog(IDialogViewModelBase vm, bool dialogResult = true)
		{
			if (openDialogWindows.TryGetValue(vm, out Window window))
			{
				window.DialogResult = dialogResult;
				window.Close();
			}
		}

		private void DialogOnClosed(object sender, EventArgs e)
		{
			if (!(sender is Window window))
			{
				return;
			}

			var vm = openDialogWindows.FirstOrDefault(k => window.Equals(k.Value)).Key;
			if (vm != null)
			{
				var result = openDialogWindows.Remove(vm);
				log.Info($"Removing window for {vm.GetType()}, result: {result}");
			}

			window.Closed -= DialogOnClosed;
		}
	}
}
