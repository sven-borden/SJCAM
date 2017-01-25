using SJCAM.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SJCAM.Custom
{
	public sealed partial class SettingsControls : UserControl
	{
		Logic.Action action;
		bool locked = false;
		public List<string> Exposure { get; set; }
		public List<string> WhiteBalance { get; set; }
		public List<string> PhotoLapse { get; set; }
		public List<string> VideoLapse { get; set; }
		public List<string> AutoPower { get; set; }


		public SettingsControls()
		{
			action = new Logic.Action();

			Exposure = new List<string>()
			{
				"+2", "+5/3", "+4/3", "+1", "+2/3", "+1/3", "0", "-1/3", "-2/3", "-1", "-4/3", "-5/3", "-2"
			};
			WhiteBalance = new List<string>()
			{
				"Auto", "DayLight", "Cloudy", "Tungsten", "Fluorescent"
			};
			PhotoLapse = new List<string>()
			{
				"3s", "5s", "10s", "20s"
			};
			VideoLapse = new List<string>()
			{
				"1s", "2s", "5s", "10s", "20s", "60s"
			};
			AutoPower = new List<string>()
			{
				"3 min", "5 min", "10 min", "off"
			};
			this.InitializeComponent();
			Loaded += async (e, o) =>
			{
				await Task.Delay(1000);
				if (ConnectionStatus.IsConnected == true)
					CheckSettings();
				else
				{
					ToggleAudio.IsEnabled = false;
					ToggleGyro.IsEnabled = false;
					ToggleWDR.IsEnabled = false;
					ToggleStamp.IsEnabled = false;
				}
			};
		}

		private void CheckSettings()
		{
			ComboExposure.SelectedIndex = CameraSettings.Exposure;
			ComboWhiteBalance.SelectedIndex = CameraSettings.WhiteBalance;
			ComboPhotoLapseInterval.SelectedIndex = CameraSettings.PhotoLapse;
			ComboVideoLapseInterval.SelectedIndex = CameraSettings.VideoLapse;
			ComboAutoPowerOff.SelectedIndex = CameraSettings.AutoPower;
			ToggleAudio.IsChecked = ConvertToBool(CameraSettings.AudioOn);
			ToggleGyro.IsChecked = ConvertToBool(CameraSettings.GyroOn);
			ToggleStamp.IsChecked = ConvertToBool(CameraSettings.TimeStamp);
			ToggleWDR.IsChecked = ConvertToBool(CameraSettings.WDROn);
		}

		private bool? ConvertToBool(int audioOn)
		{
			if (audioOn == 0)
				return false;
			else
				return true;
		}

		private void Waiting()
		{
			if(!locked)
			{
				Ring.Visibility = Visibility.Visible;
				locked = !locked;
			}
			else
			{
				Ring.Visibility = Visibility.Collapsed;
				locked = !locked;
			}
		}

		private async void ToggleGyro_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ToggleButton).IsEnabled = false;
			string par;
			if (ToggleGyro.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("9001", par);
			(sender as ToggleButton).IsEnabled = true;
			Waiting();
		}

		private async void ToggleAudio_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ToggleButton).IsEnabled = false;
			string par;
			if (ToggleAudio.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("2007", par);
			(sender as ToggleButton).IsEnabled = true;
			Waiting();
		}

		private async void ToggleWDR_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ToggleButton).IsEnabled = false;
			string par;
			if (ToggleWDR.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("2004", par);
			(sender as ToggleButton).IsEnabled = true;
			Waiting();
		}

		private async void ComboExposure_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string par =ComboExposure.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("2005", par);
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}

		private async void ComboWhiteBalance_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string par = ComboWhiteBalance.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("1007", par);
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}

		private async void ComboPhotoLapseInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string par = ComboPhotoLapseInterval.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("1012", par);
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}

		private async void ComboVideoLapseInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string par = ComboVideoLapseInterval.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("2019", par);
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}

		private async void ComboAutoPowerOff_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string par = ComboAutoPowerOff.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("3007", par);
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}

		private async void DisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			await action.GetRequestAsync("3013");
			Waiting();
		}

		private async void FormatButton_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			await action.GetRequestAsync("3010", "1");
			Waiting();
		}
	}
}
