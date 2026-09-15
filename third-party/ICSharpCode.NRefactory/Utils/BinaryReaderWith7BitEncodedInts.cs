using System;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A binary reader that can read the output of BinaryWriterWith7BitEncodedInts.
	/// </summary>
	public sealed class BinaryReaderWith7BitEncodedInts : BinaryReader
	{
		public BinaryReaderWith7BitEncodedInts(Stream stream) : base(stream)
		{
		}

		public override short ReadInt16()
		{
			return (short)((ushort)base.Read7BitEncodedInt());
		}

		[CLSCompliant(false)]
		public override ushort ReadUInt16()
		{
			return (ushort)base.Read7BitEncodedInt();
		}

		public override int ReadInt32()
		{
			return base.Read7BitEncodedInt();
		}

		[CLSCompliant(false)]
		public override uint ReadUInt32()
		{
			return (uint)base.Read7BitEncodedInt();
		}

		public override long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		[CLSCompliant(false)]
		public override ulong ReadUInt64()
		{
			ulong num = 0UL;
			int i = 0;
			while (i < 64)
			{
				byte b = this.ReadByte();
				num |= (ulong)((ulong)((long)(b & 127)) << i);
				i += 7;
				if ((b & 128) == 0)
				{
					return num;
				}
			}
			throw new FormatException("Invalid 7-bit int64");
		}
	}
}
