using System;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A binary writer that encodes all integers as 7-bit-encoded-ints.
	/// </summary>
	public sealed class BinaryWriterWith7BitEncodedInts : BinaryWriter
	{
		public BinaryWriterWith7BitEncodedInts(Stream stream) : base(stream)
		{
		}

		public override void Write(short value)
		{
			base.Write7BitEncodedInt((int)((ushort)value));
		}

		[CLSCompliant(false)]
		public override void Write(ushort value)
		{
			base.Write7BitEncodedInt((int)value);
		}

		public override void Write(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		[CLSCompliant(false)]
		public override void Write(uint value)
		{
			base.Write7BitEncodedInt((int)value);
		}

		public override void Write(long value)
		{
			this.Write((ulong)value);
		}

		[CLSCompliant(false)]
		public override void Write(ulong value)
		{
			while (value >= 128UL)
			{
				this.Write((byte)(value | 128UL));
				value >>= 7;
			}
			this.Write((byte)value);
		}
	}
}
