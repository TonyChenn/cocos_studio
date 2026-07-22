using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000011 RID: 17
	[DataModelExtension(typeof(VisualObject))]
	public class VisualObjectData : BaseObjectData, IDataInitialize
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002D08 File Offset: 0x00000F08
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002D1F File Offset: 0x00000F1F
		[ItemProperty(DefaultValue = true)]
		public bool CanEdit { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002D28 File Offset: 0x00000F28
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002D3F File Offset: 0x00000F3F
		[ItemProperty(DefaultValue = true)]
		public bool Visible { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002D48 File Offset: 0x00000F48
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002D5F File Offset: 0x00000F5F
		[ItemProperty]
		[JsonProperty]
		public string InnerClassName { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002D68 File Offset: 0x00000F68
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002D7F File Offset: 0x00000F7F
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ActionTag { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002D88 File Offset: 0x00000F88
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00002D9F File Offset: 0x00000F9F
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ZOrder { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002DA8 File Offset: 0x00000FA8
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002DBF File Offset: 0x00000FBF
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool IsAutoSize { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002DC8 File Offset: 0x00000FC8
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002DDF File Offset: 0x00000FDF
		[PropertyOrder(2147483646)]
		[ItemProperty]
		[JsonProperty]
		public SizeF Size { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002DE8 File Offset: 0x00000FE8
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002DFF File Offset: 0x00000FFF
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		[DefaultValue(true)]
		public bool VisibleForFrame { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002E08 File Offset: 0x00001008
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002E1F File Offset: 0x0000101F
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 255)]
		[DefaultValue(255)]
		public int Alpha { get; set; }

		// Token: 0x0600006F RID: 111 RVA: 0x00002E28 File Offset: 0x00001028
		public VisualObjectData()
		{
			this.CanEdit = true;
			this.Visible = true;
			this.VisibleForFrame = true;
			this.Alpha = 255;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002E58 File Offset: 0x00001058
		public void DataInitialize(VisualObject vObject)
		{
			try
			{
				this.OnDataInitialize(vObject);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("VisualObject can not be Initialize directly", exception);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002E9C File Offset: 0x0000109C
		protected virtual void OnDataInitialize(VisualObject vObject)
		{
		}
	}
}
