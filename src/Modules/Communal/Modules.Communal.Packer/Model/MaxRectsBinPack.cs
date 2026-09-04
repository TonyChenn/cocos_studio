using System;
using System.Collections.Generic;

namespace Modules.Communal.Packer.Model
{
	// Token: 0x02000005 RID: 5
	public class MaxRectsBinPack
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002374 File Offset: 0x00000574
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000238B File Offset: 0x0000058B
		public int RightEdge { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002394 File Offset: 0x00000594
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023AB File Offset: 0x000005AB
		public int BottomEdge { get; private set; }

		// Token: 0x06000024 RID: 36 RVA: 0x000023B4 File Offset: 0x000005B4
		public MaxRectsBinPack(int width, int height, bool rotations = true)
		{
			this.Init(width, height, rotations);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023F0 File Offset: 0x000005F0
		public void Init(int width, int height, bool rotations = true)
		{
			this.binWidth = width;
			this.binHeight = height;
			this.BottomEdge = (this.RightEdge = 0);
			this.allowRotations = rotations;
			CustomRectangle customRectangle = new CustomRectangle(null);
			customRectangle.X = 0;
			customRectangle.Y = 0;
			customRectangle.Width = width;
			customRectangle.Height = height;
			this.usedRectangles.Clear();
			this.freeRectangles.Clear();
			this.freeRectangles.Add(customRectangle);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002474 File Offset: 0x00000674
		public CustomRectangle Insert(int width, int height, MaxRectsBinPack.FreeRectChoiceHeuristic method)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			int num = 0;
			int num2 = 0;
			switch (method)
			{
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestShortSideFit:
				customRectangle = this.FindPositionForNewNodeBestShortSideFit(width, height, ref num, ref num2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestLongSideFit:
				customRectangle = this.FindPositionForNewNodeBestLongSideFit(width, height, ref num2, ref num);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestAreaFit:
				customRectangle = this.FindPositionForNewNodeBestAreaFit(width, height, ref num, ref num2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBottomLeftRule:
				customRectangle = this.FindPositionForNewNodeBottomLeft(width, height, ref num, ref num2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectContactPointRule:
				customRectangle = this.FindPositionForNewNodeContactPoint(width, height, ref num);
				break;
			}
			CustomRectangle result;
			if (customRectangle.Height == 0)
			{
				result = customRectangle;
			}
			else
			{
				int num3 = this.freeRectangles.Count;
				for (int i = 0; i < num3; i++)
				{
					if (this.SplitFreeNode(this.freeRectangles[i], ref customRectangle))
					{
						this.freeRectangles.RemoveAt(i);
						i--;
						num3--;
					}
				}
				this.PruneFreeList();
				this.usedRectangles.Add(customRectangle);
				this.CaculateEdge(customRectangle);
				result = customRectangle;
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000258C File Offset: 0x0000078C
		public void Insert(List<CustomRectangle> rects, List<CustomRectangle> dst, MaxRectsBinPack.FreeRectChoiceHeuristic method)
		{
			dst.Clear();
			while (rects.Count > 0)
			{
				int num = int.MaxValue;
				int num2 = int.MaxValue;
				int num3 = -1;
				CustomRectangle customRectangle = new CustomRectangle(null);
				for (int i = 0; i < rects.Count; i++)
				{
					int num4 = 0;
					int num5 = 0;
					CustomRectangle customRectangle2 = this.ScoreRect(rects[i].Width, rects[i].Height, method, ref num4, ref num5);
					if (num4 < num || (num4 == num && num5 < num2))
					{
						num = num4;
						num2 = num5;
						customRectangle = customRectangle2;
						customRectangle.UserData = rects[i].UserData;
						customRectangle.Rotated = customRectangle2.Rotated;
						num3 = i;
					}
				}
				if (num3 == -1)
				{
					break;
				}
				this.PlaceRect(customRectangle);
				this.CaculateEdge(customRectangle);
				rects.RemoveAt(num3);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002698 File Offset: 0x00000898
		private void PlaceRect(CustomRectangle node)
		{
			int num = this.freeRectangles.Count;
			for (int i = 0; i < num; i++)
			{
				if (this.SplitFreeNode(this.freeRectangles[i], ref node))
				{
					this.freeRectangles.RemoveAt(i);
					i--;
					num--;
				}
			}
			this.PruneFreeList();
			this.usedRectangles.Add(node);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000270C File Offset: 0x0000090C
		private CustomRectangle ScoreRect(int width, int height, MaxRectsBinPack.FreeRectChoiceHeuristic method, ref int score1, ref int score2)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			score1 = int.MaxValue;
			score2 = int.MaxValue;
			switch (method)
			{
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestShortSideFit:
				customRectangle = this.FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestLongSideFit:
				customRectangle = this.FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestAreaFit:
				customRectangle = this.FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectBottomLeftRule:
				customRectangle = this.FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2);
				break;
			case MaxRectsBinPack.FreeRectChoiceHeuristic.RectContactPointRule:
				customRectangle = this.FindPositionForNewNodeContactPoint(width, height, ref score1);
				score1 = -score1;
				break;
			}
			if (customRectangle.Height == 0)
			{
				score1 = int.MaxValue;
				score2 = int.MaxValue;
			}
			return customRectangle;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027C8 File Offset: 0x000009C8
		public float Occupancy()
		{
			ulong num = 0UL;
			for (int i = 0; i < this.usedRectangles.Count; i++)
			{
				num += (ulong)(this.usedRectangles[i].Width * this.usedRectangles[i].Height);
			}
			return num / (float)(this.binWidth * this.binHeight);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002830 File Offset: 0x00000A30
		private CustomRectangle FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			bestY = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].Width >= width && this.freeRectangles[i].Height >= height)
				{
					int num = this.freeRectangles[i].Y + height;
					if (num < bestY || (num == bestY && this.freeRectangles[i].X < bestX))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = width;
						customRectangle.Height = height;
						bestY = num;
						bestX = this.freeRectangles[i].X;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].Width >= height && this.freeRectangles[i].Height >= width)
				{
					int num = this.freeRectangles[i].Y + width;
					if (num < bestY || (num == bestY && this.freeRectangles[i].X < bestX))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = height;
						customRectangle.Height = width;
						bestY = num;
						bestX = this.freeRectangles[i].X;
					}
				}
			}
			return customRectangle;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A28 File Offset: 0x00000C28
		private CustomRectangle FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			bestShortSideFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].Width >= width && this.freeRectangles[i].Height >= height)
				{
					int val = Math.Abs(this.freeRectangles[i].Width - width);
					int val2 = Math.Abs(this.freeRectangles[i].Height - height);
					int num = Math.Min(val, val2);
					int num2 = Math.Max(val, val2);
					if (num < bestShortSideFit || (num == bestShortSideFit && num2 < bestLongSideFit))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = width;
						customRectangle.Height = height;
						bestShortSideFit = num;
						bestLongSideFit = num2;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].Width >= height && this.freeRectangles[i].Height >= width)
				{
					int val3 = Math.Abs(this.freeRectangles[i].Width - height);
					int val4 = Math.Abs(this.freeRectangles[i].Height - width);
					int num3 = Math.Min(val3, val4);
					int num4 = Math.Max(val3, val4);
					if (num3 < bestShortSideFit || (num3 == bestShortSideFit && num4 < bestLongSideFit))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = height;
						customRectangle.Height = width;
						bestShortSideFit = num3;
						bestLongSideFit = num4;
					}
				}
			}
			return customRectangle;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C54 File Offset: 0x00000E54
		private CustomRectangle FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			bestLongSideFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].Width >= width && this.freeRectangles[i].Height >= height)
				{
					int val = Math.Abs(this.freeRectangles[i].Width - width);
					int val2 = Math.Abs(this.freeRectangles[i].Height - height);
					int num = Math.Min(val, val2);
					int num2 = Math.Max(val, val2);
					if (num2 < bestLongSideFit || (num2 == bestLongSideFit && num < bestShortSideFit))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = width;
						customRectangle.Height = height;
						bestShortSideFit = num;
						bestLongSideFit = num2;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].Width >= height && this.freeRectangles[i].Height >= width)
				{
					int val = Math.Abs(this.freeRectangles[i].Width - height);
					int val2 = Math.Abs(this.freeRectangles[i].Height - width);
					int num = Math.Min(val, val2);
					int num2 = Math.Max(val, val2);
					if (num2 < bestLongSideFit || (num2 == bestLongSideFit && num < bestShortSideFit))
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = height;
						customRectangle.Height = width;
						bestShortSideFit = num;
						bestLongSideFit = num2;
					}
				}
			}
			return customRectangle;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002E7C File Offset: 0x0000107C
		private CustomRectangle FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			bestAreaFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				int num = this.freeRectangles[i].Width * this.freeRectangles[i].Height - width * height;
				if (this.freeRectangles[i].Width >= width && this.freeRectangles[i].Height >= height)
				{
					int val = Math.Abs(this.freeRectangles[i].Width - width);
					int val2 = Math.Abs(this.freeRectangles[i].Height - height);
					int num2 = Math.Min(val, val2);
					if (num < bestAreaFit || (num == bestAreaFit && num2 < bestShortSideFit))
					{
						customRectangle.Rotated = false;
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = width;
						customRectangle.Height = height;
						bestShortSideFit = num2;
						bestAreaFit = num;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].Width >= height && this.freeRectangles[i].Height >= width)
				{
					int val = Math.Abs(this.freeRectangles[i].Width - height);
					int val2 = Math.Abs(this.freeRectangles[i].Height - width);
					int num2 = Math.Min(val, val2);
					if (num < bestAreaFit || (num == bestAreaFit && num2 < bestShortSideFit))
					{
						customRectangle.Rotated = true;
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = height;
						customRectangle.Height = width;
						bestShortSideFit = num2;
						bestAreaFit = num;
					}
				}
			}
			return customRectangle;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000030C4 File Offset: 0x000012C4
		private int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
		{
			int result;
			if (i1end < i2start || i2end < i1start)
			{
				result = 0;
			}
			else
			{
				result = Math.Min(i1end, i2end) - Math.Max(i1start, i2start);
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003100 File Offset: 0x00001300
		private int ContactPointScoreNode(int x, int y, int width, int height)
		{
			int num = 0;
			if (x == 0 || x + width == this.binWidth)
			{
				num += height;
			}
			if (y == 0 || y + height == this.binHeight)
			{
				num += width;
			}
			for (int i = 0; i < this.usedRectangles.Count; i++)
			{
				if (this.usedRectangles[i].X == x + width || this.usedRectangles[i].X + this.usedRectangles[i].Width == x)
				{
					num += this.CommonIntervalLength(this.usedRectangles[i].Y, this.usedRectangles[i].Y + this.usedRectangles[i].Height, y, y + height);
				}
				if (this.usedRectangles[i].Y == y + height || this.usedRectangles[i].Y + this.usedRectangles[i].Height == y)
				{
					num += this.CommonIntervalLength(this.usedRectangles[i].X, this.usedRectangles[i].X + this.usedRectangles[i].Width, x, x + width);
				}
			}
			return num;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003288 File Offset: 0x00001488
		private CustomRectangle FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
		{
			CustomRectangle customRectangle = new CustomRectangle(null);
			bestContactScore = -1;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].Width >= width && this.freeRectangles[i].Height >= height)
				{
					int num = this.ContactPointScoreNode(this.freeRectangles[i].X, this.freeRectangles[i].Y, width, height);
					if (num > bestContactScore)
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = width;
						customRectangle.Height = height;
						bestContactScore = num;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].Width >= height && this.freeRectangles[i].Height >= width)
				{
					int num = this.ContactPointScoreNode(this.freeRectangles[i].X, this.freeRectangles[i].Y, height, width);
					if (num > bestContactScore)
					{
						customRectangle.X = this.freeRectangles[i].X;
						customRectangle.Y = this.freeRectangles[i].Y;
						customRectangle.Width = height;
						customRectangle.Height = width;
						bestContactScore = num;
					}
				}
			}
			return customRectangle;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000343C File Offset: 0x0000163C
		private bool SplitFreeNode(CustomRectangle freeNode, ref CustomRectangle usedNode)
		{
			bool result;
			if (usedNode.X >= freeNode.X + freeNode.Width || usedNode.X + usedNode.Width <= freeNode.X || usedNode.Y >= freeNode.Y + freeNode.Height || usedNode.Y + usedNode.Height <= freeNode.Y)
			{
				result = false;
			}
			else
			{
				if (usedNode.X < freeNode.X + freeNode.Width && usedNode.X + usedNode.Width > freeNode.X)
				{
					if (usedNode.Y > freeNode.Y && usedNode.Y < freeNode.Y + freeNode.Height)
					{
						CustomRectangle customRectangle = freeNode.Copy();
						customRectangle.Height = usedNode.Y - customRectangle.Y;
						this.freeRectangles.Add(customRectangle);
					}
					if (usedNode.Y + usedNode.Height < freeNode.Y + freeNode.Height)
					{
						CustomRectangle customRectangle = freeNode.Copy();
						customRectangle.Y = usedNode.Y + usedNode.Height;
						customRectangle.Height = freeNode.Y + freeNode.Height - (usedNode.Y + usedNode.Height);
						this.freeRectangles.Add(customRectangle);
					}
				}
				if (usedNode.Y < freeNode.Y + freeNode.Height && usedNode.Y + usedNode.Height > freeNode.Y)
				{
					if (usedNode.X > freeNode.X && usedNode.X < freeNode.X + freeNode.Width)
					{
						CustomRectangle customRectangle = freeNode.Copy();
						customRectangle.Width = usedNode.X - customRectangle.X;
						this.freeRectangles.Add(customRectangle);
					}
					if (usedNode.X + usedNode.Width < freeNode.X + freeNode.Width)
					{
						CustomRectangle customRectangle = freeNode.Copy();
						customRectangle.X = usedNode.X + usedNode.Width;
						customRectangle.Width = freeNode.X + freeNode.Width - (usedNode.X + usedNode.Width);
						this.freeRectangles.Add(customRectangle);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000036D4 File Offset: 0x000018D4
		private void PruneFreeList()
		{
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				for (int j = i + 1; j < this.freeRectangles.Count; j++)
				{
					if (this.IsContainedIn(this.freeRectangles[i], this.freeRectangles[j]))
					{
						this.freeRectangles.RemoveAt(i);
						i--;
						break;
					}
					if (this.IsContainedIn(this.freeRectangles[j], this.freeRectangles[i]))
					{
						this.freeRectangles.RemoveAt(j);
						j--;
					}
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003794 File Offset: 0x00001994
		private bool IsContainedIn(CustomRectangle a, CustomRectangle b)
		{
			return a.X >= b.X && a.Y >= b.Y && a.X + a.Width <= b.X + b.Width && a.Y + a.Height <= b.Y + b.Height;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003804 File Offset: 0x00001A04
		private void CaculateEdge(CustomRectangle latestnode)
		{
			if (latestnode.Right > this.RightEdge)
			{
				this.RightEdge = latestnode.Right;
			}
			if (latestnode.Bottom > this.BottomEdge)
			{
				this.BottomEdge = latestnode.Bottom;
			}
		}

		// Token: 0x04000005 RID: 5
		public int binWidth = 0;

		// Token: 0x04000006 RID: 6
		public int binHeight = 0;

		// Token: 0x04000007 RID: 7
		public bool allowRotations;

		// Token: 0x04000008 RID: 8
		public List<CustomRectangle> usedRectangles = new List<CustomRectangle>();

		// Token: 0x04000009 RID: 9
		public List<CustomRectangle> freeRectangles = new List<CustomRectangle>();

		// Token: 0x02000006 RID: 6
		public enum FreeRectChoiceHeuristic
		{
			// Token: 0x0400000D RID: 13
			RectBestShortSideFit,
			// Token: 0x0400000E RID: 14
			RectBestLongSideFit,
			// Token: 0x0400000F RID: 15
			RectBestAreaFit,
			// Token: 0x04000010 RID: 16
			RectBottomLeftRule,
			// Token: 0x04000011 RID: 17
			RectContactPointRule
		}
	}
}
