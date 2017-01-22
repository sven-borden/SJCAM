using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic.Wifi
{
	public class WifiSpot
	{
		public string MAC { get; set; }
		public double SignalStrength { get; set; }
		public byte SignalBar { get; set; }
		public string SSID { get; set; }
		public bool PswNeeded { get; set; }
		public string Psw { get; set; }

		public WifiSpot()
		{

		}
	}
}
