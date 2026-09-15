using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListDict : Dictionary<string, IPListElement>, IPListElement, IXmlSerializable
	{
		public string Tag
		{
			get
			{
				return "dict";
			}
		}

		public byte TypeCode
		{
			get
			{
				return 13;
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
			byte[] array2 = new byte[reader.CurrentElementLength * (int)reader.ElementIdxSize];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			if (reader.BaseStream.Read(array2, 0, array2.Length) != array2.Length)
			{
				throw new PListFormatException();
			}
			for (int i = 0; i < reader.CurrentElementLength; i++)
			{
				IPListElement iplistElement = reader.ReadInternal((int)((reader.ElementIdxSize == 1) ? ((short)array[i]) : IPAddress.NetworkToHostOrder(BitConverter.ToInt16(array, 2 * i))));
				if (!(iplistElement is PListString))
				{
					throw new PListFormatException("Key is no String");
				}
				IPListElement value = reader.ReadInternal((int)((reader.ElementIdxSize == 1) ? ((short)array2[i]) : IPAddress.NetworkToHostOrder(BitConverter.ToInt16(array2, 2 * i))));
				base.Add((PListString)iplistElement, value);
			}
		}

		public int GetPListElementLength()
		{
			return base.Count;
		}

		public int GetPListElementCount()
		{
			int num = 1;
			foreach (IPListElement iplistElement in base.Values)
			{
				num += iplistElement.GetPListElementCount();
			}
			num += base.Keys.Count;
			return num;
		}

		public void WriteBinary(PListBinaryWriter writer)
		{
			byte[] array = new byte[(int)writer.ElementIdxSize * base.Count];
			byte[] array2 = new byte[(int)writer.ElementIdxSize * base.Count];
			long position = writer.BaseStream.Position;
			writer.BaseStream.Write(array, 0, array.Length);
			writer.BaseStream.Write(array2, 0, array2.Length);
			KeyValuePair<string, IPListElement>[] array3 = this.ToArray<KeyValuePair<string, IPListElement>>();
			for (int i = 0; i < base.Count; i++)
			{
				int idx = writer.WriteInternal(PListElementFactory.Instance.CreateKeyElement(array3[i].Key));
				writer.FormatIdx(idx).CopyTo(array, (int)writer.ElementIdxSize * i);
			}
			for (int i = 0; i < base.Count; i++)
			{
				int idx = writer.WriteInternal(array3[i].Value);
				writer.FormatIdx(idx).CopyTo(array2, (int)writer.ElementIdxSize * i);
			}
			writer.BaseStream.Seek(position, SeekOrigin.Begin);
			writer.BaseStream.Write(array, 0, array.Length);
			writer.BaseStream.Write(array2, 0, array2.Length);
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
					reader.ReadStartElement("key");
					string key = reader.ReadString();
					reader.ReadEndElement();
					IPListElement iplistElement = PListElementFactory.Instance.Create(reader.LocalName);
					iplistElement.ReadXml(reader);
					if (base.ContainsKey(key))
					{
						base[key] = iplistElement;
					}
					else
					{
						base.Add(key, iplistElement);
					}
					reader.MoveToContent();
				}
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			foreach (string text in base.Keys)
			{
				writer.WriteStartElement("key");
				writer.WriteValue(text);
				writer.WriteEndElement();
				base[text].WriteXml(writer);
			}
			writer.WriteEndElement();
		}
	}
}
