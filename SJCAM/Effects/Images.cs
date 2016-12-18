using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SJCAM.Effects
{
	class Images
	{
		public Images()
		{
		}

		public static async Task<Color> GetDominant()
		{
			Color dominantColor = new Color() { R = 0, G = 0, B = 0 } ;
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

			return dominantColor;
		}
	}
}
