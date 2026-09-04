using System;
using System.Collections.Generic;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	// Token: 0x02000009 RID: 9
	public class ArevaloRectanglePacker : RectanglePacker
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000038CC File Offset: 0x00001ACC
		public ArevaloRectanglePacker(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
		{
			this.packedRectangles = new List<Rectangle>();
			this.anchors = new List<Point>();
			this.anchors.Add(new Point(0, 0));
			this.actualPackingAreaWidth = 1;
			this.actualPackingAreaHeight = 1;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000391C File Offset: 0x00001B1C
		public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
		{
			int num = this.selectAnchorRecursive(rectangleWidth, rectangleHeight, this.actualPackingAreaWidth, this.actualPackingAreaHeight);
			bool result;
			if (num == -1)
			{
				placement = Point.Empty;
				result = false;
			}
			else
			{
				placement = this.anchors[num];
				this.optimizePlacement(ref placement, rectangleWidth, rectangleHeight);
				bool flag = placement.X + rectangleWidth > this.anchors[num].X && placement.Y + rectangleHeight > this.anchors[num].Y;
				if (flag)
				{
					this.anchors.RemoveAt(num);
				}
				this.insertAnchor(new Point(placement.X + rectangleWidth, placement.Y));
				this.insertAnchor(new Point(placement.X, placement.Y + rectangleHeight));
				this.packedRectangles.Add(new Rectangle(placement.X, placement.Y, rectangleWidth, rectangleHeight));
				result = true;
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003A2C File Offset: 0x00001C2C
		private void optimizePlacement(ref Point placement, int rectangleWidth, int rectangleHeight)
		{
			Rectangle rectangle = new Rectangle(placement.X, placement.Y, rectangleWidth, rectangleHeight);
			int x = placement.X;
			while (this.isFree(ref rectangle, base.PackingAreaWidth, base.PackingAreaHeight))
			{
				x = rectangle.X;
				rectangle.X--;
			}
			rectangle.X = placement.X;
			int y = placement.Y;
			while (this.isFree(ref rectangle, base.PackingAreaWidth, base.PackingAreaHeight))
			{
				y = rectangle.Y;
				rectangle.Y--;
			}
			if (placement.X - x > placement.Y - y)
			{
				placement.X = x;
			}
			else
			{
				placement.Y = y;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003B04 File Offset: 0x00001D04
		private int selectAnchorRecursive(int rectangleWidth, int rectangleHeight, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			int num = this.findFirstFreeAnchor(rectangleWidth, rectangleHeight, testedPackingAreaWidth, testedPackingAreaHeight);
			int result;
			if (num != -1)
			{
				this.actualPackingAreaWidth = testedPackingAreaWidth;
				this.actualPackingAreaHeight = testedPackingAreaHeight;
				result = num;
			}
			else
			{
				bool flag = testedPackingAreaWidth < base.PackingAreaWidth;
				bool flag2 = testedPackingAreaHeight < base.PackingAreaHeight;
				bool flag3 = !flag || testedPackingAreaHeight < testedPackingAreaWidth;
				if (flag2 && flag3)
				{
					result = this.selectAnchorRecursive(rectangleWidth, rectangleHeight, testedPackingAreaWidth, Math.Min(testedPackingAreaHeight * 2, base.PackingAreaHeight));
				}
				else if (flag)
				{
					result = this.selectAnchorRecursive(rectangleWidth, rectangleHeight, Math.Min(testedPackingAreaWidth * 2, base.PackingAreaWidth), testedPackingAreaHeight);
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003BBC File Offset: 0x00001DBC
		private int findFirstFreeAnchor(int rectangleWidth, int rectangleHeight, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			Rectangle rectangle = new Rectangle(0, 0, rectangleWidth, rectangleHeight);
			for (int i = 0; i < this.anchors.Count; i++)
			{
				rectangle.X = this.anchors[i].X;
				rectangle.Y = this.anchors[i].Y;
				if (this.isFree(ref rectangle, testedPackingAreaWidth, testedPackingAreaHeight))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003C50 File Offset: 0x00001E50
		private bool isFree(ref Rectangle rectangle, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			bool flag = rectangle.X < 0 || rectangle.Y < 0 || rectangle.Right > testedPackingAreaWidth || rectangle.Bottom > testedPackingAreaHeight;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.packedRectangles.Count; i++)
				{
					if (this.packedRectangles[i].IntersectsWith(rectangle))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003CDC File Offset: 0x00001EDC
		private void insertAnchor(Point anchor)
		{
			int num = this.anchors.BinarySearch(anchor, ArevaloRectanglePacker.AnchorRankComparer.Default);
			if (num < 0)
			{
				num = ~num;
			}
			this.anchors.Insert(num, anchor);
		}

		// Token: 0x04000019 RID: 25
		private int actualPackingAreaWidth;

		// Token: 0x0400001A RID: 26
		private int actualPackingAreaHeight;

		// Token: 0x0400001B RID: 27
		private List<Rectangle> packedRectangles;

		// Token: 0x0400001C RID: 28
		private List<Point> anchors;

		// Token: 0x0200000A RID: 10
		private class AnchorRankComparer : IComparer<Point>
		{
			// Token: 0x06000042 RID: 66 RVA: 0x00003D18 File Offset: 0x00001F18
			public int Compare(Point left, Point right)
			{
				return left.X + left.Y - (right.X + right.Y);
			}

			// Token: 0x0400001D RID: 29
			public static ArevaloRectanglePacker.AnchorRankComparer Default = new ArevaloRectanglePacker.AnchorRankComparer();
		}
	}
}
