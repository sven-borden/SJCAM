using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic.Settings
{
	public static class PhotoSettings
	{
		private static readonly string _PhotoResolution = "1002";
		public static int PhotoResolution { get; set; }

		private static readonly string _NbPhotoLeft = "";
		public static int NbPhotoLeft { get; set; }

		public static bool LoadPhotoSettings(string _settings)
		{
			string[] Cmd = _settings.Split(new string[] { "<Cmd>", "</Cmd>" }, StringSplitOptions.RemoveEmptyEntries);
			string[] Status = _settings.Split(new string[] { "<Status>", "</Status>" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < Cmd.Length; i++)
			{
				if (Cmd[i].Contains(_PhotoResolution))
					PhotoResolution = Convert.ToInt32(Status[i]);
			}
			return true;
		}
	}
}
