using System;
using System.Collections.Generic;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	// Token: 0x0200000B RID: 11
	public class CygonRectanglePacker : RectanglePacker
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003D5D File Offset: 0x00001F5D
		public CygonRectanglePacker(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
		{
			this.heightSlices = new List<Point>();
			this.heightSlices.Add(new Point(0, 0));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003D88 File Offset: 0x00001F88
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
				bool flag = this.tryFindBestPlacement(rectangleWidth, rectangleHeight, out placement);
				if (flag)
				{
					this.integrateRectangle(placement.X, rectangleWidth, placement.Y + rectangleHeight);
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003DF0 File Offset: 0x00001FF0
		private bool tryFindBestPlacement(int rectangleWidth, int rectangleHeight, out Point placement)
		{
			int num = -1;
			int y = 0;
			int num2 = base.PackingAreaHeight;
			int num3 = 0;
			int i = this.heightSlices.BinarySearch(new Point(rectangleWidth, 0), CygonRectanglePacker.SliceStartComparer.Default);
			if (i < 0)
			{
				i = ~i;
			}
			while (i <= this.heightSlices.Count)
			{
				int y2 = this.heightSlices[num3].Y;
				for (int j = num3 + 1; j < i; j++)
				{
					if (this.heightSlices[j].Y > y2)
					{
						y2 = this.heightSlices[j].Y;
					}
				}
				if (y2 + rectangleHeight <= base.PackingAreaHeight)
				{
					int num4 = y2;
					if (num4 < num2)
					{
						num = num3;
						y = y2;
						num2 = num4;
					}
				}
				num3++;
				if (num3 >= this.heightSlices.Count)
				{
					break;
				}
				int num5 = this.heightSlices[num3].X + rectangleWidth;
				while (i <= this.heightSlices.Count)
				{
					int num6;
					if (i == this.heightSlices.Count)
					{
						num6 = base.PackingAreaWidth;
					}
					else
					{
						num6 = this.heightSlices[i].X;
					}
					if (num6 > num5)
					{
						break;
					}
					i++;
				}
				if (i > this.heightSlices.Count)
				{
					break;
				}
			}
			bool result;
			if (num == -1)
			{
				placement = Point.Empty;
				result = false;
			}
			else
			{
				placement = new Point(this.heightSlices[num].X, y);
				result = true;
			}
			return result;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003FF8 File Offset: 0x000021F8
		private void integrateRectangle(int left, int width, int bottom)
		{
			int num = this.heightSlices.BinarySearch(new Point(left, 0), CygonRectanglePacker.SliceStartComparer.Default);
			int y = this.heightSlices[num].Y;
			this.heightSlices[num] = new Point(left, bottom);
			int num2 = left + width;
			num++;
			if (num >= this.heightSlices.Count)
			{
				if (num2 < base.PackingAreaWidth)
				{
					this.heightSlices.Add(new Point(num2, y));
				}
			}
			else
			{
				int num3 = this.heightSlices.BinarySearch(num, this.heightSlices.Count - num, new Point(num2, 0), CygonRectanglePacker.SliceStartComparer.Default);
				if (num3 > 0)
				{
					this.heightSlices.RemoveRange(num, num3 - num);
				}
				else
				{
					num3 = ~num3;
					int y2;
					if (num3 == num)
					{
						y2 = y;
					}
					else
					{
						y2 = this.heightSlices[num3 - 1].Y;
					}
					this.heightSlices.RemoveRange(num, num3 - num);
					if (num2 < base.PackingAreaWidth)
					{
						this.heightSlices.Insert(num, new Point(num2, y2));
					}
				}
			}
		}

		// Token: 0x0400001E RID: 30
		private List<Point> heightSlices;

		// Token: 0x0200000C RID: 12
		private class SliceStartComparer : IComparer<Point>
		{
			// Token: 0x06000049 RID: 73 RVA: 0x00004140 File Offset: 0x00002340
			public int Compare(Point left, Point right)
			{
				return left.X - right.X;
			}

			// Token: 0x0400001F RID: 31
			public static CygonRectanglePacker.SliceStartComparer Default = new CygonRectanglePacker.SliceStartComparer();
		}
	}
}
