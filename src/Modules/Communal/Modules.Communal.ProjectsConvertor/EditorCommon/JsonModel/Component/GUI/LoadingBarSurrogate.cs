using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class LoadingBarSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string texture { get; set; }

		[DataMember]
		public ResourceDataSurrogate textureData { get; set; }

		[DefaultValue(100)]
		[DataMember]
		public int percent { get; set; }

		[DataMember]
		public int direction { get; set; }

		[DataMember]
		public float capInsetsX { get; set; }

		[DataMember]
		public float capInsetsY { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public bool scale9Enable { get; set; }

		protected LoadingBarSurrogate()
		{
			this.percent = 100;
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			LoadingBarObjectData loadingBarObjectData = obj as LoadingBarObjectData;
			loadingBarObjectData.ProgressInfo = this.percent;
			loadingBarObjectData.ImageFileData = WidgetSurrogate.ConvertResourceData(this.textureData);
			loadingBarObjectData.ProgressType = (LoadingBarDirectionType)this.direction;
		}
	}
}
