using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200001C RID: 28
	[DataModelExtension(typeof(ImageViewObject))]
	public class ImageViewObjectData : WidgetObjectData
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004E4C File Offset: 0x0000304C
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00004E63 File Offset: 0x00003063
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool FlipX { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004E6C File Offset: 0x0000306C
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004E83 File Offset: 0x00003083
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00004E8C File Offset: 0x0000308C
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00004EA4 File Offset: 0x000030A4
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
					this.fileData = ImageViewObjectData.DefaultFile;
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00004EE0 File Offset: 0x000030E0
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00004EF7 File Offset: 0x000030F7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool Scale9Enable { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004F00 File Offset: 0x00003100
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00004F17 File Offset: 0x00003117
		[ItemProperty(DefaultValue = 0)]
		public int LeftEage { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004F20 File Offset: 0x00003120
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00004F37 File Offset: 0x00003137
		[ItemProperty(DefaultValue = 0)]
		public int RightEage { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004F40 File Offset: 0x00003140
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00004F57 File Offset: 0x00003157
		[ItemProperty(DefaultValue = 0)]
		public int TopEage { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004F60 File Offset: 0x00003160
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004F77 File Offset: 0x00003177
		[ItemProperty(DefaultValue = 0)]
		public int BottomEage { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004F80 File Offset: 0x00003180
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00004F97 File Offset: 0x00003197
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9OriginX { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004FA0 File Offset: 0x000031A0
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00004FB7 File Offset: 0x000031B7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int Scale9OriginY { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00004FC0 File Offset: 0x000031C0
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00004FD7 File Offset: 0x000031D7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00004FE0 File Offset: 0x000031E0
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00004FF7 File Offset: 0x000031F7
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9Height { get; set; }

		// Token: 0x0600013C RID: 316 RVA: 0x0000500C File Offset: 0x0000320C
		protected override void OnDataInitialize(VisualObject vObject)
		{
			ImageViewObject imageViewObject = vObject as ImageViewObject;
			if (imageViewObject != null)
			{
				if (this.FileData != null && imageViewObject.FileData.GetResourceData().Type != this.FileData.Type)
				{
					imageViewObject.FileData = null;
				}
			}
		}

		// Token: 0x04000076 RID: 118
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/ImageFile.png");

		// Token: 0x04000077 RID: 119
		private static readonly SizeF defaultImageSize = new SizeF(46f, 46f);

		// Token: 0x04000078 RID: 120
		private ResourceItemData fileData;
	}
}
