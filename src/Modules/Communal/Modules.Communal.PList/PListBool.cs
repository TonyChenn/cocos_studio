using System;
using System.Xml;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x0200000C RID: 12
	public class PListBool : PListElement<bool>
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000339C File Offset: 0x0000159C
		public override string Tag
		{
			get
			{
				return "boolean";
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000033B4 File Offset: 0x000015B4
		public override byte TypeCode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000033C8 File Offset: 0x000015C8
		public override bool IsBinaryUnique
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000033DC File Offset: 0x000015DC
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000033F3 File Offset: 0x000015F3
		public override bool Value { get; set; }

		// Token: 0x0600005E RID: 94 RVA: 0x000033FC File Offset: 0x000015FC
		public PListBool()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003407 File Offset: 0x00001607
		public PListBool(bool value)
		{
			this.Value = value;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000341A File Offset: 0x0000161A
		public override void ReadXml(XmlReader reader)
		{
			this.Parse(reader.LocalName);
			reader.ReadStartElement();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003431 File Offset: 0x00001631
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.ToXmlString());
			writer.WriteEndElement();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003448 File Offset: 0x00001648
		protected override void Parse(string value)
		{
			this.Value = (value == "true");
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003460 File Offset: 0x00001660
		protected override string ToXmlString()
		{
			return this.Value ? "true" : "false";
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003488 File Offset: 0x00001688
		public override void ReadBinary(PListBinaryReader reader)
		{
			if (reader.CurrentElementLength != 8 && reader.CurrentElementLength != 9)
			{
				throw new PListFormatException();
			}
			this.Value = (reader.CurrentElementLength == 9);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000034C8 File Offset: 0x000016C8
		public override void WriteBinary(PListBinaryWriter writer)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000034CC File Offset: 0x000016CC
		public override int GetPListElementLength()
		{
			return this.Value ? 9 : 8;
		}
	}
}
