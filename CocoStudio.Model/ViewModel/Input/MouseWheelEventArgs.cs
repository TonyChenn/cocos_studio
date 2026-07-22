using System;
using Gdk;

namespace CocoStudio.Model.ViewModel.Input
{
	// Token: 0x02000124 RID: 292
	public class MouseWheelEventArgs
	{
		// Token: 0x06000AFE RID: 2814 RVA: 0x0002B6FD File Offset: 0x000298FD
		public MouseWheelEventArgs(Point point, int delta)
		{
			this.Point = point;
			this.Delta = delta;
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002B718 File Offset: 0x00029918
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x0002B72F File Offset: 0x0002992F
		public int Delta { get; private set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0002B738 File Offset: 0x00029938
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0002B74F File Offset: 0x0002994F
		public Point Point { get; private set; }
	}
}
