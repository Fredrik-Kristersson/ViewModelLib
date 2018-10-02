using System;
using System.Windows.Input;

namespace ViewModelLib
{
	/// <summary>
	/// A simple dialog (window) ViewModel, the dialog is just off the edge (xpos+width) 
	/// of its parent dialog(window) the y-pos can be set. It contains an IntegerTextBox (3 digits max), 
	/// a close without saving command, and a close & save command. 
	/// This ViewModel could (should) be inherited from and customized with the supplied methods and variables.
	/// </summary>
	public class IntegerTextBoxDialogViewModel : ViewModelBase
	{
		public static int NO_RESULT = -1;
		protected static string INPUT_VALUE_OK = string.Empty;

		private IDialogService dlgService;
		private IPositionInfo posInfo;
		private string textBoxText = string.Empty;
		private bool isError = false;
		private double yOffset = 0;
		private string startValue = string.Empty;

		private MyCommand closeSaveCommand;
		private MyCommand closeNoSaveCommand;

		/// <summary>
		/// A dialog (window) just off the edge (xpos+width) of its parent dialog(window), 
		/// the y-axis can be altered via yOffset.
		/// TODO: It should probably be possible to change the width(/height) of the dialog.
		/// </summary>
		/// <param name="dlgService"></param>
		/// <param name="posInfo"></param>
		/// <param name="yOffset">Determines how far from the top the dialog will appear in y-axis</param>
		public IntegerTextBoxDialogViewModel(IDialogService dlgService, IPositionInfo posInfo, double yOffset)
		{
			this.dlgService = dlgService;
			this.posInfo = posInfo;
			this.yOffset = yOffset;

			closeSaveCommand = new MyCommand(CloseSave, CanCloseSave);
			closeNoSaveCommand = new MyCommand(CloseNoSave);
		}

		/// <summary>
		/// A dialog (window) just off the edge (xpos+width) of its parent dialog(window), the y-axis can be altered via yOffset.
		/// TODO: It should probably be possible to change the width(/height) of the dialog.
		/// </summary>
		/// <param name="dlgService"></param>
		/// <param name="posInfo"></param>
		/// <param name="yOffset">Determines how far from the top the dialog will appear in y-axis</param>
		/// <param name="startValue">The value the textbox should have when opening dialog</param>		
		public IntegerTextBoxDialogViewModel(IDialogService dlgService, IPositionInfo posInfo, double yOffset, int startValue)
			: this(dlgService, posInfo, yOffset)
		{
			this.startValue = startValue + string.Empty;
			TextBoxText = this.startValue;
		}

		public int Result { get; set; }

		#region simple binding properties

		public string TextBoxText
		{
			get { return textBoxText; }
			set
			{
				textBoxText = value;
				OnPropertyChanged("TextBoxText");
			}
		}

		public double XPosition
		{
			get
			{
				return posInfo.XPosition + posInfo.OwnerWidth;
			}
			set
			{
				// See Xaml comments in dialog why we need this
				OnPropertyChanged("XPosition");
			}
		}
		public double YPosition
		{
			get
			{
				return posInfo.YPosition + yOffset;
			}
			set
			{
				// See Xaml comments in dialog why we need this
				OnPropertyChanged("YPosition");
			}
		}
		#endregion

		#region Commands

		public ICommand CloseSaveCommand { get { return closeSaveCommand; } }

		private void CloseSave(object parameter)
		{
			if (!isError)
			{
				// If there is no error textBoxText is an int, -> no exception checking
				Result = Int32.Parse(textBoxText);
				dlgService.CloseDialog(this, true);
			}
		}

		/// <summary>
		/// Override this in subclass if some values are not ok to close and save dialog with.
		/// </summary>
		/// <returns></returns>
		protected virtual bool CanCloseSave()
		{
			return true;
		}

		public ICommand CloseNoSaveCommand { get { return closeNoSaveCommand; } }

		private void CloseNoSave(object parameter)
		{
			Result = NO_RESULT;
			dlgService.CloseDialog(this, false);
		}

		#endregion

		/// <summary>
		/// Override this for a more detailed error checking of the value in the textbox
		/// </summary>
		/// <returns></returns>
		protected virtual string CheckInput(int textBoxValue)
		{
			return INPUT_VALUE_OK;
		}

		#region IDataErrorInfo

		public override string Error
		{
			get { return ""; }
		}

		public override string this[string columnName]
		{
			get
			{
				if (columnName == "TextBoxText")
				{
					int intTextBoxValue = -1;
					if (textBoxText == null || string.Empty.Equals(textBoxText))
					{
						isError = true;
						return null;
					}
					try
					{
						if (((string)textBoxText).Length > 0)
							intTextBoxValue = Int32.Parse((String)textBoxText);
					}
					catch (Exception e)
					{
						isError = true;
						//throw new Exception("Illegal characters or " + e.Message);
						return "Illegal characters or " + e.Message;
					}

					string checkValue = CheckInput(intTextBoxValue);
					if (checkValue != INPUT_VALUE_OK)
					{
						isError = true;
						return checkValue;
					}
				}
				isError = false;
				return null;
			}
		}

		#endregion
	}
}
