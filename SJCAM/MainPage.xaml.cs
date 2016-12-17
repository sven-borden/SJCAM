using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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
		CanvasBitmap bitmapTiger;
		ICanvasImage effect;

		public MainPage()
        {
            this.InitializeComponent();
			Init();
        }

		private async void Init()
		{
			
		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			appSplit.IsPaneOpen = !appSplit.IsPaneOpen;
		}

		private void backgroundCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
		{
			var size = sender.Size;
			var ds = args.DrawingSession;

			ds.DrawImage(effect, new Rect(0, 0, size.Width, size.Height), new Rect(0, 0, 1000, 625));
			Debug.WriteLine("Draw");
		}


		private void backgroundCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
		{
			Debug.WriteLine("CreateResource 1");
			args.TrackAsyncAction(backgroundCanvas_CreateResourcesAsync(sender).AsAsyncAction());
		}

		private async Task backgroundCanvas_CreateResourcesAsync(CanvasControl sender)
		{
			Debug.WriteLine("CreateReources 2");
			bitmapTiger = await CanvasBitmap.LoadAsync(sender, new Uri("http://d13fo19pohlmh8.cloudfront.net/media/assets/09e3fc49dbbcf9b40049c1e8ecf068ea.jpeg"));

			effect = CreateGaussianBlur();
		}

		private ICanvasImage CreateGaussianBlur()
		{
			var blurEffect = new GaussianBlurEffect
			{
				Source = bitmapTiger,
				BorderMode = EffectBorderMode.Hard,
				BlurAmount = 3
			};
			Debug.WriteLine("Gaussian");
			return blurEffect;
		}
	}
}
