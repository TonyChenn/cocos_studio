using System;
using CocoStudio.Model;

namespace Modules.Communal.Render
{
	// Token: 0x02000022 RID: 34
	public class MouseGesturesEventArgs : EventArgs
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00007A10 File Offset: 0x00005C10
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00007A27 File Offset: 0x00005C27
		public double X { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00007A30 File Offset: 0x00005C30
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00007A47 File Offset: 0x00005C47
		public double Y { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007A50 File Offset: 0x00005C50
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00007A67 File Offset: 0x00005C67
		public double Zoom { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00007A70 File Offset: 0x00005C70
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00007A87 File Offset: 0x00005C87
		public bool Retval { get; set; }

		// Token: 0x06000122 RID: 290 RVA: 0x00007A90 File Offset: 0x00005C90
		public PointF GetPoint()
		{
			return new PointF((float)this.X, (float)this.Y);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public bool CheckRetval()
		{
			return this.Retval;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007AD0 File Offset: 0x00005CD0
		public MouseGesturesEventArgs(double x, double y, double zoom)
		{
			this.X = x;
			this.Y = y;
			this.Zoom = zoom;
		}
	}
}
