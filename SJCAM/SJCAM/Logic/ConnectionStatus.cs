using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic
{
	static class ConnectionStatus
	{
		/// <summary>
		/// Get state of connection
		/// </summary>
		/// <returns>True if over Wifi, false otherwise</returns>
		public static bool IsOverWifi()
		{
			switch (ConnectionHelper.ConnectionType)
			{
				case ConnectionType.Ethernet:
					return false;
				case ConnectionType.WiFi:
					return true;
				case ConnectionType.Data:
					// Data
					return false;
				case ConnectionType.Unknown:
					// Unknown
					return false;
			}
			return false;
		}
	}
}
