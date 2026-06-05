using System;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000136 RID: 310
	public class FormatItem : FormatStringSegmentBase
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002045E File Offset: 0x0001F45E
		public FormatItem(int index, int? alignment = null, string formatString = null)
		{
			this.Index = index;
			this.Alignment = alignment;
			this.FormatString = formatString;
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0002047B File Offset: 0x0001F47B
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00020483 File Offset: 0x0001F483
		public int Index { get; private set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0002048C File Offset: 0x0001F48C
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x00020494 File Offset: 0x0001F494
		public int? Alignment { get; private set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0002049D File Offset: 0x0001F49D
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x000204A5 File Offset: 0x0001F4A5
		public string FormatString { get; private set; }

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000204B0 File Offset: 0x0001F4B0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(FormatItem))
			{
				return false;
			}
			FormatItem other = (FormatItem)obj;
			return this.FieldsEquals(other);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x000204E9 File Offset: 0x0001F4E9
		public bool Equals(FormatItem other)
		{
			return other != null && this.FieldsEquals(other);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x000204F8 File Offset: 0x0001F4F8
		private bool FieldsEquals(FormatItem other)
		{
			return this.Index == other.Index && this.Alignment == other.Alignment && this.FormatString == other.FormatString && base.StartLocation == other.StartLocation && base.EndLocation == other.EndLocation;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00020578 File Offset: 0x0001F578
		public override int GetHashCode()
		{
			int num = 23;
			num = num * 37 + this.Index.GetHashCode();
			num = num * 37 + this.Alignment.GetHashCode();
			num = num * 37 + this.FormatString.GetHashCode();
			num = num * 37 + base.StartLocation.GetHashCode();
			return num * 37 + base.EndLocation.GetHashCode();
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000205F4 File Offset: 0x0001F5F4
		public override string ToString()
		{
			return string.Format("[FormatItem: Index={0}, Alignment={1}, FormatString={2}, StartLocation={3}, EndLocation={4}]", new object[]
			{
				this.Index,
				this.Alignment,
				this.FormatString,
				base.StartLocation,
				base.EndLocation
			});
		}
	}
}
