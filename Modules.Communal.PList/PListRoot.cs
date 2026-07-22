using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000014 RID: 20
	[XmlRoot("plist")]
	public class PListRoot : IXmlSerializable
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000040B8 File Offset: 0x000022B8
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000040CF File Offset: 0x000022CF
		public PListFormat Format { get; set; }

		// Token: 0x060000B7 RID: 183 RVA: 0x000040D8 File Offset: 0x000022D8
		public static PListRoot Load(string fileName)
		{
			PListRoot result;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				result = PListRoot.Load(fileStream);
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000411C File Offset: 0x0000231C
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

		// Token: 0x060000B9 RID: 185 RVA: 0x000041BC File Offset: 0x000023BC
		public void Save(string fileName, PListFormat format)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				this.Save(fileStream, format);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004200 File Offset: 0x00002400
		public void Save(string fileName)
		{
			this.Save(fileName, this.Format);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004211 File Offset: 0x00002411
		public void Save(Stream stream)
		{
			this.Save(stream, this.Format);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004224 File Offset: 0x00002424
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000042C0 File Offset: 0x000024C0
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000042D7 File Offset: 0x000024D7
		public IPListElement Root { get; set; }

		// Token: 0x060000BF RID: 191 RVA: 0x000042E0 File Offset: 0x000024E0
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000042F3 File Offset: 0x000024F3
		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement("plist");
			this.Root = PListElementFactory.Instance.Create(reader.LocalName);
			this.Root.ReadXml(reader);
			reader.ReadEndElement();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004330 File Offset: 0x00002530
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
