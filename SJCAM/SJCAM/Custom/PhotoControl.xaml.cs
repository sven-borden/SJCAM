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
	public sealed partial class PhotoControl : UserControl
	{
		public List<string> ResolutionString { get; set; }
		Logic.Action action;
		bool locked = false;
		public PhotoControl()
		{
			ResolutionString = new List<string>()
			{
				"16Mpx", "14Mpx", "12Mpx", "10Mpx", "8Mpx", "5Mpx","3Mpx", "VGA"
			};
			action = new Logic.Action();
			this.InitializeComponent();
			if (ConnectionStatus.IsConnected == true)
			{
				CheckSettings();
			}
			else
			{
				SnapButton.IsEnabled = false;
			}
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
				Debug.WriteLine(Cmd[i]);
				if (Cmd[i].Contains("1002"))
				{
					try
					{
						ResolutionCombo.SelectedIndex = Convert.ToInt32(Status[i]);
						return;
					}
					catch(IndexOutOfRangeException outOfRange)
					{
						Debug.WriteLine(outOfRange.Message);
					}
				}
			}
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
			string msg = await action.GetRequestAsync("1001");
			(sender as Button).IsEnabled = true;
			Waiting();
		}

		private async void ResolutionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int selected = ResolutionCombo.SelectedIndex;
			Waiting();
			(sender as ComboBox).IsEnabled = false;
			string msg = await action.GetRequestAsync("1002", selected.ToString());
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
