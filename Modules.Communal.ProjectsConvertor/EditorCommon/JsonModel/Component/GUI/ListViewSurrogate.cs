using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000023 RID: 35
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ListViewSurrogate : WidgetSurrogate
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006C45 File Offset: 0x00004E45
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006C4D File Offset: 0x00004E4D
		[DataMember]
		public string backGroundImage { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006C56 File Offset: 0x00004E56
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00006C5E File Offset: 0x00004E5E
		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00006C67 File Offset: 0x00004E67
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00006C6F File Offset: 0x00004E6F
		[DefaultValue(150)]
		[DataMember]
		public int bgColorR { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00006C78 File Offset: 0x00004E78
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00006C80 File Offset: 0x00004E80
		[DataMember]
		[DefaultValue(150)]
		public int bgColorG { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00006C89 File Offset: 0x00004E89
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00006C91 File Offset: 0x00004E91
		[DataMember]
		[DefaultValue(255)]
		public int bgColorB { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006C9A File Offset: 0x00004E9A
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006CA2 File Offset: 0x00004EA2
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006CAB File Offset: 0x00004EAB
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00006CB3 File Offset: 0x00004EB3
		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00006CBC File Offset: 0x00004EBC
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00006CC4 File Offset: 0x00004EC4
		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorB { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00006CCD File Offset: 0x00004ECD
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00006CD5 File Offset: 0x00004ED5
		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorR { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006CDE File Offset: 0x00004EDE
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00006CE6 File Offset: 0x00004EE6
		[DataMember]
		[DefaultValue(150)]
		public int bgEndColorG { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00006CEF File Offset: 0x00004EEF
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00006CF7 File Offset: 0x00004EF7
		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorB { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00006D00 File Offset: 0x00004F00
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00006D08 File Offset: 0x00004F08
		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00006D11 File Offset: 0x00004F11
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00006D19 File Offset: 0x00004F19
		[DefaultValue(100)]
		[DataMember]
		public int bgColorOpacity { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00006D22 File Offset: 0x00004F22
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00006D2A File Offset: 0x00004F2A
		[DataMember]
		public float vectorX { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00006D33 File Offset: 0x00004F33
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00006D3B File Offset: 0x00004F3B
		[DefaultValue(-0.5f)]
		[DataMember]
		public float vectorY { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00006D44 File Offset: 0x00004F44
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00006D4C File Offset: 0x00004F4C
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00006D55 File Offset: 0x00004F55
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00006D5D File Offset: 0x00004F5D
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00006D66 File Offset: 0x00004F66
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00006D6E File Offset: 0x00004F6E
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00006D77 File Offset: 0x00004F77
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00006D7F File Offset: 0x00004F7F
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00006D88 File Offset: 0x00004F88
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00006D90 File Offset: 0x00004F90
		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00006D99 File Offset: 0x00004F99
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00006DA1 File Offset: 0x00004FA1
		[DataMember]
		public float innerWidth { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00006DAA File Offset: 0x00004FAA
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00006DB2 File Offset: 0x00004FB2
		[DataMember]
		public float innerHeight { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00006DBB File Offset: 0x00004FBB
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00006DC3 File Offset: 0x00004FC3
		[DataMember]
		public bool clipAble { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00006DCC File Offset: 0x00004FCC
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00006DD4 File Offset: 0x00004FD4
		[DataMember]
		public bool bounceEnable { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00006DDD File Offset: 0x00004FDD
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00006DE5 File Offset: 0x00004FE5
		[DefaultValue(2)]
		[DataMember]
		public int direction { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00006DEE File Offset: 0x00004FEE
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00006DF6 File Offset: 0x00004FF6
		[DataMember]
		[DefaultValue(3)]
		public int gravity { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00006DFF File Offset: 0x00004FFF
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00006E07 File Offset: 0x00005007
		[DataMember]
		public int itemMargin { get; set; }

		// Token: 0x060001E0 RID: 480 RVA: 0x00006E10 File Offset: 0x00005010
		protected ListViewSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006E20 File Offset: 0x00005020
		private void InitDefaultValue()
		{
			this.bgColorR = 150;
			this.bgColorG = 150;
			this.bgColorB = 255;
			this.bgStartColorR = 255;
			this.bgStartColorG = 255;
			this.bgStartColorB = 255;
			this.bgEndColorR = 150;
			this.bgEndColorG = 150;
			this.bgEndColorB = 255;
			this.colorType = 1;
			this.bgColorOpacity = 100;
			this.vectorY = -0.5f;
			this.capInsetsWidth = 1f;
			this.capInsetsHeight = 1f;
			this.direction = 2;
			this.gravity = 3;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006ED0 File Offset: 0x000050D0
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ListViewObjectData listViewObjectData = obj as ListViewObjectData;
			listViewObjectData.ClipAble = this.clipAble;
			listViewObjectData.BackColorAlpha = this.bgColorOpacity;
			listViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			listViewObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			listViewObjectData.ColorVector = ValueConvertHelper.AngleToVector(listViewObjectData.ColorAngle);
			listViewObjectData.ComboBoxIndex = this.colorType;
			listViewObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			listViewObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			listViewObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			listViewObjectData.IsBounceEnabled = this.bounceEnable;
			listViewObjectData.ScrollDirectionType = (ScrollViewDirectionType)this.direction;
			listViewObjectData.DirectionType = (ListViewDirectionType)this.direction;
			listViewObjectData.Size = new SizeF(this.width, this.height);
			listViewObjectData.InnerNodeSize = new SizeValue((int)this.innerWidth, (int)this.innerHeight);
			listViewObjectData.ItemMargin = this.itemMargin;
			listViewObjectData.HorizontalType = (ListViewHorizontal)this.gravity;
			listViewObjectData.VerticalType = (ListViewVertical)this.gravity;
			listViewObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (listViewObjectData.Scale9Enable && listViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(listViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				listViewObjectData.LeftEage = leftEage;
				listViewObjectData.RightEage = rightEage;
				listViewObjectData.TopEage = topEage;
				listViewObjectData.BottomEage = bottomEage;
				listViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				listViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				listViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				listViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
