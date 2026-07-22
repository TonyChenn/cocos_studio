using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x0200000F RID: 15
	public class PListFill : IPListElement, IXmlSerializable
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003800 File Offset: 0x00001A00
		public string Tag
		{
			get
			{
				return "fill";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003818 File Offset: 0x00001A18
		public byte TypeCode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000382C File Offset: 0x00001A2C
		public bool IsBinaryUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003840 File Offset: 0x00001A40
		public void ReadBinary(PListBinaryReader reader)
		{
			if (reader.CurrentElementLength != 15)
			{
				throw new PListFormatException();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003864 File Offset: 0x00001A64
		public int GetPListElementLength()
		{
			return 15;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003878 File Offset: 0x00001A78
		public int GetPListElementCount()
		{
			return 1;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000388B File Offset: 0x00001A8B
		public void WriteBinary(PListBinaryWriter writer)
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003890 File Offset: 0x00001A90
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000038A3 File Offset: 0x00001AA3
		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(this.Tag);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000038B3 File Offset: 0x00001AB3
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			writer.WriteEndElement();
		}
	}
}
