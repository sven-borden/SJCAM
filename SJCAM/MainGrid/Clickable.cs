using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SJCAM.MainGrid
{
	public class Clickable
	{
		public string Name { get; set; }
		public TextBlock Glyph { get; set; }

		public Clickable(string _name, string _glyph)
		{
			Name = _name;
			Glyph = new TextBlock() { Text = _glyph, FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets") };
		}
	}
}
