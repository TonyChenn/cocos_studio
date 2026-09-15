using System;
using System.Globalization;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Holds 16 boolean values.
	/// </summary>
	[CLSCompliant(false)]
	[Serializable]
	public struct BitVector16 : IEquatable<BitVector16>
	{
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

		public override bool Equals(object obj)
		{
			return obj is BitVector16 && this.Equals((BitVector16)obj);
		}

		public bool Equals(BitVector16 other)
		{
			return this.data == other.data;
		}

		public override int GetHashCode()
		{
			return (int)this.data;
		}

		public static bool operator ==(BitVector16 left, BitVector16 right)
		{
			return left.data == right.data;
		}

		public static bool operator !=(BitVector16 left, BitVector16 right)
		{
			return left.data != right.data;
		}

		public override string ToString()
		{
			return this.data.ToString("x4", CultureInfo.InvariantCulture);
		}

		private ushort data;
	}
}
