using System;
using System.Collections.Generic;
using System.Linq;

namespace Gtk
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class FileDropInfo
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000AC54 File Offset: 0x00008E54
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000AC6B File Offset: 0x00008E6B
		public IEnumerable<string> FileArray { get; private set; }

		// Token: 0x060002C1 RID: 705 RVA: 0x0000AC74 File Offset: 0x00008E74
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

		// Token: 0x060002C2 RID: 706 RVA: 0x0000ACC9 File Offset: 0x00008EC9
		public FileDropInfo(IEnumerable<string> filearray)
		{
			this.FileArray = filearray;
		}
	}
}
