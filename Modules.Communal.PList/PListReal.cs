using System;
using System.Globalization;
using System.Linq;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000012 RID: 18
	public class PListReal : PListElement<double>
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003C54 File Offset: 0x00001E54
		public override string Tag
		{
			get
			{
				return "real";
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003C6C File Offset: 0x00001E6C
		public override byte TypeCode
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003C80 File Offset: 0x00001E80
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003C97 File Offset: 0x00001E97
		public override double Value { get; set; }

		// Token: 0x060000A2 RID: 162 RVA: 0x00003CA0 File Offset: 0x00001EA0
		public PListReal()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003CAB File Offset: 0x00001EAB
		public PListReal(double value)
		{
			this.Value = value;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003CBE File Offset: 0x00001EBE
		protected override void Parse(string value)
		{
			this.Value = double.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003CD4 File Offset: 0x00001ED4
		protected override string ToXmlString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003CFC File Offset: 0x00001EFC
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
				throw new PListFormatException("Real < 32Bit");
			case 1:
				throw new PListFormatException("Real < 32Bit");
			case 2:
				this.Value = (double)BitConverter.ToSingle(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			case 3:
				this.Value = BitConverter.ToDouble(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			default:
				throw new PListFormatException("Real > 64Bit");
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public override int GetPListElementLength()
		{
			return 3;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003DC4 File Offset: 0x00001FC4
		public override void WriteBinary(PListBinaryWriter writer)
		{
			byte[] array = BitConverter.GetBytes(this.Value).Reverse<byte>().ToArray<byte>();
			writer.BaseStream.Write(array, 0, array.Length);
		}
	}
}
