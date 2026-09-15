using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class PanelSurrogate : WidgetSurrogate
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
		[DefaultValue(200)]
		public int bgColorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgColorB { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorB { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorR { get; set; }

		[DefaultValue(200)]
		[DataMember]
		public int bgEndColorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorB { get; set; }

		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		[DataMember]
		[DefaultValue(100)]
		public int bgColorOpacity { get; set; }

		[DefaultValue(0f)]
		[DataMember]
		public float vectorX { get; set; }

		[DefaultValue(-0.5f)]
		[DataMember]
		public float vectorY { get; set; }

		[DataMember]
		public float capInsetsX { get; set; }

		[DataMember]
		public float capInsetsY { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		[DataMember]
		[DefaultValue(1f)]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		[DataMember]
		public int layoutType { get; set; }

		[DataMember]
		public bool adaptScreen { get; set; }

		protected PanelSurrogate()
		{
			this.InitDefaultValue();
		}

		private void InitDefaultValue()
		{
			this.bgColorR = 150;
			this.bgColorG = 200;
			this.bgColorB = 255;
			this.bgStartColorR = 255;
			this.bgStartColorG = 255;
			this.bgStartColorB = 255;
			this.bgEndColorR = 150;
			this.bgEndColorG = 200;
			this.bgEndColorB = 255;
			this.colorType = 1;
			this.bgColorOpacity = 100;
			this.vectorX = 0f;
			this.vectorY = -0.5f;
			this.capInsetsHeight = 1f;
			this.capInsetsWidth = 1f;
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			PanelObjectData panelObjectData = obj as PanelObjectData;
			panelObjectData.ClipAble = this.clipAble;
			panelObjectData.BackColorAlpha = this.bgColorOpacity;
			panelObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			panelObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			panelObjectData.ColorVector = ValueConvertHelper.AngleToVector(panelObjectData.ColorAngle);
			panelObjectData.Size = new SizeF(this.width, this.height);
			panelObjectData.ComboBoxIndex = this.colorType;
			panelObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			panelObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			panelObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			panelObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (panelObjectData.Scale9Enable && panelObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(panelObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				panelObjectData.LeftEage = leftEage;
				panelObjectData.RightEage = rightEage;
				panelObjectData.TopEage = topEage;
				panelObjectData.BottomEage = bottomEage;
				panelObjectData.Scale9OriginX = (int)this.capInsetsX;
				panelObjectData.Scale9OriginY = (int)this.capInsetsY;
				panelObjectData.Scale9Width = (int)this.capInsetsWidth;
				panelObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
