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
	// Token: 0x02000006 RID: 6
	public class PListDict : Dictionary<string, IPListElement>, IPListElement, IXmlSerializable
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A64 File Offset: 0x00000C64
		public string Tag
		{
			get
			{
				return "dict";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002A7C File Offset: 0x00000C7C
		public byte TypeCode
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002A90 File Offset: 0x00000C90
		public bool IsBinaryUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AA4 File Offset: 0x00000CA4
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

		// Token: 0x0600002E RID: 46 RVA: 0x00002BB0 File Offset: 0x00000DB0
		public int GetPListElementLength()
		{
			return base.Count;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BC8 File Offset: 0x00000DC8
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

		// Token: 0x06000030 RID: 48 RVA: 0x00002C3C File Offset: 0x00000E3C
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

		// Token: 0x06000031 RID: 49 RVA: 0x00002D84 File Offset: 0x00000F84
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D98 File Offset: 0x00000F98
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

		// Token: 0x06000033 RID: 51 RVA: 0x00002E38 File Offset: 0x00001038
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
