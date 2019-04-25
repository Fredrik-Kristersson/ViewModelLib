using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ViewModelLib.Converters
{
	public class SliderValueWidthConverter : IMultiValueConverter
	{
		public object Convert(
			object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Slider slider = null;

			foreach (var value in values)
			{
				if (value == null)
				{
					return 0;
				}

				if (value is Slider)
				{
					slider = (Slider)value;
				}
			}

			if (slider == null)
			{
				return 0;
			}

			double width = slider.ActualWidth;
			var percent = slider.Value / slider.Maximum;
			var result = Math.Round(width * percent, MidpointRounding.ToEven);

			return result;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
