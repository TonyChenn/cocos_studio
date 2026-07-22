using System;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	// Token: 0x02000008 RID: 8
	public abstract class RectanglePacker
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003856 File Offset: 0x00001A56
		protected RectanglePacker(int packingAreaWidth, int packingAreaHeight)
		{
			this.packingAreaWidth = packingAreaWidth;
			this.packingAreaHeight = packingAreaHeight;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003870 File Offset: 0x00001A70
		public virtual Point Pack(int rectangleWidth, int rectangleHeight)
		{
			Point result;
			if (!this.TryPack(rectangleWidth, rectangleHeight, out result))
			{
				throw new OutOfSpaceException("Rectangle does not fit in packing area");
			}
			return result;
		}

		// Token: 0x06000038 RID: 56
		public abstract bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000389C File Offset: 0x00001A9C
		protected int PackingAreaWidth
		{
			get
			{
				return this.packingAreaWidth;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000038B4 File Offset: 0x00001AB4
		protected int PackingAreaHeight
		{
			get
			{
				return this.packingAreaHeight;
			}
		}

		// Token: 0x04000017 RID: 23
		private int packingAreaWidth;

		// Token: 0x04000018 RID: 24
		private int packingAreaHeight;
	}
}
