using System;

namespace CocoStudio.Model.Event
{
	// Token: 0x02000073 RID: 115
	public class CanvasSizeChangeEventArgs
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000134CC File Offset: 0x000116CC
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x000134E3 File Offset: 0x000116E3
		public string CanvasName { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000134EC File Offset: 0x000116EC
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00013503 File Offset: 0x00011703
		public SizeF NewSize { get; private set; }

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001350C File Offset: 0x0001170C
		public CanvasSizeChangeEventArgs(string canvasName, SizeF newSize)
		{
			this.CanvasName = canvasName;
			this.NewSize = newSize;
		}
	}
}
