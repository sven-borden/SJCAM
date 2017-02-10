using System.Collections.Generic;

namespace SJCAM.Parameters
{
	public static class SJ5000WIFI
	{
		public static List<string> PhotoResolution = new List<string>()
		{
			"14Mpx",
			"12Mpx",
			"10Mpx",
			"8Mpx",
			"5Mpx",
			"3Mpx",
			"VGA"
		};

		public static List<string> VideoResolution = new List<string>()
		{
			"1080p 60fps",
			"1080p 30fps",
			"720p 120fps",
			"720p 60fps",
			"720p 30fps",
			"480p 240fps"
		};

		public static bool Gyro = false;
	}
}
