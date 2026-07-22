using System;

namespace CocoStudio.Model.Event
{
	// Token: 0x020000A3 RID: 163
	public class CanvasZoomChangeEventArgs
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00018424 File Offset: 0x00016624
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0001843B File Offset: 0x0001663B
		public PointF MousePoint { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00018444 File Offset: 0x00016644
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0001845B File Offset: 0x0001665B
		public float ZoomDelta { get; private set; }

		// Token: 0x06000578 RID: 1400 RVA: 0x00018464 File Offset: 0x00016664
		public CanvasZoomChangeEventArgs(float zoomDelta, PointF mousePoint)
		{
			this.ZoomDelta = zoomDelta;
			this.MousePoint = mousePoint;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001847F File Offset: 0x0001667F
		public CanvasZoomChangeEventArgs(float zoomDelta)
		{
			this.ZoomDelta = zoomDelta;
		}
	}
}
