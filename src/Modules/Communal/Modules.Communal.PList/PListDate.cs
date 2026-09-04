using System;
using System.Globalization;
using System.Linq;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x0200000E RID: 14
	public class PListDate : PListElement<DateTime>
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003610 File Offset: 0x00001810
		public override string Tag
		{
			get
			{
				return "date";
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003628 File Offset: 0x00001828
		public override byte TypeCode
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000363C File Offset: 0x0000183C
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003653 File Offset: 0x00001853
		public override DateTime Value { get; set; }

		// Token: 0x06000076 RID: 118 RVA: 0x0000365C File Offset: 0x0000185C
		public PListDate()
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003667 File Offset: 0x00001867
		public PListDate(DateTime value)
		{
			this.Value = value;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000367A File Offset: 0x0000187A
		protected override void Parse(string value)
		{
			this.Value = DateTime.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003690 File Offset: 0x00001890
		protected override string ToXmlString()
		{
			return this.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffffZ");
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000036C0 File Offset: 0x000018C0
		public override void ReadBinary(PListBinaryReader reader)
		{
			byte[] array = new byte[1 << reader.CurrentElementLength];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			double value;
			switch (reader.CurrentElementLength)
			{
			case 0:
				throw new PListFormatException("Date < 32Bit");
			case 1:
				throw new PListFormatException("Date < 32Bit");
			case 2:
				value = (double)BitConverter.ToSingle(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			case 3:
				value = BitConverter.ToDouble(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			default:
				throw new PListFormatException("Date > 64Bit");
			}
			this.Value = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003794 File Offset: 0x00001994
		public override int GetPListElementLength()
		{
			return 3;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000037A8 File Offset: 0x000019A8
		public override void WriteBinary(PListBinaryWriter writer)
		{
			DateTime d = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			byte[] array = BitConverter.GetBytes((this.Value - d).TotalSeconds).Reverse<byte>().ToArray<byte>();
			writer.BaseStream.Write(array, 0, array.Length);
		}
	}
}
