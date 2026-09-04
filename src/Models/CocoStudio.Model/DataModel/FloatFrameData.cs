using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000036 RID: 54
	[DataModelExtension(typeof(FloatFrame))]
	[DataItem(Name = "FloatFrame")]
	public class FloatFrameData : FrameData
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00006D04 File Offset: 0x00004F04
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00006D1B File Offset: 0x00004F1B
		[JsonProperty]
		[ItemProperty]
		public float Value { get; set; }

		// Token: 0x06000239 RID: 569 RVA: 0x00006D30 File Offset: 0x00004F30
		public override bool FrameEquals(FrameData framedata)
		{
			FloatFrameData floatFrameData = framedata as FloatFrameData;
			bool flag = floatFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == floatFrameData.Value;
		}
	}
}
