using System;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A binary reader that can read the output of BinaryWriterWith7BitEncodedInts.
	/// </summary>
	// Token: 0x02000104 RID: 260
	public sealed class BinaryReaderWith7BitEncodedInts : BinaryReader
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x00019712 File Offset: 0x00018712
		public BinaryReaderWith7BitEncodedInts(Stream stream) : base(stream)
		{
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001971B File Offset: 0x0001871B
		public override short ReadInt16()
		{
			return (short)((ushort)base.Read7BitEncodedInt());
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00019725 File Offset: 0x00018725
		[CLSCompliant(false)]
		public override ushort ReadUInt16()
		{
			return (ushort)base.Read7BitEncodedInt();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001972E File Offset: 0x0001872E
		public override int ReadInt32()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00019736 File Offset: 0x00018736
		[CLSCompliant(false)]
		public override uint ReadUInt32()
		{
			return (uint)base.Read7BitEncodedInt();
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001973E File Offset: 0x0001873E
		public override long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00019748 File Offset: 0x00018748
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
