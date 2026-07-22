using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000014 RID: 20
	[DataModelExtension(typeof(WidgetObject))]
	[DataInclude(typeof(UISizeType))]
	public class WidgetObjectData : NodeObjectData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000033D8 File Offset: 0x000015D8
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x000033EF File Offset: 0x000015EF
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool TouchEnable { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000033F8 File Offset: 0x000015F8
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000340F File Offset: 0x0000160F
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[PropertyOrder(2147483647)]
		[DefaultValue(false)]
		public bool StretchWidthEnable { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003418 File Offset: 0x00001618
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000342F File Offset: 0x0000162F
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[PropertyOrder(2147483647)]
		public bool StretchHeightEnable { get; set; }

		// Token: 0x04000040 RID: 64
		internal static readonly string DefaultFont = "";
	}
}
