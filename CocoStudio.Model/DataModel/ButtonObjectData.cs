using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000015 RID: 21
	[DataModelExtension(typeof(ButtonObject))]
	public class ButtonObjectData : WidgetObjectData
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003450 File Offset: 0x00001650
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00003467 File Offset: 0x00001667
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003470 File Offset: 0x00001670
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003487 File Offset: 0x00001687
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool FlipY { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003490 File Offset: 0x00001690
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000034A7 File Offset: 0x000016A7
		[DefaultValue(0)]
		[JsonProperty]
		[ItemProperty(DefaultValue = 0)]
		public int FontSize { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000034B0 File Offset: 0x000016B0
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000034C7 File Offset: 0x000016C7
		[JsonProperty]
		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		public string ButtonText { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000034D0 File Offset: 0x000016D0
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000034E7 File Offset: 0x000016E7
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FontResource { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000034F0 File Offset: 0x000016F0
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00003507 File Offset: 0x00001707
		[ItemProperty]
		[JsonProperty]
		public ColorData TextColor { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003510 File Offset: 0x00001710
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00003528 File Offset: 0x00001728
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData DisabledFileData
		{
			get
			{
				return this.disabledFileData;
			}
			set
			{
				this.disabledFileData = value;
				if (this.disabledFileData == ResourceItemData.DefaultMarker)
				{
					this.disabledFileData = ButtonObjectData.Default_DisabledFile;
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003560 File Offset: 0x00001760
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003578 File Offset: 0x00001778
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData PressedFileData
		{
			get
			{
				return this.pressedFileData;
			}
			set
			{
				this.pressedFileData = value;
				if (this.pressedFileData == ResourceItemData.DefaultMarker)
				{
					this.pressedFileData = ButtonObjectData.Default_PressedFile;
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000035B0 File Offset: 0x000017B0
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000035C8 File Offset: 0x000017C8
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData NormalFileData
		{
			get
			{
				return this.normalFileData;
			}
			set
			{
				this.normalFileData = value;
				if (this.normalFileData == ResourceItemData.DefaultMarker)
				{
					this.normalFileData = ButtonObjectData.Default_NormalFile;
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003604 File Offset: 0x00001804
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000361B File Offset: 0x0000181B
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool Scale9Enable { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003624 File Offset: 0x00001824
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000363B File Offset: 0x0000183B
		[ItemProperty(DefaultValue = 0)]
		public int LeftEage { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003644 File Offset: 0x00001844
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000365B File Offset: 0x0000185B
		[ItemProperty(DefaultValue = 0)]
		public int RightEage { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003664 File Offset: 0x00001864
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x0000367B File Offset: 0x0000187B
		[ItemProperty(DefaultValue = 0)]
		public int TopEage { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003684 File Offset: 0x00001884
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000369B File Offset: 0x0000189B
		[ItemProperty(DefaultValue = 0)]
		public int BottomEage { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000036A4 File Offset: 0x000018A4
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000036BB File Offset: 0x000018BB
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginX { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000036C4 File Offset: 0x000018C4
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x000036DB File Offset: 0x000018DB
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginY { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000036E4 File Offset: 0x000018E4
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000036FB File Offset: 0x000018FB
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003704 File Offset: 0x00001904
		// (set) Token: 0x060000DD RID: 221 RVA: 0x0000371B File Offset: 0x0000191B
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		public int Scale9Height { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003724 File Offset: 0x00001924
		// (set) Token: 0x060000DF RID: 223 RVA: 0x0000373B File Offset: 0x0000193B
		[DefaultValue(true)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		public bool DisplayState { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003744 File Offset: 0x00001944
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000375B File Offset: 0x0000195B
		[ItemProperty(DefaultValue = 1)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(1)]
		public int OutlineSize { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003764 File Offset: 0x00001964
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000377B File Offset: 0x0000197B
		[ItemProperty]
		[JsonProperty]
		public ColorData OutlineColor { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003784 File Offset: 0x00001984
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000379B File Offset: 0x0000199B
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool OutlineEnabled { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000037A4 File Offset: 0x000019A4
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000037BB File Offset: 0x000019BB
		[JsonProperty]
		[ItemProperty]
		public ColorData ShadowColor { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000037C4 File Offset: 0x000019C4
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000037DB File Offset: 0x000019DB
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 2)]
		[DefaultValue(2)]
		public float ShadowOffsetX { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000037E4 File Offset: 0x000019E4
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000037FB File Offset: 0x000019FB
		[DefaultValue(-2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = -2)]
		public float ShadowOffsetY { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003804 File Offset: 0x00001A04
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000381B File Offset: 0x00001A1B
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ShadowBlurRadius { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00003824 File Offset: 0x00001A24
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000383B File Offset: 0x00001A3B
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool ShadowEnabled { get; set; }

		// Token: 0x060000F0 RID: 240 RVA: 0x00003844 File Offset: 0x00001A44
		public ButtonObjectData()
		{
			this.ButtonText = string.Empty;
			this.DisplayState = true;
			this.ShadowOffsetX = 2f;
			this.ShadowOffsetY = -2f;
			this.OutlineSize = 1;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003884 File Offset: 0x00001A84
		protected override void OnDataInitialize(VisualObject vObject)
		{
			ButtonObject buttonObject = vObject as ButtonObject;
			if (buttonObject != null)
			{
				if (this.NormalFileData != null && buttonObject.NormalFileData != null && buttonObject.NormalFileData.GetResourceData().Type != this.NormalFileData.Type)
				{
					buttonObject.NormalFileData = ResourceFile.DefaultMarker;
				}
			}
		}

		// Token: 0x04000044 RID: 68
		internal static readonly ResourceItemData Default_NormalFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Normal.png");

		// Token: 0x04000045 RID: 69
		internal static readonly ResourceItemData Default_PressedFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Press.png");

		// Token: 0x04000046 RID: 70
		internal static readonly ResourceItemData Default_DisabledFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Disable.png");

		// Token: 0x04000047 RID: 71
		private ResourceItemData disabledFileData;

		// Token: 0x04000048 RID: 72
		private ResourceItemData pressedFileData;

		// Token: 0x04000049 RID: 73
		private ResourceItemData normalFileData;
	}
}
