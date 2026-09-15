using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListNull : IPListElement, IXmlSerializable
	{
		public string Tag
		{
			get
			{
				return "null";
			}
		}

		public byte TypeCode
		{
			get
			{
				return 0;
			}
		}

		public bool IsBinaryUnique
		{
			get
			{
				return false;
			}
		}

		public void ReadBinary(PListBinaryReader reader)
		{
			if (reader.CurrentElementLength != 0)
			{
				throw new PListFormatException();
			}
		}

		public int GetPListElementLength()
		{
			return 0;
		}

		public int GetPListElementCount()
		{
			return 1;
		}

		public void WriteBinary(PListBinaryWriter writer)
		{
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(this.Tag);
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			writer.WriteEndElement();
		}
	}
}
