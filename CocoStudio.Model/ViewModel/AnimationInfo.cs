using System;
using System.Drawing;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000CA RID: 202
	public class AnimationInfo : ICloneable
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x00019CB3 File Offset: 0x00017EB3
		public AnimationInfo()
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00019CC9 File Offset: 0x00017EC9
		public AnimationInfo(string name, int start, int end)
		{
			this.name = name;
			this.startIndex = start;
			this.endIndex = end;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00019CF4 File Offset: 0x00017EF4
		public AnimationInfo(AnimationInfo other)
		{
			this.name = other.Name;
			this.startIndex = other.StartIndex;
			this.endIndex = other.EndIndex;
			this.rendercolor = other.RenderColor;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00019D48 File Offset: 0x00017F48
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00019D60 File Offset: 0x00017F60
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				if (this.startIndex != value)
				{
					this.startIndex = value;
					this.NotifyChanged();
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00019D8C File Offset: 0x00017F8C
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00019DA4 File Offset: 0x00017FA4
		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
			set
			{
				if (this.endIndex != value)
				{
					this.endIndex = value;
					this.NotifyChanged();
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00019DD0 File Offset: 0x00017FD0
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00019DE8 File Offset: 0x00017FE8
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.NotifyChanged();
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00019E1C File Offset: 0x0001801C
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00019E54 File Offset: 0x00018054
		public Color RenderColor
		{
			get
			{
				if (this.rendercolor.IsEmpty)
				{
					this.rendercolor = AnimationInfo.GetRandomColor();
				}
				return this.rendercolor;
			}
			set
			{
				if (!this.rendercolor.Equals(value))
				{
					this.rendercolor = value;
					this.NotifyChanged();
				}
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00019E90 File Offset: 0x00018090
		public static Color GetRandomColor()
		{
			int color = AnimationInfo.rand.Next(28, 167);
			Color baseColor = Color.FromKnownColor((KnownColor)color);
			return Color.FromArgb(150, baseColor);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000666 RID: 1638 RVA: 0x00019EC8 File Offset: 0x000180C8
		// (remove) Token: 0x06000667 RID: 1639 RVA: 0x00019F04 File Offset: 0x00018104
		public event AnimationInfoChangedHandler AnimationInfoChanged;

		// Token: 0x06000668 RID: 1640 RVA: 0x00019F40 File Offset: 0x00018140
		private void NotifyChanged()
		{
			if (this.AnimationInfoChanged != null)
			{
				this.AnimationInfoChanged();
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00019F6C File Offset: 0x0001816C
		public object Clone()
		{
			return new AnimationInfo
			{
				name = this.Name,
				startIndex = this.StartIndex,
				endIndex = this.endIndex
			};
		}

		// Token: 0x040002C1 RID: 705
		public const string InfoAllName = "-- ALL --";

		// Token: 0x040002C2 RID: 706
		private int startIndex;

		// Token: 0x040002C3 RID: 707
		private int endIndex;

		// Token: 0x040002C4 RID: 708
		private string name;

		// Token: 0x040002C5 RID: 709
		private Color rendercolor = Color.Empty;

		// Token: 0x040002C6 RID: 710
		private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
	}
}
