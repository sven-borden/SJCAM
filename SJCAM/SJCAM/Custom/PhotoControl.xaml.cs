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
				ResolutionCombo.SelectedIndex = PhotoSettings.PhotoResolution;
				return;
			}
			catch (ArgumentException outOfRange)
			{
				Debug.WriteLine(outOfRange.Message);
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
