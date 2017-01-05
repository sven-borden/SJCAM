using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic.Settings
{
	class ToggleSetting : Setting
	{
		public bool IsOn { get; set; }

		public ToggleSetting()
		{
			
		}

		public ToggleSetting(string _n)
		{
			this.Name = _n;
		}
	}
}
