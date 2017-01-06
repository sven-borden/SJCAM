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
		public Action()
		{

		}

		public async Task<bool> BuildRequestAsync(string cmdNumber, string param = "")
		{
			string tmp = baseURL + "?custom=1&cmd=" + cmdNumber;
			if (param != "")
				tmp += "&par=" + param;
			HttpClient client = new HttpClient();
			try
			{
				HttpResponseMessage msg = await client.GetAsync(tmp);
				if (msg.IsSuccessStatusCode)
					return true;
				else
					return false;
			}
			catch(Exception e)
			{
				Debug.WriteLine("[REQUEST]: get error to get request");
				return false;
			}
		}
	}
}
