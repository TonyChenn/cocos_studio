using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000023 RID: 35
	[DataModelExtension(typeof(PanelObject))]
	public class PanelObjectData : WidgetObjectData
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000056D0 File Offset: 0x000038D0
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000056E7 File Offset: 0x000038E7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool ClipAble { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000056F0 File Offset: 0x000038F0
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005707 File Offset: 0x00003907
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(255)]
		[ItemProperty(DefaultValue = 255)]
		public int BackColorAlpha { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00005710 File Offset: 0x00003910
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00005727 File Offset: 0x00003927
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00005730 File Offset: 0x00003930
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00005747 File Offset: 0x00003947
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		public int ComboBoxIndex { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00005750 File Offset: 0x00003950
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00005767 File Offset: 0x00003967
		[JsonProperty]
		[ItemProperty]
		public ColorData SingleColor { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00005770 File Offset: 0x00003970
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00005787 File Offset: 0x00003987
		[ItemProperty]
		[JsonProperty]
		public ColorData FirstColor { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00005790 File Offset: 0x00003990
		// (set) Token: 0x06000188 RID: 392 RVA: 0x000057A7 File Offset: 0x000039A7
		[ItemProperty]
		[JsonProperty]
		public ColorData EndColor { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000057B0 File Offset: 0x000039B0
		// (set) Token: 0x0600018A RID: 394 RVA: 0x000057C7 File Offset: 0x000039C7
		[JsonProperty]
		[ItemProperty]
		public ScaleValue ColorVector { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000057D0 File Offset: 0x000039D0
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000057E7 File Offset: 0x000039E7
		[ItemProperty(DefaultValue = 0)]
		public float ColorAngle { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000057F0 File Offset: 0x000039F0
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00005807 File Offset: 0x00003A07
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool Scale9Enable { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00005810 File Offset: 0x00003A10
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00005827 File Offset: 0x00003A27
		[ItemProperty(DefaultValue = 0)]
		public int LeftEage { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00005830 File Offset: 0x00003A30
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00005847 File Offset: 0x00003A47
		[ItemProperty(DefaultValue = 0)]
		public int RightEage { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00005850 File Offset: 0x00003A50
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00005867 File Offset: 0x00003A67
		[ItemProperty(DefaultValue = 0)]
		public int TopEage { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00005870 File Offset: 0x00003A70
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00005887 File Offset: 0x00003A87
		[ItemProperty(DefaultValue = 0)]
		public int BottomEage { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00005890 File Offset: 0x00003A90
		// (set) Token: 0x06000198 RID: 408 RVA: 0x000058A7 File Offset: 0x00003AA7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int Scale9OriginX { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000199 RID: 409 RVA: 0x000058B0 File Offset: 0x00003AB0
		// (set) Token: 0x0600019A RID: 410 RVA: 0x000058C7 File Offset: 0x00003AC7
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginY { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000058D0 File Offset: 0x00003AD0
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000058E7 File Offset: 0x00003AE7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000058F0 File Offset: 0x00003AF0
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00005907 File Offset: 0x00003B07
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9Height { get; set; }

		// Token: 0x0600019F RID: 415 RVA: 0x00005910 File Offset: 0x00003B10
		public PanelObjectData()
		{
			this.BackColorAlpha = 255;
			this.SingleColor = Color.FromArgb(255, 0, 0, 0);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005940 File Offset: 0x00003B40
		public PanelObjectData(bool bWithColor) : this()
		{
			this.ComboBoxIndex = (bWithColor ? 1 : 0);
		}
	}
}
