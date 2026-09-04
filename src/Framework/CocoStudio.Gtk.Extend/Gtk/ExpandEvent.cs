using System;

namespace Gtk
{
	// Token: 0x02000018 RID: 24
	public class ExpandEvent : EventArgs
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004AF4 File Offset: 0x00002CF4
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004B0B File Offset: 0x00002D0B
		public string Name { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004B14 File Offset: 0x00002D14
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004B2B File Offset: 0x00002D2B
		public bool Expand { get; private set; }

		// Token: 0x060000B6 RID: 182 RVA: 0x00004B34 File Offset: 0x00002D34
		public ExpandEvent(string name, bool expand)
		{
			this.Name = name;
			this.Expand = expand;
		}
	}
}
