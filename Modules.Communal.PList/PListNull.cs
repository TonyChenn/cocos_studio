using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000011 RID: 17
	public class PListNull : IPListElement, IXmlSerializable
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003B80 File Offset: 0x00001D80
		public string Tag
		{
			get
			{
				return "null";
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003B98 File Offset: 0x00001D98
		public byte TypeCode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003BAC File Offset: 0x00001DAC
		public bool IsBinaryUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public void ReadBinary(PListBinaryReader reader)
		{
			if (reader.CurrentElementLength != 0)
			{
				throw new PListFormatException();
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public int GetPListElementLength()
		{
			return 0;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public int GetPListElementCount()
		{
			return 1;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003C0B File Offset: 0x00001E0B
		public void WriteBinary(PListBinaryWriter writer)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003C10 File Offset: 0x00001E10
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003C23 File Offset: 0x00001E23
		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(this.Tag);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003C33 File Offset: 0x00001E33
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			writer.WriteEndElement();
		}
	}
}
