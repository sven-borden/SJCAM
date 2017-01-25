using SJCAM.Logic.Wifi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SJCAM.Logic
{
	public static class WrittingReading
	{
		public static void Serialize(List<WifiSpot> wifi, string _Path)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<WifiSpot>));
				FileStream fs = new FileStream(_Path, FileMode.OpenOrCreate);
				serializer.Serialize(fs, wifi);
			}
			catch(Exception e)
			{
				e.ToString();
			}
		}

		public static List<WifiSpot> Deserialize(string _Path)
		{
			List<WifiSpot> wifi = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<WifiSpot>));
				FileStream fs = new FileStream(_Path, FileMode.Open);
				wifi = (List<WifiSpot>)serializer.Deserialize(fs);
			}
			catch(Exception e)
			{
				e.ToString();
			}
			return wifi;
		} 
	}
}
