using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000011 RID: 17
	[DataModelExtension(typeof(BoneObject))]
	public class BoneNodeObjectData : AbstractNodeObjectData
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000046FC File Offset: 0x000028FC
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004704 File Offset: 0x00002904
		[JsonProperty]
		[ItemProperty]
		public float Length { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000470D File Offset: 0x0000290D
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00004715 File Offset: 0x00002915
		[ItemProperty]
		[JsonProperty]
		public PointF Position { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007E RID: 126 RVA: 0x0000471E File Offset: 0x0000291E
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00004726 File Offset: 0x00002926
		[ItemProperty]
		[JsonProperty]
		public ScaleValue Scale { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000472F File Offset: 0x0000292F
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00004737 File Offset: 0x00002937
		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float RotationSkewX { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004740 File Offset: 0x00002940
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00004748 File Offset: 0x00002948
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		public float RotationSkewY { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004751 File Offset: 0x00002951
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00004759 File Offset: 0x00002959
		[JsonProperty]
		[ItemProperty]
		public ColorData CColor { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004762 File Offset: 0x00002962
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000476A File Offset: 0x0000296A
		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004773 File Offset: 0x00002973
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000477B File Offset: 0x0000297B
		[ItemProperty]
		public ColorData BoneColor { get; set; }

		// Token: 0x0600008A RID: 138 RVA: 0x00004784 File Offset: 0x00002984
		public BoneNodeObjectData()
		{
			this.CColor = new ColorData();
			this.BoneColor = new ColorData(byte.MaxValue, 25, 25, 25);
			this.RotationSkewX = 0f;
			this.RotationSkewY = 0f;
			this.Scale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);
			this.Position = new PointF(0f, 0f);
			base.Size = new SizeF(0f, 0f);
		}
	}
}
