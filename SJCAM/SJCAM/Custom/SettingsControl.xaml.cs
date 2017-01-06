using System;
using System.Collections.Generic;
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
			CheckSettings();
		}

		private async void CheckSettings()
		{
			string x = await action.GetRequestAsync("3014");
			if (x == null)
				return;
			string[] Cmd = x.Split(new string[] { "<Cmd>", "</Cmd>" }, StringSplitOptions.RemoveEmptyEntries);
			string[] Status = x.Split(new string[] { "<Status>", "</Status>" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < Cmd.Length; i++)
			{
				if (Cmd[i].Contains("9001"))
				{
					if (Status[i] == "1")
						ToggleGyro.IsChecked = true;
					else
						ToggleGyro.IsChecked = false;
					continue;
				}

				if (Cmd[i].Contains("2007"))
				{
					if (Status[i] == "1")
						ToggleAudio.IsChecked = true;
					else
						ToggleAudio.IsChecked = false;
					continue;
				}

				if (Cmd[i].Contains("2004"))
				{
					if (Status[i] == "1")
						ToggleWDR.IsChecked = true;
					else
						ToggleWDR.IsChecked = false;
					continue;
				}

				if (Cmd[i].Contains("2005"))
				{ ComboExposure.SelectedIndex = Convert.ToInt32(Status[i]); continue; }
				if (Cmd[i].Contains("1007"))
				{ ComboWhiteBalance.SelectedIndex = Convert.ToInt32(Status[i]); continue; }
				if (Cmd[i].Contains("1012"))
				{ ComboPhotoLapseInterval.SelectedIndex = Convert.ToInt32(Status[i]); continue; }
				if (Cmd[i].Contains("2019"))
				{ ComboVideoLapseInterval.SelectedIndex = Convert.ToInt32(Status[i]); continue; }
				if (Cmd[i].Contains("3007"))
				{ ComboAutoPowerOff.SelectedIndex = Convert.ToInt32(Status[i]); continue; }
			}
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
			string par;
			if (ToggleGyro.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("9001", par);
			Waiting();
		}

		private async void ToggleAudio_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par;
			if (ToggleAudio.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("2007", par);
			Waiting();
		}

		private async void ToggleWDR_Click(object sender, RoutedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par;
			if (ToggleWDR.IsChecked == true)
				par = "1";
			else
				par = "0";
			string result = await action.GetRequestAsync("2004", par);
			Waiting();
		}

		private async void ComboExposure_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par =ComboExposure.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("2005", par);
			Waiting();
		}

		private async void ComboWhiteBalance_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par = ComboWhiteBalance.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("1007", par);
			Waiting();
		}

		private async void ComboPhotoLapseInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par = ComboPhotoLapseInterval.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("1012", par);
			Waiting();
		}

		private async void ComboVideoLapseInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par = ComboVideoLapseInterval.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("2019", par);
			Waiting();
		}

		private async void ComboAutoPowerOff_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (locked)
				return;
			Waiting();
			string par = ComboAutoPowerOff.SelectedIndex.ToString();
			string result = await action.GetRequestAsync("3007", par);
			Waiting();
		}
	}
}
