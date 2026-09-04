using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000022 RID: 34
	[DataInclude(typeof(TextHorizontalType))]
	[DataModelExtension(typeof(TextObject))]
	[DataInclude(typeof(TextVerticalType))]
	public class TextObjectData : WidgetObjectData
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000540C File Offset: 0x0000360C
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00005423 File Offset: 0x00003623
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool IsCustomSize { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000542C File Offset: 0x0000362C
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00005443 File Offset: 0x00003643
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000544C File Offset: 0x0000364C
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005463 File Offset: 0x00003663
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000546C File Offset: 0x0000366C
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005483 File Offset: 0x00003683
		[JsonProperty]
		[ItemProperty]
		public int FontSize { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000548C File Offset: 0x0000368C
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000054A3 File Offset: 0x000036A3
		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000054AC File Offset: 0x000036AC
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000054C3 File Offset: 0x000036C3
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData FontResource { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000054CC File Offset: 0x000036CC
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000054E3 File Offset: 0x000036E3
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool TouchScaleChangeAble { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000054EC File Offset: 0x000036EC
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00005503 File Offset: 0x00003703
		[DefaultValue(TextHorizontalType.HT_Left)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = TextHorizontalType.HT_Left)]
		public TextHorizontalType HorizontalAlignmentType { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000550C File Offset: 0x0000370C
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00005523 File Offset: 0x00003723
		[ItemProperty(DefaultValue = TextVerticalType.VT_Top)]
		[DefaultValue(TextVerticalType.VT_Top)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public TextVerticalType VerticalAlignmentType { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000552C File Offset: 0x0000372C
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00005543 File Offset: 0x00003743
		[ItemProperty(DefaultValue = 1)]
		[DefaultValue(1)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int OutlineSize { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000554C File Offset: 0x0000374C
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00005563 File Offset: 0x00003763
		[ItemProperty]
		[JsonProperty]
		public ColorData OutlineColor { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000556C File Offset: 0x0000376C
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00005583 File Offset: 0x00003783
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool OutlineEnabled { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000558C File Offset: 0x0000378C
		// (set) Token: 0x06000170 RID: 368 RVA: 0x000055A3 File Offset: 0x000037A3
		[ItemProperty]
		[JsonProperty]
		public ColorData ShadowColor { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000055AC File Offset: 0x000037AC
		// (set) Token: 0x06000172 RID: 370 RVA: 0x000055C3 File Offset: 0x000037C3
		[DefaultValue(2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 2)]
		public float ShadowOffsetX { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000055CC File Offset: 0x000037CC
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000055E3 File Offset: 0x000037E3
		[ItemProperty(DefaultValue = -2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(-2)]
		public float ShadowOffsetY { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000055EC File Offset: 0x000037EC
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00005603 File Offset: 0x00003803
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ShadowBlurRadius { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000560C File Offset: 0x0000380C
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00005623 File Offset: 0x00003823
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool ShadowEnabled { get; set; }

		// Token: 0x06000179 RID: 377 RVA: 0x0000562C File Offset: 0x0000382C
		public TextObjectData()
		{
			this.IsCustomSize = false;
			this.ShadowOffsetX = 2f;
			this.ShadowOffsetY = -2f;
			this.OutlineSize = 1;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005660 File Offset: 0x00003860
		protected override void OnDataInitialize(VisualObject vObject)
		{
			TextObject textObject = vObject as TextObject;
			if (textObject != null)
			{
				if ((this.IsCustomSize && this.FontResource != null && textObject.FontResource.GetResourceData().Type == this.FontResource.Type) || !this.IsCustomSize)
				{
					textObject.Size = base.Size;
				}
			}
		}
	}
}
