using System;
using System.Globalization;
using System.Net;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListInteger : PListElement<long>
	{
		public override string Tag
		{
			get
			{
				return "integer";
			}
		}

		public override byte TypeCode
		{
			get
			{
				return 1;
			}
		}

		public override long Value { get; set; }

		public PListInteger()
		{
		}

		public PListInteger(long value)
		{
			this.Value = value;
		}

		protected override void Parse(string value)
		{
			this.Value = long.Parse(value, CultureInfo.InvariantCulture);
		}

		protected override string ToXmlString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void ReadBinary(PListBinaryReader reader)
		{
			byte[] array = new byte[1 << reader.CurrentElementLength];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			switch (reader.CurrentElementLength)
			{
			case 0:
				this.Value = (long)((ulong)array[0]);
				break;
			case 1:
				this.Value = (long)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(array, 0));
				break;
			case 2:
				this.Value = (long)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 0));
				break;
			case 3:
				this.Value = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(array, 0));
				break;
			default:
				throw new PListFormatException("Int > 64Bit");
			}
		}

		public override int GetPListElementLength()
		{
			int result;
			if (this.Value >= 0L && this.Value <= 255L)
			{
				result = 0;
			}
			else if (this.Value >= -32768L && this.Value <= 32767L)
			{
				result = 1;
			}
			else if (this.Value >= -2147483648L && this.Value <= 2147483647L)
			{
				result = 2;
			}
			else if (this.Value >= -9223372036854775808L && this.Value <= 9223372036854775807L)
			{
				result = 3;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public override void WriteBinary(PListBinaryWriter writer)
		{
			int plistElementLength = this.GetPListElementLength();
			byte[] array = null;
			switch (plistElementLength)
			{
			case 0:
				array = new byte[]
				{
					(byte)this.Value
				};
				break;
			case 1:
				array = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Value));
				break;
			case 2:
				array = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)this.Value));
				break;
			case 3:
				array = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Value));
				break;
			}
			writer.BaseStream.Write(array, 0, array.Length);
		}
	}
}
