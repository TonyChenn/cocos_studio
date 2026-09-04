using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003E RID: 62
	[DataItem(Name = "Timeline")]
	[DataModelExtension(typeof(Timeline))]
	public class TimelineData : BaseObjectData, ICustomDataItem
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000073D8 File Offset: 0x000055D8
		// (set) Token: 0x06000264 RID: 612 RVA: 0x000073EF File Offset: 0x000055EF
		[JsonProperty]
		[ItemProperty]
		public int ActionTag { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000073F8 File Offset: 0x000055F8
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00007410 File Offset: 0x00005610
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00007428 File Offset: 0x00005628
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000743F File Offset: 0x0000563F
		[ItemProperty]
		[JsonProperty]
		public string Property { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00007448 File Offset: 0x00005648
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000745F File Offset: 0x0000565F
		[JsonProperty]
		public List<FrameData> Frames { get; set; }

		// Token: 0x0600026B RID: 619 RVA: 0x00007468 File Offset: 0x00005668
		public TimelineData()
		{
			this.Frames = new List<FrameData>();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00007480 File Offset: 0x00005680
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

		// Token: 0x0600026D RID: 621 RVA: 0x00007500 File Offset: 0x00005700
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

		// Token: 0x0600026E RID: 622 RVA: 0x000075D0 File Offset: 0x000057D0
		private void ConvertToPropertyName(string frameType)
		{
			if (string.IsNullOrEmpty(this.Property))
			{
				this.Property = frameType.Substring(0, frameType.Length - 5);
			}
		}

		// Token: 0x04000101 RID: 257
		private string frameType;
	}
}
