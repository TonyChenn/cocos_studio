using System;
using System.Collections;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000070 RID: 112
	public class DictionaryDataType : DataType
	{
		// Token: 0x060003AC RID: 940 RVA: 0x0000DF23 File Offset: 0x0000C123
		internal DictionaryDataType(Type type) : base(type)
		{
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000DF2C File Offset: 0x0000C12C
		public override bool IsSimpleType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000DF2F File Offset: 0x0000C12F
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000DF32 File Offset: 0x0000C132
		public override bool CanReuseInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000DF38 File Offset: 0x0000C138
		internal static bool IsDictionaryType(Type t)
		{
			Type type;
			Type type2;
			return typeof(IDictionary).IsAssignableFrom(t) && DictionaryDataType.GetMapData(t, out type, out type2);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000DF68 File Offset: 0x0000C168
		internal static bool GetMapData(Type t, out Type keyType, out Type valueType)
		{
			Type type;
			valueType = (type = null);
			keyType = type;
			MethodInfo method = t.GetMethod("Add");
			if (method == null)
			{
				return false;
			}
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 2)
			{
				return false;
			}
			keyType = parameters[0].ParameterType;
			valueType = parameters[1].ParameterType;
			return true;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		protected internal override object GetMapData(object[] attributes, string scope)
		{
			Type valueType;
			Type valueType2;
			DictionaryDataType.GetMapData(base.ValueType, out valueType, out valueType2);
			DictionaryDataType.MapData mapData = new DictionaryDataType.MapData();
			mapData.KeyName = "Key";
			mapData.ValueName = "Value";
			mapData.ItemName = "Item";
			DataType dataType = null;
			DataType dataType2 = null;
			ItemPropertyAttribute itemPropertyAttribute = base.FindPropertyAttribute(attributes, scope + "/key");
			if (itemPropertyAttribute != null)
			{
				if (itemPropertyAttribute.ValueType != null)
				{
					valueType = itemPropertyAttribute.ValueType;
				}
				if (itemPropertyAttribute.SerializationDataType != null)
				{
					dataType = (DataType)Activator.CreateInstance(itemPropertyAttribute.SerializationDataType, new object[]
					{
						valueType
					});
				}
				if (!string.IsNullOrEmpty(itemPropertyAttribute.Name))
				{
					mapData.KeyName = itemPropertyAttribute.Name;
				}
			}
			if (dataType != null)
			{
				mapData.KeyType = dataType;
			}
			else
			{
				mapData.KeyType = base.Context.GetConfigurationDataType(valueType);
			}
			mapData.KeyMapData = mapData.KeyType.GetMapData(attributes, scope + "/key");
			itemPropertyAttribute = base.FindPropertyAttribute(attributes, scope + "/value");
			if (itemPropertyAttribute != null)
			{
				if (itemPropertyAttribute.ValueType != null)
				{
					valueType2 = itemPropertyAttribute.ValueType;
				}
				if (itemPropertyAttribute.SerializationDataType != null)
				{
					dataType2 = (DataType)Activator.CreateInstance(itemPropertyAttribute.SerializationDataType, new object[]
					{
						valueType2
					});
				}
				if (!string.IsNullOrEmpty(itemPropertyAttribute.Name))
				{
					mapData.ValueName = itemPropertyAttribute.Name;
				}
			}
			if (dataType2 != null)
			{
				mapData.ValueType = dataType2;
			}
			else
			{
				mapData.ValueType = base.Context.GetConfigurationDataType(valueType2);
			}
			mapData.ValueMapData = mapData.ValueType.GetMapData(attributes, scope + "/value");
			itemPropertyAttribute = base.FindPropertyAttribute(attributes, scope + "/item");
			if (itemPropertyAttribute != null && !string.IsNullOrEmpty(itemPropertyAttribute.Name))
			{
				mapData.ItemName = itemPropertyAttribute.Name;
			}
			return mapData;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		protected virtual DictionaryDataType.MapData GetDefaultData()
		{
			if (this.defaultData != null)
			{
				return this.defaultData;
			}
			this.defaultData = new DictionaryDataType.MapData();
			Type type;
			Type type2;
			DictionaryDataType.GetMapData(base.ValueType, out type, out type2);
			this.defaultData.KeyName = "Key";
			this.defaultData.ValueName = "Value";
			this.defaultData.ItemName = "Item";
			this.defaultData.KeyType = base.Context.GetConfigurationDataType(type);
			this.defaultData.ValueType = base.Context.GetConfigurationDataType(type2);
			return this.defaultData;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000E240 File Offset: 0x0000C440
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mdata, object collection)
		{
			DictionaryDataType.MapData mapData = (mdata != null) ? ((DictionaryDataType.MapData)mdata) : this.GetDefaultData();
			DataItem dataItem = new DataItem();
			dataItem.Name = base.Name;
			dataItem.UniqueNames = false;
			IDictionary dictionary = (IDictionary)collection;
			foreach (object obj in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				DataItem dataItem2 = new DataItem();
				dataItem2.Name = mapData.ItemName;
				dataItem2.UniqueNames = true;
				DataNode dataNode = mapData.KeyType.Serialize(serCtx, null, dictionaryEntry.Key);
				dataNode.Name = mapData.KeyName;
				DataNode dataNode2 = mapData.ValueType.Serialize(serCtx, null, dictionaryEntry.Value);
				dataNode2.Name = mapData.ValueName;
				dataItem2.ItemData.Add(dataNode);
				dataItem2.ItemData.Add(dataNode2);
				dataItem.ItemData.Add(dataItem2);
			}
			return dataItem;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000E358 File Offset: 0x0000C558
		protected internal override object OnDeserialize(SerializationContext serCtx, object mdata, DataNode data)
		{
			object obj = Activator.CreateInstance(base.ValueType);
			base.Deserialize(serCtx, mdata, data, obj);
			return obj;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000E37C File Offset: 0x0000C57C
		protected internal override void OnDeserialize(SerializationContext serCtx, object mdata, DataNode data, object collectionInstance)
		{
			DictionaryDataType.MapData mapData = (mdata != null) ? ((DictionaryDataType.MapData)mdata) : this.GetDefaultData();
			DataCollection itemData = ((DataItem)data).ItemData;
			IDictionary dictionary = (IDictionary)collectionInstance;
			foreach (object obj in itemData)
			{
				DataItem dataItem = (DataItem)obj;
				DataNode dataNode = dataItem.ItemData[mapData.KeyName];
				if (dataNode != null)
				{
					DataNode dataNode2 = dataItem.ItemData[mapData.ValueName];
					object key = mapData.KeyType.Deserialize(serCtx, null, dataNode);
					object value = (dataNode2 != null) ? mapData.ValueType.Deserialize(serCtx, null, dataNode2) : null;
					dictionary[key] = value;
				}
			}
		}

		// Token: 0x04000141 RID: 321
		private DictionaryDataType.MapData defaultData;

		// Token: 0x02000071 RID: 113
		protected class MapData
		{
			// Token: 0x04000142 RID: 322
			public string ItemName;

			// Token: 0x04000143 RID: 323
			public string KeyName;

			// Token: 0x04000144 RID: 324
			public string ValueName;

			// Token: 0x04000145 RID: 325
			public DataType KeyType;

			// Token: 0x04000146 RID: 326
			public DataType ValueType;

			// Token: 0x04000147 RID: 327
			public object KeyMapData;

			// Token: 0x04000148 RID: 328
			public object ValueMapData;
		}
	}
}
