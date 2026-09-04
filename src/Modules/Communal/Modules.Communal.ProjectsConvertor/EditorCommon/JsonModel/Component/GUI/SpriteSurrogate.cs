using System;
using System.Runtime.Serialization;
using Mono.Addins;
using Newtonsoft.Json;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200002C RID: 44
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class SpriteSurrogate : WidgetSurrogate
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000317 RID: 791 RVA: 0x000083F5 File Offset: 0x000065F5
		// (set) Token: 0x06000318 RID: 792 RVA: 0x000083FD File Offset: 0x000065FD
		[JsonIgnore]
		public override bool touchAble { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00008406 File Offset: 0x00006606
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000840E File Offset: 0x0000660E
		[JsonIgnore]
		public override int positionType { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00008417 File Offset: 0x00006617
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000841F File Offset: 0x0000661F
		[JsonIgnore]
		public override float positionPercentX { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00008428 File Offset: 0x00006628
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00008430 File Offset: 0x00006630
		[JsonIgnore]
		public override float positionPercentY { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00008439 File Offset: 0x00006639
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00008441 File Offset: 0x00006641
		[JsonIgnore]
		public override int sizeType { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000844A File Offset: 0x0000664A
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00008452 File Offset: 0x00006652
		[JsonIgnore]
		public override float sizePercentX { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000845B File Offset: 0x0000665B
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00008463 File Offset: 0x00006663
		[JsonIgnore]
		public override float sizePercentY { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000846C File Offset: 0x0000666C
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00008474 File Offset: 0x00006674
		[JsonIgnore]
		public override bool useMergedTexture { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000847D File Offset: 0x0000667D
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00008485 File Offset: 0x00006685
		[JsonIgnore]
		public override bool ignoreSize { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000848E File Offset: 0x0000668E
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00008496 File Offset: 0x00006696
		[JsonIgnore]
		public override LayoutSurrogate layoutParameter { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000849F File Offset: 0x0000669F
		// (set) Token: 0x0600032C RID: 812 RVA: 0x000084A7 File Offset: 0x000066A7
		[JsonIgnore]
		public override string customProperty { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000084B0 File Offset: 0x000066B0
		// (set) Token: 0x0600032E RID: 814 RVA: 0x000084B8 File Offset: 0x000066B8
		[DataMember]
		public string fileName { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000084C1 File Offset: 0x000066C1
		// (set) Token: 0x06000330 RID: 816 RVA: 0x000084C9 File Offset: 0x000066C9
		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

		// Token: 0x06000331 RID: 817 RVA: 0x000084D2 File Offset: 0x000066D2
		protected SpriteSurrogate()
		{
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000084DA File Offset: 0x000066DA
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
