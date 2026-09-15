using System;
using System.Xml;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListBool : PListElement<bool>
	{
		public override string Tag
		{
			get
			{
				return "boolean";
			}
		}

		public override byte TypeCode
		{
			get
			{
				return 0;
			}
		}

		public override bool IsBinaryUnique
		{
			get
			{
				return true;
			}
		}

		public override bool Value { get; set; }

		public PListBool()
		{
		}

		public PListBool(bool value)
		{
			this.Value = value;
		}

		public override void ReadXml(XmlReader reader)
		{
			this.Parse(reader.LocalName);
			reader.ReadStartElement();
		}

		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.ToXmlString());
			writer.WriteEndElement();
		}

		protected override void Parse(string value)
		{
			this.Value = (value == "true");
		}

		protected override string ToXmlString()
		{
			return this.Value ? "true" : "false";
		}

		public override void ReadBinary(PListBinaryReader reader)
		{
			if (reader.CurrentElementLength != 8 && reader.CurrentElementLength != 9)
			{
				throw new PListFormatException();
			}
			this.Value = (reader.CurrentElementLength == 9);
		}

		public override void WriteBinary(PListBinaryWriter writer)
		{
		}

		public override int GetPListElementLength()
		{
			return this.Value ? 9 : 8;
		}
	}
}
