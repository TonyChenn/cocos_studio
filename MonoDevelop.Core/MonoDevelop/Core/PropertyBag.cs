using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core
{
	// Token: 0x020000C1 RID: 193
	[DataItem("Properties")]
	public class PropertyBag : ICustomDataItem, IDisposable
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001A240 File Offset: 0x00018440
		public bool IsEmpty
		{
			get
			{
				return this.properties == null || this.properties.Count == 0;
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001A25A File Offset: 0x0001845A
		public T GetValue<T>()
		{
			return this.GetValue<T>(typeof(T).FullName);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001A271 File Offset: 0x00018471
		public T GetValue<T>(string name)
		{
			return this.GetValue<T>(name, null);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001A27C File Offset: 0x0001847C
		public T GetValue<T>(string name, DataContext ctx)
		{
			object obj;
			if (this.properties != null && this.properties.TryGetValue(name, out obj))
			{
				if (obj is DataNode)
				{
					obj = this.Deserialize(name, (DataNode)obj, typeof(T), ctx ?? this.context);
					this.properties[name] = obj;
					this.OnChanged(name);
				}
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001A2F0 File Offset: 0x000184F0
		public T GetValue<T>(string name, T defaultValue)
		{
			return this.GetValue<T>(name, defaultValue, null);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001A2FC File Offset: 0x000184FC
		public T GetValue<T>(string name, T defaultValue, DataContext ctx)
		{
			object obj;
			if (this.properties != null && this.properties.TryGetValue(name, out obj))
			{
				if (obj is DataNode)
				{
					obj = this.Deserialize(name, (DataNode)obj, typeof(T), ctx ?? this.context);
					this.properties[name] = obj;
				}
				return (T)((object)obj);
			}
			return defaultValue;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001A361 File Offset: 0x00018561
		public void SetValue<T>(T value)
		{
			this.SetValue<T>(typeof(T).FullName, value);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001A379 File Offset: 0x00018579
		public void SetValue<T>(string name, T value)
		{
			if (this.properties == null)
			{
				this.properties = new Dictionary<string, object>();
			}
			this.properties[name] = value;
			this.OnChanged(name);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001A3A7 File Offset: 0x000185A7
		public bool RemoveValue<T>()
		{
			return this.RemoveValue(typeof(T).FullName);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001A3BE File Offset: 0x000185BE
		public bool RemoveValue(string name)
		{
			if (this.properties != null && this.properties.Remove(name))
			{
				this.OnChanged(name);
				return true;
			}
			return false;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001A3E0 File Offset: 0x000185E0
		public bool HasValue<T>()
		{
			return this.HasValue(typeof(T).FullName);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001A3F7 File Offset: 0x000185F7
		public bool HasValue(string name)
		{
			return this.properties != null && this.properties.ContainsKey(name);
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060006A0 RID: 1696 RVA: 0x0001A410 File Offset: 0x00018610
		// (remove) Token: 0x060006A1 RID: 1697 RVA: 0x0001A448 File Offset: 0x00018648
		public event EventHandler<PropertyBagChangedEventArgs> Changed;

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001A480 File Offset: 0x00018680
		private void OnChanged(string name)
		{
			EventHandler<PropertyBagChangedEventArgs> changed = this.Changed;
			if (changed != null)
			{
				changed(this, new PropertyBagChangedEventArgs(name));
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001A4A4 File Offset: 0x000186A4
		public void Dispose()
		{
			if (this.properties != null)
			{
				foreach (object obj in this.properties.Values)
				{
					IDisposable disposable = obj as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				this.properties = null;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001A514 File Offset: 0x00018714
		private object Deserialize(string name, DataNode node, Type type, DataContext ctx)
		{
			if (type.IsAssignableFrom(typeof(XmlElement)))
			{
				DataItem dataItem = node as DataItem;
				if (dataItem == null || dataItem.ItemData.Count > 1)
				{
					throw new InvalidOperationException("Can't convert property to an XmlElement object.");
				}
				if (dataItem.ItemData.Count == 0)
				{
					return null;
				}
				XmlConfigurationWriter xmlConfigurationWriter = new XmlConfigurationWriter();
				XmlDocument doc = new XmlDocument();
				return xmlConfigurationWriter.Write(doc, dataItem.ItemData[0]);
			}
			else
			{
				if (ctx == null)
				{
					throw new InvalidOperationException("Can't deserialize property '" + name + "'. Serialization context not set.");
				}
				return new DataSerializer(ctx)
				{
					SerializationContext = 
					{
						BaseFile = this.sourceFile
					}
				}.Deserialize(type, node);
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001A5C4 File Offset: 0x000187C4
		DataCollection ICustomDataItem.Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = new DataCollection();
			if (this.IsEmpty)
			{
				return dataCollection;
			}
			foreach (KeyValuePair<string, object> keyValuePair in this.properties)
			{
				if (keyValuePair.Value != null)
				{
					DataNode dataNode;
					if (keyValuePair.Value is XmlElement)
					{
						DataItem dataItem = new DataItem();
						XmlConfigurationReader xmlConfigurationReader = new XmlConfigurationReader();
						dataItem.ItemData.Add(xmlConfigurationReader.Read((XmlElement)keyValuePair.Value));
						dataNode = dataItem;
					}
					else if (keyValuePair.Value is DataNode)
					{
						dataNode = (DataNode)keyValuePair.Value;
					}
					else
					{
						dataNode = handler.SerializationContext.Serializer.Serialize(keyValuePair.Value, keyValuePair.Value.GetType());
					}
					dataNode.Name = this.EscapeName(keyValuePair.Key);
					dataCollection.Add(dataNode);
				}
			}
			return dataCollection;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001A6CC File Offset: 0x000188CC
		void ICustomDataItem.Deserialize(ITypeSerializer handler, DataCollection data)
		{
			if (data.Count == 0)
			{
				return;
			}
			this.properties = new Dictionary<string, object>();
			this.context = handler.SerializationContext.Serializer.DataContext;
			this.sourceFile = handler.SerializationContext.BaseFile;
			foreach (object obj in data)
			{
				DataNode dataNode = (DataNode)obj;
				if (dataNode.Name != "ctype")
				{
					this.properties[this.UnescapeName(dataNode.Name)] = dataNode;
				}
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001A780 File Offset: 0x00018980
		private string EscapeName(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length);
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == '_')
				{
					stringBuilder.Append("__");
				}
				else if (c != '.' && c != '-' && !char.IsLetter(c) && (!char.IsNumber(c) || i == 0))
				{
					int num = (int)c;
					string text = num.ToString("X");
					stringBuilder.Append("_" + text.Length.ToString());
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001A830 File Offset: 0x00018A30
		private string UnescapeName(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length);
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == '_')
				{
					if (i + 1 >= str.Length)
					{
						return stringBuilder.ToString();
					}
					if (str[i + 1] == '_')
					{
						stringBuilder.Append(c);
						i++;
					}
					else
					{
						int num = int.Parse(str.Substring(i + 1, 1));
						if (i + 2 + num - 1 >= str.Length)
						{
							return stringBuilder.ToString();
						}
						int num2;
						if (int.TryParse(str.Substring(i + 2, num), NumberStyles.HexNumber, null, out num2))
						{
							stringBuilder.Append((char)num2);
						}
						i += num + 1;
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400022C RID: 556
		private Dictionary<string, object> properties;

		// Token: 0x0400022D RID: 557
		private DataContext context;

		// Token: 0x0400022E RID: 558
		private string sourceFile;
	}
}
