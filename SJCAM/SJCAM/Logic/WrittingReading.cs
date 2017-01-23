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
			XmlSerializer serializer = new XmlSerializer(typeof(List<WifiSpot>));
			FileStream fs = new FileStream(_Path, FileMode.CreateNew);
			serializer.Serialize(fs, wifi);
		}

		public static List<WifiSpot> Deserialize(string _Path)
		{
			List<WifiSpot> wifi = null;

			XmlSerializer serializer = new XmlSerializer(typeof(List<WifiSpot>));
			FileStream fs = new FileStream(_Path, FileMode.Open);
			wifi = (List<WifiSpot>)serializer.Deserialize(fs);

			return wifi;
		} 
	}
}
