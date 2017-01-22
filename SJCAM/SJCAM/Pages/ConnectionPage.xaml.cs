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


		public ConnectionPage()
		{
			ListAvailableNetwork = new ObservableCollection<WifiSpot>();
			this.InitializeComponent();
			currentView = ApplicationView.GetForCurrentView();
			Window.Current.SizeChanged += Current_SizeChanged;
			Background.Blur(8f, 1000, 500).Start();
			WIFI();
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
				using (var stream = File.OpenRead(Path.Combine(localFolder.Path, "Networks")))
				{
					var serializer = new XmlSerializer(knownWifi.GetType());
					knownWifi = serializer.Deserialize(stream) as List<WifiSpot>;
				}
			}
			catch(Exception e)
			{

			}
			//knownWifi = localFolder.Values["KnownNetworks"] as List<WifiSpot>;
			if (knownWifi == null)
				return false;
			var tmp = await ConnectionStatus.GetAvailableNetwork();
			foreach (WifiSpot wifi in tmp)
				if (knownWifi.Contains(wifi))
				{
					bool a = await ConnectionStatus.WifiNameAsync(wifi);
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
			WifiSpot wifi = ListView.SelectedItem as WifiSpot;
			if (wifi.PswNeeded == false)
				Connect(wifi);
			else
				PopupPassword.IsOpen = true;
		}

		private async void Connect(WifiSpot wifi)
		{
			PopupRing.Visibility = Visibility.Visible;
			bool result = await ConnectionStatus.WifiNameAsync(wifi);
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
				using (MemoryStream ms = new MemoryStream())
				{
					var writer = new System.IO.StreamWriter(ms);
					var serializer = new XmlSerializer(knownWifi.GetType());
					serializer.Serialize(writer, this);
					writer.Flush();

					//if the serialization succeed, rewrite the file.
					File.WriteAllBytes(Path.Combine(localFolder.Path, "Newtworks"), ms.ToArray());
				}
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
			WifiSpot spot = (ListView.SelectedItem as WifiSpot);
			spot.Psw = password;
			Connect(spot);
		}
	}
}
