using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Direct
{
	class Mode
	{
		/// <summary>
		/// Resolution
		/// </summary>
		public int Resolution { get; set; }
		public enum ResolutionValue
		{
			R12M,
			R10M,
			R8M,
			R5M,
			R3M,
			R2M,
			RVGA,
			R1M
		}

		/// <summary>
		/// TimeLapse
		/// </summary>
		public int TimeLapseInterval { get; set; }
		public enum TimeLapseIntervalValue
		{
			T3 = 3,
			T5 = 5,
			T10 = 10,
			T20 = 20
		}

		/// <summary>
		/// Exposure
		/// </summary>
		public int Exposure { get; set; }
		public enum ExposureValue
		{
			E2,
			E53,
			E43,
			E1,
			E23,
			E13,
			E0,
			EM13,
			EM23,
			EM1,
			EM43,
			EM53,
			EM2
		}

		/// <summary>
		/// WhiteBalance
		/// </summary>
		public int WhiteBalance { get; set; }
		public enum WhiteBalanceValue
		{
			Auto,
			Daylight,
			Cloudy,
			Tungsten,
			Fluorescent
		}

		/// <summary>
		/// VideoLapseInterval
		/// </summary>
		public int VideoLapseInterval { get; set; }
		public enum VideoLapseIntervalValue
		{
			S1,
			S2,
			S5,
			S10,
			S30,
			S60
		}

		/// <summary>
		/// Mode
		/// </summary>
		public int _Mode { get; set; }
		public enum ModeValue
		{
			Photo,
			Video,
			Replay,
			VideoTimeLapse,
			PhotoTimeLapse
		}

		/// <summary>
		/// Audio
		/// </summary>
		public int Audio { get; set; }
		public enum AudioValue { Off, On};
		
		/// <summary>
		/// WDR
		/// </summary>
		public int WDR { get; set; }
		public enum WDRValue { Off, On}; 

		/// <summary>
		/// CyclicRecord
		/// </summary>
		public int CyclicRecord { get; set; }
		public enum CyclicRecordValue
		{
			Auto,
			M3,
			M5,
			M10,
			Off
		}

		/// <summary>
		/// Frequency
		/// </summary>
		public int Frequency { get; set; }
		public enum FrequencyValue { Low = 50, Std = 60}

		/// <summary>
		/// DateSpamp
		/// </summary>
		public int DateStamp { get; set; }
		public enum DateStampValue { Off, On }

		public Mode()
		{
			Resolution = (int)ResolutionValue.R12M;
			TimeLapseInterval = (int)TimeLapseIntervalValue.T5;
			Exposure = (int)ExposureValue.E0;
			WhiteBalance = (int)WhiteBalanceValue.Auto;
			_Mode = (int)ModeValue.Photo;
		}
	}
}
