using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000021 RID: 33
	[DataModelExtension(typeof(TextBMFontObject))]
	public class TextBMFontObjectData : WidgetObjectData
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005324 File Offset: 0x00003524
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000533B File Offset: 0x0000353B
		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005344 File Offset: 0x00003544
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000535C File Offset: 0x0000355C
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData LabelBMFontFile_CNB
		{
			get
			{
				return this.labelBMFontFile_CNB;
			}
			set
			{
				this.labelBMFontFile_CNB = value;
				if (this.labelBMFontFile_CNB == null)
				{
					this.labelBMFontFile_CNB = TextBMFontObjectData.DefaultFntFont;
				}
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000539C File Offset: 0x0000359C
		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextBMFontObject textBMFontObject = vObject as TextBMFontObject;
			if (textBMFontObject != null)
			{
				if (this.labelBMFontFile_CNB != null && textBMFontObject.LabelBMFontFile_CNB.GetResourceData().Type != this.labelBMFontFile_CNB.Type)
				{
					textBMFontObject.LabelBMFontFile_CNB = null;
				}
			}
		}

		// Token: 0x0400008A RID: 138
		internal static readonly ResourceItemData DefaultFntFont = new ResourceItemData(EnumResourceType.Default, "Default/defaultBMFont.fnt");

		// Token: 0x0400008B RID: 139
		private ResourceItemData labelBMFontFile_CNB;
	}
}
