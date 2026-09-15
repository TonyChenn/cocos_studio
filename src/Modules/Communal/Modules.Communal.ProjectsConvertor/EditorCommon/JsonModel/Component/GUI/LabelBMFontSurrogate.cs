using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelBMFontSurrogate : WidgetSurrogate
	{
		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

		[DefaultValue("Text Label")]
		[DataMember]
		public string text { get; set; }

		protected LabelBMFontSurrogate()
		{
			this.text = "Text Label";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextBMFontObjectData textBMFontObjectData = obj as TextBMFontObjectData;
			textBMFontObjectData.LabelBMFontFile_CNB = WidgetSurrogate.ConvertResourceData(this.fileNameData);
			textBMFontObjectData.LabelText = this.text;
		}
	}
}
