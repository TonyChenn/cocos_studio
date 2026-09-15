using System;
using System.IO;
using System.Net;

namespace Modules.Communal.PList.Internal
{
	public class PListBinaryReader
	{
		internal Stream BaseStream { get; private set; }

		internal byte ElementIdxSize { get; private set; }

		internal PListBinaryReader()
		{
		}

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

		internal byte CurrentElementTypeCode { get; private set; }

		internal int CurrentElementLength { get; private set; }

		internal IPListElement ReadInternal(int elemIdx)
		{
			this.BaseStream.Seek((long)this.m_Offsets[elemIdx], SeekOrigin.Begin);
			return this.ReadInternal();
		}

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

		private int[] m_Offsets;
	}
}
