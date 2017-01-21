using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SJCAM.Logic;
using SJCAM.Custom;
using SJCAM.Style;
using System.Collections.ObjectModel;
using SJCAM.Logic.Settings;
using System.Threading.Tasks;
using SJCAM.Logic.Wifi;
using Windows.Networking.Connectivity;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SJCAM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public string Title = string.Empty;
		public string Description = string.Empty;
		public string ConnectionStatusText = string.Empty;
		public Visibility connectionBarVisibility;
		private Logic.Action action;
		public ObservableCollection<WifiSpot> ListAvailableWifi;
		private string Password = string.Empty;

        public MainPage()
        {
			Title = "SJCAM";
			Description = "Choose a category";
			ConnectionStatusText = "Try connecting";
			connectionBarVisibility = Visibility.Collapsed;
			action = new Logic.Action();
			ListAvailableWifi = new ObservableCollection<WifiSpot>();
			RetreiveWifi();
            this.InitializeComponent();

			CheckConnection();
			NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
		}

		private void NetworkInformation_NetworkStatusChanged(object sender)
		{
			
		}

		private async void RetreiveWifi()
		{
			List<WifiSpot> tmp = await ConnectionStatus.GetAvailableNetwork();
			ListAvailableWifi.Clear();
			foreach (WifiSpot t in tmp)
				if(!ListAvailableWifi.Contains(t))
					ListAvailableWifi.Add(t);
			this.Bindings.Update();
		}

		private async void CheckConnection()
		{
			connectionBarVisibility = Visibility.Visible;
			bool _conn = await ConnectionStatus.WifiNameAsync(ConnectStatusProgressBar);
			if (_conn)
				ConnectionStatusText = "Connected";
			else
			{
				ConnectionStatusText = "Not Connected";
				ConnectStatusProgressBar.IsIndeterminate = true;
				ShowPopup();
			}
			this.Bindings.Update();
			ConnectStatusBar.Background = AppColor.GetConnectionColor(_conn);
			DispatcherTimer coverOut = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
			coverOut.Tick += (t, e) => { connectionBarVisibility = Visibility.Collapsed; ConnectStatusBar.Visibility = connectionBarVisibility; (t as DispatcherTimer).Stop(); }; 
			coverOut.Start();
		}

		private void ShowPopup()
		{
			PopupStack.MaxWidth = this.ActualWidth - 100;
			PopupStack.MaxHeight = this.ActualHeight - 100;
			WifiPopup.VerticalOffset = 50;
			WifiPopup.HorizontalOffset = 50;
			WifiPopup.IsOpen = true;
		}

		private async void Image_Loaded(object sender, RoutedEventArgs e)
		{
			await (sender as Image).Blur(8f,2000,1000).StartAsync();
		}

		private async void PhotoButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			Title = "Photo";
			this.Bindings.Update();
			CanvasPlace.Children.Clear();
			CanvasPlace.Children.Add(new PhotoControl());
			try
			{ await action.GetRequestAsync("3001", "0"); }
			catch(Exception ex)
			{ };
		}

		private async void VideoButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			Title = "Video";
			this.Bindings.Update();
			CanvasPlace.Children.Clear();
			CanvasPlace.Children.Add(new VideoControl());
			try
			{ await action.GetRequestAsync("3001", "1"); }
			catch(Exception ex)
			{ };
		}

		private void OtherButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			Title = "Settings";
			this.Bindings.Update();
			CanvasPlace.Children.Clear();
			CanvasPlace.Children.Add(new SettingsControls());
		}


		private async void FileButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			Title = "Files"; Description = "Here is SJCAM files";
			this.Bindings.Update();
			CanvasPlace.Children.Clear();
			CanvasPlace.Children.Add(new FilesControl());
		}

		private async void ValidateWifiButton_Click(object sender, RoutedEventArgs e)
		{
			PopupRing.Visibility = Visibility.Visible;
			WifiSpot spot = ListAvailableWifi[WifiListView.SelectedIndex];
			ConnectStatusProgressBar.Visibility = Visibility.Visible;
			ConnectionStatusText = "Try connecting : " + spot.SSID;
			this.Bindings.Update();
			bool connection = await ConnectionStatus.WifiNameAsync(ConnectStatusProgressBar, WifiListView.SelectedItem as WifiSpot);
			ConnectStatusBar.Background = AppColor.GetConnectionColor(connection);
			DispatcherTimer coverOut = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
			coverOut.Tick += (t, ex) => { connectionBarVisibility = Visibility.Collapsed; ConnectStatusBar.Visibility = connectionBarVisibility; (t as DispatcherTimer).Stop(); };
			coverOut.Start();
			PopupRing.Visibility = Visibility.Collapsed;
			if (connection)
			{
				WifiPopup.IsOpen = false;
			}
			else
				WifiPopup.IsOpen = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ValidateWifiButton.IsEnabled = false;
			WifiPopup.IsOpen = false;
			PopupRing.Visibility = Visibility.Collapsed;
		}

		private void WifiListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			WifiSpot wifi = WifiListView.SelectedItem as WifiSpot;
			if (wifi.PswNeeded)
				PasswordPopup.IsOpen = true;
			ValidateWifiButton.IsEnabled = true;
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			PopupRing.Visibility = Visibility.Visible;
			RetreiveWifi();
			PopupRing.Visibility = Visibility.Collapsed;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Password = (sender as TextBox).Text;
		}

		private void ValidatePassword_Click(object sender, RoutedEventArgs e)
		{
			PasswordPopup.IsOpen = false;
		}
	}
}
