using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200002B RID: 43
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class SliderSurrogate : WidgetSurrogate
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00008114 File Offset: 0x00006314
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000811C File Offset: 0x0000631C
		[DataMember]
		public string barFileName { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00008125 File Offset: 0x00006325
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000812D File Offset: 0x0000632D
		[DataMember]
		public string ballNormal { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00008136 File Offset: 0x00006336
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000813E File Offset: 0x0000633E
		[DataMember]
		public string ballPressed { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00008147 File Offset: 0x00006347
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000814F File Offset: 0x0000634F
		[DataMember]
		public string ballDisabled { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00008158 File Offset: 0x00006358
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00008160 File Offset: 0x00006360
		[DataMember]
		public ResourceDataSurrogate barFileNameData { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00008169 File Offset: 0x00006369
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00008171 File Offset: 0x00006371
		[DataMember]
		public ResourceDataSurrogate ballNormalData { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000817A File Offset: 0x0000637A
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x00008182 File Offset: 0x00006382
		[DataMember]
		public ResourceDataSurrogate ballPressedData { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000818B File Offset: 0x0000638B
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00008193 File Offset: 0x00006393
		[DataMember]
		public ResourceDataSurrogate ballDisabledData { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000819C File Offset: 0x0000639C
		// (set) Token: 0x060002EC RID: 748 RVA: 0x000081A4 File Offset: 0x000063A4
		[DataMember]
		public int percent { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000081AD File Offset: 0x000063AD
		// (set) Token: 0x060002EE RID: 750 RVA: 0x000081B5 File Offset: 0x000063B5
		[DataMember]
		public float capInsetsX { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060002EF RID: 751 RVA: 0x000081BE File Offset: 0x000063BE
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x000081C6 File Offset: 0x000063C6
		[DataMember]
		public float capInsetsY { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000081CF File Offset: 0x000063CF
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x000081D7 File Offset: 0x000063D7
		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsWidth { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000081E0 File Offset: 0x000063E0
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x000081E8 File Offset: 0x000063E8
		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x000081F1 File Offset: 0x000063F1
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x000081F9 File Offset: 0x000063F9
		[DataMember]
		public float barCapInsetsX { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00008202 File Offset: 0x00006402
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000820A File Offset: 0x0000640A
		[DataMember]
		public float barCapInsetsY { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00008213 File Offset: 0x00006413
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0000821B File Offset: 0x0000641B
		[DataMember]
		[DefaultValue(1f)]
		public float barCapInsetsWidth { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00008224 File Offset: 0x00006424
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000822C File Offset: 0x0000642C
		[DefaultValue(1f)]
		[DataMember]
		public float barCapInsetsHeight { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00008235 File Offset: 0x00006435
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000823D File Offset: 0x0000643D
		[DataMember]
		public float progressBarCapInsetsX { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00008246 File Offset: 0x00006446
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000824E File Offset: 0x0000644E
		[DataMember]
		public float progressBarCapInsetsY { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00008257 File Offset: 0x00006457
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000825F File Offset: 0x0000645F
		[DataMember]
		public float progressBarCapInsetsWidth { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00008268 File Offset: 0x00006468
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00008270 File Offset: 0x00006470
		[DataMember]
		public float progressBarCapInsetsHeight { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00008279 File Offset: 0x00006479
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00008281 File Offset: 0x00006481
		[DataMember]
		public float scale9Width { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000828A File Offset: 0x0000648A
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00008292 File Offset: 0x00006492
		[DataMember]
		public float scale9Height { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000829B File Offset: 0x0000649B
		// (set) Token: 0x0600030A RID: 778 RVA: 0x000082A3 File Offset: 0x000064A3
		[DataMember]
		public bool scale9Enable { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000082AC File Offset: 0x000064AC
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000082B4 File Offset: 0x000064B4
		[DataMember]
		public float slidBallAnchorPointX { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000082BD File Offset: 0x000064BD
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000082C5 File Offset: 0x000064C5
		[DataMember]
		public float slidBallAnchorPointY { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000082CE File Offset: 0x000064CE
		// (set) Token: 0x06000310 RID: 784 RVA: 0x000082D6 File Offset: 0x000064D6
		[DataMember]
		[DefaultValue(290f)]
		public float length { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000082DF File Offset: 0x000064DF
		// (set) Token: 0x06000312 RID: 786 RVA: 0x000082E7 File Offset: 0x000064E7
		[DataMember]
		public bool progressBarVisible { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000082F0 File Offset: 0x000064F0
		// (set) Token: 0x06000314 RID: 788 RVA: 0x000082F8 File Offset: 0x000064F8
		[DataMember]
		public ResourceDataSurrogate progressBarData { get; set; }

		// Token: 0x06000315 RID: 789 RVA: 0x00008301 File Offset: 0x00006501
		protected SliderSurrogate()
		{
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
			this.barCapInsetsHeight = 1f;
			this.barCapInsetsWidth = 1f;
			this.length = 290f;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008340 File Offset: 0x00006540
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			SliderObjectData sliderObjectData = obj as SliderObjectData;
			sliderObjectData.BackGroundData = WidgetSurrogate.ConvertResourceData(this.barFileNameData);
			sliderObjectData.BallNormalData = WidgetSurrogate.ConvertResourceData(this.ballNormalData);
			sliderObjectData.BallPressedData = WidgetSurrogate.ConvertResourceData(this.ballPressedData);
			sliderObjectData.BallDisabledData = WidgetSurrogate.ConvertResourceData(this.ballDisabledData);
			sliderObjectData.ProgressBarData = WidgetSurrogate.ConvertResourceData(this.progressBarData);
			if (sliderObjectData.BallNormalData == null)
			{
				sliderObjectData.BallNormalData = ResourceItemData.DefaultMarker;
			}
			if (sliderObjectData.BackGroundData == null)
			{
				sliderObjectData.BackGroundData = ResourceItemData.DefaultMarker;
			}
			sliderObjectData.PercentInfo = this.percent;
			sliderObjectData.DisplayState = true;
		}
	}
}
