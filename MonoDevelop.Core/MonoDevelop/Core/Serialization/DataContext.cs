using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Mono.Addins;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000066 RID: 102
	public class DataContext
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000D4B5 File Offset: 0x0000B6B5
		public DataContext()
		{
			this.attributeProvider = TypeAttributeProvider.Instance;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public DataContext(ISerializationAttributeProvider attributeProvider)
		{
			this.attributeProvider = attributeProvider;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000D52F File Offset: 0x0000B72F
		// (set) Token: 0x06000362 RID: 866 RVA: 0x0000D537 File Offset: 0x0000B737
		public ISerializationAttributeProvider AttributeProvider
		{
			get
			{
				return this.attributeProvider;
			}
			set
			{
				this.attributeProvider = value;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D540 File Offset: 0x0000B740
		public virtual SerializationContext CreateSerializationContext()
		{
			return new SerializationContext();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D548 File Offset: 0x0000B748
		public DataNode SaveConfigurationData(SerializationContext serCtx, object obj, Type type)
		{
			if (type == null)
			{
				type = obj.GetType();
			}
			DataType configurationDataType = this.GetConfigurationDataType(type);
			return configurationDataType.Serialize(serCtx, null, obj);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D578 File Offset: 0x0000B778
		public object LoadConfigurationData(SerializationContext serCtx, Type type, DataNode data)
		{
			DataType configurationDataType = this.GetConfigurationDataType(type);
			return configurationDataType.Deserialize(serCtx, null, data);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000D598 File Offset: 0x0000B798
		public void SetConfigurationItemData(SerializationContext serCtx, object obj, DataItem data)
		{
			ClassDataType classDataType = (ClassDataType)this.GetConfigurationDataType(obj.GetType());
			classDataType.Deserialize(serCtx, null, data, obj);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000D5C4 File Offset: 0x0000B7C4
		public object CreateConfigurationData(SerializationContext serCtx, Type type, DataNode data)
		{
			DataType configurationDataType = this.GetConfigurationDataType(type);
			return configurationDataType.CreateInstance(serCtx, data);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000D5E1 File Offset: 0x0000B7E1
		public void RegisterProperty(Type targetType, string name, Type propertyType)
		{
			this.RegisterProperty(targetType, name, propertyType, true, false);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000D5F0 File Offset: 0x0000B7F0
		public void RegisterProperty(Type targetType, string name, Type propertyType, bool isExternal, bool skipEmpty)
		{
			if (!typeof(IExtendedDataItem).IsAssignableFrom(targetType))
			{
				throw new InvalidOperationException("The type '" + targetType + "' does not implement the IExtendedDataItem interface and cannot be extended with new properties");
			}
			ClassDataType classDataType = (ClassDataType)this.GetConfigurationDataType(targetType);
			classDataType.AddProperty(new ItemProperty(name, propertyType)
			{
				Unsorted = true,
				IsExternal = isExternal,
				SkipEmpty = skipEmpty
			});
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000D658 File Offset: 0x0000B858
		public void RegisterProperty(RuntimeAddin addin, string targetType, string name, string propertyType, bool isExternal, bool skipEmpty)
		{
			DataContext.TypeRef typeRef;
			if (!this.pendingTypesByTypeName.TryGetValue(targetType, out typeRef))
			{
				typeRef = new DataContext.TypeRef(addin, targetType);
				this.pendingTypesByTypeName[targetType] = typeRef;
			}
			if (typeRef.DataType != null)
			{
				this.RegisterProperty(addin.GetType(targetType, true), name, addin.GetType(propertyType, true), isExternal, skipEmpty);
				return;
			}
			DataContext.PropertyRef propertyRef = new DataContext.PropertyRef(addin, targetType, name, propertyType, isExternal, skipEmpty);
			if (typeRef.Properties == null)
			{
				typeRef.Properties = propertyRef;
				return;
			}
			DataContext.PropertyRef propertyRef2 = typeRef.Properties;
			while (propertyRef2.Next != null)
			{
				propertyRef2 = propertyRef2.Next;
			}
			propertyRef2.Next = propertyRef;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
		public void UnregisterProperty(Type targetType, string name)
		{
			ClassDataType classDataType = (ClassDataType)this.GetConfigurationDataType(targetType);
			classDataType.RemoveProperty(name);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000D714 File Offset: 0x0000B914
		public void UnregisterProperty(RuntimeAddin addin, string targetType, string name)
		{
			DataContext.TypeRef typeRef;
			if (!this.pendingTypesByTypeName.TryGetValue(targetType, out typeRef))
			{
				return;
			}
			if (typeRef.DataType != null)
			{
				Type type = addin.GetType(targetType, false);
				if (type != null)
				{
					this.UnregisterProperty(type, name);
				}
				return;
			}
			DataContext.PropertyRef propertyRef = typeRef.Properties;
			DataContext.PropertyRef propertyRef2 = null;
			while (propertyRef != null)
			{
				if (propertyRef.Name == name)
				{
					if (propertyRef2 != null)
					{
						propertyRef2.Next = propertyRef.Next;
						return;
					}
					typeRef.Properties = null;
					return;
				}
				else
				{
					propertyRef2 = propertyRef;
					propertyRef = propertyRef.Next;
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000D794 File Offset: 0x0000B994
		public IEnumerable<ItemProperty> GetProperties(SerializationContext serCtx, object instance)
		{
			ClassDataType classDataType = (ClassDataType)this.GetConfigurationDataType(instance.GetType());
			return classDataType.GetProperties(serCtx, instance);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D7BB File Offset: 0x0000B9BB
		public void IncludeType(Type type)
		{
			this.GetConfigurationDataType(type);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		public void IncludeType(RuntimeAddin addin, string typeName, string itemName)
		{
			if (string.IsNullOrEmpty(itemName))
			{
				int num = typeName.LastIndexOf('.');
				if (num >= 0)
				{
					itemName = typeName.Substring(num + 1);
				}
				else
				{
					itemName = typeName;
				}
			}
			DataContext.TypeRef typeRef;
			if (!this.pendingTypesByTypeName.TryGetValue(typeName, out typeRef))
			{
				typeRef = new DataContext.TypeRef(addin, typeName);
				this.pendingTypesByTypeName[typeName] = typeRef;
			}
			else
			{
				typeRef.Addin = addin;
			}
			this.pendingTypes[itemName] = typeRef;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D835 File Offset: 0x0000BA35
		public void SetTypeInfo(DataItem item, Type type)
		{
			item.ItemData.Add(new DataValue("ctype", this.GetConfigurationDataType(type).Name));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000D858 File Offset: 0x0000BA58
		public void RegisterProperty(Type targetType, ItemProperty property)
		{
			if (!typeof(IExtendedDataItem).IsAssignableFrom(targetType))
			{
				throw new InvalidOperationException("The type '" + targetType + "' does not implement the IExtendedDataItem interface and cannot be extended with new properties");
			}
			ClassDataType classDataType = (ClassDataType)this.GetConfigurationDataType(targetType);
			classDataType.AddProperty(property);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000D8A1 File Offset: 0x0000BAA1
		public IEnumerable<DataType> DataTypes
		{
			get
			{
				return this.configurationTypes.Values;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		public virtual DataType GetConfigurationDataType(string typeName)
		{
			DataType configurationDataType;
			if (this.configurationTypesByName.TryGetValue(typeName, out configurationDataType))
			{
				return configurationDataType;
			}
			DataContext.TypeRef typeRef;
			if (this.pendingTypes.TryGetValue(typeName, out typeRef))
			{
				Type type = typeRef.Addin.GetType(typeRef.TypeName, true);
				configurationDataType = this.GetConfigurationDataType(type);
				typeRef.DataType = configurationDataType;
				return configurationDataType;
			}
			Type type2 = Type.GetType("System." + typeName);
			if (type2 != null)
			{
				return this.GetConfigurationDataType(type2);
			}
			return null;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000D928 File Offset: 0x0000BB28
		public DataType GetConfigurationDataType(Type type)
		{
			DataType result;
			lock (this.configurationTypes)
			{
				DataType dataType;
				if (!this.configurationTypes.TryGetValue(type, out dataType))
				{
					if (dataType != null)
					{
						return dataType;
					}
					dataType = this.CreateConfigurationDataType(type);
					this.configurationTypes[type] = dataType;
					dataType.SetContext(this);
					this.configurationTypesByName[dataType.Name] = dataType;
					DataContext.TypeRef typeRef;
					if (this.pendingTypesByTypeName.TryGetValue(type.FullName, out typeRef))
					{
						typeRef.DataType = dataType;
						this.pendingTypes.Remove(dataType.Name);
						for (DataContext.PropertyRef propertyRef = typeRef.Properties; propertyRef != null; propertyRef = propertyRef.Next)
						{
							this.RegisterProperty(propertyRef.Addin, typeRef.TypeName, propertyRef.Name, propertyRef.PropertyType, propertyRef.IsExternal, propertyRef.SkipEmpty);
						}
						typeRef.Properties = null;
					}
				}
				result = dataType;
			}
			return result;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DA28 File Offset: 0x0000BC28
		protected virtual DataType CreateConfigurationDataType(Type type)
		{
			if (type.IsEnum)
			{
				return new EnumDataType(type);
			}
			if (type.IsPrimitive)
			{
				return new PrimitiveDataType(type);
			}
			if (type == typeof(string))
			{
				return new StringDataType();
			}
			if (type == typeof(DateTime))
			{
				return new DateTimeDataType();
			}
			if (type == typeof(TimeSpan))
			{
				return new TimeSpanDataType();
			}
			if (type == typeof(FilePath))
			{
				return new FilePathDataType();
			}
			if (type == typeof(XmlElement))
			{
				return new XmlElementDataType();
			}
			if (DictionaryDataType.IsDictionaryType(type))
			{
				return new DictionaryDataType(type);
			}
			ICollectionHandler collectionHandler = this.GetCollectionHandler(type);
			if (collectionHandler != null)
			{
				return new CollectionDataType(type, collectionHandler);
			}
			return this.CreateClassDataType(type);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		protected virtual DataType CreateClassDataType(Type type)
		{
			return new ClassDataType(type);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		protected internal virtual ICollectionHandler GetCollectionHandler(Type type)
		{
			if (type.IsArray)
			{
				return new ArrayHandler(type);
			}
			if (type == typeof(ArrayList))
			{
				return ArrayListHandler.Instance;
			}
			return GenericCollectionHandler.CreateHandler(type);
		}

		// Token: 0x04000127 RID: 295
		private Dictionary<Type, DataType> configurationTypes = new Dictionary<Type, DataType>();

		// Token: 0x04000128 RID: 296
		private Dictionary<string, DataType> configurationTypesByName = new Dictionary<string, DataType>();

		// Token: 0x04000129 RID: 297
		private Dictionary<string, DataContext.TypeRef> pendingTypes = new Dictionary<string, DataContext.TypeRef>();

		// Token: 0x0400012A RID: 298
		private Dictionary<string, DataContext.TypeRef> pendingTypesByTypeName = new Dictionary<string, DataContext.TypeRef>();

		// Token: 0x0400012B RID: 299
		private ISerializationAttributeProvider attributeProvider;

		// Token: 0x02000067 RID: 103
		private class TypeRef
		{
			// Token: 0x06000378 RID: 888 RVA: 0x0000DB2B File Offset: 0x0000BD2B
			public TypeRef(RuntimeAddin addin, string typeName)
			{
				this.TypeName = typeName;
				this.Addin = addin;
			}

			// Token: 0x0400012C RID: 300
			public string TypeName;

			// Token: 0x0400012D RID: 301
			public RuntimeAddin Addin;

			// Token: 0x0400012E RID: 302
			public DataContext.PropertyRef Properties;

			// Token: 0x0400012F RID: 303
			public DataType DataType;
		}

		// Token: 0x02000068 RID: 104
		private class PropertyRef
		{
			// Token: 0x06000379 RID: 889 RVA: 0x0000DB41 File Offset: 0x0000BD41
			public PropertyRef(RuntimeAddin addin, string targetType, string name, string propertyType, bool isExternal, bool skipEmpty)
			{
				this.Addin = addin;
				this.TargetType = targetType;
				this.Name = name;
				this.PropertyType = propertyType;
				this.IsExternal = isExternal;
				this.SkipEmpty = skipEmpty;
			}

			// Token: 0x04000130 RID: 304
			public string TargetType;

			// Token: 0x04000131 RID: 305
			public string Name;

			// Token: 0x04000132 RID: 306
			public string PropertyType;

			// Token: 0x04000133 RID: 307
			public bool IsExternal;

			// Token: 0x04000134 RID: 308
			public bool SkipEmpty;

			// Token: 0x04000135 RID: 309
			public DataContext.PropertyRef Next;

			// Token: 0x04000136 RID: 310
			public RuntimeAddin Addin;
		}
	}
}
