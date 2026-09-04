using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000026 RID: 38
	[DataInclude(typeof(LoadingBarDirectionType))]
	[DataModelExtension(typeof(LoadingBarObject))]
	public class LoadingBarObjectData : WidgetObjectData
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005AB0 File Offset: 0x00003CB0
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00005AC7 File Offset: 0x00003CC7
		[ItemProperty(DefaultValue = 80)]
		[DefaultValue(80)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ProgressInfo { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00005AD0 File Offset: 0x00003CD0
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00005AE7 File Offset: 0x00003CE7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(LoadingBarDirectionType.Left_To_Right)]
		[ItemProperty(DefaultValue = LoadingBarDirectionType.Left_To_Right)]
		public LoadingBarDirectionType ProgressType { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00005AF0 File Offset: 0x00003CF0
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00005B08 File Offset: 0x00003D08
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData ImageFileData
		{
			get
			{
				return this.imageFileData;
			}
			set
			{
				this.imageFileData = value;
				if (this.imageFileData == ResourceItemData.DefaultMarker)
				{
					this.imageFileData = LoadingBarObjectData.DefaultFile;
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005B41 File Offset: 0x00003D41
		public LoadingBarObjectData()
		{
			this.ProgressInfo = 80;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005B58 File Offset: 0x00003D58
		protected override void OnDataInitialize(VisualObject vObject)
		{
			LoadingBarObject loadingBarObject = vObject as LoadingBarObject;
			if (loadingBarObject != null)
			{
				if (this.ImageFileData != null && loadingBarObject.ImageFileData.GetResourceData().Type != this.ImageFileData.Type)
				{
					loadingBarObject.ImageFileData = null;
				}
			}
		}

		// Token: 0x040000B7 RID: 183
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/LoadingBarFile.png");

		// Token: 0x040000B8 RID: 184
		private ResourceItemData imageFileData;
	}
}
