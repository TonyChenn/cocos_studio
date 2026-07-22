using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002C RID: 44
	[DataModelExtension(typeof(SimpleAudioObject))]
	public class SimpleAudioObjectData : NodeObjectData
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00006480 File Offset: 0x00004680
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00006497 File Offset: 0x00004697
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float Volume { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000064A0 File Offset: 0x000046A0
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x000064B7 File Offset: 0x000046B7
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool Loop { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000064C0 File Offset: 0x000046C0
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x000064D7 File Offset: 0x000046D7
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }
	}
}
