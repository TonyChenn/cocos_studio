using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000007 RID: 7
	[DataModelExtension]
	public class GameFileData : BaseObjectData
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002474 File Offset: 0x00000674
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000248B File Offset: 0x0000068B
		[ItemProperty]
		[JsonProperty]
		public TimelineActionData Animation { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002494 File Offset: 0x00000694
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000024AB File Offset: 0x000006AB
		[ItemProperty]
		[JsonProperty]
		public List<AnimationInfoData> AnimationList { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024B4 File Offset: 0x000006B4
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000024CB File Offset: 0x000006CB
		[JsonProperty]
		[ItemProperty]
		public AbstractNodeObjectData ObjectData { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024D4 File Offset: 0x000006D4
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000024EB File Offset: 0x000006EB
		[JsonProperty]
		public List<string> UsedResources { get; set; }

		// Token: 0x0600002D RID: 45 RVA: 0x000024F4 File Offset: 0x000006F4
		public GameFileData()
		{
			this.Animation = new TimelineActionData();
			this.AnimationList = new List<AnimationInfoData>();
		}
	}
}
