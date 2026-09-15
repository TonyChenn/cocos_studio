using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelSurrogate : WidgetSurrogate
	{
		[DataMember]
		[DefaultValue("微软雅黑")]
		public string fontName { get; set; }

		[DataMember]
		public ResourceDataSurrogate fontFile { get; set; }

		[DefaultValue(20)]
		[DataMember]
		public int fontSize { get; set; }

		[DefaultValue("Text Label")]
		[DataMember]
		public string text { get; set; }

		[DataMember]
		public float areaWidth { get; set; }

		[DataMember]
		public float areaHeight { get; set; }

		[DataMember]
		public int hAlignment { get; set; }

		[DataMember]
		public int vAlignment { get; set; }

		[DataMember]
		public bool touchScaleEnable { get; set; }

		protected LabelSurrogate()
		{
			this.fontSize = 20;
			this.fontName = "";
			this.text = "Text Label";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextObjectData textObjectData = obj as TextObjectData;
			textObjectData.FlipX = this.flipX;
			textObjectData.FlipY = this.flipY;
			textObjectData.FontSize = this.fontSize;
			textObjectData.LabelText = this.text;
			textObjectData.Size = new SizeF(this.width, this.height);
			textObjectData.TouchScaleChangeAble = this.touchScaleEnable;
			textObjectData.HorizontalAlignmentType = (TextHorizontalType)this.hAlignment;
			textObjectData.VerticalAlignmentType = (TextVerticalType)this.vAlignment;
			textObjectData.IsCustomSize = !this.ignoreSize;
		}
	}
}
