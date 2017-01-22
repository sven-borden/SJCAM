using Microsoft.Toolkit.Uwp.UI.Animations;
using SJCAM.Logic;
using SJCAM.Logic.Wifi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
			WifiSpot wifi = listView.SelectedItem as WifiSpot;
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
				;//TODO goto next page
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			//Skip this section
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
			WifiSpot spot = (listView.SelectedItem as WifiSpot);
			spot.Psw = password;
			Connect(spot);
		}
	}
}
