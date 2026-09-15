using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Modules.Communal.PList.Internal
{
	public class PListBinaryWriter
	{
		internal Stream BaseStream { get; private set; }

		internal byte ElementIdxSize { get; private set; }

		internal List<int> Offsets { get; private set; }

		internal PListBinaryWriter()
		{
		}

		public void Write(Stream stream, IPListElement element)
		{
			this.BaseStream = stream;
			this.Offsets = new List<int>();
			this.BaseStream.Write(PListBinaryWriter.s_PListHeader, 0, PListBinaryWriter.s_PListHeader.Length);
			int num = element.GetPListElementCount();
			if (num <= 255)
			{
				this.ElementIdxSize = 1;
			}
			else if (num <= 32767)
			{
				this.ElementIdxSize = 2;
			}
			else
			{
				this.ElementIdxSize = 4;
			}
			int host = this.WriteInternal(element);
			num = this.Offsets.Count;
			int num2 = (int)this.BaseStream.Position;
			byte b;
			if (num2 <= 255)
			{
				b = 1;
			}
			else if (num2 <= 32767)
			{
				b = 2;
			}
			else
			{
				b = 4;
			}
			for (int i = 0; i < this.Offsets.Count; i++)
			{
				byte[] array = null;
				switch (b)
				{
				case 1:
					array = new byte[]
					{
						(byte)this.Offsets[i]
					};
					break;
				case 2:
					array = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Offsets[i]));
					break;
				case 4:
					array = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Offsets[i]));
					break;
				}
				this.BaseStream.Write(array, 0, array.Length);
			}
			byte[] array2 = new byte[32];
			array2[6] = b;
			array2[7] = this.ElementIdxSize;
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num)).CopyTo(array2, 12);
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(host)).CopyTo(array2, 20);
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num2)).CopyTo(array2, 28);
			this.BaseStream.Write(array2, 0, array2.Length);
		}

		internal byte[] FormatIdx(int idx)
		{
			switch (this.ElementIdxSize)
			{
			case 1:
				return new byte[]
				{
					(byte)idx
				};
			case 2:
				return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)idx));
			case 4:
				return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(idx));
			}
			throw new PListFormatException("Invalid ElementIdxSize");
		}

		internal int WriteInternal(IPListElement element)
		{
			int num = this.Offsets.Count;
			if (element.IsBinaryUnique && element is IEquatable<IPListElement>)
			{
				if (!this.m_UniqueElements.ContainsKey(element.TypeCode))
				{
					this.m_UniqueElements.Add(element.TypeCode, new Dictionary<IPListElement, int>());
				}
				if (!this.m_UniqueElements[element.TypeCode].ContainsKey(element))
				{
					this.m_UniqueElements[element.TypeCode][element] = num;
				}
				else
				{
					if (!(element is PListBool))
					{
						return this.m_UniqueElements[element.TypeCode][element];
					}
					num = this.m_UniqueElements[element.TypeCode][element];
				}
			}
			int item = (int)this.BaseStream.Position;
			this.Offsets.Add(item);
			int plistElementLength = element.GetPListElementLength();
			byte value = (byte)((int)element.TypeCode << 4 | ((plistElementLength < 15) ? plistElementLength : 15));
			this.BaseStream.WriteByte(value);
			if (plistElementLength >= 15)
			{
				IPListElement iplistElement = PListElementFactory.Instance.CreateLengthElement(plistElementLength);
				byte value2 = (byte)((int)iplistElement.TypeCode << 4 | iplistElement.GetPListElementLength());
				this.BaseStream.WriteByte(value2);
				iplistElement.WriteBinary(this);
			}
			element.WriteBinary(this);
			return num;
		}

		private static readonly byte[] s_PListHeader = new byte[]
		{
			98,
			112,
			108,
			105,
			115,
			116,
			48,
			48
		};

		private Dictionary<byte, Dictionary<IPListElement, int>> m_UniqueElements = new Dictionary<byte, Dictionary<IPListElement, int>>();
	}
}
