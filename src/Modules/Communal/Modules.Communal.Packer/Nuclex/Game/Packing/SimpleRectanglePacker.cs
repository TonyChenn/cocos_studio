using System;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	// Token: 0x02000018 RID: 24
	public class SimpleRectanglePacker : RectanglePacker
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00005EF4 File Offset: 0x000040F4
		public SimpleRectanglePacker(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005F04 File Offset: 0x00004104
		public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
		{
			bool result;
			if (rectangleWidth > base.PackingAreaWidth || rectangleHeight > base.PackingAreaHeight)
			{
				placement = Point.Empty;
				result = false;
			}
			else
			{
				if (this.column + rectangleWidth > base.PackingAreaWidth)
				{
					this.currentLine += this.lineHeight;
					this.lineHeight = 0;
					this.column = 0;
				}
				if (this.currentLine + rectangleHeight > base.PackingAreaHeight)
				{
					placement = Point.Empty;
					result = false;
				}
				else
				{
					placement = new Point(this.column, this.currentLine);
					this.column += rectangleWidth;
					if (rectangleHeight > this.lineHeight)
					{
						this.lineHeight = rectangleHeight;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0400003C RID: 60
		private int currentLine;

		// Token: 0x0400003D RID: 61
		private int lineHeight;

		// Token: 0x0400003E RID: 62
		private int column;
	}
}
