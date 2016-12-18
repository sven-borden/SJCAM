using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SJCAM.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SJCAM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		Color Dominant;
		public string Title { get; set; }
		public string Description { get; set; }

		public MainPage()
        {
            this.InitializeComponent();
			Title = "SJCAM";	
			Description = "Welcome!";	
		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			appSplit.IsPaneOpen = !appSplit.IsPaneOpen;
		}

		private async void backgroundImage_Loaded(object sender, RoutedEventArgs e)
		{
			Dominant = await Images.saveImage(backgroundImage);
			splitGrid.Background = new SolidColorBrush(Dominant);
			var view = ApplicationView.GetForCurrentView();
			view.TitleBar.BackgroundColor = Dominant;
			view.TitleBar.ButtonBackgroundColor = Dominant;
		}
	}
}
