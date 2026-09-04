using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000012 RID: 18
	public class CancelableEventArgs<TPayload>
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000031DC File Offset: 0x000013DC
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000031F4 File Offset: 0x000013F4
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				if (value)
				{
					this.cancel = true;
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003214 File Offset: 0x00001414
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000322B File Offset: 0x0000142B
		public TPayload Payload { get; set; }

		// Token: 0x06000088 RID: 136 RVA: 0x00003234 File Offset: 0x00001434
		public CancelableEventArgs(TPayload payload)
		{
			this.Payload = payload;
		}

		// Token: 0x0400001E RID: 30
		private bool cancel;
	}
}
