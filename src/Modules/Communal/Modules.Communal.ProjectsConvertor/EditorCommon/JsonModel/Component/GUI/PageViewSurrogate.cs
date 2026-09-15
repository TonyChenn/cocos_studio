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
	internal class PageViewSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string backGroundImage { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		[DataMember]
		public bool clipAble { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgColorR { get; set; }

		[DataMember]
		[DefaultValue(150)]
		public int bgColorG { get; set; }

		[DataMember]
		[DefaultValue(100)]
		public int bgColorB { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorR { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorB { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorR { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorG { get; set; }

		[DataMember]
		[DefaultValue(100)]
		public int bgEndColorB { get; set; }

		[DefaultValue(1)]
		[DataMember]
		public int colorType { get; set; }

		[DefaultValue(100)]
		[DataMember]
		public int bgColorOpacity { get; set; }

		[DataMember]
		public float vectorX { get; set; }

		[DataMember]
		[DefaultValue(-0.5f)]
		public float vectorY { get; set; }

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
		public bool backGroundScale9Enable { get; set; }

		protected PageViewSurrogate()
		{
			this.InitDefaultValue();
		}

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
