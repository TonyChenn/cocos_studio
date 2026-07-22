using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002E RID: 46
	[DataModelExtension(typeof(SpriteObject))]
	public class SpriteObjectData : NodeObjectData
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00006794 File Offset: 0x00004994
		// (set) Token: 0x060001FA RID: 506 RVA: 0x000067AB File Offset: 0x000049AB
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000067B4 File Offset: 0x000049B4
		// (set) Token: 0x060001FC RID: 508 RVA: 0x000067CB File Offset: 0x000049CB
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000067D4 File Offset: 0x000049D4
		// (set) Token: 0x060001FE RID: 510 RVA: 0x000067EC File Offset: 0x000049EC
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData FileData
		{
			get
			{
				return this.fileData;
			}
			set
			{
				this.fileData = value;
				if (this.fileData == ResourceItemData.DefaultMarker)
				{
					this.fileData = SpriteObjectData.DefaultFile;
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00006828 File Offset: 0x00004A28
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000683F File Offset: 0x00004A3F
		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		// Token: 0x06000201 RID: 513 RVA: 0x00006848 File Offset: 0x00004A48
		public SpriteObjectData()
		{
			this.BlendFunc = new BlendFuncValue();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006860 File Offset: 0x00004A60
		protected override void OnDataInitialize(VisualObject vObject)
		{
			SpriteObject spriteObject = vObject as SpriteObject;
			if (spriteObject != null)
			{
				if (this.FileData != null && spriteObject.FileData.GetResourceData().Type != this.FileData.Type)
				{
					spriteObject.FileData = null;
				}
			}
		}

		// Token: 0x040000DA RID: 218
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/Sprite.png");

		// Token: 0x040000DB RID: 219
		private ResourceItemData fileData;
	}
}
