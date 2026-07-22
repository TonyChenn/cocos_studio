using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000A RID: 10
	[DataItem(Name = "BlendFuncFrame")]
	[DataModelExtension(typeof(BlendFuncFrame))]
	public class BlendFuncFrameData : FrameData
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002694 File Offset: 0x00000894
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000026AB File Offset: 0x000008AB
		[ItemProperty]
		[JsonProperty]
		public int Src { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000026B4 File Offset: 0x000008B4
		// (set) Token: 0x0600003E RID: 62 RVA: 0x000026CB File Offset: 0x000008CB
		[ItemProperty]
		[JsonProperty]
		public int Dst { get; set; }

		// Token: 0x06000040 RID: 64 RVA: 0x000026E0 File Offset: 0x000008E0
		public override bool FrameEquals(FrameData framedata)
		{
			BlendFuncFrameData blendFuncFrameData = framedata as BlendFuncFrameData;
			return blendFuncFrameData != null && base.FrameEquals(framedata) && this.Src == blendFuncFrameData.Src && this.Dst == blendFuncFrameData.Dst;
		}
	}
}
