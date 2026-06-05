using System;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A binary writer that encodes all integers as 7-bit-encoded-ints.
	/// </summary>
	// Token: 0x02000105 RID: 261
	public sealed class BinaryWriterWith7BitEncodedInts : BinaryWriter
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x0001978E File Offset: 0x0001878E
		public BinaryWriterWith7BitEncodedInts(Stream stream) : base(stream)
		{
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00019797 File Offset: 0x00018797
		public override void Write(short value)
		{
			base.Write7BitEncodedInt((int)((ushort)value));
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000197A1 File Offset: 0x000187A1
		[CLSCompliant(false)]
		public override void Write(ushort value)
		{
			base.Write7BitEncodedInt((int)value);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000197AA File Offset: 0x000187AA
		public override void Write(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000197B3 File Offset: 0x000187B3
		[CLSCompliant(false)]
		public override void Write(uint value)
		{
			base.Write7BitEncodedInt((int)value);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x000197BC File Offset: 0x000187BC
		public override void Write(long value)
		{
			this.Write((ulong)value);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000197C5 File Offset: 0x000187C5
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
