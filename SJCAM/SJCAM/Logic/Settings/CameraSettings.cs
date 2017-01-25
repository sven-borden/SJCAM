using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic
{
	public static class CameraSettings
	{
		private static readonly string _Exposure = "2005";
		public static int Exposure { get; set; }

		private static readonly string _WhiteBalance = "1007";
		public static int WhiteBalance { get; set; }

		private static readonly string _PhotoLapse = "1012";
		public static int PhotoLapse { get; set; }

		private static readonly string _VideoLapse = "2019";
		public static int VideoLapse { get; set; }

		private static readonly string _AutoPower = "3007";
		public static int AutoPower { get; set; }

		private static readonly string _GyroOn = "9001";
		public static int GyroOn { get; set; }

		private static readonly string _WDROn = "2004";
		public static int WDROn { get; set; }

		private static readonly string _AudioOn = "2007";
		public static int AudioOn { get; set; }

		private static readonly string _TimeStamp = "2005";
		public static int TimeStamp { get; set; }

		private static readonly string _Frequency = "";
		public static int Frequency { get; set; }

		private static readonly string _VideoFormat = "";
		public static int VideoFormat { get; set; }

		public static bool LoadCameraSettings(string _settings)
		{
			string[] Cmd = _settings.Split(new string[] { "<Cmd>", "</Cmd>" }, StringSplitOptions.RemoveEmptyEntries);
			string[] Status = _settings.Split(new string[] { "<Status>", "</Status>" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < Cmd.Length; i++)
			{
				if (Cmd[i].Contains(_Exposure))
					Exposure = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_WhiteBalance))
					WhiteBalance = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_PhotoLapse))
					PhotoLapse = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_VideoLapse))
					VideoLapse = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_AutoPower))
					AutoPower = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_GyroOn))
					GyroOn = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_AudioOn))
					AudioOn = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_WDROn))
					WDROn = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_TimeStamp))
					TimeStamp = Convert.ToInt32(Status[i]);
			}
			return true;
		}
	}
}
