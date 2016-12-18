using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SJCAM.Effects
{
	class Images
	{
		public Images()
		{
			
		}

		public static async Task<Color> saveImage(Image sourceImage)
		{
			Color dominantColor = new Color() { R = 0, G = 0, B = 0 } ;
			try
			{
				double Red = 0;
				double Green = 0;
				double Blue = 0;

				CanvasDevice device = new CanvasDevice();
				CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(device, new Uri("http://1.bp.blogspot.com/-fIIXrDNarGE/UGo3e-o_f9I/AAAAAAAAGEs/f7SmLFsIhjg/s1600/landscape_hd.jpg"));
				CanvasRenderTarget renderer = new CanvasRenderTarget(device, bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height, bitmap.Dpi);

				double size = bitmap.SizeInPixels.Width * bitmap.SizeInPixels.Height;
				foreach(Color item in bitmap.GetPixelColors())
				{
					Red += item.R;
					Green += item.G;
					Blue += item.B;
				}
				Red /= size;
				Green /= size;
				Blue /= size;
				Debug.WriteLine(Red + " " + Green + " " + Blue);
				dominantColor.R = (byte)Red;
				dominantColor.G = (byte)Green;
				dominantColor.B = (byte)Blue;
				dominantColor.A = byte.MaxValue;

				Debug.WriteLine(dominantColor.R + " " + dominantColor.G + " " + dominantColor.B);
				using (var ds = renderer.CreateDrawingSession())
				{
					var blur = new GaussianBlurEffect();
					blur.BlurAmount = 5.0f;
					blur.BorderMode = EffectBorderMode.Hard;
					blur.Optimization = EffectOptimization.Quality;
					blur.Source = bitmap;
					ds.DrawImage(blur);
				}
				var saveFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("temp.jpg", CreationCollisionOption.ReplaceExisting);

				using (var outStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
				{
					await renderer.SaveAsync(outStream, CanvasBitmapFileFormat.Jpeg);
					string path = ApplicationData.Current.LocalFolder.Path + "/temp.jpg";
					sourceImage.Source = new BitmapImage(new Uri(path));
					sourceImage.Stretch = Stretch.UniformToFill;
				}
			}
			catch(Exception e)
			{
				e.ToString();
			}
			return dominantColor;
		}
	}
}
