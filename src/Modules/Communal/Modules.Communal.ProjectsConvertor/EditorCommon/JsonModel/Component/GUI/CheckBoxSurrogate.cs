using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200001C RID: 28
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class CheckBoxSurrogate : WidgetSurrogate
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000559B File Offset: 0x0000379B
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000055A3 File Offset: 0x000037A3
		[DataMember]
		public string backGroundBox { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000055AC File Offset: 0x000037AC
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000055B4 File Offset: 0x000037B4
		[DataMember]
		public string backGroundBoxSelected { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000055BD File Offset: 0x000037BD
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000055C5 File Offset: 0x000037C5
		[DataMember]
		public string backGroundBoxDisabled { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000055CE File Offset: 0x000037CE
		// (set) Token: 0x06000132 RID: 306 RVA: 0x000055D6 File Offset: 0x000037D6
		[DataMember]
		public string frontCross { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000055DF File Offset: 0x000037DF
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000055E7 File Offset: 0x000037E7
		[DataMember]
		public string frontCrossDisabled { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000055F0 File Offset: 0x000037F0
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000055F8 File Offset: 0x000037F8
		[DataMember]
		public ResourceDataSurrogate backGroundBoxData { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005601 File Offset: 0x00003801
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00005609 File Offset: 0x00003809
		[DataMember]
		public ResourceDataSurrogate backGroundBoxSelectedData { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005612 File Offset: 0x00003812
		// (set) Token: 0x0600013A RID: 314 RVA: 0x0000561A File Offset: 0x0000381A
		[DataMember]
		public ResourceDataSurrogate backGroundBoxDisabledData { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005623 File Offset: 0x00003823
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000562B File Offset: 0x0000382B
		[DataMember]
		public ResourceDataSurrogate frontCrossData { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005634 File Offset: 0x00003834
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000563C File Offset: 0x0000383C
		[DataMember]
		public ResourceDataSurrogate frontCrossDisabledData { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005645 File Offset: 0x00003845
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000564D File Offset: 0x0000384D
		[DataMember]
		public bool selectedState { get; set; }

		// Token: 0x06000141 RID: 321 RVA: 0x00005656 File Offset: 0x00003856
		protected CheckBoxSurrogate()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005660 File Offset: 0x00003860
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			CheckBoxObjectData checkBoxObjectData = obj as CheckBoxObjectData;
			checkBoxObjectData.CheckedState = this.selectedState;
			checkBoxObjectData.NormalBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxData);
			checkBoxObjectData.PressedBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxSelectedData);
			checkBoxObjectData.DisableBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxDisabledData);
			checkBoxObjectData.NodeNormalFileData = WidgetSurrogate.ConvertResourceData(this.frontCrossData);
			checkBoxObjectData.NodeDisableFileData = WidgetSurrogate.ConvertResourceData(this.frontCrossDisabledData);
			if (checkBoxObjectData.NormalBackFileData == null)
			{
				checkBoxObjectData.NormalBackFileData = ResourceItemData.DefaultMarker;
			}
			if (checkBoxObjectData.NodeNormalFileData == null)
			{
				checkBoxObjectData.NodeNormalFileData = ResourceItemData.DefaultMarker;
			}
			checkBoxObjectData.DisplayState = true;
		}
	}
}
