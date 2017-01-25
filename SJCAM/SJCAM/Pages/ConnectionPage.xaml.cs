using Microsoft.Toolkit.Uwp.UI.Animations;
using SJCAM.Logic;
using SJCAM.Logic.Wifi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SJCAM.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ConnectionPage : Page
	{
		ApplicationView currentView;
		public ObservableCollection<WifiSpot> ListAvailableNetwork;
		bool refreshing = false;
		private string password = string.Empty;
		private List<WifiSpot> knownWifi;
		StorageFolder localFolder = ApplicationData.Current.LocalFolder;
		string FilePath;

		public ConnectionPage()
		{
			ListAvailableNetwork = new ObservableCollection<WifiSpot>();
			FilePath = Path.Combine(localFolder.Path, "Networks");
			this.InitializeComponent();
			currentView = ApplicationView.GetForCurrentView();
			Window.Current.SizeChanged += Current_SizeChanged;
			Animate();
			WIFI();
		}

		private async void Animate()
		{
			Background.Blur(8f, 1000, 500).Start();
			foreach (var item in MainStack.Children)
				item.Fade(0, 0, 0).Start();
			MainStack.Fade(1, 1, 1).Start();
			foreach (var item in MainStack.Children)
				await item.Fade(1, 800, 500).StartAsync();
		}

		private async void WIFI()
		{
			refreshing = true;
			PopupRing.IsEnabled = true;
			PopupRing.Visibility = Visibility.Visible;
			if (await TryConnectingKnownNetwork())
			{
				GotoNextPage();
				return;
			}
			ObservableCollection<WifiSpot> tmp = new ObservableCollection<WifiSpot>();
			tmp = await ConnectionStatus.GetAvailableNetwork();
			List<WifiSpot> toRemove = new List<WifiSpot>();
			foreach (WifiSpot wifi in ListAvailableNetwork)
				if (!tmp.Contains(wifi))
					toRemove.Add(wifi);

			foreach (WifiSpot wifi in toRemove)
				ListAvailableNetwork.Remove(wifi);

			foreach (WifiSpot wifi in tmp)
				if (!ListAvailableNetwork.Contains(wifi))
					ListAvailableNetwork.Insert(0,wifi);
			PopupRing.Visibility = Visibility.Collapsed;
			refreshing = false;
		}

		private async Task<bool> TryConnectingKnownNetwork()
		{
			knownWifi = new List<WifiSpot>();
			try
			{
				knownWifi = WrittingReading.Deserialize(FilePath);
			}
			catch(Exception e)
			{

			}
			//knownWifi = localFolder.Values["KnownNetworks"] as List<WifiSpot>;
			if (knownWifi == null)
				return false;
			var tmp = await ConnectionStatus.GetAvailableNetwork();
			List<string> SSID = new List<string>();
			foreach (WifiSpot wifi in knownWifi)
				SSID.Add(wifi.SSID);

			foreach (WifiSpot wifi in tmp)
				if (SSID.Contains(wifi.SSID))
				{
					bool a = await ConnectionStatus.ConnectToWifi(wifi);
					if(a)
						return true;
				}
			return false;
		}

		private async void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
		{
			ApplicationView tmp = ApplicationView.GetForCurrentView();
			if (tmp.Orientation == currentView.Orientation)
				return;
			currentView = tmp;
			if (currentView.Orientation == ApplicationViewOrientation.Landscape)
				await Background.Rotate(90, (float)(Background.Width / 2), (float)(Background.Height / 2), 300, 0).StartAsync();
			if (currentView.Orientation == ApplicationViewOrientation.Portrait)
				await Background.Rotate(-90, (float)Background.Width / 2, (float)Background.Height / 2, 300, 0).StartAsync();
		}

		private void ValidateWifiButton_Click(object sender, RoutedEventArgs e)
		{
			PopupPassword.Width = this.ActualWidth;
			PopupPassword.VerticalOffset = 150;
			WifiSpot wifi = mylist.SelectedItem as WifiSpot;
			if (wifi.PswNeeded == false)
				Connect(wifi);
			else
				PopupPassword.IsOpen = true;
		}

		private async void Connect(WifiSpot wifi)
		{
			PopupRing.Visibility = Visibility.Visible;
			bool result = await ConnectionStatus.ConnectToWifi(wifi);
			if(!result)
			{
				MessageDialog msg = new MessageDialog("Failed to connect");
				await msg.ShowAsync();
				PopupRing.Visibility = Visibility.Collapsed;
				return;
			}
			PopupRing.Visibility = Visibility.Collapsed;
			if (result)
			{
				//Save the wifispot
				if (knownWifi == null)
					knownWifi = new List<WifiSpot>();
				knownWifi.Add(wifi);
				WrittingReading.Serialize(knownWifi, FilePath);
				GotoNextPage();//TODO goto next page
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			//Skip this section
			GotoNextPage();
		}

		private async void GotoNextPage()
		{
			foreach (var item in MainStack.Children)
				await item.Fade(0, 500, 0).StartAsync();
			await Background.Blur(0, 200, 0).StartAsync();
			Frame.Navigate(typeof(DevicePage));
		}

		private void listView_RefreshRequested(object sender, EventArgs e)
		{
			if(!refreshing)
				WIFI();
		}

		private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ValidateWifiButton.IsEnabled = true;
		}

		private void ValidatePassword_Click(object sender, RoutedEventArgs e)
		{
			password = PswBox.Text;
			if (password == string.Empty)
				return;
			PopupPassword.IsOpen = false;
			ValidateWifiButton.IsEnabled = false;
			WifiSpot spot = (mylist.SelectedItem as WifiSpot);
			spot.Psw = password;
			Connect(spot);
		}
	}
}
