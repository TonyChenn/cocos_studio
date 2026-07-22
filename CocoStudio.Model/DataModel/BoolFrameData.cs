using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000032 RID: 50
	[DataItem(Name = "BoolFrame")]
	[DataModelExtension(typeof(BoolFrame))]
	public class BoolFrameData : FrameData
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00006B6C File Offset: 0x00004D6C
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00006B83 File Offset: 0x00004D83
		[JsonProperty]
		[ItemProperty]
		public bool Value { get; set; }

		// Token: 0x0600022A RID: 554 RVA: 0x00006B98 File Offset: 0x00004D98
		public override bool FrameEquals(FrameData framedata)
		{
			BoolFrameData boolFrameData = framedata as BoolFrameData;
			bool flag = boolFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == boolFrameData.Value;
		}
	}
}
