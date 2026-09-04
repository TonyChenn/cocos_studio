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
	// Token: 0x02000005 RID: 5
	public class PListArray : List<IPListElement>, IPListElement, IXmlSerializable
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000027A8 File Offset: 0x000009A8
		public string Tag
		{
			get
			{
				return "array";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027C0 File Offset: 0x000009C0
		public byte TypeCode
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool IsBinaryUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027E8 File Offset: 0x000009E8
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

		// Token: 0x06000023 RID: 35 RVA: 0x0000286C File Offset: 0x00000A6C
		public int GetPListElementLength()
		{
			return base.Count;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002884 File Offset: 0x00000A84
		public int GetPListElementCount()
		{
			int num = 1;
			foreach (IPListElement iplistElement in this)
			{
				num += iplistElement.GetPListElementCount();
			}
			return num;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000028E8 File Offset: 0x00000AE8
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

		// Token: 0x06000026 RID: 38 RVA: 0x00002994 File Offset: 0x00000B94
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029A8 File Offset: 0x00000BA8
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

		// Token: 0x06000028 RID: 40 RVA: 0x00002A14 File Offset: 0x00000C14
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
