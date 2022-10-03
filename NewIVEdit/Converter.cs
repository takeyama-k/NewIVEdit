using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
namespace NewIVEdit.Converter
{

    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
	public class CellColorConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string valuestring = value as string;
			if (valuestring == "") return new SolidColorBrush(Colors.LightGray);
			else return new SolidColorBrush(Colors.Red);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(string), typeof(SolidColorBrush))]
	public class CellColorConverterOrange : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string valuestring = value as string;
			if (valuestring == "") return new SolidColorBrush(Colors.LightGray);
			else return new SolidColorBrush(Colors.Orange);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	[ValueConversion(typeof(bool), typeof(SolidColorBrush))]
	public class CellColorConverter_Radio : IMultiValueConverter
	{

		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value == null || value.Length < 3) return new SolidColorBrush(Colors.Gray);
			bool[] valuebool = new bool[value.Length];
			int idx = 0;
			foreach (var vb in value)
			{
				if (vb == DependencyProperty.UnsetValue) return new SolidColorBrush(Colors.Gray);
				valuebool[idx] = (bool)vb;
				idx++;
			}
			if (valuebool[0] == true) return new SolidColorBrush(Colors.LightGray);
			else if(valuebool[1] == true) return new SolidColorBrush(Colors.Purple);
			else if (valuebool[2] == true) return new SolidColorBrush(Colors.DarkGray);
			else return new SolidColorBrush(Colors.Red);
		}
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(bool), typeof(string))]
	public class CellConverter_Postfix : IMultiValueConverter
	{

		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.Length < 3) return "";
			bool[] valuebool = new bool[value.Length];
			int idx = 0;
			foreach (var vb in value)
			{
				if (vb == DependencyProperty.UnsetValue) return "";
				valuebool[idx] = (bool)vb;
				idx++;
			}
			if (valuebool[0] == true) return "";
			else if (valuebool[1] == true) return "Y";
			else if (valuebool[2] == true) return "E";
			else return "";
		}
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}


	[ValueConversion(typeof(string), typeof(bool))]
	public class CellToolTipConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string valuestring = value as string;
			if (valuestring == "") return false;
			else return true;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(uint), typeof(SolidColorBrush))]
	public class InvoiceFlagConverter : IMultiValueConverter
	{

		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.Length < 2) return new SolidColorBrush(Colors.Gray);
			int[] valueint = new int[value.Length];
			int idx = 0;
			foreach (var vu in value)
			{
				if (vu == DependencyProperty.UnsetValue) return new SolidColorBrush(Colors.Gray);
				valueint[idx] = (int)vu;
				idx++;
			}
			if (valueint[0] == 63 && valueint[1] == 63) return new SolidColorBrush(Colors.LightGray);
			else if (valueint[0] == 63 && valueint[1] < 63) return new SolidColorBrush(Colors.Orange) ;
			else if (valueint[0] < 63 && valueint[1] < 63) return new SolidColorBrush(Colors.Red);
			else return new SolidColorBrush(Colors.Red);
		}
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
