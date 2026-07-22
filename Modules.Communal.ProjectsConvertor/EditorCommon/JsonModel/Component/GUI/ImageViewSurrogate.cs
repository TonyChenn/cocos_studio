using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200001E RID: 30
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ImageViewSurrogate : WidgetSurrogate
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006598 File Offset: 0x00004798
		// (set) Token: 0x06000150 RID: 336 RVA: 0x000065A0 File Offset: 0x000047A0
		[DataMember]
		public string fileName { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000065A9 File Offset: 0x000047A9
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000065B1 File Offset: 0x000047B1
		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000065BA File Offset: 0x000047BA
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000065C2 File Offset: 0x000047C2
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000065CB File Offset: 0x000047CB
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000065D3 File Offset: 0x000047D3
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000065DC File Offset: 0x000047DC
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000065E4 File Offset: 0x000047E4
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsWidth { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000065ED File Offset: 0x000047ED
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000065F5 File Offset: 0x000047F5
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000065FE File Offset: 0x000047FE
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00006606 File Offset: 0x00004806
		[DataMember]
		[DefaultValue(80f)]
		public float scale9Width { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000660F File Offset: 0x0000480F
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00006617 File Offset: 0x00004817
		[DataMember]
		[DefaultValue(80f)]
		public float scale9Height { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006620 File Offset: 0x00004820
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006628 File Offset: 0x00004828
		[DataMember]
		public bool scale9Enable { get; set; }

		// Token: 0x06000161 RID: 353 RVA: 0x00006631 File Offset: 0x00004831
		protected ImageViewSurrogate()
		{
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
			this.scale9Height = 80f;
			this.scale9Width = 80f;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006668 File Offset: 0x00004868
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ImageViewObjectData imageViewObjectData = obj as ImageViewObjectData;
			imageViewObjectData.FlipX = this.flipX;
			imageViewObjectData.FlipY = this.flipY;
			imageViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.fileNameData);
			imageViewObjectData.Scale9Enable = this.scale9Enable;
			if (imageViewObjectData.Scale9Enable && imageViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(imageViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				imageViewObjectData.LeftEage = leftEage;
				imageViewObjectData.RightEage = rightEage;
				imageViewObjectData.TopEage = topEage;
				imageViewObjectData.BottomEage = bottomEage;
				imageViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				imageViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				imageViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				imageViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
