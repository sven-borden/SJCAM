using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SJCAM.Style
{
	public static class AppColor
	{
		private static SolidColorBrush Connected = new SolidColorBrush(Colors.Green);
		private static SolidColorBrush NotConnected = new SolidColorBrush(Colors.Red);
		public static SolidColorBrush PivotSelectedColor = new SolidColorBrush(new Color() { A = 0x7F, R = 0x00, G = 0x8B, B = 0xFF });//#7F008BFF
		public static SolidColorBrush GetConnectionColor(bool _connectionStatus)
		{
			return _connectionStatus ? Connected : NotConnected;
		}
	}
}
