using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListArray : List<IPListElement>, IPListElement, IXmlSerializable
	{
		public string Tag
		{
			get
			{
				return "array";
			}
		}

		public byte TypeCode
		{
			get
			{
				return 10;
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
			byte[] array = new byte[reader.CurrentElementLength * (int)reader.ElementIdxSize];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			for (int i = 0; i < reader.CurrentElementLength; i++)
			{
				base.Add(reader.ReadInternal((int)((reader.ElementIdxSize == 1) ? ((short)array[i]) : IPAddress.NetworkToHostOrder(BitConverter.ToInt16(array, 2 * i)))));
			}
		}

		public int GetPListElementLength()
		{
			return base.Count;
		}

		public int GetPListElementCount()
		{
			int num = 1;
			foreach (IPListElement iplistElement in this)
			{
				num += iplistElement.GetPListElementCount();
			}
			return num;
		}

		public void WriteBinary(PListBinaryWriter writer)
		{
			byte[] array = new byte[(int)writer.ElementIdxSize * base.Count];
			long position = writer.BaseStream.Position;
			writer.BaseStream.Write(array, 0, array.Length);
			for (int i = 0; i < base.Count; i++)
			{
				int idx = writer.WriteInternal(base[i]);
				writer.FormatIdx(idx).CopyTo(array, (int)writer.ElementIdxSize * i);
			}
			writer.BaseStream.Seek(position, SeekOrigin.Begin);
			writer.BaseStream.Write(array, 0, array.Length);
			writer.BaseStream.Seek(0L, SeekOrigin.End);
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (!isEmptyElement)
			{
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					IPListElement iplistElement = PListElementFactory.Instance.Create(reader.LocalName);
					iplistElement.ReadXml(reader);
					base.Add(iplistElement);
					reader.MoveToContent();
				}
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			for (int i = 0; i < base.Count; i++)
			{
				base[i].WriteXml(writer);
			}
			writer.WriteEndElement();
		}
	}
}
