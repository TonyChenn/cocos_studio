using System;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002B RID: 43
	[DataItem("ProjectNodeObjectData")]
	[DataModelExtension(typeof(FileNodeObject))]
	public class FileNodeObjectData : NodeObjectData
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000063C8 File Offset: 0x000045C8
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x000063DF File Offset: 0x000045DF
		[JsonConverter(typeof(CsdToJsonConvertor))]
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000063E8 File Offset: 0x000045E8
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x000063FF File Offset: 0x000045FF
		[PropertyOrder(2147483647)]
		[ItemProperty]
		[JsonProperty]
		public bool StretchWidthEnable { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00006408 File Offset: 0x00004608
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000641F File Offset: 0x0000461F
		[PropertyOrder(2147483647)]
		[ItemProperty]
		[JsonProperty]
		public bool StretchHeightEnable { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00006428 File Offset: 0x00004628
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000643F File Offset: 0x0000463F
		[ItemProperty]
		[JsonProperty]
		public float InnerActionSpeed { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00006448 File Offset: 0x00004648
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000645F File Offset: 0x0000465F
		[ItemProperty]
		[PropertyOrder(2147483647)]
		public bool CustomSizeEnabled { get; set; }

		// Token: 0x060001E0 RID: 480 RVA: 0x00006468 File Offset: 0x00004668
		public FileNodeObjectData()
		{
			this.ctype = "ProjectNodeObjectData";
		}
	}
}
