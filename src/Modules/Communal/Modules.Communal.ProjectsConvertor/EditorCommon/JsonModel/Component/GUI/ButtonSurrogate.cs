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
	internal class ButtonSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string normal { get; set; }

		[DataMember]
		public string pressed { get; set; }

		[DataMember]
		public string disabled { get; set; }

		[DataMember]
		public ResourceDataSurrogate normalData { get; set; }

		[DataMember]
		public ResourceDataSurrogate pressedData { get; set; }

		[DataMember]
		public ResourceDataSurrogate disabledData { get; set; }

		[DataMember]
		public string text { get; set; }

		[DefaultValue("微软雅黑")]
		[DataMember]
		public string fontName { get; set; }

		[DataMember]
		public int fontType { get; set; }

		[DefaultValue(14)]
		[DataMember]
		public int fontSize { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int textColorR { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int textColorG { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int textColorB { get; set; }

		[DataMember]
		public float capInsetsX { get; set; }

		[DataMember]
		public float capInsetsY { get; set; }

		[DataMember]
		public float capInsetsWidth { get; set; }

		[DataMember]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public float scale9Width { get; set; }

		[DataMember]
		public float scale9Height { get; set; }

		[DataMember]
		public bool scale9Enable { get; set; }

		protected ButtonSurrogate()
		{
			this.InitDefaultValue();
		}

		private void InitDefaultValue()
		{
			this.fontName = "";
			this.fontSize = 14;
			this.textColorR = 255;
			this.textColorG = 255;
			this.textColorB = 255;
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ButtonObjectData buttonObjectData = obj as ButtonObjectData;
			buttonObjectData.FlipX = this.flipX;
			buttonObjectData.FlipY = this.flipY;
			buttonObjectData.FontSize = this.fontSize;
			buttonObjectData.ButtonText = this.text;
			buttonObjectData.TextColor = new ColorData(byte.MaxValue, (byte)this.textColorR, (byte)this.textColorG, (byte)this.textColorB);
			buttonObjectData.NormalFileData = WidgetSurrogate.ConvertResourceData(this.normalData);
			buttonObjectData.PressedFileData = WidgetSurrogate.ConvertResourceData(this.pressedData);
			buttonObjectData.DisabledFileData = WidgetSurrogate.ConvertResourceData(this.disabledData);
			if (buttonObjectData.NormalFileData == null)
			{
				buttonObjectData.NormalFileData = ResourceItemData.DefaultMarker;
			}
			buttonObjectData.Scale9Enable = this.scale9Enable;
			if (buttonObjectData.Scale9Enable && buttonObjectData.NormalFileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(buttonObjectData.NormalFileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				buttonObjectData.LeftEage = leftEage;
				buttonObjectData.RightEage = rightEage;
				buttonObjectData.TopEage = topEage;
				buttonObjectData.BottomEage = bottomEage;
				buttonObjectData.Scale9OriginX = (int)this.capInsetsX;
				buttonObjectData.Scale9OriginY = (int)this.capInsetsY;
				buttonObjectData.Scale9Width = (int)this.capInsetsWidth;
				buttonObjectData.Scale9Height = (int)this.capInsetsHeight;
				buttonObjectData.DisplayState = true;
			}
			buttonObjectData.Size = new SizeF(this.width, this.height);
		}
	}
}
