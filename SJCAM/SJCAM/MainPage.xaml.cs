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
		public ObservableCollection<object> ListSettings;
		private Logic.Action action;

        public MainPage()
        {
			ListSettings = new ObservableCollection<object>
			{
				new Setting(){Name = "test", Description = "descrip" },
				new Setting(){Name = "test", Description = "2" }
			};
			Title = "SJCAM";
			Description = "Choose a category";
			ConnectionStatusText = "Try connecting";
			connectionBarVisibility = Visibility.Collapsed;
			action = new Logic.Action();
            this.InitializeComponent();

			CheckConnection();
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
			}
			//this.Bindings.Update();
			ConnectStatusBar.Background = AppColor.GetConnectionColor(_conn);
			DispatcherTimer coverOut = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
			coverOut.Tick += (t, e) => { connectionBarVisibility = Visibility.Collapsed; ConnectStatusBar.Visibility = connectionBarVisibility; (t as DispatcherTimer).Stop(); }; 
			coverOut.Start();
		}

		private async void Image_Loaded(object sender, RoutedEventArgs e)
		{
			await (sender as Image).Blur(8f,2000,1000).StartAsync();

		}

		private async void PhotoButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			CanvasPlace.Children.Clear();
			CanvasPlace.Children.Add(new PhotoControl());
			await action.BuildRequestAsync("3001", "0");
		}

		private async void VideoButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			CanvasPlace.Children.Clear();
			await action.BuildRequestAsync("3001", "1");
		}

		private async void OtherButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			await Task.Delay(5);
		}
	}
}
