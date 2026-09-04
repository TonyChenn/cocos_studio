using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000CC RID: 204
	public class RenderFrameIndexChangedEventArgs : EventArgs
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00019FDC File Offset: 0x000181DC
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x00019FF3 File Offset: 0x000181F3
		public Frame Frame { get; private set; }

		// Token: 0x0600066E RID: 1646 RVA: 0x00019FFC File Offset: 0x000181FC
		public RenderFrameIndexChangedEventArgs()
		{
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001A007 File Offset: 0x00018207
		public RenderFrameIndexChangedEventArgs(Frame frame)
		{
			this.Frame = frame;
		}
	}
}
