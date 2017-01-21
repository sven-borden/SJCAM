using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
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
	public sealed partial class ConnectionPage : Page
	{
		ApplicationView currentView;

		public ConnectionPage()
		{
			this.InitializeComponent();
			currentView = ApplicationView.GetForCurrentView();
			Window.Current.SizeChanged += Current_SizeChanged;
			Background.Blur(5f, 1000, 200).Start();
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
	}
}
