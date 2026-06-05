using System;
using System.Xml;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008A RID: 138
	public class XmlConfigurationReader
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x0000FA58 File Offset: 0x0000DC58
		public DataNode Read(XmlReader reader)
		{
			DataItem dataItem = new DataItem();
			dataItem.UniqueNames = false;
			reader.MoveToContent();
			string localName = reader.LocalName;
			dataItem.Name = localName;
			while (reader.MoveToNextAttribute())
			{
				if (!(reader.LocalName == "xmlns"))
				{
					DataNode dataNode = this.ReadAttribute(reader.LocalName, reader.Value);
					if (dataNode != null)
					{
						DataValue dataValue = dataNode as DataValue;
						if (dataValue != null)
						{
							dataValue.StoreAsAttribute = true;
						}
						dataItem.ItemData.Add(dataNode);
					}
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return dataItem;
			}
			reader.ReadStartElement();
			string text = "";
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					DataNode dataNode2 = this.ReadChild(reader, dataItem);
					if (dataNode2 != null)
					{
						dataItem.ItemData.Add(dataNode2);
					}
				}
				else if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
				{
					text += reader.Value;
					reader.Skip();
				}
				else
				{
					reader.Skip();
				}
			}
			reader.ReadEndElement();
			if (!dataItem.HasItemData && text != "")
			{
				return new DataValue(localName, text);
			}
			return dataItem;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000FB84 File Offset: 0x0000DD84
		public DataNode Read(XmlElement elem)
		{
			DataItem dataItem = new DataItem();
			dataItem.UniqueNames = false;
			dataItem.Name = elem.LocalName;
			foreach (object obj in elem.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				if (!(xmlAttribute.LocalName == "xmlns"))
				{
					DataNode dataNode = this.ReadAttribute(xmlAttribute.LocalName, xmlAttribute.Value);
					if (dataNode != null)
					{
						DataValue dataValue = dataNode as DataValue;
						if (dataValue != null)
						{
							dataValue.StoreAsAttribute = true;
						}
						dataItem.ItemData.Add(dataNode);
					}
				}
			}
			string text = "";
			foreach (object obj2 in elem.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj2;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					DataNode dataNode2 = this.ReadChild((XmlElement)xmlNode, dataItem);
					if (dataNode2 != null)
					{
						dataItem.ItemData.Add(dataNode2);
					}
				}
				else if (xmlNode.NodeType == XmlNodeType.Text)
				{
					text += ((XmlText)xmlNode).Value;
				}
			}
			if (!dataItem.HasItemData && text != "")
			{
				return new DataValue(dataItem.Name, text);
			}
			return dataItem;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000FCFC File Offset: 0x0000DEFC
		protected bool MoveToNextElement(XmlReader reader)
		{
			reader.MoveToContent();
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
				reader.Skip();
			}
			return false;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000FD23 File Offset: 0x0000DF23
		protected virtual DataNode ReadAttribute(string name, string value)
		{
			return new DataValue(name, value);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		protected virtual DataNode ReadChild(XmlElement elem, DataItem parent)
		{
			return this.GetChildReader(parent).Read(elem);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000FD3B File Offset: 0x0000DF3B
		protected virtual DataNode ReadChild(XmlReader reader, DataItem parent)
		{
			return this.GetChildReader(parent).Read(reader);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000FD4A File Offset: 0x0000DF4A
		protected virtual XmlConfigurationReader GetChildReader(DataItem parent)
		{
			return this;
		}

		// Token: 0x04000180 RID: 384
		public static XmlConfigurationReader DefaultReader = new XmlConfigurationReader();
	}
}
