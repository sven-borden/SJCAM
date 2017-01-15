using System;
using Windows.UI.Xaml.Data;

namespace SJCAM.Logic.Wifi
{
	public class StrenghToString : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double _strength = (double)value;
			string signal = _strength + " dBm";
			return signal;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
