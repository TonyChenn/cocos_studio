using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ImageViewSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string fileName { get; set; }

		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

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
		[DefaultValue(80f)]
		public float scale9Width { get; set; }

		[DataMember]
		[DefaultValue(80f)]
		public float scale9Height { get; set; }

		[DataMember]
		public bool scale9Enable { get; set; }

		protected ImageViewSurrogate()
		{
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
			this.scale9Height = 80f;
			this.scale9Width = 80f;
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ImageViewObjectData imageViewObjectData = obj as ImageViewObjectData;
			imageViewObjectData.FlipX = this.flipX;
			imageViewObjectData.FlipY = this.flipY;
			imageViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.fileNameData);
			imageViewObjectData.Scale9Enable = this.scale9Enable;
			if (imageViewObjectData.Scale9Enable && imageViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(imageViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				imageViewObjectData.LeftEage = leftEage;
				imageViewObjectData.RightEage = rightEage;
				imageViewObjectData.TopEage = topEage;
				imageViewObjectData.BottomEage = bottomEage;
				imageViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				imageViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				imageViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				imageViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
