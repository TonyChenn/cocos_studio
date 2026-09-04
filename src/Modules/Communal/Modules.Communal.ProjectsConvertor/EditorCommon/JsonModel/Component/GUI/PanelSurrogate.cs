using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000027 RID: 39
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class PanelSurrogate : WidgetSurrogate
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000075A3 File Offset: 0x000057A3
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000075AB File Offset: 0x000057AB
		[DataMember]
		public string backGroundImage { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000075B4 File Offset: 0x000057B4
		// (set) Token: 0x06000229 RID: 553 RVA: 0x000075BC File Offset: 0x000057BC
		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000075C5 File Offset: 0x000057C5
		// (set) Token: 0x0600022B RID: 555 RVA: 0x000075CD File Offset: 0x000057CD
		[DataMember]
		public bool clipAble { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000075D6 File Offset: 0x000057D6
		// (set) Token: 0x0600022D RID: 557 RVA: 0x000075DE File Offset: 0x000057DE
		[DefaultValue(150)]
		[DataMember]
		public int bgColorR { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000075E7 File Offset: 0x000057E7
		// (set) Token: 0x0600022F RID: 559 RVA: 0x000075EF File Offset: 0x000057EF
		[DataMember]
		[DefaultValue(200)]
		public int bgColorG { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000075F8 File Offset: 0x000057F8
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00007600 File Offset: 0x00005800
		[DefaultValue(255)]
		[DataMember]
		public int bgColorB { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00007609 File Offset: 0x00005809
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00007611 File Offset: 0x00005811
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000761A File Offset: 0x0000581A
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00007622 File Offset: 0x00005822
		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000762B File Offset: 0x0000582B
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00007633 File Offset: 0x00005833
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorB { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000763C File Offset: 0x0000583C
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00007644 File Offset: 0x00005844
		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorR { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000764D File Offset: 0x0000584D
		// (set) Token: 0x0600023B RID: 571 RVA: 0x00007655 File Offset: 0x00005855
		[DefaultValue(200)]
		[DataMember]
		public int bgEndColorG { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000765E File Offset: 0x0000585E
		// (set) Token: 0x0600023D RID: 573 RVA: 0x00007666 File Offset: 0x00005866
		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorB { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000766F File Offset: 0x0000586F
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00007677 File Offset: 0x00005877
		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00007680 File Offset: 0x00005880
		// (set) Token: 0x06000241 RID: 577 RVA: 0x00007688 File Offset: 0x00005888
		[DataMember]
		[DefaultValue(100)]
		public int bgColorOpacity { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00007691 File Offset: 0x00005891
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00007699 File Offset: 0x00005899
		[DefaultValue(0f)]
		[DataMember]
		public float vectorX { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000076A2 File Offset: 0x000058A2
		// (set) Token: 0x06000245 RID: 581 RVA: 0x000076AA File Offset: 0x000058AA
		[DefaultValue(-0.5f)]
		[DataMember]
		public float vectorY { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000246 RID: 582 RVA: 0x000076B3 File Offset: 0x000058B3
		// (set) Token: 0x06000247 RID: 583 RVA: 0x000076BB File Offset: 0x000058BB
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000076C4 File Offset: 0x000058C4
		// (set) Token: 0x06000249 RID: 585 RVA: 0x000076CC File Offset: 0x000058CC
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000076D5 File Offset: 0x000058D5
		// (set) Token: 0x0600024B RID: 587 RVA: 0x000076DD File Offset: 0x000058DD
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000076E6 File Offset: 0x000058E6
		// (set) Token: 0x0600024D RID: 589 RVA: 0x000076EE File Offset: 0x000058EE
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsHeight { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000076F7 File Offset: 0x000058F7
		// (set) Token: 0x0600024F RID: 591 RVA: 0x000076FF File Offset: 0x000058FF
		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00007708 File Offset: 0x00005908
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00007710 File Offset: 0x00005910
		[DataMember]
		public int layoutType { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00007719 File Offset: 0x00005919
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00007721 File Offset: 0x00005921
		[DataMember]
		public bool adaptScreen { get; set; }

		// Token: 0x06000254 RID: 596 RVA: 0x0000772A File Offset: 0x0000592A
		protected PanelSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00007738 File Offset: 0x00005938
		private void InitDefaultValue()
		{
			this.bgColorR = 150;
			this.bgColorG = 200;
			this.bgColorB = 255;
			this.bgStartColorR = 255;
			this.bgStartColorG = 255;
			this.bgStartColorB = 255;
			this.bgEndColorR = 150;
			this.bgEndColorG = 200;
			this.bgEndColorB = 255;
			this.colorType = 1;
			this.bgColorOpacity = 100;
			this.vectorX = 0f;
			this.vectorY = -0.5f;
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000077E4 File Offset: 0x000059E4
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			PanelObjectData panelObjectData = obj as PanelObjectData;
			panelObjectData.ClipAble = this.clipAble;
			panelObjectData.BackColorAlpha = this.bgColorOpacity;
			panelObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			panelObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			panelObjectData.ColorVector = ValueConvertHelper.AngleToVector(panelObjectData.ColorAngle);
			panelObjectData.Size = new SizeF(this.width, this.height);
			panelObjectData.ComboBoxIndex = this.colorType;
			panelObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			panelObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			panelObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			panelObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (panelObjectData.Scale9Enable && panelObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(panelObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				panelObjectData.LeftEage = leftEage;
				panelObjectData.RightEage = rightEage;
				panelObjectData.TopEage = topEage;
				panelObjectData.BottomEage = bottomEage;
				panelObjectData.Scale9OriginX = (int)this.capInsetsX;
				panelObjectData.Scale9OriginY = (int)this.capInsetsY;
				panelObjectData.Scale9Width = (int)this.capInsetsWidth;
				panelObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
