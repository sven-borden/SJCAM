using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace SJCAM.Logic
{
	static class ConnectionStatus
	{

		public static Task WiFiConnectionKind { get; private set; }

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

		public async static Task<bool> WifiNameAsync(ProgressBar bar)
		{
			try
			{
				bar.IsIndeterminate = false;
				Debug.WriteLine("Checking...");
				bar.Value++;
				var access = await WiFiAdapter.RequestAccessAsync();
				bar.Value++;
				Debug.WriteLine("Request");
				WiFiAdapter firstAdapter;

				switch (access)
				{
					case WiFiAccessStatus.DeniedBySystem:
						Debug.WriteLine("[WIFI]: System blocking");
						return false;
					case WiFiAccessStatus.DeniedByUser:
						Debug.WriteLine("[WIFI]: User blocking");
						return false;
					case WiFiAccessStatus.Unspecified:
						Debug.WriteLine("[WIFI]: Unknow error");
						return false;
					default:
						break;
				}
				bar.Value++;
				var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
				if (result.Count <= 0)
					return false;
				bar.Value++;
				firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
				await firstAdapter.ScanAsync();

				bar.Value++;
				var report = firstAdapter.NetworkReport;
				foreach (var i in report.AvailableNetworks)
					Debug.WriteLine(i.Ssid);

				foreach (var network in report.AvailableNetworks)
				{
					if (network.Ssid.Contains("M20"))
					{
						bar.Value += 3;
						PasswordCredential pass = new PasswordCredential(null, null, "12345678");
						var connectionResult = await firstAdapter.ConnectAsync(network, WiFiReconnectionKind.Manual, pass);
						Debug.WriteLine("After");
						if (connectionResult.ConnectionStatus != WiFiConnectionStatus.Success)
						{
							return false;
						}
						else
						{
							bar.Value = bar.Maximum;
							return true;
						}
					}
				}
				return false;
			}
			catch(Exception e)
			{
				Debug.WriteLine("[WIFI ERROR]: exception : " + e.ToString());
				return false;
			}
		}
	}
}
