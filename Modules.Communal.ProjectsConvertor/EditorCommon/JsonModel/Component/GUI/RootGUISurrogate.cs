using System;
using System.Runtime.Serialization;
using Mono.Addins;
using Newtonsoft.Json;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000028 RID: 40
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class RootGUISurrogate : WidgetSurrogate
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000798B File Offset: 0x00005B8B
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00007993 File Offset: 0x00005B93
		[JsonIgnore]
		public override float x { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000799C File Offset: 0x00005B9C
		// (set) Token: 0x0600025A RID: 602 RVA: 0x000079A4 File Offset: 0x00005BA4
		[JsonIgnore]
		public override float y { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600025B RID: 603 RVA: 0x000079AD File Offset: 0x00005BAD
		// (set) Token: 0x0600025C RID: 604 RVA: 0x000079B5 File Offset: 0x00005BB5
		[JsonIgnore]
		public override float rotation { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600025D RID: 605 RVA: 0x000079BE File Offset: 0x00005BBE
		// (set) Token: 0x0600025E RID: 606 RVA: 0x000079C6 File Offset: 0x00005BC6
		[JsonIgnore]
		public override bool flipX { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000079CF File Offset: 0x00005BCF
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000079D7 File Offset: 0x00005BD7
		[JsonIgnore]
		public override bool flipY { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000079E0 File Offset: 0x00005BE0
		// (set) Token: 0x06000262 RID: 610 RVA: 0x000079E8 File Offset: 0x00005BE8
		[JsonIgnore]
		public override int colorR { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000079F1 File Offset: 0x00005BF1
		// (set) Token: 0x06000264 RID: 612 RVA: 0x000079F9 File Offset: 0x00005BF9
		[JsonIgnore]
		public override int colorG { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00007A02 File Offset: 0x00005C02
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00007A0A File Offset: 0x00005C0A
		[JsonIgnore]
		public override int colorB { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00007A13 File Offset: 0x00005C13
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00007A1B File Offset: 0x00005C1B
		[JsonIgnore]
		public override int opacity { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00007A24 File Offset: 0x00005C24
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00007A2C File Offset: 0x00005C2C
		[JsonIgnore]
		public override bool touchAble { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00007A35 File Offset: 0x00005C35
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00007A3D File Offset: 0x00005C3D
		[JsonIgnore]
		public override int ZOrder { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00007A46 File Offset: 0x00005C46
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00007A4E File Offset: 0x00005C4E
		[JsonIgnore]
		public override string classType { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00007A57 File Offset: 0x00005C57
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00007A5F File Offset: 0x00005C5F
		[JsonIgnore]
		public override int positionType { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00007A68 File Offset: 0x00005C68
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00007A70 File Offset: 0x00005C70
		[JsonIgnore]
		public override float positionPercentX { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00007A79 File Offset: 0x00005C79
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00007A81 File Offset: 0x00005C81
		[JsonIgnore]
		public override float positionPercentY { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00007A8A File Offset: 0x00005C8A
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00007A92 File Offset: 0x00005C92
		[JsonIgnore]
		public override int sizeType { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00007A9B File Offset: 0x00005C9B
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00007AA3 File Offset: 0x00005CA3
		[JsonIgnore]
		public override float sizePercentX { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007AAC File Offset: 0x00005CAC
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00007AB4 File Offset: 0x00005CB4
		[JsonIgnore]
		public override float sizePercentY { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00007ABD File Offset: 0x00005CBD
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00007AC5 File Offset: 0x00005CC5
		[JsonIgnore]
		public override bool useMergedTexture { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00007ACE File Offset: 0x00005CCE
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00007AD6 File Offset: 0x00005CD6
		[JsonIgnore]
		public override int actionTag { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00007ADF File Offset: 0x00005CDF
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00007AE7 File Offset: 0x00005CE7
		[JsonIgnore]
		public override int tag { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00007AF0 File Offset: 0x00005CF0
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00007AF8 File Offset: 0x00005CF8
		[JsonIgnore]
		public override float anchorPointX { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00007B01 File Offset: 0x00005D01
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00007B09 File Offset: 0x00005D09
		[JsonIgnore]
		public override float anchorPointY { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00007B12 File Offset: 0x00005D12
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00007B1A File Offset: 0x00005D1A
		[JsonIgnore]
		public override bool ignoreSize { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00007B23 File Offset: 0x00005D23
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00007B2B File Offset: 0x00005D2B
		[JsonIgnore]
		public override LayoutSurrogate layoutParameter { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00007B34 File Offset: 0x00005D34
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00007B3C File Offset: 0x00005D3C
		[JsonIgnore]
		public override string customProperty { get; set; }

		// Token: 0x0600028B RID: 651 RVA: 0x00007B45 File Offset: 0x00005D45
		protected RootGUISurrogate()
		{
			this.classname = "Node";
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00007B58 File Offset: 0x00005D58
		public override void SetValue(object obj)
		{
		}
	}
}
