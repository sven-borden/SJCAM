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
using SJCAM.Style;
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
        public MainPage()
        {
			Title = "SJCAM";
			Description = "Choose a category"
            this.InitializeComponent();
			CheckConnection();
        }

		private void CheckConnection()
		{
			ConnectStatusBar.Background = AppColor.GetConnectionColor(ConnectionStatus.IsOverWifi());
		}

		private async void Image_Loaded(object sender, RoutedEventArgs e)
		{
			await (sender as Image).Blur(8f,2000,300).StartAsync();

		}
	}
}
