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
	internal class TextFieldSurrogate : WidgetSurrogate
	{
		[DefaultValue("微软雅黑")]
		[DataMember]
		public string fontName { get; set; }

		[DataMember]
		public ResourceDataSurrogate fontFile { get; set; }

		[DataMember]
		[DefaultValue(20)]
		public int fontSize { get; set; }

		[DataMember]
		[DefaultValue("Text Field")]
		public string text { get; set; }

		[DefaultValue("input words here")]
		[DataMember]
		public string placeHolder { get; set; }

		[DataMember]
		public bool passwordEnable { get; set; }

		[DefaultValue("*")]
		[DataMember]
		public string passwordStyleText { get; set; }

		[DataMember]
		public bool maxLengthEnable { get; set; }

		[DataMember]
		[DefaultValue(10)]
		public int maxLength { get; set; }

		[DataMember]
		public float areaWidth { get; set; }

		[DataMember]
		public float areaHeight { get; set; }

		protected TextFieldSurrogate()
		{
			this.InitDefalutValue();
		}

		private void InitDefalutValue()
		{
			this.fontName = "";
			this.fontSize = 20;
			this.text = "Text Field";
			this.passwordStyleText = "*";
			this.maxLength = 10;
			this.placeHolder = "input words here";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextFieldObjectData textFieldObjectData = obj as TextFieldObjectData;
			textFieldObjectData.FontSize = this.fontSize;
			textFieldObjectData.LabelText = this.text;
			textFieldObjectData.Size = new SizeF(this.width, this.height);
			if (this.placeHolder != null)
			{
				textFieldObjectData.PlaceHolderText = this.placeHolder;
			}
			else
			{
				textFieldObjectData.PlaceHolderText = "input words here";
			}
			textFieldObjectData.MaxLengthEnable = this.maxLengthEnable;
			textFieldObjectData.MaxLengthText = this.maxLength;
			textFieldObjectData.PasswordEnable = this.passwordEnable;
			textFieldObjectData.PasswordStyleText = this.passwordStyleText;
		}
	}
}
