using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002F RID: 47
	[DataModelExtension(typeof(TextFieldObject))]
	public class TextFieldObjectData : WidgetObjectData
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000068D0 File Offset: 0x00004AD0
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000068E7 File Offset: 0x00004AE7
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FontResource { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000068F0 File Offset: 0x00004AF0
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00006907 File Offset: 0x00004B07
		[ItemProperty]
		[JsonProperty]
		public int FontSize { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00006910 File Offset: 0x00004B10
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00006927 File Offset: 0x00004B27
		[JsonProperty]
		[ItemProperty]
		public bool IsCustomSize { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00006930 File Offset: 0x00004B30
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00006947 File Offset: 0x00004B47
		[ItemProperty]
		[JsonProperty]
		public string LabelText { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00006950 File Offset: 0x00004B50
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00006967 File Offset: 0x00004B67
		[JsonProperty]
		[ItemProperty]
		public string PlaceHolderText { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00006970 File Offset: 0x00004B70
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00006987 File Offset: 0x00004B87
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool MaxLengthEnable { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00006990 File Offset: 0x00004B90
		// (set) Token: 0x06000211 RID: 529 RVA: 0x000069A7 File Offset: 0x00004BA7
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int MaxLengthText { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000069B0 File Offset: 0x00004BB0
		// (set) Token: 0x06000213 RID: 531 RVA: 0x000069C7 File Offset: 0x00004BC7
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool PasswordEnable { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000069D0 File Offset: 0x00004BD0
		// (set) Token: 0x06000215 RID: 533 RVA: 0x000069E7 File Offset: 0x00004BE7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = "*")]
		[DefaultValue("*")]
		public string PasswordStyleText { get; set; }

		// Token: 0x06000216 RID: 534 RVA: 0x000069F0 File Offset: 0x00004BF0
		public TextFieldObjectData()
		{
			this.PasswordStyleText = "*";
			this.IsCustomSize = true;
		}
	}
}
