using System;
using System.Globalization;
using System.Net;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000010 RID: 16
	public class PListInteger : PListElement<long>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000038D4 File Offset: 0x00001AD4
		public override string Tag
		{
			get
			{
				return "integer";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000038EC File Offset: 0x00001AEC
		public override byte TypeCode
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003900 File Offset: 0x00001B00
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003917 File Offset: 0x00001B17
		public override long Value { get; set; }

		// Token: 0x0600008C RID: 140 RVA: 0x00003920 File Offset: 0x00001B20
		public PListInteger()
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000392B File Offset: 0x00001B2B
		public PListInteger(long value)
		{
			this.Value = value;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000393E File Offset: 0x00001B3E
		protected override void Parse(string value)
		{
			this.Value = long.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003954 File Offset: 0x00001B54
		protected override string ToXmlString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000397C File Offset: 0x00001B7C
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

		// Token: 0x06000091 RID: 145 RVA: 0x00003A34 File Offset: 0x00001C34
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

		// Token: 0x06000092 RID: 146 RVA: 0x00003AEC File Offset: 0x00001CEC
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
