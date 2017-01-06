using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic
{
	public class Action
	{
		private const string baseURL = "http://192.168.1.254";
		bool DEBUG = false;
		public Action()
		{

		}

		private string BuildRequestAsync(string cmdNumber, string param = "")
		{
			string tmp = baseURL + "?custom=1&cmd=" + cmdNumber;
			if (param != "")
				tmp += "&par=" + param;
			return tmp;
		}

		public async Task<string> GetRequestAsync(string cmdNumber, string param = "")
		{
			string tmp = BuildRequestAsync(cmdNumber, param);
			HttpClient client = new HttpClient();
			try
			{
				string msg = await client.GetStringAsync(tmp);
				Debug.WriteLineIf(DEBUG, msg);
				return msg;
			}
			catch (Exception e)
			{
				Debug.WriteLine("[REQUEST]: get error to get request");
				return null;
			}
		}
	}
}
