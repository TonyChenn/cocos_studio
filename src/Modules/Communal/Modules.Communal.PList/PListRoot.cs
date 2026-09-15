using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	[XmlRoot("plist")]
	public class PListRoot : IXmlSerializable
	{
		public PListFormat Format { get; set; }

		public static PListRoot Load(string fileName)
		{
			PListRoot result;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				result = PListRoot.Load(fileStream);
			}
			return result;
		}

		public static PListRoot Load(Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PListRoot));
			byte[] array = new byte[8];
			stream.Read(array, 0, array.Length);
			stream.Seek(0L, SeekOrigin.Begin);
			PListRoot plistRoot;
			if (Encoding.Default.GetString(array) == "bplist00")
			{
				PListBinaryReader plistBinaryReader = new PListBinaryReader();
				plistRoot = new PListRoot();
				plistRoot.Format = PListFormat.Binary;
				plistRoot.Root = plistBinaryReader.Read(stream);
			}
			else
			{
				plistRoot = (PListRoot)xmlSerializer.Deserialize(stream);
				plistRoot.Format = PListFormat.Xml;
			}
			return plistRoot;
		}

		public void Save(string fileName, PListFormat format)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				this.Save(fileStream, format);
			}
		}

		public void Save(string fileName)
		{
			this.Save(fileName, this.Format);
		}

		public void Save(Stream stream)
		{
			this.Save(stream, this.Format);
		}

		public void Save(Stream stream, PListFormat format)
		{
			if (format == PListFormat.Xml)
			{
				XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
				{
					Encoding = Encoding.UTF8,
					Indent = true,
					IndentChars = "\t",
					NewLineChars = "\n"
				});
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteDocType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
				this.WriteXml(xmlWriter);
				xmlWriter.Flush();
			}
			else
			{
				PListBinaryWriter plistBinaryWriter = new PListBinaryWriter();
				plistBinaryWriter.Write(stream, this.Root);
			}
		}

		public IPListElement Root { get; set; }

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement("plist");
			this.Root = PListElementFactory.Instance.Create(reader.LocalName);
			this.Root.ReadXml(reader);
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("plist");
			writer.WriteAttributeString("version", "1.0");
			if (this.Root != null)
			{
				this.Root.WriteXml(writer);
			}
			writer.WriteEndElement();
		}
	}
}
