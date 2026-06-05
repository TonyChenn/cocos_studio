using System;
using System.Collections;
using System.Xml;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000089 RID: 137
	public class XmlConfigurationWriter
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000F62B File Offset: 0x0000D82B
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0000F633 File Offset: 0x0000D833
		public string[] StoreInElementExceptions { get; set; }

		// Token: 0x0600047E RID: 1150 RVA: 0x0000F63C File Offset: 0x0000D83C
		public void Write(XmlWriter writer, DataNode data)
		{
			if (data is DataValue)
			{
				writer.WriteElementString(data.Name, ((DataValue)data).Value);
				return;
			}
			if (data is DataItem)
			{
				writer.WriteStartElement(data.Name);
				this.WriteAttributes(writer, (DataItem)data);
				this.WriteChildren(writer, (DataItem)data);
				writer.WriteEndElement();
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000F6A0 File Offset: 0x0000D8A0
		public XmlElement Write(XmlDocument doc, DataNode data)
		{
			XmlElement xmlElement = doc.CreateElement(data.Name);
			if (data is DataValue)
			{
				xmlElement.InnerText = ((DataValue)data).Value;
			}
			else if (data is DataItem)
			{
				this.WriteAttributes(xmlElement, (DataItem)data);
				this.WriteChildren(xmlElement, (DataItem)data);
			}
			return xmlElement;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		protected virtual void WriteAttributes(XmlElement elem, DataItem item)
		{
			if (this.StoreAllInElements)
			{
				return;
			}
			foreach (object obj in item.ItemData)
			{
				DataNode dataNode = (DataNode)obj;
				DataValue dataValue = dataNode as DataValue;
				if (dataValue != null && (item.UniqueNames || dataValue.StoreAsAttribute))
				{
					this.WriteAttribute(elem, dataValue.Name, dataValue.Value);
				}
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000F780 File Offset: 0x0000D980
		protected virtual void WriteAttributes(XmlWriter writer, DataItem item)
		{
			foreach (object obj in item.ItemData)
			{
				DataNode dataNode = (DataNode)obj;
				DataValue dataValue = dataNode as DataValue;
				if (dataValue != null && (item.UniqueNames || dataValue.StoreAsAttribute) && this.StoreAsAttribute(dataValue))
				{
					this.WriteAttribute(writer, dataValue.Name, dataValue.Value);
				}
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000F808 File Offset: 0x0000DA08
		protected virtual void WriteAttribute(XmlElement elem, string name, string value)
		{
			elem.SetAttribute(name, value);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000F812 File Offset: 0x0000DA12
		protected virtual void WriteAttribute(XmlWriter writer, string name, string value)
		{
			writer.WriteAttributeString(name, value);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000F81C File Offset: 0x0000DA1C
		protected virtual void WriteChildren(XmlWriter writer, DataItem item)
		{
			if (item.UniqueNames)
			{
				using (IEnumerator enumerator = item.ItemData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DataNode dataNode = (DataNode)obj;
						if (!(dataNode is DataValue) || !this.StoreAsAttribute((DataValue)dataNode))
						{
							this.WriteChild(writer, dataNode);
						}
					}
					return;
				}
			}
			foreach (object obj2 in item.ItemData)
			{
				DataNode dataNode2 = (DataNode)obj2;
				DataValue dataValue = dataNode2 as DataValue;
				if (dataValue == null || !dataValue.StoreAsAttribute || !this.StoreAsAttribute(dataValue))
				{
					this.WriteChild(writer, dataNode2);
				}
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000F904 File Offset: 0x0000DB04
		protected virtual void WriteChildren(XmlElement elem, DataItem item)
		{
			if (item.UniqueNames)
			{
				using (IEnumerator enumerator = item.ItemData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DataNode dataNode = (DataNode)obj;
						if (!(dataNode is DataValue) || !this.StoreAsAttribute((DataValue)dataNode))
						{
							this.WriteChild(elem, dataNode);
						}
					}
					return;
				}
			}
			foreach (object obj2 in item.ItemData)
			{
				DataNode dataNode2 = (DataNode)obj2;
				DataValue dataValue = dataNode2 as DataValue;
				if (dataValue == null || !dataValue.StoreAsAttribute || !this.StoreAsAttribute(dataValue))
				{
					this.WriteChild(elem, dataNode2);
				}
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000F9EC File Offset: 0x0000DBEC
		protected virtual void WriteChild(XmlElement elem, DataNode data)
		{
			elem.AppendChild(this.GetChildWriter(data).Write(elem.OwnerDocument, data));
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000FA08 File Offset: 0x0000DC08
		protected virtual void WriteChild(XmlWriter writer, DataNode data)
		{
			this.GetChildWriter(data).Write(writer, data);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000FA18 File Offset: 0x0000DC18
		public virtual bool StoreAsAttribute(DataValue val)
		{
			return !this.StoreAllInElements || (this.StoreInElementExceptions != null && ((IList)this.StoreInElementExceptions).Contains(val.Name));
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000FA3F File Offset: 0x0000DC3F
		protected virtual XmlConfigurationWriter GetChildWriter(DataNode data)
		{
			return this;
		}

		// Token: 0x0400017D RID: 381
		public static XmlConfigurationWriter DefaultWriter = new XmlConfigurationWriter();

		// Token: 0x0400017E RID: 382
		public bool StoreAllInElements;
	}
}
