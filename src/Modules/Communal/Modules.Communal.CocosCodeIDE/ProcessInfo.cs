using System;
using System.Collections.Generic;

namespace Modules.Communal.CocosCodeIDE
{
	public class ProcessInfo
	{
		public int processID { get; set; }

		public int port { get; set; }

		public List<string> projectDirs { get; set; }
	}
}
