using System;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000091 RID: 145
	public class FilpValue
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00015A58 File Offset: 0x00013C58
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00015A70 File Offset: 0x00013C70
		public bool FlipX
		{
			get
			{
				return this.filpX;
			}
			set
			{
				this.filpX = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00015A7C File Offset: 0x00013C7C
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00015A94 File Offset: 0x00013C94
		public bool FlipY
		{
			get
			{
				return this.filpY;
			}
			set
			{
				this.filpY = value;
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00015A9E File Offset: 0x00013C9E
		public FilpValue(bool filpX, bool filpY)
		{
			this.FlipX = filpX;
			this.FlipY = filpY;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00015ABC File Offset: 0x00013CBC
		public bool Equals(FilpValue others)
		{
			return !(others == null) && (this.FlipX == others.FlipX && this.FlipY == others.FlipY);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00015B0C File Offset: 0x00013D0C
		public override bool Equals(object obj)
		{
			return obj is FilpValue && this.Equals((FilpValue)obj);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00015B40 File Offset: 0x00013D40
		public override int GetHashCode()
		{
			int hashCode = this.FlipX.GetHashCode();
			return hashCode ^ this.FlipY.GetHashCode();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00015B74 File Offset: 0x00013D74
		public static bool operator ==(FilpValue leftValue, FilpValue rightValue)
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

		// Token: 0x06000503 RID: 1283 RVA: 0x00015BA8 File Offset: 0x00013DA8
		public static bool operator !=(FilpValue leftValue, FilpValue rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x0400024B RID: 587
		private bool filpX;

		// Token: 0x0400024C RID: 588
		private bool filpY;
	}
}
