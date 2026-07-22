using System;
using System.IO;
using System.Net;

namespace Modules.Communal.PList.Internal
{
	// Token: 0x02000003 RID: 3
	public class PListBinaryReader
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002067 File Offset: 0x00000267
		internal Stream BaseStream { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002070 File Offset: 0x00000270
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002087 File Offset: 0x00000287
		internal byte ElementIdxSize { get; private set; }

		// Token: 0x0600000C RID: 12 RVA: 0x00002090 File Offset: 0x00000290
		internal PListBinaryReader()
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000209C File Offset: 0x0000029C
		public IPListElement Read(Stream stream)
		{
			this.BaseStream = stream;
			byte[] array = new byte[32];
			this.BaseStream.Seek(-32L, SeekOrigin.End);
			if (this.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException("Invalid Header Size");
			}
			byte b = array[6];
			this.ElementIdxSize = array[7];
			int num = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 12));
			int elemIdx = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 20));
			int num2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 28));
			byte[] array2 = new byte[num * (int)b];
			this.BaseStream.Seek((long)num2, SeekOrigin.Begin);
			if (this.BaseStream.Read(array2, 0, array2.Length) != array2.Length)
			{
				throw new PListFormatException("Invalid offsetTable Size");
			}
			this.m_Offsets = new int[num];
			for (int i = 0; i < this.m_Offsets.Length; i++)
			{
				byte[] array3 = new byte[4];
				for (int j = 0; j < (int)b; j++)
				{
					array3[(int)(b - 1) - j] = array2[i * (int)b + j];
				}
				this.m_Offsets[i] = BitConverter.ToInt32(array3, 0);
			}
			return this.ReadInternal(elemIdx);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021E8 File Offset: 0x000003E8
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000021FF File Offset: 0x000003FF
		internal byte CurrentElementTypeCode { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002208 File Offset: 0x00000408
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000221F File Offset: 0x0000041F
		internal int CurrentElementLength { get; private set; }

		// Token: 0x06000012 RID: 18 RVA: 0x00002228 File Offset: 0x00000428
		internal IPListElement ReadInternal(int elemIdx)
		{
			this.BaseStream.Seek((long)this.m_Offsets[elemIdx], SeekOrigin.Begin);
			return this.ReadInternal();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002258 File Offset: 0x00000458
		internal IPListElement ReadInternal()
		{
			byte[] array = new byte[1];
			if (this.BaseStream.Read(array, 0, array.Length) != 1)
			{
				throw new PListFormatException("Didn't read type Byte");
			}
			int num = (int)(array[0] & 15);
			byte b = (byte)(array[0] >> 4 & 15);
			if (b != 0 && num == 15)
			{
				IPListElement iplistElement = this.ReadInternal();
				if (!(iplistElement is PListInteger))
				{
					throw new PListFormatException("Element Len is no Integer");
				}
				num = (int)((PListInteger)iplistElement).Value;
			}
			IPListElement iplistElement2 = PListElementFactory.Instance.Create(b, num);
			byte currentElementTypeCode = this.CurrentElementTypeCode;
			int currentElementLength = this.CurrentElementLength;
			this.CurrentElementTypeCode = b;
			this.CurrentElementLength = num;
			iplistElement2.ReadBinary(this);
			this.CurrentElementTypeCode = currentElementTypeCode;
			this.CurrentElementLength = currentElementLength;
			return iplistElement2;
		}

		// Token: 0x04000001 RID: 1
		private int[] m_Offsets;
	}
}
