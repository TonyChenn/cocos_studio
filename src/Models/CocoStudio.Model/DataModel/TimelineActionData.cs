using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension]
	public class TimelineActionData : BaseObjectData, ICustomDataItem
	{
		[ItemProperty]
		[JsonProperty]
		public int Duration { get; set; }

		[JsonProperty]
		[ItemProperty]
		public float Speed { get; set; }

		[JsonProperty]
		[ItemProperty]
		public string ActivedAnimationName { get; set; }

		[JsonProperty]
		public List<TimelineData> Timelines { get; set; }

		public TimelineActionData()
		{
			this.Timelines = new List<TimelineData>();
			this.Speed = 1f;
		}

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
