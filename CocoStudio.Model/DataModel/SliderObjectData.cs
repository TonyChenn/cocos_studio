using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002D RID: 45
	[DataModelExtension(typeof(SliderObject))]
	public class SliderObjectData : WidgetObjectData
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000064EC File Offset: 0x000046EC
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00006504 File Offset: 0x00004704
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData BackGroundData
		{
			get
			{
				return this.backGroundData;
			}
			set
			{
				this.backGroundData = value;
				if (this.backGroundData == ResourceItemData.DefaultMarker)
				{
					this.backGroundData = SliderObjectData.DefaultBackgroundFile;
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00006540 File Offset: 0x00004740
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00006558 File Offset: 0x00004758
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData ProgressBarData
		{
			get
			{
				return this.progressBarData;
			}
			set
			{
				this.progressBarData = value;
				if (this.progressBarData == ResourceItemData.DefaultMarker)
				{
					this.progressBarData = SliderObjectData.DefaultProgressBarFile;
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00006594 File Offset: 0x00004794
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000065AC File Offset: 0x000047AC
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData BallNormalData
		{
			get
			{
				return this.ballNormalData;
			}
			set
			{
				this.ballNormalData = value;
				if (this.ballNormalData == ResourceItemData.DefaultMarker)
				{
					this.ballNormalData = SliderObjectData.DefaultBallNormalFile;
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000065E4 File Offset: 0x000047E4
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000065FC File Offset: 0x000047FC
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData BallPressedData
		{
			get
			{
				return this.ballPressedData;
			}
			set
			{
				this.ballPressedData = value;
				if (this.ballPressedData == ResourceItemData.DefaultMarker)
				{
					this.ballPressedData = SliderObjectData.DefaultBallPressedFile;
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00006634 File Offset: 0x00004834
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000664C File Offset: 0x0000484C
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData BallDisabledData
		{
			get
			{
				return this.ballDisabledData;
			}
			set
			{
				this.ballDisabledData = value;
				if (this.ballDisabledData == ResourceItemData.DefaultMarker)
				{
					this.ballDisabledData = SliderObjectData.DefaultBallDisabledFile;
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00006684 File Offset: 0x00004884
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000669B File Offset: 0x0000489B
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int PercentInfo { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000066A4 File Offset: 0x000048A4
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x000066BB File Offset: 0x000048BB
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		[DefaultValue(true)]
		public bool DisplayState { get; set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x000066C4 File Offset: 0x000048C4
		public SliderObjectData()
		{
			this.DisplayState = true;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000066D8 File Offset: 0x000048D8
		protected override void OnDataInitialize(VisualObject vObject)
		{
			SliderObject sliderObject = vObject as SliderObject;
			if (sliderObject != null)
			{
				if (this.BackGroundData != null && sliderObject.BackGroundData.GetResourceData().Type != this.BackGroundData.Type)
				{
					sliderObject.BackGroundData = null;
				}
			}
		}

		// Token: 0x040000CE RID: 206
		internal static readonly ResourceItemData DefaultBackgroundFile = new ResourceItemData(EnumResourceType.Default, "Default/Slider_Back.png");

		// Token: 0x040000CF RID: 207
		internal static readonly ResourceItemData DefaultProgressBarFile = new ResourceItemData(EnumResourceType.Default, "Default/Slider_PressBar.png");

		// Token: 0x040000D0 RID: 208
		internal static readonly ResourceItemData DefaultBallNormalFile = new ResourceItemData(EnumResourceType.Default, "Default/SliderNode_Normal.png");

		// Token: 0x040000D1 RID: 209
		internal static readonly ResourceItemData DefaultBallPressedFile = new ResourceItemData(EnumResourceType.Default, "Default/SliderNode_Press.png");

		// Token: 0x040000D2 RID: 210
		internal static readonly ResourceItemData DefaultBallDisabledFile = new ResourceItemData(EnumResourceType.Default, "Default/SliderNode_Disable.png");

		// Token: 0x040000D3 RID: 211
		private ResourceItemData backGroundData;

		// Token: 0x040000D4 RID: 212
		private ResourceItemData progressBarData;

		// Token: 0x040000D5 RID: 213
		private ResourceItemData ballNormalData;

		// Token: 0x040000D6 RID: 214
		private ResourceItemData ballPressedData;

		// Token: 0x040000D7 RID: 215
		private ResourceItemData ballDisabledData;
	}
}
