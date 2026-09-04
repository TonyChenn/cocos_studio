using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000039 RID: 57
	[DataItem(Name = "IntFrame")]
	[DataModelExtension(typeof(IntFrame))]
	public class IntFrameData : FrameData
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00006FA4 File Offset: 0x000051A4
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00006FBB File Offset: 0x000051BB
		[ItemProperty]
		[JsonProperty]
		public int Value { get; set; }

		// Token: 0x06000247 RID: 583 RVA: 0x00006FD0 File Offset: 0x000051D0
		public override bool FrameEquals(FrameData framedata)
		{
			IntFrameData intFrameData = framedata as IntFrameData;
			bool flag = intFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Value == intFrameData.Value;
		}
	}
}
