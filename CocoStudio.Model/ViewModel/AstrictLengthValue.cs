using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000117 RID: 279
	public class AstrictLengthValue
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0002A5E0 File Offset: 0x000287E0
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x0002A5F8 File Offset: 0x000287F8
		public bool MaxLengthEnable
		{
			get
			{
				return this.maxLengthEnable;
			}
			set
			{
				if (this.maxLengthEnable != value)
				{
					this.maxLengthEnable = value;
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0002A620 File Offset: 0x00028820
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0002A638 File Offset: 0x00028838
		public int MaxLengthText
		{
			get
			{
				return this.maxLengthText;
			}
			set
			{
				if (this.maxLengthText != value)
				{
					this.maxLengthText = value;
				}
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002A65F File Offset: 0x0002885F
		public AstrictLengthValue(bool maxLengthEnable, int maxLengthText)
		{
			this.MaxLengthEnable = maxLengthEnable;
			this.MaxLengthText = maxLengthText;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002A67C File Offset: 0x0002887C
		public bool Equals(AstrictLengthValue others)
		{
			return !(others == null) && (this.MaxLengthEnable == others.MaxLengthEnable && this.MaxLengthText == others.MaxLengthText);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002A6CC File Offset: 0x000288CC
		public override bool Equals(object obj)
		{
			return obj is AstrictLengthValue && this.Equals((AstrictLengthValue)obj);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002A700 File Offset: 0x00028900
		public override int GetHashCode()
		{
			int hashCode = this.MaxLengthEnable.GetHashCode();
			return hashCode ^ this.MaxLengthText.GetHashCode();
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002A734 File Offset: 0x00028934
		public static bool operator ==(AstrictLengthValue leftValue, AstrictLengthValue rightValue)
		{
			bool result;
			if (object.ReferenceEquals(leftValue, null))
			{
				result = object.ReferenceEquals(rightValue, null);
			}
			else
			{
				result = leftValue.Equals(rightValue);
			}
			return result;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002A768 File Offset: 0x00028968
		public static bool operator !=(AstrictLengthValue leftValue, AstrictLengthValue rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x0400046E RID: 1134
		private bool maxLengthEnable;

		// Token: 0x0400046F RID: 1135
		private int maxLengthText;
	}
}
