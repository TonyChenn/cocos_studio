using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000007 RID: 7
	[DataModelExtension(typeof(Sprite3DObject))]
	public class Sprite3DObjectData : Node3DObjectData
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000251F File Offset: 0x0000071F
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002527 File Offset: 0x00000727
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
					this.fileData = Sprite3DObjectData.DefaultFile;
					base.Size = Sprite3DObjectData.defaultSpriteSize;
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002554 File Offset: 0x00000754
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000255C File Offset: 0x0000075C
		[JsonProperty]
		[ItemProperty(DefaultValue = false)]
		public bool RunAction3D { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002565 File Offset: 0x00000765
		// (set) Token: 0x0600004E RID: 78 RVA: 0x0000256D File Offset: 0x0000076D
		[ItemProperty(DefaultValue = false)]
		[JsonProperty]
		public bool IsFlipped { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002576 File Offset: 0x00000776
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000257E File Offset: 0x0000077E
		[ItemProperty]
		[JsonProperty]
		public LightFlag LightFlag { get; set; }

		// Token: 0x06000051 RID: 81 RVA: 0x00002587 File Offset: 0x00000787
		public Sprite3DObjectData()
		{
			this.LightFlag = LightFlag.LIGHT0;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002598 File Offset: 0x00000798
		protected override void OnDataInitialize(VisualObject vObject)
		{
			Sprite3DObject sprite3DObject = vObject as Sprite3DObject;
			if (sprite3DObject != null && this.FileData != null && sprite3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				sprite3DObject.FileData = null;
			}
		}

		// Token: 0x04000021 RID: 33
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/empty.c3t");

		// Token: 0x04000022 RID: 34
		private static readonly SizeF defaultSpriteSize = new SizeF(1f, 1f);

		// Token: 0x04000023 RID: 35
		private ResourceItemData fileData;
	}
}
