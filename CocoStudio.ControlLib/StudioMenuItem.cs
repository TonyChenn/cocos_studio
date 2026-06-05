using System;
using Gtk;

namespace CocoStudio.ControlLib
{
	// Token: 0x02000011 RID: 17
	public class StudioMenuItem : MenuItem
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00006F5E File Offset: 0x0000515E
		public StudioMenuItem(string header) : base(header)
		{
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006F6C File Offset: 0x0000516C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00006F84 File Offset: 0x00005184
		public object Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}

		// Token: 0x04000077 RID: 119
		private object tag;
	}
}
