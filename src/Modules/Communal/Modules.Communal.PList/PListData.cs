using System;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x0200000D RID: 13
	public class PListData : PListElement<byte[]>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000034EC File Offset: 0x000016EC
		public override string Tag
		{
			get
			{
				return "data";
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003504 File Offset: 0x00001704
		public override byte TypeCode
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003518 File Offset: 0x00001718
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000352F File Offset: 0x0000172F
		public override byte[] Value { get; set; }

		// Token: 0x0600006B RID: 107 RVA: 0x00003538 File Offset: 0x00001738
		public PListData()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003543 File Offset: 0x00001743
		public PListData(byte[] value)
		{
			this.Value = value;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003556 File Offset: 0x00001756
		protected override void Parse(string value)
		{
			this.Value = Convert.FromBase64String(value);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003568 File Offset: 0x00001768
		protected override string ToXmlString()
		{
			return Convert.ToBase64String(this.Value);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003588 File Offset: 0x00001788
		public override void ReadBinary(PListBinaryReader reader)
		{
			this.Value = new byte[reader.CurrentElementLength];
			if (reader.BaseStream.Read(this.Value, 0, this.Value.Length) != this.Value.Length)
			{
				throw new PListFormatException();
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000035D8 File Offset: 0x000017D8
		public override int GetPListElementLength()
		{
			return this.Value.Length;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000035F2 File Offset: 0x000017F2
		public override void WriteBinary(PListBinaryWriter writer)
		{
			writer.BaseStream.Write(this.Value, 0, this.Value.Length);
		}
	}
}
