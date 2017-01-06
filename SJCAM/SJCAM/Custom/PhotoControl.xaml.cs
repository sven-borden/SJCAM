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
	public sealed partial class PhotoControl : UserControl
	{
		public List<string> ResolutionString { get; set; }
		Logic.Action action;
		bool locked = false;
		public PhotoControl()
		{
			ResolutionString = new List<string>()
			{
				"16Mpx", "12Mpx", "8Mpx", "5Mpx", "3Mpx", "2Mpx", "VGA"
			};
			action = new Logic.Action();
			this.InitializeComponent();
		}

		/// <summary>
		/// Snap a picture
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Button_ClickAsync(object sender, RoutedEventArgs e)
		{
			Waiting();
			await action.BuildRequestAsync("1001");
			Waiting();
		}

		private async void ResolutionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int selected = ResolutionCombo.SelectedIndex;
			Waiting();
			await action.BuildRequestAsync("1002", selected.ToString());
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
