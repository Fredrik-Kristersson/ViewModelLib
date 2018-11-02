using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ViewModelLib
{
	/// <summary>
	/// Interaction logic for TwoIntegerNumericUpDown.xaml
	/// </summary>
	public partial class TwoIntegerNumericUpDown : UserControl
	{
		private static int DEFAULT_MAX_VALUE = 99;
		private static int DEFAULT_BUTTONS_INTERVAL = 10;

		public TwoIntegerNumericUpDown()
		{
			InitializeComponent();
			MaxValue = DEFAULT_MAX_VALUE;
			textBoxValue.Text = "00";
			upButton.Interval = DEFAULT_BUTTONS_INTERVAL;
			DownButton.Interval = DEFAULT_BUTTONS_INTERVAL;
		}

		#region Dependency Properties

		public string Text 
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
		  DependencyProperty.Register("Text", typeof(string), typeof(TwoIntegerNumericUpDown),
		  new FrameworkPropertyMetadata(
			"00",
			new PropertyChangedCallback(OnTextPropertyChanged)));

		private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//To be called whenever the DP is changed.
			TwoIntegerNumericUpDown tinup = sender as TwoIntegerNumericUpDown;
			if (tinup != null && e.NewValue != null)
			{
				tinup.textBoxValue.Text = e.NewValue.ToString();				
			}
		}

		public int MaxValue
		{
			get { return (int)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}

		public static readonly DependencyProperty MaxValueProperty =
		  DependencyProperty.Register("MaxValue", typeof(int), typeof(TwoIntegerNumericUpDown));

		public int ButtonsInterval
		{
			get { return (int)GetValue(ButtonsIntervalProperty); }
			set
			{
				SetValue(ButtonsIntervalProperty, value);
			}
		}

		public static readonly DependencyProperty ButtonsIntervalProperty =
		  DependencyProperty.Register("ButtonsInterval", typeof(int), typeof(TwoIntegerNumericUpDown),
		  new FrameworkPropertyMetadata(
			DEFAULT_BUTTONS_INTERVAL,
			new PropertyChangedCallback(OnButtonsIntervalPropertyChanged)));


		private static void OnButtonsIntervalPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//To be called whenever the DP is changed.
			TwoIntegerNumericUpDown tinup = sender as TwoIntegerNumericUpDown;
			if (tinup != null)
			{
				tinup.upButton.Interval = (int)e.NewValue;
				tinup.DownButton.Interval = (int)e.NewValue;
			}			
		}

		#endregion

		private void UpClick(object sender, RoutedEventArgs args)
		{
			Int32 value = Convert.ToInt32(textBoxValue.Text);
			value++;
			if (value > MaxValue)
			{
				value = 0;

			}
			string stringValue = ((value).ToString());
			Text = stringValue.Length == 1 ? "0" + stringValue : stringValue;
		}

		private void DownClick(object sender, RoutedEventArgs args)
		{
			Int32 value = Convert.ToInt32(textBoxValue.Text);
			value--;
			if (value < 0)
			{
				value = MaxValue;

			}
			string stringValue = ((value).ToString());
			Text = stringValue.Length == 1 ? "0" + stringValue : stringValue;
		}
	}
}
