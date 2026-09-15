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
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ScrollViewSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string backGroundImage { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgColorR { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgColorG { get; set; }

		[DefaultValue(100)]
		[DataMember]
		public int bgColorB { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		[DefaultValue(255)]
		[DataMember]
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

		[DefaultValue(100)]
		[DataMember]
		public int bgEndColorB { get; set; }

		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		[DataMember]
		[DefaultValue(100)]
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

		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		[DataMember]
		[DefaultValue(200f)]
		public float innerWidth { get; set; }

		[DefaultValue(200f)]
		[DataMember]
		public float innerHeight { get; set; }

		[DataMember]
		[DefaultValue(1)]
		public int direction { get; set; }

		[DataMember]
		public bool clipAble { get; set; }

		[DataMember]
		public bool bounceEnable { get; set; }

		[DataMember]
		public int layoutType { get; set; }

		protected ScrollViewSurrogate()
		{
			this.InitDefaultValue();
		}

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
