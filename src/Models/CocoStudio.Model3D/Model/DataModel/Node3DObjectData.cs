using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000003 RID: 3
	[DataModelExtension(typeof(Node3DObject))]
	public class Node3DObjectData : AbstractNodeObjectData
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002207 File Offset: 0x00000407
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000220F File Offset: 0x0000040F
		[JsonProperty]
		[ItemProperty]
		public ColorData CColor { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002218 File Offset: 0x00000418
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002220 File Offset: 0x00000420
		[ItemProperty]
		[JsonProperty]
		public Point3F Position3D { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002229 File Offset: 0x00000429
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002231 File Offset: 0x00000431
		[ItemProperty]
		[JsonProperty]
		public Point3F Rotation3D { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000223A File Offset: 0x0000043A
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002242 File Offset: 0x00000442
		[ItemProperty]
		[JsonProperty]
		public Point3F Scale3D { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000224B File Offset: 0x0000044B
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002253 File Offset: 0x00000453
		[JsonProperty]
		[ItemProperty]
		public int CameraFlagMode { get; set; }

		// Token: 0x06000022 RID: 34 RVA: 0x0000225C File Offset: 0x0000045C
		public Node3DObjectData()
		{
			this.CameraFlagMode = 31;
			base.Alpha = 255;
			base.VisibleForFrame = true;
			this.CColor = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}
}
