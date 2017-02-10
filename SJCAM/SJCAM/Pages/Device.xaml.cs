using Microsoft.Toolkit.Uwp.UI.Animations;
using SJCAM.Custom;
using SJCAM.Logic;
using SJCAM.Logic.Settings;
using SJCAM.Style;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SJCAM.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Device : Page
    {
        ApplicationView currentView;
        public string Model { get; private set; }
        ApplicationDataContainer localSettings;
        Logic.Action action;

        public Device()
        {
			ConnectionStatus.IsConnected = true;
            localSettings = ApplicationData.Current.LocalSettings;
            Model = localSettings.Values["CameraModel"] as string;
            action = new Logic.Action();
            this.InitializeComponent();
            currentView = ApplicationView.GetForCurrentView();
            Window.Current.SizeChanged += Current_SizeChanged;
            Animate();
            this.Loaded += (e, o) =>
            {
                if (ConnectionStatus.IsConnected == false)
                {
                    ShowErrorMsgAsync("Not connected to a camera");
                    return;
                }
                StreamPlayer.Width = this.ActualWidth - 40;
                StreamPlayer.Height = 150;
                StreamPlayer.Loaded += (r, p) =>
                {
                    //StreamPlayer.Play();
                };
            };

            LoadSettings();
        }

        private async void LoadSettings()
        {
            string settings = await action.GetRequestAsync("3014");
            CameraSettings.LoadCameraSettings(settings);
            PhotoSettings.LoadPhotoSettings(settings);
            VideoSettings.LoadVideoSettings(settings);
            PhotoStack.Children.Add(new PhotoControl());
            VideoStack.Children.Add(new VideoControl());
            SettingsStack.Children.Add(new SettingsControls());
        }

        private async void Animate()
        {
            Background.Blur(8f, 1000, 1000).Start();
            foreach (var item in MainGrid.Children)
                item.Fade(0, 0, 0).Start();
            MainGrid.Fade(1, 1, 1).Start();
            foreach (var item in MainGrid.Children)
                await item.Fade(1, 800, 0).StartAsync();
            MainPivot.SelectedIndex = 0;
            HeaderPhoto.Background = AppColor.PivotSelectedColor;
        }

        private async void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetAllPivotToTransparent();
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    HeaderPhoto.Background = AppColor.PivotSelectedColor;
                    try
                    { await action.GetRequestAsync("3001", "0"); }
                    catch (Exception ex)
                    { ShowErrorMsgAsync(ex.Message); };
                    break;
                case 1:
                    HeaderVideo.Background = AppColor.PivotSelectedColor;
                    try
                    { await action.GetRequestAsync("3001", "1"); }
                    catch (Exception ex)
                    { ShowErrorMsgAsync(ex.Message); };
                    break;
                case 2:
                    HeaderSettings.Background = AppColor.PivotSelectedColor;
                    break;
                case 3:
                    HeaderFiles.Background = AppColor.PivotSelectedColor;
                    break;
                default:
                    break;
            }
        }

        private void SetAllPivotToTransparent()
        {
            HeaderPhoto.Background = HeaderVideo.Background = HeaderSettings.Background = HeaderFiles.Background = new SolidColorBrush(Colors.Transparent);
        }

        private async void ShowErrorMsgAsync(string msg)
        {
            MessageDialog d = new MessageDialog(msg, "Beta testing error");
            await d.ShowAsync();
        }

        private async void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
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
