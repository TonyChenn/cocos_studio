using System;
using System.Globalization;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Holds 16 boolean values.
	/// </summary>
	// Token: 0x02000106 RID: 262
	[CLSCompliant(false)]
	[Serializable]
	public struct BitVector16 : IEquatable<BitVector16>
	{
		// Token: 0x170003D7 RID: 983
		public bool this[ushort mask]
		{
			get
			{
				return (this.data & mask) != 0;
			}
			set
			{
				if (value)
				{
					this.data |= mask;
					return;
				}
				this.data = (ushort)(this.data & (ushort)(~mask));
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x00019824 File Offset: 0x00018824
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0001982C File Offset: 0x0001882C
		public ushort Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00019835 File Offset: 0x00018835
		public override bool Equals(object obj)
		{
			return obj is BitVector16 && this.Equals((BitVector16)obj);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001984D File Offset: 0x0001884D
		public bool Equals(BitVector16 other)
		{
			return this.data == other.data;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001985E File Offset: 0x0001885E
		public override int GetHashCode()
		{
			return (int)this.data;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00019866 File Offset: 0x00018866
		public static bool operator ==(BitVector16 left, BitVector16 right)
		{
			return left.data == right.data;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00019878 File Offset: 0x00018878
		public static bool operator !=(BitVector16 left, BitVector16 right)
		{
			return left.data != right.data;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001988D File Offset: 0x0001888D
		public override string ToString()
		{
			return this.data.ToString("x4", CultureInfo.InvariantCulture);
		}

		// Token: 0x04000312 RID: 786
		private ushort data;
	}
}
