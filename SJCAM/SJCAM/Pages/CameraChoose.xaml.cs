using Microsoft.Toolkit.Uwp.UI.Animations;
using SJCAM.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SJCAM.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CameraChoose : Page
	{
		public ObservableCollection<StartupModel> Source;
		ApplicationView currentView;
		public CameraChoose()
		{
			InitSources();
			this.InitializeComponent();
			MainStack.Fade(0, 0, 0).Start();
			currentView = ApplicationView.GetForCurrentView();
			Window.Current.SizeChanged += Current_SizeChanged;

			Animate();
		}

		private async void Animate()
		{
			Background.Blur(8f, 1000, 1000).Start();
			foreach (var item in MainStack.Children)
				item.Fade(0, 0, 0).Start();
			MainStack.Fade(1, 1, 1).Start();
			foreach (var item in MainStack.Children)
				await item.Fade(1, 800, 0).StartAsync();
		}

		private void InitSources()
		{
			Source = new ObservableCollection<StartupModel>();
			Source.Add(new StartupModel()
			{
				Description = "M20",
				ImageSource = "ms-appx:///Images/M20.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "M10+",
				ImageSource = "ms-appx:///Images/M10Plus.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "M10 Wifi",
				ImageSource = "ms-appx:///Images/M10WIFI.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "M10",
				ImageSource = "ms-appx:///Images/M10.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "SJ5000x Elite",
				ImageSource = "ms-appx:///Images/SJ5000X.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "SJ5000 Wifi",
				ImageSource = "ms-appx:///Images/SJ5000WIFI.png"
			});
			Source.Add(new StartupModel()
			{
				Description = "SJ5000",
				ImageSource = "ms-appx:///Images/SJ5000.png"
			});
		}

		private async void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
		{
			ApplicationView tmp = ApplicationView.GetForCurrentView();
			if (tmp.Orientation == currentView.Orientation)
				return;
			currentView = tmp;
			if (currentView.Orientation == ApplicationViewOrientation.Landscape)
				await Background.Rotate(90, (float)(Background.Width / 2), (float)(Background.Height / 2), 300, 0).StartAsync();
			if (currentView.Orientation == ApplicationViewOrientation.Portrait)
				await Background.Rotate(-90, (float)Background.Width / 2, (float)Background.Height / 2, 300, 0).StartAsync();
		}

		private async void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			StartupModel model = (sender as GridView).SelectedItem as StartupModel;
			Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			localSettings.Values["CameraModel"] = model.Description;
			//			await (sender as GridView).Fade(0, 400, 0).Offset(0, 50, 400, 0).StartAsync();
			foreach (var item in MainStack.Children)
				await item.Fade(0, 500, 0).StartAsync();
			await Background.Blur(0, 200, 0).StartAsync();
			Frame.Navigate(typeof(ConnectionPage));
		}
	}
}