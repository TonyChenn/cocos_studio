using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class SliderSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string barFileName { get; set; }

		[DataMember]
		public string ballNormal { get; set; }

		[DataMember]
		public string ballPressed { get; set; }

		[DataMember]
		public string ballDisabled { get; set; }

		[DataMember]
		public ResourceDataSurrogate barFileNameData { get; set; }

		[DataMember]
		public ResourceDataSurrogate ballNormalData { get; set; }

		[DataMember]
		public ResourceDataSurrogate ballPressedData { get; set; }

		[DataMember]
		public ResourceDataSurrogate ballDisabledData { get; set; }

		[DataMember]
		public int percent { get; set; }

		[DataMember]
		public float capInsetsX { get; set; }

		[DataMember]
		public float capInsetsY { get; set; }

		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsWidth { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public float barCapInsetsX { get; set; }

		[DataMember]
		public float barCapInsetsY { get; set; }

		[DataMember]
		[DefaultValue(1f)]
		public float barCapInsetsWidth { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float barCapInsetsHeight { get; set; }

		[DataMember]
		public float progressBarCapInsetsX { get; set; }

		[DataMember]
		public float progressBarCapInsetsY { get; set; }

		[DataMember]
		public float progressBarCapInsetsWidth { get; set; }

		[DataMember]
		public float progressBarCapInsetsHeight { get; set; }

		[DataMember]
		public float scale9Width { get; set; }

		[DataMember]
		public float scale9Height { get; set; }

		[DataMember]
		public bool scale9Enable { get; set; }

		[DataMember]
		public float slidBallAnchorPointX { get; set; }

		[DataMember]
		public float slidBallAnchorPointY { get; set; }

		[DataMember]
		[DefaultValue(290f)]
		public float length { get; set; }

		[DataMember]
		public bool progressBarVisible { get; set; }

		[DataMember]
		public ResourceDataSurrogate progressBarData { get; set; }

		protected SliderSurrogate()
		{
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
			this.barCapInsetsHeight = 1f;
			this.barCapInsetsWidth = 1f;
			this.length = 290f;
		}

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
