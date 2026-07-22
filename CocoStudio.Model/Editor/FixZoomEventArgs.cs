using System;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000057 RID: 87
	public class FixZoomEventArgs : EventArgs
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000C4DF File Offset: 0x0000A6DF
		public bool Type { get; set; }

		// Token: 0x0600030C RID: 780 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
		public FixZoomEventArgs(bool type)
		{
			this.Type = type;
		}
	}
}
