using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SJCAM.Custom
{
	public sealed partial class FilesControl : UserControl
	{
		Logic.Action action;
		public FilesControl()
		{
			action = new Logic.Action();
			this.InitializeComponent();
			Setup();
		}

		private async void Setup()
		{
			try
			{
				string s = await action.GetRequestAsync("3015"); Debug.WriteLine(s);
				await action.FileRequestAsync(@"\DCIM\Movie\2016_1230_155824_001.MP4");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			};
		}
	}
}
