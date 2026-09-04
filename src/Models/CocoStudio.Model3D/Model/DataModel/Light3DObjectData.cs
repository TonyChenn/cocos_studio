using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000004 RID: 4
	[DataModelExtension(typeof(Light3DObject))]
	public class Light3DObjectData : Node3DObjectData
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000022A8 File Offset: 0x000004A8
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000022B0 File Offset: 0x000004B0
		[JsonProperty]
		[ItemProperty(DefaultValue = LightType.DIRECTIONAL)]
		[DefaultValue(LightType.DIRECTIONAL)]
		public LightType Type { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000022B9 File Offset: 0x000004B9
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000022C1 File Offset: 0x000004C1
		[ItemProperty]
		[JsonProperty]
		public LightFlag Flag { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000022CA File Offset: 0x000004CA
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000022D2 File Offset: 0x000004D2
		[JsonProperty]
		[ItemProperty(DefaultValue = 1f)]
		[DefaultValue(1f)]
		public float Intensity { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000022DB File Offset: 0x000004DB
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000022E3 File Offset: 0x000004E3
		[ItemProperty(DefaultValue = true)]
		[JsonProperty]
		[DefaultValue(true)]
		public bool Enable { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000022EC File Offset: 0x000004EC
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000022F4 File Offset: 0x000004F4
		[JsonProperty]
		[ItemProperty(DefaultValue = 5f)]
		[DefaultValue(5f)]
		public float Range { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000022FD File Offset: 0x000004FD
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002305 File Offset: 0x00000505
		[ItemProperty(DefaultValue = 30f)]
		[JsonProperty]
		[DefaultValue(30f)]
		public float OuterAngle { get; set; }

		// Token: 0x0600002F RID: 47 RVA: 0x0000230E File Offset: 0x0000050E
		public Light3DObjectData()
		{
			this.Enable = true;
			this.Flag = LightFlag.LIGHT0;
			this.Type = LightType.DIRECTIONAL;
			this.Range = 5f;
			this.OuterAngle = 30f;
			this.Intensity = 1f;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000234C File Offset: 0x0000054C
		protected override void OnDataInitialize(VisualObject vObject)
		{
		}
	}
}
