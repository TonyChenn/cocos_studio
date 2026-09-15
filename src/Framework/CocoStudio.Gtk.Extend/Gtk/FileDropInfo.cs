using System;
using System.Collections.Generic;
using System.Linq;

namespace Gtk
{
	[Serializable]
	public class FileDropInfo
	{
		public IEnumerable<string> FileArray { get; private set; }

		public FileDropInfo(string fileArray)
		{
			if (!string.IsNullOrEmpty(fileArray))
			{
				this.FileArray = fileArray.Split(new string[]
				{
					"file:///",
					"\r\n",
					"\0"
				}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			}
		}

		public FileDropInfo(IEnumerable<string> filearray)
		{
			this.FileArray = filearray;
		}
	}
}
