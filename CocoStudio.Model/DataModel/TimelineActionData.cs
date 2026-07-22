using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003D RID: 61
	[DataModelExtension]
	public class TimelineActionData : BaseObjectData, ICustomDataItem
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000258 RID: 600 RVA: 0x000071E4 File Offset: 0x000053E4
		// (set) Token: 0x06000259 RID: 601 RVA: 0x000071FB File Offset: 0x000053FB
		[ItemProperty]
		[JsonProperty]
		public int Duration { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00007204 File Offset: 0x00005404
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000721B File Offset: 0x0000541B
		[JsonProperty]
		[ItemProperty]
		public float Speed { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00007224 File Offset: 0x00005424
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000723B File Offset: 0x0000543B
		[JsonProperty]
		[ItemProperty]
		public string ActivedAnimationName { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00007244 File Offset: 0x00005444
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000725B File Offset: 0x0000545B
		[JsonProperty]
		public List<TimelineData> Timelines { get; set; }

		// Token: 0x06000260 RID: 608 RVA: 0x00007264 File Offset: 0x00005464
		public TimelineActionData()
		{
			this.Timelines = new List<TimelineData>();
			this.Speed = 1f;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00007288 File Offset: 0x00005488
		public DataCollection Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			foreach (TimelineData obj in this.Timelines)
			{
				DataNode entry = handler.SerializationContext.Serializer.Serialize(obj);
				dataCollection.Add(entry);
			}
			return dataCollection;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00007308 File Offset: 0x00005508
		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			handler.Deserialize(this, data);
			foreach (object obj in data)
			{
				DataNode dataNode = (DataNode)obj;
				DataItem dataItem = dataNode as DataItem;
				if (dataItem != null)
				{
					DataType configurationDataType = handler.SerializationContext.Serializer.DataContext.GetConfigurationDataType(dataNode.Name);
					TimelineData item = configurationDataType.Deserialize(handler.SerializationContext, null, dataItem) as TimelineData;
					this.Timelines.Add(item);
				}
			}
			base.ExtendedProperties.Clear();
		}
	}
}
