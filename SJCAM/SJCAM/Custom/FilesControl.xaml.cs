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
		List<string> NamePhotoList;
		List<string> NameVideoList;
		public ObservableCollection<string> Photo;
		public ObservableCollection<string> Video;

		public FilesControl()
		{
			action = new Logic.Action();
			NamePhotoList = new List<string>();
			NameVideoList = new List<string>();
			Photo = new ObservableCollection<string>();
			Video = new ObservableCollection<string>();
			this.InitializeComponent();
			Setup();
		}

		private async void Setup()
		{
			try
			{
				string s = await action.GetRequestAsync("3015"); Debug.WriteLine(s);
				ParseXML(s);
				UpdateXAML();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			};
		}

		private void UpdateXAML()
		{
			foreach (string item in NamePhotoList)
				Photo.Add("http://192.168.1.254/DCIM/PHOTO/"+item);
			this.Bindings.Update();
			Debug.WriteLine("Items : " + NamePhotoList.Count);
		}

		private void ParseXML(string input)
		{
			string[] _input = input.Split(new string[] { "<NAME>", "</NAME>" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string item in _input)
			{
				if (item.Contains("SIZE"))
					continue;
				if (item.Contains(".JPG"))
					NamePhotoList.Add(item);
				if (item.Contains(".MP4"))
					NameVideoList.Add(item);
			}				
		}
	}
}
