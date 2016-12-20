using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Direct
{
	public class Command
	{
		protected string BaseUri = "http://192.168.1.254/";
		public string LiveFeed = "rtsp://192.168.1.254/sjcam.mov";
		public string command { get; }
		public Command()
		{
			command = "";
		}

		public string BuildRequest(string cmdNumber, string param = "")
		{
			string tmp = BaseUri + "?custom=1&cmd=" + cmdNumber;
			if (param != "")
				tmp += "&par=" + param;
			return tmp;
		}
	}
}
