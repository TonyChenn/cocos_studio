using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000024 RID: 36
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class LoadingBarSurrogate : WidgetSurrogate
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000070D8 File Offset: 0x000052D8
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x000070E0 File Offset: 0x000052E0
		[DataMember]
		public string texture { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000070E9 File Offset: 0x000052E9
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x000070F1 File Offset: 0x000052F1
		[DataMember]
		public ResourceDataSurrogate textureData { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000070FA File Offset: 0x000052FA
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007102 File Offset: 0x00005302
		[DefaultValue(100)]
		[DataMember]
		public int percent { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000710B File Offset: 0x0000530B
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00007113 File Offset: 0x00005313
		[DataMember]
		public int direction { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000711C File Offset: 0x0000531C
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00007124 File Offset: 0x00005324
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000712D File Offset: 0x0000532D
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00007135 File Offset: 0x00005335
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000713E File Offset: 0x0000533E
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007146 File Offset: 0x00005346
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000714F File Offset: 0x0000534F
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00007157 File Offset: 0x00005357
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007160 File Offset: 0x00005360
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00007168 File Offset: 0x00005368
		[DataMember]
		public bool scale9Enable { get; set; }

		// Token: 0x060001F5 RID: 501 RVA: 0x00007171 File Offset: 0x00005371
		protected LoadingBarSurrogate()
		{
			this.percent = 100;
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007198 File Offset: 0x00005398
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			LoadingBarObjectData loadingBarObjectData = obj as LoadingBarObjectData;
			loadingBarObjectData.ProgressInfo = this.percent;
			loadingBarObjectData.ImageFileData = WidgetSurrogate.ConvertResourceData(this.textureData);
			loadingBarObjectData.ProgressType = (LoadingBarDirectionType)this.direction;
		}
	}
}
