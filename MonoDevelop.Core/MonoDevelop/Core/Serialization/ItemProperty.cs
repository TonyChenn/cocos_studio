using System;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200007F RID: 127
	public class ItemProperty
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E927 File Offset: 0x0000CB27
		public ItemProperty()
		{
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E92F File Offset: 0x0000CB2F
		public ItemProperty(string name, Type propType)
		{
			this.name = name;
			this.propType = propType;
			this.BuildNameList();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E94C File Offset: 0x0000CB4C
		private void BuildNameList()
		{
			if (this.name.IndexOf('/') != -1)
			{
				this.nameList = this.name.Split(new char[]
				{
					'/'
				});
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E987 File Offset: 0x0000CB87
		internal void SetContext(DataContext ctx)
		{
			this.ctx = ctx;
			if (this.dataType == null)
			{
				if (this.propType == null)
				{
					throw new InvalidOperationException("Property type not specified");
				}
				this.dataType = ctx.GetConfigurationDataType(this.propType);
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		internal void Initialize(object[] attributes, string scope)
		{
			this.mapData = this.dataType.GetMapData(attributes, scope);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		internal MemberInfo Member
		{
			get
			{
				return this.member;
			}
			set
			{
				this.CheckReadOnly();
				this.member = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000E9EF File Offset: 0x0000CBEF
		internal virtual string MemberName
		{
			get
			{
				if (!(this.member != null))
				{
					return null;
				}
				return this.member.Name;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000EA14 File Offset: 0x0000CC14
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.CheckReadOnly();
				this.name = value;
				this.BuildNameList();
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0000EA8C File Offset: 0x0000CC8C
		public object DefaultValue
		{
			get
			{
				if (this.defaultValue != null && this.propType != null && this.propType.IsEnum && !this.propType.IsInstanceOfType(this.defaultValue))
				{
					this.defaultValue = Enum.ToObject(this.propType, this.defaultValue);
				}
				return this.defaultValue;
			}
			set
			{
				this.CheckReadOnly();
				this.defaultValue = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000EA9B File Offset: 0x0000CC9B
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000EAA3 File Offset: 0x0000CCA3
		public Type PropertyType
		{
			get
			{
				return this.propType;
			}
			set
			{
				this.CheckReadOnly();
				this.propType = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000EAB2 File Offset: 0x0000CCB2
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000EABA File Offset: 0x0000CCBA
		public bool ExpandedCollection
		{
			get
			{
				return this.expandedCollection;
			}
			set
			{
				this.expandedCollection = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000EAC3 File Offset: 0x0000CCC3
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000EACB File Offset: 0x0000CCCB
		internal ICollectionHandler ExpandedCollectionHandler
		{
			get
			{
				return this.expandedCollectionHandler;
			}
			set
			{
				this.expandedCollectionHandler = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000EADC File Offset: 0x0000CCDC
		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000EAE5 File Offset: 0x0000CCE5
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000EAED File Offset: 0x0000CCED
		public bool WriteOnly
		{
			get
			{
				return this.writeOnly;
			}
			set
			{
				this.writeOnly = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000EAF6 File Offset: 0x0000CCF6
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000EAFE File Offset: 0x0000CCFE
		public DataType DataType
		{
			get
			{
				return this.dataType;
			}
			set
			{
				this.CheckReadOnly();
				this.dataType = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000EB0D File Offset: 0x0000CD0D
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000EB15 File Offset: 0x0000CD15
		public bool SkipEmpty { get; set; }

		// Token: 0x06000413 RID: 1043 RVA: 0x0000EB1E File Offset: 0x0000CD1E
		public bool IsExtendedProperty(Type forType)
		{
			return this.member == null || !this.member.DeclaringType.IsAssignableFrom(forType);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000EB44 File Offset: 0x0000CD44
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0000EB76 File Offset: 0x0000CD76
		public virtual object[] CustomAttributes
		{
			get
			{
				if (this.customAttributes != null)
				{
					return this.customAttributes;
				}
				if (this.member != null)
				{
					return this.member.GetCustomAttributes(true);
				}
				return new object[0];
			}
			set
			{
				this.customAttributes = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000EB7F File Offset: 0x0000CD7F
		internal string[] NameList
		{
			get
			{
				return this.nameList;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000EB87 File Offset: 0x0000CD87
		internal bool IsNested
		{
			get
			{
				return this.nameList != null;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000EB95 File Offset: 0x0000CD95
		internal string SingleName
		{
			get
			{
				if (this.nameList == null)
				{
					return this.name;
				}
				return this.nameList[this.nameList.Length - 1];
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000EBB7 File Offset: 0x0000CDB7
		internal DataContext Context
		{
			get
			{
				return this.ctx;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
		internal virtual object GetValue(object obj)
		{
			if (this.member != null)
			{
				FieldInfo fieldInfo = this.member as FieldInfo;
				if (fieldInfo != null)
				{
					return fieldInfo.GetValue(obj);
				}
				return ((PropertyInfo)this.member).GetValue(obj, null);
			}
			else if (obj is IExtendedDataItem)
			{
				IExtendedDataItem extendedDataItem = (IExtendedDataItem)obj;
				if (this.initValue == null)
				{
					return extendedDataItem.ExtendedProperties[this.Name];
				}
				if (!extendedDataItem.ExtendedProperties.Contains(this.Name))
				{
					return this.initValue;
				}
				return extendedDataItem.ExtendedProperties[this.Name];
			}
			else
			{
				if (this.initValue != null)
				{
					return this.initValue;
				}
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Invalid object property: ",
					obj.GetType(),
					".",
					this.Name
				}));
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		internal virtual void SetValue(object obj, object value)
		{
			if (this.member != null)
			{
				FieldInfo fieldInfo = this.member as FieldInfo;
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(obj, value);
					return;
				}
				PropertyInfo propertyInfo = this.member as PropertyInfo;
				propertyInfo.SetValue(obj, value, null);
				return;
			}
			else
			{
				if (obj is IExtendedDataItem)
				{
					((IExtendedDataItem)obj).ExtendedProperties[this.Name] = value;
					return;
				}
				if (this.initValue == null)
				{
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"Invalid object property: ",
						obj.GetType(),
						".",
						this.Name
					}));
				}
				return;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000ED50 File Offset: 0x0000CF50
		internal bool HasSetter
		{
			get
			{
				return this.member == null || this.member is FieldInfo || (this.member is PropertyInfo && ((PropertyInfo)this.member).CanWrite);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0000ED8E File Offset: 0x0000CF8E
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0000ED96 File Offset: 0x0000CF96
		internal bool Unsorted
		{
			get
			{
				return this.unsorted;
			}
			set
			{
				this.unsorted = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000ED9F File Offset: 0x0000CF9F
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000EDA7 File Offset: 0x0000CFA7
		public bool IsExternal
		{
			get
			{
				return this.external;
			}
			set
			{
				this.external = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000EDB0 File Offset: 0x0000CFB0
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public object InitValue
		{
			get
			{
				return this.initValue;
			}
			set
			{
				this.initValue = value;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000EDC1 File Offset: 0x0000CFC1
		internal bool CanSerialize(SerializationContext serCtx, object instance)
		{
			return serCtx.Serializer.CanSerializeProperty(this, serCtx, instance);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000EDD1 File Offset: 0x0000CFD1
		internal DataNode Serialize(SerializationContext serCtx, object instance, object value)
		{
			return serCtx.Serializer.OnSerializeProperty(this, serCtx, instance, value);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000EDE4 File Offset: 0x0000CFE4
		internal DataNode OnSerialize(SerializationContext serCtx, object value)
		{
			DataNode dataNode = this.dataType.Serialize(serCtx, this.mapData, value);
			if (dataNode != null)
			{
				if (dataNode is DataItem && (this.DataType is CollectionDataType || this.SkipEmpty))
				{
					DataItem dataItem = (DataItem)dataNode;
					if (!dataItem.HasItemData)
					{
						return null;
					}
					if (dataItem.ItemData.Count == 1 && dataItem.ItemData["ctype"] != null)
					{
						return null;
					}
				}
				dataNode.Name = this.SingleName;
			}
			return dataNode;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000EE65 File Offset: 0x0000D065
		internal bool CanDeserialize(SerializationContext serCtx, object instance)
		{
			return serCtx.Serializer.CanDeserializeProperty(this, serCtx, instance);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000EE75 File Offset: 0x0000D075
		internal object Deserialize(SerializationContext serCtx, object instance, DataNode data)
		{
			return serCtx.Serializer.OnDeserializeProperty(this, serCtx, instance, data);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000EE86 File Offset: 0x0000D086
		internal object OnDeserialize(SerializationContext serCtx, DataNode data)
		{
			return this.dataType.Deserialize(serCtx, this.mapData, data);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000EE9B File Offset: 0x0000D09B
		internal void Deserialize(SerializationContext serCtx, object instance, DataNode data, object valueInstance)
		{
			serCtx.Serializer.OnDeserializeProperty(this, serCtx, instance, data, valueInstance);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		internal void OnDeserialize(SerializationContext serCtx, DataNode data, object valueInstance)
		{
			this.dataType.Deserialize(serCtx, this.mapData, data, valueInstance);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		private void CheckReadOnly()
		{
			if (this.ctx != null)
			{
				throw new InvalidOperationException("Property can't be modified, it is already bound to a configuration context");
			}
		}

		// Token: 0x04000158 RID: 344
		private string name;

		// Token: 0x04000159 RID: 345
		private MemberInfo member;

		// Token: 0x0400015A RID: 346
		private DataType dataType;

		// Token: 0x0400015B RID: 347
		private Type propType;

		// Token: 0x0400015C RID: 348
		private object defaultValue;

		// Token: 0x0400015D RID: 349
		private DataContext ctx;

		// Token: 0x0400015E RID: 350
		private object mapData;

		// Token: 0x0400015F RID: 351
		private bool expandedCollection;

		// Token: 0x04000160 RID: 352
		private ICollectionHandler expandedCollectionHandler;

		// Token: 0x04000161 RID: 353
		private string[] nameList;

		// Token: 0x04000162 RID: 354
		private bool readOnly;

		// Token: 0x04000163 RID: 355
		private bool writeOnly;

		// Token: 0x04000164 RID: 356
		private bool unsorted;

		// Token: 0x04000165 RID: 357
		private bool external;

		// Token: 0x04000166 RID: 358
		private object initValue;

		// Token: 0x04000167 RID: 359
		private object[] customAttributes;
	}
}
