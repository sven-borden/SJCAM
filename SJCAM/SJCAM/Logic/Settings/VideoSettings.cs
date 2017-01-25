using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic.Settings
{
	public static class VideoSettings
	{
		private static readonly string _VideoResolution = "2002";
		public static int VideoResolution { get; set; }

		private static readonly string _IsRecording = "2016";
		public static int IsRecording { get; set; }

		private static readonly string _TimeRemaining = "2009";
		public static int TimeRemaining { get; set; }

		public static bool LoadVideoSettings(string _settings)
		{
			string[] Cmd = _settings.Split(new string[] { "<Cmd>", "</Cmd>" }, StringSplitOptions.RemoveEmptyEntries);
			string[] Status = _settings.Split(new string[] { "<Status>", "</Status>" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < Cmd.Length; i++)
			{
				if (Cmd[i].Contains(_VideoResolution))
					VideoResolution = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_IsRecording))
					IsRecording = Convert.ToInt32(Status[i]);
				if (Cmd[i].Contains(_TimeRemaining))
					TimeRemaining = Convert.ToInt32(Status[i]);
			}
			return true;
		}
	}
}
