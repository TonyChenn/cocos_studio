using System;

namespace CocoStudio.Model.Event
{
	// Token: 0x0200007B RID: 123
	public class PasteObjectsChangeEventArgs
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00013604 File Offset: 0x00011804
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x000135FA File Offset: 0x000117FA
		public PointF PastePosition { get; set; }

		// Token: 0x06000467 RID: 1127 RVA: 0x0001361B File Offset: 0x0001181B
		public PasteObjectsChangeEventArgs(PointF position)
		{
			this.PastePosition = position;
		}
	}
}
