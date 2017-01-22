using Microsoft.Toolkit.Uwp;
using SJCAM.Logic.Wifi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace SJCAM.Logic
{
	static class ConnectionStatus
	{
		public static bool IsConnected { get; private set; }
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

		public async static Task<ObservableCollection<WifiSpot>> GetAvailableNetwork()
		{
			ObservableCollection<WifiSpot> list = new ObservableCollection<WifiSpot>();
			try
			{
				var access = await WiFiAdapter.RequestAccessAsync();
				WiFiAdapter firstAdapter;

				switch (access)
				{
					case WiFiAccessStatus.DeniedBySystem:
						Debug.WriteLine("[WIFI]: System blocking");
						return null;
					case WiFiAccessStatus.DeniedByUser:
						Debug.WriteLine("[WIFI]: User blocking");
						return null;
					case WiFiAccessStatus.Unspecified:
						Debug.WriteLine("[WIFI]: Unknow error");
						return null;
					default:
						break;
				}

				var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
				if (result.Count <= 0)
					return null;
				firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
				await firstAdapter.ScanAsync();

				var report = firstAdapter.NetworkReport;
				bool ispresent = false;
				foreach (var network in report.AvailableNetworks)
				{
					Debug.WriteLine(network.Ssid);
					ispresent = false;
					foreach (WifiSpot item in list)
						if (item.SSID == network.Ssid)
							ispresent = true;

					if (!ispresent)
					{
						WifiSpot wifi = new WifiSpot();
						wifi.MAC = network.Bssid;
						wifi.SignalStrength = network.ChannelCenterFrequencyInKilohertz;
						wifi.SignalBar = network.SignalBars;
						wifi.SSID = network.Ssid;

						if (network.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.None)
							wifi.PswNeeded = false;
						else
							wifi.PswNeeded = true;
						list.Add(wifi);
					}
					
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("[Get WIFI List]: ERROR");
			}

			return list;
		}

		public async static Task<bool> WifiNameAsync(WifiSpot wifi)
		{
			IsConnected = false;
			if (wifi == null)
				return false;
			try
			{
				Debug.WriteLine("Connecting to : " + wifi.SSID);
				var access = await WiFiAdapter.RequestAccessAsync();
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
				var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
				if (result.Count <= 0)
					return false;
				firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
				await firstAdapter.ScanAsync();

				var report = firstAdapter.NetworkReport;
				foreach (var i in report.AvailableNetworks)
					Debug.WriteLine(i.Ssid);

				foreach (var network in report.AvailableNetworks)
				{
					if (network.Ssid.Contains(wifi.SSID))
					{
						WiFiConnectionResult connectionResult = null;
						if (wifi.PswNeeded)
						{
							try
							{
								PasswordCredential pass = new PasswordCredential();
								pass.Password = wifi.Psw;
								connectionResult = await firstAdapter.ConnectAsync(network, WiFiReconnectionKind.Manual, pass);
							}
							catch(Exception e)
							{
								Debug.WriteLine(e.ToString());
							}
						}
						else
							connectionResult = await firstAdapter.ConnectAsync(network, WiFiReconnectionKind.Manual);

						if (connectionResult.ConnectionStatus != WiFiConnectionStatus.Success)
							return false;
						else
						{
							IsConnected = true;
							return IsConnected;
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
