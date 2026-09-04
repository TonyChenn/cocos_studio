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
	// Token: 0x0200002A RID: 42
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ScrollViewSurrogate : WidgetSurrogate
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00007CBA File Offset: 0x00005EBA
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00007CC2 File Offset: 0x00005EC2
		[DataMember]
		public string backGroundImage { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00007CCB File Offset: 0x00005ECB
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x00007CD3 File Offset: 0x00005ED3
		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00007CDC File Offset: 0x00005EDC
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x00007CE4 File Offset: 0x00005EE4
		[DataMember]
		[DefaultValue(255)]
		public int bgColorR { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00007CED File Offset: 0x00005EED
		// (set) Token: 0x060002AB RID: 683 RVA: 0x00007CF5 File Offset: 0x00005EF5
		[DefaultValue(150)]
		[DataMember]
		public int bgColorG { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00007CFE File Offset: 0x00005EFE
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00007D06 File Offset: 0x00005F06
		[DefaultValue(100)]
		[DataMember]
		public int bgColorB { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00007D0F File Offset: 0x00005F0F
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00007D17 File Offset: 0x00005F17
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00007D20 File Offset: 0x00005F20
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00007D28 File Offset: 0x00005F28
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorG { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00007D31 File Offset: 0x00005F31
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x00007D39 File Offset: 0x00005F39
		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorB { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00007D42 File Offset: 0x00005F42
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00007D4A File Offset: 0x00005F4A
		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorR { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00007D53 File Offset: 0x00005F53
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00007D5B File Offset: 0x00005F5B
		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorG { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00007D64 File Offset: 0x00005F64
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00007D6C File Offset: 0x00005F6C
		[DefaultValue(100)]
		[DataMember]
		public int bgEndColorB { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00007D75 File Offset: 0x00005F75
		// (set) Token: 0x060002BB RID: 699 RVA: 0x00007D7D File Offset: 0x00005F7D
		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00007D86 File Offset: 0x00005F86
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00007D8E File Offset: 0x00005F8E
		[DataMember]
		[DefaultValue(100)]
		public int bgColorOpacity { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00007D97 File Offset: 0x00005F97
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00007D9F File Offset: 0x00005F9F
		[DataMember]
		public float vectorX { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x00007DA8 File Offset: 0x00005FA8
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00007DB0 File Offset: 0x00005FB0
		[DataMember]
		[DefaultValue(-0.5f)]
		public float vectorY { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00007DB9 File Offset: 0x00005FB9
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00007DC1 File Offset: 0x00005FC1
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00007DCA File Offset: 0x00005FCA
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00007DD2 File Offset: 0x00005FD2
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00007DDB File Offset: 0x00005FDB
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00007DE3 File Offset: 0x00005FE3
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsWidth { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00007DEC File Offset: 0x00005FEC
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00007DF4 File Offset: 0x00005FF4
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsHeight { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00007DFD File Offset: 0x00005FFD
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00007E05 File Offset: 0x00006005
		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00007E0E File Offset: 0x0000600E
		// (set) Token: 0x060002CD RID: 717 RVA: 0x00007E16 File Offset: 0x00006016
		[DataMember]
		[DefaultValue(200f)]
		public float innerWidth { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00007E1F File Offset: 0x0000601F
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00007E27 File Offset: 0x00006027
		[DefaultValue(200f)]
		[DataMember]
		public float innerHeight { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00007E30 File Offset: 0x00006030
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00007E38 File Offset: 0x00006038
		[DataMember]
		[DefaultValue(1)]
		public int direction { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00007E41 File Offset: 0x00006041
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00007E49 File Offset: 0x00006049
		[DataMember]
		public bool clipAble { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00007E52 File Offset: 0x00006052
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00007E5A File Offset: 0x0000605A
		[DataMember]
		public bool bounceEnable { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00007E63 File Offset: 0x00006063
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00007E6B File Offset: 0x0000606B
		[DataMember]
		public int layoutType { get; set; }

		// Token: 0x060002D8 RID: 728 RVA: 0x00007E74 File Offset: 0x00006074
		protected ScrollViewSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00007E84 File Offset: 0x00006084
		private void InitDefaultValue()
		{
			this.bgColorR = 255;
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
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
			this.innerHeight = 200f;
			this.innerWidth = 200f;
			this.direction = 1;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00007F3C File Offset: 0x0000613C
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ScrollViewObjectData scrollViewObjectData = obj as ScrollViewObjectData;
			scrollViewObjectData.ClipAble = this.clipAble;
			scrollViewObjectData.BackColorAlpha = this.bgColorOpacity;
			scrollViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			scrollViewObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			scrollViewObjectData.ColorVector = ValueConvertHelper.AngleToVector(scrollViewObjectData.ColorAngle);
			scrollViewObjectData.ComboBoxIndex = this.colorType;
			scrollViewObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			scrollViewObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			scrollViewObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			scrollViewObjectData.IsBounceEnabled = this.bounceEnable;
			scrollViewObjectData.Size = new SizeF(this.width, this.height);
			scrollViewObjectData.ScrollDirectionType = (ScrollViewDirectionType)this.direction;
			scrollViewObjectData.InnerNodeSize = new SizeValue((int)this.innerWidth, (int)this.innerHeight);
			scrollViewObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (scrollViewObjectData.Scale9Enable && scrollViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(scrollViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				scrollViewObjectData.LeftEage = leftEage;
				scrollViewObjectData.RightEage = rightEage;
				scrollViewObjectData.TopEage = topEage;
				scrollViewObjectData.BottomEage = bottomEage;
				scrollViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				scrollViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				scrollViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				scrollViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
