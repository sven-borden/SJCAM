using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCAM.Logic.Settings
{
	class MultipleOptionsSetting : Setting
	{
		public List<string> Possibilities { get; set; }		

		public MultipleOptionsSetting()
		{
			
		}

		public MultipleOptionsSetting(string _n)
		{
			this.Name = _n;
		}
	}
}
