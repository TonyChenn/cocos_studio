using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000038 RID: 56
	[DataItem(Name = "InnerActionFrame")]
	[DataModelExtension(typeof(InnerActionFrame))]
	public class InnerActionFrameData : FrameData
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00006EE0 File Offset: 0x000050E0
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00006EF7 File Offset: 0x000050F7
		[JsonProperty]
		[ItemProperty]
		public InnerActionType InnerActionType { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00006F00 File Offset: 0x00005100
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00006F17 File Offset: 0x00005117
		[ItemProperty]
		[JsonProperty]
		public string CurrentAniamtionName { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00006F20 File Offset: 0x00005120
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00006F37 File Offset: 0x00005137
		[ItemProperty]
		[JsonProperty]
		public int SingleFrameIndex { get; set; }

		// Token: 0x06000243 RID: 579 RVA: 0x00006F40 File Offset: 0x00005140
		public override bool FrameEquals(FrameData framedata)
		{
			InnerActionFrameData innerActionFrameData = framedata as InnerActionFrameData;
			bool flag = innerActionFrameData != null;
			return flag && base.FrameEquals(framedata) && this.InnerActionType == innerActionFrameData.InnerActionType && this.CurrentAniamtionName.Equals(innerActionFrameData.CurrentAniamtionName) && this.SingleFrameIndex == innerActionFrameData.SingleFrameIndex;
		}
	}
}
