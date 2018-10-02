using System;
using System.Windows.Data;

namespace ViewModelLib
{
	public class StringToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			string stringValue = value as string;
			return !string.IsNullOrEmpty(stringValue);
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
