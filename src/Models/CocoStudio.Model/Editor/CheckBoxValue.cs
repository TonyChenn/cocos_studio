using System;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000090 RID: 144
	public class CheckBoxValue
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x000158B4 File Offset: 0x00013AB4
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x000158CC File Offset: 0x00013ACC
		public bool IsChecked
		{
			get
			{
				return this.isChecked;
			}
			set
			{
				if (this.isChecked != value)
				{
					this.isChecked = value;
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x000158F4 File Offset: 0x00013AF4
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0001590C File Offset: 0x00013B0C
		public bool CanEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				if (this.isEnabled != value)
				{
					this.isEnabled = value;
				}
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00015933 File Offset: 0x00013B33
		public CheckBoxValue(bool isChecked, bool isEnabled)
		{
			this.IsChecked = isChecked;
			this.CanEnabled = isEnabled;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00015950 File Offset: 0x00013B50
		public bool Equals(CheckBoxValue others)
		{
			return !(others == null) && (this.IsChecked == others.IsChecked && this.CanEnabled == others.CanEnabled);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000159A0 File Offset: 0x00013BA0
		public override bool Equals(object obj)
		{
			return obj is CheckBoxValue && this.Equals((CheckBoxValue)obj);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000159D4 File Offset: 0x00013BD4
		public override int GetHashCode()
		{
			int hashCode = this.IsChecked.GetHashCode();
			return hashCode ^ this.CanEnabled.GetHashCode();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00015A08 File Offset: 0x00013C08
		public static bool operator ==(CheckBoxValue leftValue, CheckBoxValue rightValue)
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

		// Token: 0x060004F9 RID: 1273 RVA: 0x00015A3C File Offset: 0x00013C3C
		public static bool operator !=(CheckBoxValue leftValue, CheckBoxValue rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x04000249 RID: 585
		private bool isChecked;

		// Token: 0x0400024A RID: 586
		private bool isEnabled;
	}
}
