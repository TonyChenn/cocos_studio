using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000013 RID: 19
	[DataInclude(typeof(ScriptFileData))]
	[DataModelExtension(typeof(NodeObject))]
	[DataInclude(typeof(VerticalBerthEdge))]
	[DataInclude(typeof(HorizontalBerthEdge))]
	public class NodeObjectData : AbstractNodeObjectData
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003000 File Offset: 0x00001200
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003017 File Offset: 0x00001217
		[ItemProperty]
		[PropertyOrder(-2147483648)]
		[JsonProperty]
		public ScaleValue AnchorPoint { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003020 File Offset: 0x00001220
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003037 File Offset: 0x00001237
		[ItemProperty]
		[JsonProperty]
		public PointF Position { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003040 File Offset: 0x00001240
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00003057 File Offset: 0x00001257
		[JsonProperty]
		[ItemProperty]
		public ScaleValue Scale { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003060 File Offset: 0x00001260
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00003077 File Offset: 0x00001277
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RotationSkewX { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003080 File Offset: 0x00001280
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00003097 File Offset: 0x00001297
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RotationSkewY { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000030A0 File Offset: 0x000012A0
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000030B7 File Offset: 0x000012B7
		[ItemProperty]
		[JsonProperty]
		public ColorData CColor { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000030C0 File Offset: 0x000012C0
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000030D7 File Offset: 0x000012D7
		[ItemProperty]
		[DefaultValue(true)]
		[JsonProperty]
		public bool IconVisible { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000030E0 File Offset: 0x000012E0
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000030F7 File Offset: 0x000012F7
		[ItemProperty(DefaultValue = false)]
		public bool PrePositionEnabled { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003100 File Offset: 0x00001300
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003117 File Offset: 0x00001317
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool PositionPercentXEnabled { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003120 File Offset: 0x00001320
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003137 File Offset: 0x00001337
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool PositionPercentYEnabled { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003140 File Offset: 0x00001340
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00003157 File Offset: 0x00001357
		[JsonProperty]
		[ItemProperty]
		public PointF PrePosition { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003160 File Offset: 0x00001360
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003177 File Offset: 0x00001377
		[ItemProperty(DefaultValue = false)]
		public bool PreSizeEnable { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003180 File Offset: 0x00001380
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003197 File Offset: 0x00001397
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool PercentWidthEnable { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000031A0 File Offset: 0x000013A0
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000031B7 File Offset: 0x000013B7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool PercentHeightEnable { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000031C0 File Offset: 0x000013C0
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000031D8 File Offset: 0x000013D8
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool PercentWidthEnabled
		{
			get
			{
				return this.PercentWidthEnable;
			}
			set
			{
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000031DC File Offset: 0x000013DC
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000031F4 File Offset: 0x000013F4
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool PercentHeightEnabled
		{
			get
			{
				return this.PercentHeightEnable;
			}
			set
			{
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000031F8 File Offset: 0x000013F8
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x0000320F File Offset: 0x0000140F
		[ItemProperty]
		[JsonProperty]
		public SizeF PreSize { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003218 File Offset: 0x00001418
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000322F File Offset: 0x0000142F
		[DefaultValue(HorizontalBerthEdge.None)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = HorizontalBerthEdge.None)]
		public HorizontalBerthEdge HorizontalEdge { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003238 File Offset: 0x00001438
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000324F File Offset: 0x0000144F
		[ItemProperty(DefaultValue = VerticalBerthEdge.None)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(VerticalBerthEdge.None)]
		public VerticalBerthEdge VerticalEdge { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003258 File Offset: 0x00001458
		// (set) Token: 0x060000AA RID: 170 RVA: 0x0000326F File Offset: 0x0000146F
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float LeftMargin { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003278 File Offset: 0x00001478
		// (set) Token: 0x060000AC RID: 172 RVA: 0x0000328F File Offset: 0x0000148F
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RightMargin { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003298 File Offset: 0x00001498
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000032AF File Offset: 0x000014AF
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		public float TopMargin { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000032B8 File Offset: 0x000014B8
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000032CF File Offset: 0x000014CF
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float BottomMargin { get; set; }

		// Token: 0x060000B1 RID: 177 RVA: 0x000032D8 File Offset: 0x000014D8
		public NodeObjectData()
		{
			base.Alpha = 255;
			this.RotationSkewX = 0f;
			this.RotationSkewY = 0f;
			base.VisibleForFrame = true;
			this.Scale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);
			this.Position = new PointF(0f, 0f);
			this.CColor = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.AnchorPoint = new ScaleValue(0.5f, 0.5f, 0.1, -99999999.0, 99999999.0);
			this.PreSize = new SizeF();
			base.Size = new SizeF();
			this.PrePosition = new PointF();
		}
	}
}
