using System;
using System.Windows;
using System.Windows.Controls;

namespace ViewModelLib
{
	/// <summary>
	/// A textbox that only accepts (a maximum number of) integers.
	/// The default maximum number of digits is 5, but can be changed via dependencyproperty (or normal property).
	/// </summary>
	public class IntegerTextBox : TextBox
	{
		private const int DefaultMaxNumberIntegers = 5;

		public IntegerTextBox()
				: base()
		{
			MaxNumberIntegers = DefaultMaxNumberIntegers;
			// Select (highlight) all text when focus is gained. Not sure if this is good for all situations...
			AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
		}

		private static void SelectAllText(object sender, RoutedEventArgs e)
		{
			var textBox = e.OriginalSource as TextBox;
			textBox?.SelectAll();
		}

		public int MaxNumberIntegers
		{
			get { return (int)GetValue(MaxNumberIntegersProperty); }
			set { SetValue(MaxNumberIntegersProperty, value); }
		}

		public static readonly DependencyProperty MaxNumberIntegersProperty =
			DependencyProperty.Register("MaxNumberIntegers", typeof(int), typeof(IntegerTextBox));

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (Text.Length > MaxNumberIntegers)
			{
				Text = Text.Substring(0, MaxNumberIntegers);
				SelectionStart = MaxNumberIntegers;
				return;
			}
			var selectionStart = SelectionStart;
			var newText = string.Empty;
			foreach (char c in Text)
			{
				if (char.IsDigit(c) || char.IsControl(c))
				{
					newText += c;
				}
			}
			Text = newText;
			SelectionStart = selectionStart <= Text.Length ? selectionStart : Text.Length;
		}
	}
}
