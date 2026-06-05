using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000063 RID: 99
	public class CollectionDataType : DataType
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000CCF3 File Offset: 0x0000AEF3
		internal CollectionDataType(Type type, ICollectionHandler handler) : base(type)
		{
			this.handler = handler;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000CD03 File Offset: 0x0000AF03
		public override bool IsSimpleType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000CD06 File Offset: 0x0000AF06
		public override bool CanCreateInstance
		{
			get
			{
				return this.handler.CanCreateInstance;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000CD13 File Offset: 0x0000AF13
		public override bool CanReuseInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000CD18 File Offset: 0x0000AF18
		protected internal override object GetMapData(object[] attributes, string scope)
		{
			DataType dataType = null;
			Type type = null;
			ItemPropertyAttribute itemPropertyAttribute = base.FindPropertyAttribute(attributes, scope + "/*");
			if (itemPropertyAttribute != null)
			{
				type = itemPropertyAttribute.ValueType;
				if (type == null)
				{
					type = this.handler.GetItemType();
				}
				if (itemPropertyAttribute.SerializationDataType != null)
				{
					dataType = (DataType)Activator.CreateInstance(itemPropertyAttribute.SerializationDataType, new object[]
					{
						type
					});
				}
			}
			if (type == null)
			{
				type = this.handler.GetItemType();
			}
			if (dataType == null)
			{
				dataType = base.Context.GetConfigurationDataType(type);
			}
			object mapData = dataType.GetMapData(attributes, scope + "/*");
			if (itemPropertyAttribute == null && mapData == null)
			{
				return null;
			}
			return new CollectionDataType.MapData
			{
				ItemType = dataType,
				ItemName = ((itemPropertyAttribute != null && itemPropertyAttribute.Name != null) ? itemPropertyAttribute.Name : dataType.Name),
				ItemMapData = mapData
			};
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000CE04 File Offset: 0x0000B004
		protected virtual CollectionDataType.MapData GetDefaultData()
		{
			if (this.defaultData != null)
			{
				return this.defaultData;
			}
			this.defaultData = new CollectionDataType.MapData();
			this.defaultData.ItemType = base.Context.GetConfigurationDataType(this.handler.GetItemType());
			this.defaultData.ItemName = this.defaultData.ItemType.Name;
			return this.defaultData;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000CE70 File Offset: 0x0000B070
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mdata, object collection)
		{
			CollectionDataType.MapData mapData = (mdata != null) ? ((CollectionDataType.MapData)mdata) : this.GetDefaultData();
			DataItem dataItem = new DataItem();
			dataItem.Name = base.Name;
			dataItem.UniqueNames = false;
			object initialPosition = this.handler.GetInitialPosition(collection);
			while (this.handler.MoveNextItem(collection, ref initialPosition))
			{
				object currentItem = this.handler.GetCurrentItem(collection, initialPosition);
				if (currentItem != null)
				{
					DataNode dataNode = mapData.ItemType.Serialize(serCtx, mapData.ItemMapData, currentItem);
					dataNode.Name = mapData.ItemName;
					dataItem.ItemData.Add(dataNode);
				}
			}
			return dataItem;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000CF08 File Offset: 0x0000B108
		protected internal override object OnDeserialize(SerializationContext serCtx, object mdata, DataNode data)
		{
			DataCollection itemData = ((DataItem)data).ItemData;
			object position;
			object obj = this.handler.CreateCollection(out position, itemData.Count);
			this.Deserialize(serCtx, mdata, itemData, obj, position);
			return obj;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000CF44 File Offset: 0x0000B144
		protected internal override void OnDeserialize(SerializationContext serCtx, object mdata, DataNode data, object collectionInstance)
		{
			DataCollection itemData = ((DataItem)data).ItemData;
			object position;
			this.handler.ResetCollection(collectionInstance, out position, itemData.Count);
			this.Deserialize(serCtx, mdata, itemData, collectionInstance, position);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000CF80 File Offset: 0x0000B180
		private void Deserialize(SerializationContext serCtx, object mdata, DataCollection items, object collectionInstance, object position)
		{
			CollectionDataType.MapData mapData = (mdata != null) ? ((CollectionDataType.MapData)mdata) : this.GetDefaultData();
			foreach (object obj in items)
			{
				DataNode data = (DataNode)obj;
				this.handler.AddItem(ref collectionInstance, ref position, mapData.ItemType.Deserialize(serCtx, mapData.ItemMapData, data));
			}
			this.handler.FinishCreation(ref collectionInstance, position);
		}

		// Token: 0x04000121 RID: 289
		private ICollectionHandler handler;

		// Token: 0x04000122 RID: 290
		private CollectionDataType.MapData defaultData;

		// Token: 0x02000064 RID: 100
		protected class MapData
		{
			// Token: 0x04000123 RID: 291
			public string ItemName;

			// Token: 0x04000124 RID: 292
			public DataType ItemType;

			// Token: 0x04000125 RID: 293
			public object ItemMapData;
		}
	}
}
