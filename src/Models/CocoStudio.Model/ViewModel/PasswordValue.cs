using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000118 RID: 280
	public class PasswordValue
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0002A784 File Offset: 0x00028984
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0002A79C File Offset: 0x0002899C
		public bool PasswordEnable
		{
			get
			{
				return this.passwordEnable;
			}
			set
			{
				if (this.passwordEnable != value)
				{
					this.passwordEnable = value;
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0002A7C4 File Offset: 0x000289C4
		// (set) Token: 0x06000AAA RID: 2730 RVA: 0x0002A7DC File Offset: 0x000289DC
		public string PasswordStyleText
		{
			get
			{
				return this.passwordStyleText;
			}
			set
			{
				if (!(this.passwordStyleText == value))
				{
					this.passwordStyleText = value;
				}
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002A806 File Offset: 0x00028A06
		public PasswordValue(bool passwordEnable, string passwordStyleText)
		{
			this.PasswordEnable = passwordEnable;
			this.PasswordStyleText = passwordStyleText;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002A824 File Offset: 0x00028A24
		public bool Equals(PasswordValue others)
		{
			return !(others == null) && (this.PasswordEnable == others.PasswordEnable && this.PasswordStyleText == others.PasswordStyleText);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0002A878 File Offset: 0x00028A78
		public override bool Equals(object obj)
		{
			return obj is PasswordValue && this.Equals((PasswordValue)obj);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002A8AC File Offset: 0x00028AAC
		public override int GetHashCode()
		{
			int hashCode = this.PasswordEnable.GetHashCode();
			return hashCode ^ this.PasswordStyleText.GetHashCode();
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002A8DC File Offset: 0x00028ADC
		public static bool operator ==(PasswordValue leftValue, PasswordValue rightValue)
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

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002A910 File Offset: 0x00028B10
		public static bool operator !=(PasswordValue leftValue, PasswordValue rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x04000470 RID: 1136
		private bool passwordEnable;

		// Token: 0x04000471 RID: 1137
		private string passwordStyleText;
	}
}
