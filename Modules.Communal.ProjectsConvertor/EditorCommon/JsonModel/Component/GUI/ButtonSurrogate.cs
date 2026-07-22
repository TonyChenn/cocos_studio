using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200001B RID: 27
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ButtonSurrogate : WidgetSurrogate
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000527E File Offset: 0x0000347E
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005286 File Offset: 0x00003486
		[DataMember]
		public string normal { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000528F File Offset: 0x0000348F
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005297 File Offset: 0x00003497
		[DataMember]
		public string pressed { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000052A0 File Offset: 0x000034A0
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000052A8 File Offset: 0x000034A8
		[DataMember]
		public string disabled { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000052B1 File Offset: 0x000034B1
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000052B9 File Offset: 0x000034B9
		[DataMember]
		public ResourceDataSurrogate normalData { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000052C2 File Offset: 0x000034C2
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000052CA File Offset: 0x000034CA
		[DataMember]
		public ResourceDataSurrogate pressedData { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000052D3 File Offset: 0x000034D3
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000052DB File Offset: 0x000034DB
		[DataMember]
		public ResourceDataSurrogate disabledData { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000052E4 File Offset: 0x000034E4
		// (set) Token: 0x0600010D RID: 269 RVA: 0x000052EC File Offset: 0x000034EC
		[DataMember]
		public string text { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000052F5 File Offset: 0x000034F5
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000052FD File Offset: 0x000034FD
		[DefaultValue("微软雅黑")]
		[DataMember]
		public string fontName { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005306 File Offset: 0x00003506
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000530E File Offset: 0x0000350E
		[DataMember]
		public int fontType { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005317 File Offset: 0x00003517
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000531F File Offset: 0x0000351F
		[DefaultValue(14)]
		[DataMember]
		public int fontSize { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005328 File Offset: 0x00003528
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00005330 File Offset: 0x00003530
		[DefaultValue(255)]
		[DataMember]
		public int textColorR { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005339 File Offset: 0x00003539
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00005341 File Offset: 0x00003541
		[DataMember]
		[DefaultValue(255)]
		public int textColorG { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000534A File Offset: 0x0000354A
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00005352 File Offset: 0x00003552
		[DataMember]
		[DefaultValue(255)]
		public int textColorB { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000535B File Offset: 0x0000355B
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00005363 File Offset: 0x00003563
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000536C File Offset: 0x0000356C
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00005374 File Offset: 0x00003574
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000537D File Offset: 0x0000357D
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005385 File Offset: 0x00003585
		[DataMember]
		public float capInsetsWidth { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000538E File Offset: 0x0000358E
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005396 File Offset: 0x00003596
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000539F File Offset: 0x0000359F
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000053A7 File Offset: 0x000035A7
		[DataMember]
		public float scale9Width { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000053B0 File Offset: 0x000035B0
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000053B8 File Offset: 0x000035B8
		[DataMember]
		public float scale9Height { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000053C1 File Offset: 0x000035C1
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000053C9 File Offset: 0x000035C9
		[DataMember]
		public bool scale9Enable { get; set; }

		// Token: 0x06000128 RID: 296 RVA: 0x000053D2 File Offset: 0x000035D2
		protected ButtonSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000053E0 File Offset: 0x000035E0
		private void InitDefaultValue()
		{
			this.fontName = "";
			this.fontSize = 14;
			this.textColorR = 255;
			this.textColorG = 255;
			this.textColorB = 255;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005418 File Offset: 0x00003618
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ButtonObjectData buttonObjectData = obj as ButtonObjectData;
			buttonObjectData.FlipX = this.flipX;
			buttonObjectData.FlipY = this.flipY;
			buttonObjectData.FontSize = this.fontSize;
			buttonObjectData.ButtonText = this.text;
			buttonObjectData.TextColor = new ColorData(byte.MaxValue, (byte)this.textColorR, (byte)this.textColorG, (byte)this.textColorB);
			buttonObjectData.NormalFileData = WidgetSurrogate.ConvertResourceData(this.normalData);
			buttonObjectData.PressedFileData = WidgetSurrogate.ConvertResourceData(this.pressedData);
			buttonObjectData.DisabledFileData = WidgetSurrogate.ConvertResourceData(this.disabledData);
			if (buttonObjectData.NormalFileData == null)
			{
				buttonObjectData.NormalFileData = ResourceItemData.DefaultMarker;
			}
			buttonObjectData.Scale9Enable = this.scale9Enable;
			if (buttonObjectData.Scale9Enable && buttonObjectData.NormalFileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(buttonObjectData.NormalFileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				buttonObjectData.LeftEage = leftEage;
				buttonObjectData.RightEage = rightEage;
				buttonObjectData.TopEage = topEage;
				buttonObjectData.BottomEage = bottomEage;
				buttonObjectData.Scale9OriginX = (int)this.capInsetsX;
				buttonObjectData.Scale9OriginY = (int)this.capInsetsY;
				buttonObjectData.Scale9Width = (int)this.capInsetsWidth;
				buttonObjectData.Scale9Height = (int)this.capInsetsHeight;
				buttonObjectData.DisplayState = true;
			}
			buttonObjectData.Size = new SizeF(this.width, this.height);
		}
	}
}
