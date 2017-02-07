using SJCAM.Logic;
using SJCAM.Logic.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
	public sealed partial class VideoControl : UserControl
	{
		public List<string> ResolutionString { get; set; }
		Logic.Action action;
		bool locked = false;
		bool isRecording = false;
		public string RemainingTime { get; set; }

		public VideoControl()
		{
			StartAsync();
		}

		private void StartAsync()
		{
			ResolutionString = new List<string>()
			{
				"2K 30fps", "1080p 60fps", "1080p 30fps", "720p 120fps", "720p 60fps", "720p 30fps","480p 240fps"
			};

			action = new Logic.Action();
			this.InitializeComponent();
			this.Loaded += async (e, o) =>
			{
				await Task.Delay(1000);
				if (ConnectionStatus.IsConnected == true)
					CheckSettings();
				else
					SnapButton.IsEnabled = false;
			};
		}

		private void CheckSettings()
		{
			try
			{
				ResolutionCombo.SelectedIndex = VideoSettings.VideoResolution;
			}
			catch (ArgumentException e)
			{
                e.ToString();
			}
		}

		private void UpdateTimeRemain(int second)
		{
			RemainingTime = (second / 3600).ToString() + "h:"; second = second % 3600;
			RemainingTime += (second / 60).ToString() + "m:"; second = second % 60;
			RemainingTime += (second).ToString() + "s";
			Remain.Text = RemainingTime;
		}


		/// <summary>
		/// Snap a picture
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Button_ClickAsync(object sender, RoutedEventArgs e)
		{
			Waiting();
			(sender as Button).IsEnabled = false;
			if (isRecording)
			{
				string msg = await action.GetRequestAsync("2001", "0");
				SnapButton.Content = "Start";
				isRecording = !isRecording;
			}
			else
			{
				string msg = await action.GetRequestAsync("2001", "1");
				SnapButton.Content = "Stop";
				isRecording = !isRecording;
			}
			(sender as Button).IsEnabled = true;
			Waiting();
		}

		private async void ResolutionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int selected = ResolutionCombo.SelectedIndex;
			(sender as ComboBox).IsEnabled = false;
			Waiting();
			string msg = await action.GetRequestAsync("2002", selected.ToString());
			(sender as ComboBox).IsEnabled = true;
			Waiting();
		}
		private void Waiting()
		{
			if (!locked)
			{
				SnapButton.IsEnabled = false;
				Ring.Visibility = Visibility.Visible;
				locked = !locked;
			}
			else
			{
				SnapButton.IsEnabled = true;
				Ring.Visibility = Visibility.Collapsed;
				locked = !locked;
			}
		}
	}
}
