using SJCAM.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
	public sealed partial class VideoControl : UserControl
	{
		public List<string> ResolutionString { get; set; }
		Logic.Action action;
		bool locked = false;
		bool isRecording = false;
		public string RemainingTime { get; set; }

		public VideoControl()
		{
			ResolutionString = new List<string>()
			{
				"2K 30fps", "1080p 60fps", "1080p 30fps", "720p 120fps", "720p 60fps", "720p 30fps","480p 240fps"
			};

			action = new Logic.Action();
			this.InitializeComponent();
			if (ConnectionStatus.IsConnected == true)
			{
				CheckIsRecording();
				CheckSettings();
				UpdateTimeRemain();
			}
			else
			{
				SnapButton.IsEnabled = false;
			}
			this.Loaded += (e, r) =>
			{
				//Player.Width = this.ActualWidth - 40;
				//Player.Height = Player.Width / 16 * 9;
				//if(ConnectionStatus.IsConnected)
				//	Player.Source = new Uri(action.LiveFeed);
				//Player.AutoPlay = false;
				//Player.HardwareAcceleration = false;
			};	
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
				if (Cmd[i].Contains("2002"))
				{
					ResolutionCombo.SelectedIndex = Convert.ToInt32(Status[i]);
					return;
				}
			}
		}

		private async void CheckIsRecording()
		{
			string x = await action.GetRequestAsync("2016");
			if (x == null)
				return;
			string[] temp = x.Split(new string[] { "<Value>", "</Value>" }, StringSplitOptions.RemoveEmptyEntries);
			if (temp[1] == "0")
				SnapButton.Content = "Start";
			else
				SnapButton.Content = "Stop";
		}

		private async void UpdateTimeRemain()
		{
			string x = await action.GetRequestAsync("2009");
			if (x == null)
				return;
			string[] temp = x.Split(new string[] { "<Value>", "</Value>" }, StringSplitOptions.RemoveEmptyEntries);
			int second = Convert.ToInt32(temp[1]);

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
			UpdateTimeRemain();
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
