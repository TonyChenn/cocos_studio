using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000EB RID: 235
	public class SpeedChangedArgs : EventArgs
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0001F040 File Offset: 0x0001D240
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0001F057 File Offset: 0x0001D257
		public float Speed { get; private set; }

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001F060 File Offset: 0x0001D260
		public SpeedChangedArgs(float speed)
		{
			this.Speed = speed;
		}
	}
}
