using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000026 RID: 38
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class PageViewSurrogate : WidgetSurrogate
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000071ED File Offset: 0x000053ED
		// (set) Token: 0x060001FA RID: 506 RVA: 0x000071F5 File Offset: 0x000053F5
		[DataMember]
		public string backGroundImage { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000071FE File Offset: 0x000053FE
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00007206 File Offset: 0x00005406
		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000720F File Offset: 0x0000540F
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00007217 File Offset: 0x00005417
		[DataMember]
		public bool clipAble { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007220 File Offset: 0x00005420
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00007228 File Offset: 0x00005428
		[DefaultValue(150)]
		[DataMember]
		public int bgColorR { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007231 File Offset: 0x00005431
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007239 File Offset: 0x00005439
		[DataMember]
		[DefaultValue(150)]
		public int bgColorG { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00007242 File Offset: 0x00005442
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000724A File Offset: 0x0000544A
		[DataMember]
		[DefaultValue(100)]
		public int bgColorB { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00007253 File Offset: 0x00005453
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000725B File Offset: 0x0000545B
		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorR { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007264 File Offset: 0x00005464
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000726C File Offset: 0x0000546C
		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00007275 File Offset: 0x00005475
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000727D File Offset: 0x0000547D
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorB { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007286 File Offset: 0x00005486
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000728E File Offset: 0x0000548E
		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorR { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007297 File Offset: 0x00005497
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000729F File Offset: 0x0000549F
		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorG { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000072A8 File Offset: 0x000054A8
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000072B0 File Offset: 0x000054B0
		[DataMember]
		[DefaultValue(100)]
		public int bgEndColorB { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000072B9 File Offset: 0x000054B9
		// (set) Token: 0x06000212 RID: 530 RVA: 0x000072C1 File Offset: 0x000054C1
		[DefaultValue(1)]
		[DataMember]
		public int colorType { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000072CA File Offset: 0x000054CA
		// (set) Token: 0x06000214 RID: 532 RVA: 0x000072D2 File Offset: 0x000054D2
		[DefaultValue(100)]
		[DataMember]
		public int bgColorOpacity { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000072DB File Offset: 0x000054DB
		// (set) Token: 0x06000216 RID: 534 RVA: 0x000072E3 File Offset: 0x000054E3
		[DataMember]
		public float vectorX { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000072EC File Offset: 0x000054EC
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000072F4 File Offset: 0x000054F4
		[DataMember]
		[DefaultValue(-0.5f)]
		public float vectorY { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000072FD File Offset: 0x000054FD
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00007305 File Offset: 0x00005505
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000730E File Offset: 0x0000550E
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00007316 File Offset: 0x00005516
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000731F File Offset: 0x0000551F
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00007327 File Offset: 0x00005527
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsWidth { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00007330 File Offset: 0x00005530
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00007338 File Offset: 0x00005538
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00007341 File Offset: 0x00005541
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00007349 File Offset: 0x00005549
		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		// Token: 0x06000223 RID: 547 RVA: 0x00007352 File Offset: 0x00005552
		protected PageViewSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00007360 File Offset: 0x00005560
		private void InitDefaultValue()
		{
			this.bgColorR = 150;
			this.bgColorG = 150;
			this.bgColorB = 100;
			this.bgStartColorR = 255;
			this.bgStartColorG = 255;
			this.bgStartColorB = 255;
			this.bgEndColorR = 255;
			this.bgEndColorG = 150;
			this.bgEndColorB = 100;
			this.colorType = 1;
			this.bgColorOpacity = 100;
			this.vectorY = -0.5f;
			this.capInsetsWidth = 1f;
			this.capInsetsHeight = 1f;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000073FC File Offset: 0x000055FC
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			PageViewObjectData pageViewObjectData = obj as PageViewObjectData;
			pageViewObjectData.ClipAble = this.clipAble;
			pageViewObjectData.BackColorAlpha = this.bgColorOpacity;
			pageViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			pageViewObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			pageViewObjectData.ColorVector = ValueConvertHelper.AngleToVector(pageViewObjectData.ColorAngle);
			pageViewObjectData.ComboBoxIndex = this.colorType;
			pageViewObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			pageViewObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			pageViewObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			pageViewObjectData.Size = new SizeF(this.width, this.height);
			pageViewObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (pageViewObjectData.Scale9Enable && pageViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(pageViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				pageViewObjectData.LeftEage = leftEage;
				pageViewObjectData.RightEage = rightEage;
				pageViewObjectData.TopEage = topEage;
				pageViewObjectData.BottomEage = bottomEage;
				pageViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				pageViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				pageViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				pageViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
