using System;
using System.ComponentModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000024 RID: 36
	[DataModelExtension(typeof(ScrollViewObject))]
	[DataInclude(typeof(ScrollViewDirectionType))]
	public class ScrollViewObjectData : PanelObjectData
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000595C File Offset: 0x00003B5C
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00005973 File Offset: 0x00003B73
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool IsBounceEnabled { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000597C File Offset: 0x00003B7C
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00005993 File Offset: 0x00003B93
		[ItemProperty]
		[JsonProperty]
		public SizeValue InnerNodeSize { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000599C File Offset: 0x00003B9C
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000059B3 File Offset: 0x00003BB3
		[ItemProperty]
		[JsonProperty]
		public ScrollViewDirectionType ScrollDirectionType { get; set; }

		// Token: 0x060001A8 RID: 424 RVA: 0x000059C8 File Offset: 0x00003BC8
		protected override void OnDataInitialize(VisualObject vObject)
		{
			ScrollViewObject scrollViewObject = vObject as ScrollViewObject;
			if (scrollViewObject != null)
			{
				scrollViewObject.InnerNodeSize = this.InnerNodeSize;
			}
		}
	}
}
