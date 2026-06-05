using System;

namespace Gtk
{
	// Token: 0x02000019 RID: 25
	public class EntryCallBackEventArgs : EventArgs
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00004B4F File Offset: 0x00002D4F
		public EntryCallBackEventArgs(string value)
		{
			this.Value = value;
		}

		// Token: 0x04000044 RID: 68
		public string Value;
	}
}
