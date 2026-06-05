using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000060 RID: 96
	public class ClassDataType : DataType
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000B76C File Offset: 0x0000996C
		public ClassDataType(Type propType) : base(propType)
		{
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000B78B File Offset: 0x0000998B
		public override bool IsSimpleType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000B78E File Offset: 0x0000998E
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000B791 File Offset: 0x00009991
		public override bool CanReuseInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000B794 File Offset: 0x00009994
		protected override void Initialize()
		{
			IDataItemAttribute dataItemAttribute = (IDataItemAttribute)base.Context.AttributeProvider.GetCustomAttribute(base.ValueType, typeof(IDataItemAttribute), false);
			if (dataItemAttribute != null)
			{
				if (!string.IsNullOrEmpty(dataItemAttribute.Name))
				{
					base.Name = dataItemAttribute.Name;
				}
				if (dataItemAttribute.FallbackType != null)
				{
					this.fallbackType = dataItemAttribute.FallbackType;
					if (!typeof(IExtendedDataItem).IsAssignableFrom(this.fallbackType))
					{
						throw new InvalidOperationException("Fallback type '" + this.fallbackType + "' must implement IExtendedDataItem");
					}
					if (!base.ValueType.IsAssignableFrom(this.fallbackType))
					{
						throw new InvalidOperationException(string.Concat(new object[]
						{
							"Fallback type '",
							this.fallbackType,
							"' must be a subclass of '",
							base.ValueType,
							"'"
						}));
					}
				}
			}
			object[] customAttributes = Attribute.GetCustomAttributes(base.ValueType, typeof(DataIncludeAttribute), true);
			foreach (DataIncludeAttribute dataIncludeAttribute in customAttributes)
			{
				base.Context.IncludeType(dataIncludeAttribute.Type);
			}
			if (base.ValueType.BaseType != null)
			{
				ClassDataType classDataType = (ClassDataType)base.Context.GetConfigurationDataType(base.ValueType.BaseType);
				classDataType.AddSubtype(this);
				int num = 0;
				foreach (object obj in classDataType.Properties)
				{
					ItemProperty itemProperty = (ItemProperty)obj;
					this.properties.Add(itemProperty.Name, itemProperty);
					this.sortedPoperties.Insert(num++, itemProperty);
				}
				if (this.fallbackType == null && classDataType.fallbackType != null)
				{
					this.fallbackType = classDataType.fallbackType;
				}
			}
			foreach (Type type in base.ValueType.GetInterfaces())
			{
				ClassDataType classDataType2 = (ClassDataType)base.Context.GetConfigurationDataType(type);
				classDataType2.AddSubtype(this);
			}
			MemberInfo[] members = base.ValueType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MemberInfo memberInfo in members)
			{
				if ((memberInfo is FieldInfo || memberInfo is PropertyInfo) && memberInfo.DeclaringType == base.ValueType)
				{
					Type memberType = (memberInfo is FieldInfo) ? ((FieldInfo)memberInfo).FieldType : ((PropertyInfo)memberInfo).PropertyType;
					this.AddProperty(memberInfo, memberInfo.Name, memberType);
				}
			}
			foreach (ItemMember itemMember in base.Context.AttributeProvider.GetItemMembers(base.ValueType))
			{
				this.AddProperty(itemMember, itemMember.Name, itemMember.Type);
			}
			if (this.fallbackType != null)
			{
				base.Context.IncludeType(this.fallbackType);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		private void AddProperty(object member, string name, Type memberType)
		{
			object[] customAttributes = base.Context.AttributeProvider.GetCustomAttributes(member, typeof(Attribute), false);
			ItemPropertyAttribute itemPropertyAttribute = base.FindPropertyAttribute(customAttributes, "");
			if (itemPropertyAttribute == null)
			{
				return;
			}
			ItemProperty itemProperty = new ItemProperty();
			itemProperty.Name = ((itemPropertyAttribute.Name != null) ? itemPropertyAttribute.Name : name);
			itemProperty.ExpandedCollection = base.Context.AttributeProvider.IsDefined(member, typeof(ExpandedCollectionAttribute), true);
			itemProperty.DefaultValue = itemPropertyAttribute.DefaultValue;
			itemProperty.IsExternal = itemPropertyAttribute.IsExternal;
			itemProperty.SkipEmpty = itemPropertyAttribute.SkipEmpty;
			itemProperty.ReadOnly = itemPropertyAttribute.ReadOnly;
			itemProperty.WriteOnly = itemPropertyAttribute.WriteOnly;
			if (itemProperty.ExpandedCollection)
			{
				ICollectionHandler collectionHandler = base.Context.GetCollectionHandler(memberType);
				if (collectionHandler == null)
				{
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"ExpandedCollectionAttribute can't be applied to property '",
						itemProperty.Name,
						"' in type '",
						base.ValueType,
						"' becuase it is not a valid collection."
					}));
				}
				memberType = collectionHandler.GetItemType();
				itemProperty.ExpandedCollectionHandler = collectionHandler;
			}
			if (itemPropertyAttribute.ValueType != null)
			{
				itemProperty.PropertyType = itemPropertyAttribute.ValueType;
			}
			else
			{
				itemProperty.PropertyType = memberType;
			}
			if (itemPropertyAttribute.SerializationDataType != null)
			{
				try
				{
					itemProperty.DataType = (DataType)Activator.CreateInstance(itemPropertyAttribute.SerializationDataType, new object[]
					{
						itemProperty.PropertyType
					});
				}
				catch (MissingMethodException innerException)
				{
					throw new InvalidOperationException("Constructor not found for custom data type: " + itemPropertyAttribute.SerializationDataType.Name + " (Type propertyType);", innerException);
				}
			}
			if (member is MemberInfo)
			{
				itemProperty.Member = (MemberInfo)member;
				this.AddProperty(itemProperty);
			}
			else
			{
				itemProperty.InitValue = ((ItemMember)member).InitValue;
				this.AddProperty(itemProperty, ((ItemMember)member).InsertBefore);
			}
			itemProperty.Initialize(customAttributes, "");
			if (itemProperty.ExpandedCollection && itemProperty.DataType.IsSimpleType)
			{
				throw new InvalidOperationException("ExpandedCollectionAttribute is not allowed in collections of simple types");
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000BD04 File Offset: 0x00009F04
		protected internal override object GetMapData(object[] attributes, string scope)
		{
			ItemPropertyAttribute itemPropertyAttribute = base.FindPropertyAttribute(attributes, scope);
			if (itemPropertyAttribute != null)
			{
				return itemPropertyAttribute.FallbackType;
			}
			return null;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BD25 File Offset: 0x00009F25
		private void AddSubtype(ClassDataType subtype)
		{
			if (this.subtypes == null)
			{
				this.subtypes = new List<ClassDataType>();
			}
			this.subtypes.Add(subtype);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000BD46 File Offset: 0x00009F46
		private ICollection Properties
		{
			get
			{
				return this.sortedPoperties;
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000BD4E File Offset: 0x00009F4E
		public void AddProperty(ItemProperty prop)
		{
			this.AddProperty(prop, null);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000BD58 File Offset: 0x00009F58
		private void AddProperty(ItemProperty prop, string insertBefore)
		{
			if (!prop.IsNested)
			{
				using (List<ItemProperty>.Enumerator enumerator = this.sortedPoperties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemProperty itemProperty = enumerator.Current;
						if (itemProperty.IsNested && itemProperty.NameList[0] == prop.Name)
						{
							throw this.CreateNestedConflictException(prop, itemProperty);
						}
					}
					goto IL_83;
				}
			}
			ItemProperty itemProperty2 = this.properties[prop.NameList[0]] as ItemProperty;
			if (itemProperty2 != null)
			{
				throw this.CreateNestedConflictException(prop, itemProperty2);
			}
			IL_83:
			prop.SetContext(base.Context);
			if (this.properties.ContainsKey(prop.Name))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Duplicate property '",
					prop.Name,
					"' in class '",
					base.ValueType
				}));
			}
			this.properties.Add(prop.Name, prop);
			if (insertBefore != null)
			{
				bool flag = false;
				for (int i = 0; i < this.sortedPoperties.Count; i++)
				{
					ItemProperty itemProperty3 = this.sortedPoperties[i];
					if (itemProperty3.MemberName == insertBefore)
					{
						this.sortedPoperties.Insert(i, prop);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.sortedPoperties.Add(prop);
				}
			}
			else if (prop.Unsorted)
			{
				int index = this.sortedPoperties.Count;
				for (int j = 0; j < this.sortedPoperties.Count; j++)
				{
					ItemProperty itemProperty4 = this.sortedPoperties[j];
					if (itemProperty4.Unsorted && prop.Name.CompareTo(itemProperty4.Name) < 0)
					{
						index = j;
						break;
					}
				}
				this.sortedPoperties.Insert(index, prop);
			}
			else
			{
				this.sortedPoperties.Add(prop);
			}
			if (this.subtypes != null && this.subtypes.Count > 0)
			{
				foreach (ClassDataType classDataType in this.subtypes)
				{
					classDataType.AddProperty(prop);
				}
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000BF9C File Offset: 0x0000A19C
		public void RemoveProperty(string name)
		{
			ItemProperty itemProperty = (ItemProperty)this.properties[name];
			if (itemProperty == null)
			{
				return;
			}
			this.properties.Remove(name);
			this.sortedPoperties.Remove(itemProperty);
			if (this.subtypes != null && this.subtypes.Count > 0)
			{
				foreach (ClassDataType classDataType in this.subtypes)
				{
					classDataType.RemoveProperty(name);
				}
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public IEnumerable<ItemProperty> GetProperties(SerializationContext serCtx, object instance)
		{
			foreach (ItemProperty prop in this.sortedPoperties)
			{
				if (serCtx.Serializer.CanHandleProperty(prop, serCtx, instance))
				{
					yield return prop;
				}
			}
			yield break;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C224 File Offset: 0x0000A424
		private Exception CreateNestedConflictException(ItemProperty p1, ItemProperty p2)
		{
			return new InvalidOperationException(string.Concat(new string[]
			{
				"There is a conflict between the properties '",
				p1.Name,
				"' and '",
				p2.Name,
				"'. Nested element properties can't be mixed with normal element properties."
			}));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000C270 File Offset: 0x0000A470
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object obj)
		{
			string text = null;
			if (obj.GetType() != base.ValueType)
			{
				if (obj is IExtendedDataItem)
				{
					text = (string)((IExtendedDataItem)obj).ExtendedProperties["__raw_ctype"];
				}
				if (text == null)
				{
					DataType configurationDataType = base.Context.GetConfigurationDataType(obj.GetType());
					DataNode dataNode = configurationDataType.Serialize(serCtx, mapData, obj);
					DataItem dataItem = dataNode as DataItem;
					if (dataItem == null)
					{
						dataItem = new DataItem();
						dataNode.Name = "Value";
						dataItem.ItemData.Add(dataNode);
					}
					dataItem.ItemData.Add(new DataValue("ctype", configurationDataType.Name));
					dataItem.Name = base.Name;
					return dataItem;
				}
			}
			DataItem dataItem2 = new DataItem();
			dataItem2.Name = base.Name;
			ICustomDataItem customDataItem = base.Context.AttributeProvider.GetCustomDataItem(obj);
			if (customDataItem != null)
			{
				ClassTypeHandler handler = new ClassTypeHandler(serCtx, this);
				dataItem2.ItemData = customDataItem.Serialize(handler);
			}
			else
			{
				dataItem2.ItemData = this.Serialize(serCtx, obj);
			}
			if (text != null)
			{
				dataItem2.ItemData.Add(new DataValue("ctype", text));
			}
			return dataItem2;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C398 File Offset: 0x0000A598
		internal DataCollection Serialize(SerializationContext serCtx, object obj)
		{
			DataCollection dataCollection = new DataCollection();
			foreach (object obj2 in this.Properties)
			{
				ItemProperty itemProperty = (ItemProperty)obj2;
				if (!itemProperty.ReadOnly && itemProperty.CanSerialize(serCtx, obj))
				{
					object value = itemProperty.GetValue(obj);
					if (value != null && (serCtx.IsDefaultValueSerializationForced(itemProperty) || !value.Equals(itemProperty.DefaultValue)))
					{
						DataCollection dataCollection2 = dataCollection;
						if (itemProperty.IsNested)
						{
							dataCollection2 = this.GetNestedCollection(dataCollection2, itemProperty.NameList, 0);
						}
						if (itemProperty.ExpandedCollection)
						{
							ICollectionHandler expandedCollectionHandler = itemProperty.ExpandedCollectionHandler;
							object initialPosition = expandedCollectionHandler.GetInitialPosition(value);
							while (expandedCollectionHandler.MoveNextItem(value, ref initialPosition))
							{
								object currentItem = expandedCollectionHandler.GetCurrentItem(value, initialPosition);
								if (currentItem != null)
								{
									DataNode dataNode = itemProperty.Serialize(serCtx, obj, currentItem);
									dataNode.Name = itemProperty.SingleName;
									dataCollection2.Add(dataNode);
								}
							}
						}
						else
						{
							DataNode dataNode2 = itemProperty.Serialize(serCtx, obj, value);
							if (dataNode2 != null)
							{
								dataCollection2.Add(dataNode2);
							}
						}
					}
				}
			}
			if (obj is IExtendedDataItem)
			{
				DataItem dataItem = (DataItem)((IExtendedDataItem)obj).ExtendedProperties["__raw_data"];
				if (dataItem != null)
				{
					dataCollection.Merge(dataItem.ItemData);
				}
			}
			return dataCollection;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000C500 File Offset: 0x0000A700
		private DataCollection GetNestedCollection(DataCollection col, string[] nameList, int pos)
		{
			if (pos == nameList.Length - 1)
			{
				return col;
			}
			DataItem dataItem = col[nameList[pos]] as DataItem;
			if (dataItem == null)
			{
				dataItem = new DataItem();
				dataItem.Name = nameList[pos];
				col.Add(dataItem);
			}
			return this.GetNestedCollection(dataItem.ItemData, nameList, pos + 1);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000C550 File Offset: 0x0000A750
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			DataItem dataItem = data as DataItem;
			if (dataItem == null)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Invalid value found for type '",
					base.Name,
					"' ",
					data
				}));
			}
			DataValue dataValue = dataItem["ctype"] as DataValue;
			if (dataValue != null && dataValue.Value != base.Name)
			{
				bool flag;
				DataType dataType = this.FindDerivedType(dataValue.Value, mapData, out flag);
				if (flag)
				{
					dataItem.ItemData.Remove(dataValue);
				}
				if (dataType != null)
				{
					DataNode dataNode = data;
					if (dataType.IsSimpleType)
					{
						dataNode = dataItem.ItemData["Value"];
						if (dataNode == null)
						{
							throw new InvalidOperationException("Value node not found");
						}
					}
					object obj = dataType.Deserialize(serCtx, mapData, dataNode);
					if (flag && obj is IExtendedDataItem)
					{
						((IExtendedDataItem)obj).ExtendedProperties["__raw_ctype"] = dataValue.Value;
					}
					return obj;
				}
				throw new InvalidOperationException("Type not found: " + dataValue.Value);
			}
			else
			{
				ConstructorInfo constructor = base.ValueType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				if (constructor == null)
				{
					throw new InvalidOperationException("Default constructor not found for type '" + base.ValueType + "'");
				}
				object obj2 = constructor.Invoke(null);
				base.Deserialize(serCtx, null, dataItem, obj2);
				return obj2;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C6B8 File Offset: 0x0000A8B8
		protected internal override object OnCreateInstance(SerializationContext serCtx, DataNode data)
		{
			DataItem dataItem = data as DataItem;
			if (dataItem == null)
			{
				throw new InvalidOperationException("Invalid value found for type '" + base.Name + "'");
			}
			DataValue dataValue = dataItem["ctype"] as DataValue;
			if (dataValue != null && dataValue.Value != base.Name)
			{
				bool flag;
				DataType dataType = this.FindDerivedType(dataValue.Value, null, out flag);
				if (flag)
				{
					dataItem.ItemData.Remove(dataValue);
				}
				if (dataType != null)
				{
					object obj = dataType.CreateInstance(serCtx, data);
					if (flag && obj is IExtendedDataItem)
					{
						((IExtendedDataItem)obj).ExtendedProperties["__raw_ctype"] = dataValue;
					}
					return obj;
				}
				throw new InvalidOperationException("Type not found: " + dataValue.Value);
			}
			else
			{
				ConstructorInfo constructor = base.ValueType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				if (constructor == null)
				{
					throw new InvalidOperationException("Default constructor not found for type '" + base.ValueType + "'");
				}
				return constructor.Invoke(null);
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		protected internal override void OnDeserialize(SerializationContext serCtx, object mapData, DataNode data, object obj)
		{
			DataItem dataItem = (DataItem)data;
			ICustomDataItem customDataItem = base.Context.AttributeProvider.GetCustomDataItem(obj);
			if (customDataItem != null)
			{
				ClassTypeHandler handler = new ClassTypeHandler(serCtx, this);
				customDataItem.Deserialize(handler, dataItem.ItemData);
				return;
			}
			this.DeserializeNoCustom(serCtx, obj, dataItem.ItemData);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000C80C File Offset: 0x0000AA0C
		internal void DeserializeNoCustom(SerializationContext serCtx, object obj, DataCollection itemData)
		{
			foreach (object obj2 in this.Properties)
			{
				ItemProperty itemProperty = (ItemProperty)obj2;
				if (!itemProperty.CanDeserialize(serCtx, obj) && itemProperty.DefaultValue != null)
				{
					itemProperty.SetValue(obj, itemProperty.DefaultValue);
				}
			}
			DataItem dataItem = (obj is IExtendedDataItem) ? new DataItem() : null;
			this.Deserialize(serCtx, obj, itemData, dataItem, "");
			if (dataItem != null && dataItem.HasItemData)
			{
				((IExtendedDataItem)obj).ExtendedProperties["__raw_data"] = dataItem;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C8C0 File Offset: 0x0000AAC0
		private void Deserialize(SerializationContext serCtx, object obj, DataCollection itemData, DataItem ukwnDataRoot, string baseName)
		{
			Hashtable hashtable = null;
			foreach (object obj2 in itemData)
			{
				DataNode dataNode = (DataNode)obj2;
				ItemProperty itemProperty = (ItemProperty)this.properties[baseName + dataNode.Name];
				if (itemProperty == null)
				{
					if (dataNode is DataItem)
					{
						DataItem dataItem = new DataItem();
						dataItem.Name = dataNode.Name;
						dataItem.UniqueNames = ((DataItem)dataNode).UniqueNames;
						if (ukwnDataRoot != null)
						{
							ukwnDataRoot.ItemData.Add(dataItem);
						}
						this.Deserialize(serCtx, obj, ((DataItem)dataNode).ItemData, dataItem, baseName + dataNode.Name + "/");
						if (ukwnDataRoot != null && !dataItem.HasItemData)
						{
							ukwnDataRoot.ItemData.Remove(dataItem);
						}
					}
					else if (obj is IExtendedDataItem && (dataNode.Name != "ctype" || baseName.Length > 0))
					{
						ukwnDataRoot.ItemData.Add(dataNode);
					}
				}
				else if (!itemProperty.WriteOnly && itemProperty.CanDeserialize(serCtx, obj))
				{
					try
					{
						if (itemProperty.ExpandedCollection)
						{
							ICollectionHandler expandedCollectionHandler = itemProperty.ExpandedCollectionHandler;
							if (hashtable == null)
							{
								hashtable = new Hashtable();
							}
							object value2;
							object value;
							if (!hashtable.ContainsKey(itemProperty))
							{
								value = expandedCollectionHandler.CreateCollection(out value2, -1);
							}
							else
							{
								value2 = hashtable[itemProperty];
								value = itemProperty.GetValue(obj);
							}
							expandedCollectionHandler.AddItem(ref value, ref value2, itemProperty.Deserialize(serCtx, obj, dataNode));
							hashtable[itemProperty] = value2;
							itemProperty.SetValue(obj, value);
						}
						else if (itemProperty.HasSetter && itemProperty.DataType.CanCreateInstance)
						{
							itemProperty.SetValue(obj, itemProperty.Deserialize(serCtx, obj, dataNode));
						}
						else
						{
							if (!itemProperty.DataType.CanReuseInstance)
							{
								throw new InvalidOperationException("The property does not have a setter.");
							}
							object value3 = itemProperty.GetValue(obj);
							if (value3 == null)
							{
								if (itemProperty.HasSetter)
								{
									throw new InvalidOperationException(string.Concat(new object[]
									{
										"The property '",
										itemProperty.Name,
										"' is null and a new instance of '",
										itemProperty.PropertyType,
										"' can't be created."
									}));
								}
								throw new InvalidOperationException("The property '" + itemProperty.Name + "' is null and it does not have a setter.");
							}
							else
							{
								itemProperty.Deserialize(serCtx, obj, dataNode, value3);
							}
						}
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(string.Concat(new string[]
						{
							"Could not set property '",
							itemProperty.Name,
							"' in type '",
							base.Name,
							"'"
						}), innerException);
					}
				}
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		private DataType FindDerivedType(string name, object mapData, out bool isFallbackType)
		{
			isFallbackType = false;
			if (this.subtypes != null)
			{
				foreach (ClassDataType classDataType in new List<ClassDataType>(this.subtypes))
				{
					if (classDataType.Name == name)
					{
						return classDataType;
					}
					bool flag;
					DataType dataType = classDataType.FindDerivedType(name, null, out flag);
					if (dataType != null && !flag)
					{
						isFallbackType = false;
						return dataType;
					}
				}
			}
			DataType configurationDataType = base.Context.GetConfigurationDataType(name);
			if (configurationDataType != null && base.ValueType.IsAssignableFrom(configurationDataType.ValueType))
			{
				return configurationDataType;
			}
			if (mapData != null)
			{
				isFallbackType = true;
				return base.Context.GetConfigurationDataType((Type)mapData);
			}
			if (this.fallbackType != null)
			{
				isFallbackType = true;
				return base.Context.GetConfigurationDataType(this.fallbackType);
			}
			return null;
		}

		// Token: 0x0400011B RID: 283
		private Hashtable properties = new Hashtable();

		// Token: 0x0400011C RID: 284
		private List<ItemProperty> sortedPoperties = new List<ItemProperty>();

		// Token: 0x0400011D RID: 285
		private List<ClassDataType> subtypes;

		// Token: 0x0400011E RID: 286
		private Type fallbackType;
	}
}
