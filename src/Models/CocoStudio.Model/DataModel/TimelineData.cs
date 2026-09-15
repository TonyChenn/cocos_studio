using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "Timeline")]
	[DataModelExtension(typeof(Timeline))]
	public class TimelineData : BaseObjectData, ICustomDataItem
	{
		[JsonProperty]
		[ItemProperty]
		public int ActionTag { get; set; }

		[ItemProperty]
		[Obsolete("该属性已经废弃不再使用,请改用Property属性.")]
		[JsonProperty]
		public string FrameType
		{
			get
			{
				return this.frameType;
			}
			set
			{
				this.frameType = value;
				this.ConvertToPropertyName(this.frameType);
			}
		}

		[ItemProperty]
		[JsonProperty]
		public string Property { get; set; }

		[JsonProperty]
		public List<FrameData> Frames { get; set; }

		public TimelineData()
		{
			this.Frames = new List<FrameData>();
		}

		public DataCollection Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			foreach (FrameData obj in this.Frames)
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
					FrameData item = configurationDataType.Deserialize(handler.SerializationContext, null, dataItem) as FrameData;
					this.Frames.Add(item);
				}
			}
			base.ExtendedProperties.Clear();
		}

		private void ConvertToPropertyName(string frameType)
		{
			if (string.IsNullOrEmpty(this.Property))
			{
				this.Property = frameType.Substring(0, frameType.Length - 5);
			}
		}

		private string frameType;
	}
}
