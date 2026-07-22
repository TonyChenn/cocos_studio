using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000012 RID: 18
	[DataItem(Name = "AbstractNodeData")]
	[DataModelExtension(typeof(AbstractNodeObject))]
	public class AbstractNodeObjectData : VisualObjectData
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002EA0 File Offset: 0x000010A0
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002EB7 File Offset: 0x000010B7
		[ItemProperty(DefaultValue = null)]
		public ScriptFileData ScriptData { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002ED7 File Offset: 0x000010D7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(EnumCallBack.None)]
		[ItemProperty(DefaultValue = EnumCallBack.None)]
		public EnumCallBack CallBackType { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002EE0 File Offset: 0x000010E0
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002EF7 File Offset: 0x000010F7
		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string CallBackName { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002F00 File Offset: 0x00001100
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002F17 File Offset: 0x00001117
		[DefaultValue("")]
		[ItemProperty(DefaultValue = "")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string CustomClassName { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002F20 File Offset: 0x00001120
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002F37 File Offset: 0x00001137
		[DefaultValue("")]
		[ItemProperty(DefaultValue = "")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string UserData { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002F40 File Offset: 0x00001140
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002F57 File Offset: 0x00001157
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Tag { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002F60 File Offset: 0x00001160
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002F77 File Offset: 0x00001177
		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string FrameEvent { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002F80 File Offset: 0x00001180
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002F97 File Offset: 0x00001197
		[ItemProperty]
		[JsonProperty]
		public List<AbstractNodeObjectData> Children { get; set; }

		// Token: 0x06000082 RID: 130 RVA: 0x00002FA0 File Offset: 0x000011A0
		public AbstractNodeObjectData()
		{
			this.ScriptData = null;
			this.CallBackType = EnumCallBack.None;
			this.CallBackName = string.Empty;
			this.CustomClassName = string.Empty;
			this.UserData = "";
			this.Tag = 0;
			this.FrameEvent = string.Empty;
		}
	}
}
