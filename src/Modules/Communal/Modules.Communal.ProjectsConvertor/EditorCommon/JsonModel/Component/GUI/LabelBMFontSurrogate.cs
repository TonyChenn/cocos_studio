using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000020 RID: 32
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelBMFontSurrogate : WidgetSurrogate
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006974 File Offset: 0x00004B74
		// (set) Token: 0x06000172 RID: 370 RVA: 0x0000697C File Offset: 0x00004B7C
		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006985 File Offset: 0x00004B85
		// (set) Token: 0x06000174 RID: 372 RVA: 0x0000698D File Offset: 0x00004B8D
		[DefaultValue("Text Label")]
		[DataMember]
		public string text { get; set; }

		// Token: 0x06000175 RID: 373 RVA: 0x00006996 File Offset: 0x00004B96
		protected LabelBMFontSurrogate()
		{
			this.text = "Text Label";
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000069AC File Offset: 0x00004BAC
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextBMFontObjectData textBMFontObjectData = obj as TextBMFontObjectData;
			textBMFontObjectData.LabelBMFontFile_CNB = WidgetSurrogate.ConvertResourceData(this.fileNameData);
			textBMFontObjectData.LabelText = this.text;
		}
	}
}
