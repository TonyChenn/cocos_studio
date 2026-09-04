using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000034 RID: 52
	[DataModelExtension(typeof(StringFrame))]
	[DataItem(Name = "StringFrame")]
	public class StringFrameData : FrameData
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00006C88 File Offset: 0x00004E88
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00006C9F File Offset: 0x00004E9F
		[JsonProperty]
		[ItemProperty]
		public string Value { get; set; }

		// Token: 0x06000234 RID: 564 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public override bool FrameEquals(FrameData framedata)
		{
			StringFrameData stringFrameData = framedata as StringFrameData;
			bool flag = stringFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == stringFrameData.Value;
		}
	}
}
