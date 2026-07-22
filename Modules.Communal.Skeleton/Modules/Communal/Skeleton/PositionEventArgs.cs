using System;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200002C RID: 44
	public class PositionEventArgs : EventArgs
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00009FE6 File Offset: 0x000081E6
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00009FEE File Offset: 0x000081EE
		public double PointX { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00009FF7 File Offset: 0x000081F7
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00009FFF File Offset: 0x000081FF
		public double PointY { get; private set; }

		// Token: 0x060001ED RID: 493 RVA: 0x0000A008 File Offset: 0x00008208
		public PositionEventArgs(double x, double y)
		{
			this.PointX = x;
			this.PointY = y;
		}
	}
}
