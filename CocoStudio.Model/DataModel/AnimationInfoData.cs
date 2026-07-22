using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000031 RID: 49
	[DataItem(Name = "AnimationInfo")]
	[DataModelExtension(typeof(AnimationInfo))]
	public class AnimationInfoData : BaseObjectData
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00006B04 File Offset: 0x00004D04
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00006B1B File Offset: 0x00004D1B
		[JsonProperty]
		[ItemProperty]
		public int StartIndex { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00006B24 File Offset: 0x00004D24
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00006B3B File Offset: 0x00004D3B
		[JsonProperty]
		[ItemProperty]
		public int EndIndex { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00006B44 File Offset: 0x00004D44
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00006B5B File Offset: 0x00004D5B
		[ItemProperty]
		public ColorData RenderColor { get; set; }
	}
}
