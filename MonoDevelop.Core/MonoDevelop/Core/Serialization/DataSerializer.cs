using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006E RID: 110
	public class DataSerializer
	{
		// Token: 0x06000392 RID: 914 RVA: 0x0000DD48 File Offset: 0x0000BF48
		public DataSerializer(DataContext ctx)
		{
			this.dataContext = ctx;
			this.serializationContext = ctx.CreateSerializationContext();
			this.serializationContext.Serializer = this;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000DD6F File Offset: 0x0000BF6F
		public DataSerializer(DataContext ctx, string baseFile)
		{
			this.dataContext = ctx;
			this.serializationContext = ctx.CreateSerializationContext();
			this.serializationContext.BaseFile = baseFile;
			this.serializationContext.Serializer = this;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000DDA2 File Offset: 0x0000BFA2
		public SerializationContext SerializationContext
		{
			get
			{
				return this.serializationContext;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000DDAA File Offset: 0x0000BFAA
		public DataContext DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000DDB2 File Offset: 0x0000BFB2
		public DataNode Serialize(object obj)
		{
			return this.dataContext.SaveConfigurationData(this.serializationContext, obj, null);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000DDC7 File Offset: 0x0000BFC7
		public DataNode Serialize(object obj, Type type)
		{
			return this.dataContext.SaveConfigurationData(this.serializationContext, obj, type);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		public object Deserialize(Type type, DataNode data)
		{
			return this.dataContext.LoadConfigurationData(this.serializationContext, type, data);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000DDF1 File Offset: 0x0000BFF1
		public void Deserialize(object obj, DataItem data)
		{
			this.dataContext.SetConfigurationItemData(this.serializationContext, obj, data);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000DE06 File Offset: 0x0000C006
		public object CreateInstance(Type type, DataItem data)
		{
			return this.dataContext.CreateConfigurationData(this.serializationContext, type, data);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000DE1B File Offset: 0x0000C01B
		public IEnumerable<ItemProperty> GetProperties(object instance)
		{
			return this.dataContext.GetProperties(this.serializationContext, instance);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000DE2F File Offset: 0x0000C02F
		protected internal virtual DataNode OnSerialize(DataType dataType, SerializationContext serCtx, object mapData, object value)
		{
			return dataType.OnSerialize(serCtx, mapData, value);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000DE3B File Offset: 0x0000C03B
		protected internal virtual object OnDeserialize(DataType dataType, SerializationContext serCtx, object mapData, DataNode data)
		{
			return dataType.OnDeserialize(serCtx, mapData, data);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000DE47 File Offset: 0x0000C047
		protected internal virtual void OnDeserialize(DataType dataType, SerializationContext serCtx, object mapData, DataNode data, object valueInstance)
		{
			dataType.OnDeserialize(serCtx, mapData, data, valueInstance);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000DE55 File Offset: 0x0000C055
		protected internal virtual object OnCreateInstance(DataType dataType, SerializationContext serCtx, DataNode data)
		{
			return dataType.OnCreateInstance(serCtx, data);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000DE5F File Offset: 0x0000C05F
		protected internal virtual DataNode OnSerializeProperty(ItemProperty prop, SerializationContext serCtx, object instance, object value)
		{
			return prop.OnSerialize(serCtx, value);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000DE6A File Offset: 0x0000C06A
		protected internal virtual object OnDeserializeProperty(ItemProperty prop, SerializationContext serCtx, object instance, DataNode data)
		{
			return prop.OnDeserialize(serCtx, data);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000DE75 File Offset: 0x0000C075
		protected internal virtual void OnDeserializeProperty(ItemProperty prop, SerializationContext serCtx, object instance, DataNode data, object valueInstance)
		{
			prop.OnDeserialize(serCtx, data, valueInstance);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000DE82 File Offset: 0x0000C082
		protected internal virtual bool CanSerializeProperty(ItemProperty property, SerializationContext serCtx, object instance)
		{
			return this.CanHandleProperty(property, serCtx, instance);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000DE8D File Offset: 0x0000C08D
		protected internal virtual bool CanDeserializeProperty(ItemProperty property, SerializationContext serCtx, object instance)
		{
			return this.CanHandleProperty(property, serCtx, instance);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000DE98 File Offset: 0x0000C098
		protected internal virtual bool CanHandleProperty(ItemProperty property, SerializationContext serCtx, object instance)
		{
			return true;
		}

		// Token: 0x0400013D RID: 317
		private SerializationContext serializationContext;

		// Token: 0x0400013E RID: 318
		private DataContext dataContext;
	}
}
