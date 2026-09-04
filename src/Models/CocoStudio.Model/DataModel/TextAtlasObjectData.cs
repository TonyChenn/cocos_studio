using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000020 RID: 32
	[DataModelExtension(typeof(TextAtlasObject))]
	public class TextAtlasObjectData : WidgetObjectData
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000051C8 File Offset: 0x000033C8
		// (set) Token: 0x06000144 RID: 324 RVA: 0x000051DF File Offset: 0x000033DF
		[ItemProperty]
		[JsonProperty]
		public int CharWidth { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000051E8 File Offset: 0x000033E8
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000051FF File Offset: 0x000033FF
		[JsonProperty]
		[ItemProperty]
		public int CharHeight { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00005208 File Offset: 0x00003408
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000521F File Offset: 0x0000341F
		[JsonProperty]
		[ItemProperty]
		public string LabelText { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00005228 File Offset: 0x00003428
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000523F File Offset: 0x0000343F
		[JsonProperty]
		[ItemProperty]
		public string StartChar { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00005248 File Offset: 0x00003448
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00005260 File Offset: 0x00003460
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData LabelAtlasFileImage_CNB
		{
			get
			{
				return this.labelAtlasFileImage_CNB;
			}
			set
			{
				this.labelAtlasFileImage_CNB = value;
				if (this.labelAtlasFileImage_CNB == null)
				{
					this.labelAtlasFileImage_CNB = TextAtlasObjectData.DefaultFile;
				}
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000052A0 File Offset: 0x000034A0
		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextAtlasObject textAtlasObject = vObject as TextAtlasObject;
			if (textAtlasObject != null)
			{
				if ((this.LabelAtlasFileImage_CNB != null && textAtlasObject.LabelAtlasFileImage_CNB.GetResourceData().Type != this.LabelAtlasFileImage_CNB.Type) || textAtlasObject.LabelAtlasFileImage_CNB.GetResourceData().Type == EnumResourceType.Default)
				{
					textAtlasObject.LabelAtlasFileImage_CNB = null;
				}
			}
		}

		// Token: 0x04000084 RID: 132
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/TextAtlas.png");

		// Token: 0x04000085 RID: 133
		private ResourceItemData labelAtlasFileImage_CNB;
	}
}
