using System.Collections.Generic;

namespace SJCAM.Parameters
{
	public static class M10Plus
	{
		public static List<string> PhotoResolution = new List<string>()
		{
			"12Mpx",
			"10Mpx",
			"8Mpx",
			"5Mpx",
			"3Mpx",
		};

		public static List<string> VideoResolution = new List<string>()
		{
			"2K 30fps",
			"1080p 60fps",
			"1080p 30fps",
			"720p 120fps",
			"720p 60fps",
			"720p 30fps",
			"480p 240fps"
		};

		public static bool Gyro = true;
	}
}
