using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000006 RID: 6
	[DataModelExtension(typeof(Slice3DObject))]
	internal class Slice3DObjectData : Node3DObjectData
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000023FB File Offset: 0x000005FB
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002403 File Offset: 0x00000603
		[ItemProperty(DefaultValue = BillBoardMode.SLICE_BILLBOARD_NONE)]
		[DefaultValue(BillBoardMode.SLICE_BILLBOARD_NONE)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public BillBoardMode SliceBillBoardMode { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000240C File Offset: 0x0000060C
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002414 File Offset: 0x00000614
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData
		{
			get
			{
				return this.fileData;
			}
			set
			{
				this.fileData = value;
				if (this.fileData == null)
				{
					this.fileData = Slice3DObjectData.DefaultFile;
					base.Size = Slice3DObjectData.defaultSliceSize;
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002441 File Offset: 0x00000641
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002449 File Offset: 0x00000649
		[ItemProperty]
		[JsonProperty]
		public SizeF SliceSize { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002452 File Offset: 0x00000652
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000245A File Offset: 0x0000065A
		[JsonProperty]
		[ItemProperty(DefaultValue = false)]
		public bool AnimationSwitch { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002463 File Offset: 0x00000663
		// (set) Token: 0x0600003F RID: 63 RVA: 0x0000246B File Offset: 0x0000066B
		[JsonProperty]
		[ItemProperty]
		public PointF AnimationSpeed { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002474 File Offset: 0x00000674
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000247C File Offset: 0x0000067C
		[ItemProperty(DefaultValue = false)]
		[JsonProperty]
		public bool TextureSwitch { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002485 File Offset: 0x00000685
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000248D File Offset: 0x0000068D
		[JsonProperty]
		[ItemProperty]
		public PointF TextureAnimation { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002496 File Offset: 0x00000696
		// (set) Token: 0x06000045 RID: 69 RVA: 0x0000249E File Offset: 0x0000069E
		[JsonProperty]
		[ItemProperty(DefaultValue = 0)]
		public float framerate { get; set; }

		// Token: 0x06000047 RID: 71 RVA: 0x000024B0 File Offset: 0x000006B0
		protected override void OnDataInitialize(VisualObject vObject)
		{
			Slice3DObject slice3DObject = vObject as Slice3DObject;
			if (slice3DObject != null && this.FileData != null && slice3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				slice3DObject.FileData = null;
			}
		}

		// Token: 0x04000017 RID: 23
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/Sprite.png");

		// Token: 0x04000018 RID: 24
		private static readonly SizeF defaultSliceSize = new SizeF(46f, 46f);

		// Token: 0x04000019 RID: 25
		private ResourceItemData fileData;
	}
}
