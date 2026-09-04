using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000022 RID: 34
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class LayoutSurrogate : BaseEntitySurrogate
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006B39 File Offset: 0x00004D39
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00006B41 File Offset: 0x00004D41
		[DataMember]
		public int type { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006B4A File Offset: 0x00004D4A
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00006B52 File Offset: 0x00004D52
		[DataMember]
		public int gravity { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006B5B File Offset: 0x00004D5B
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00006B63 File Offset: 0x00004D63
		[DataMember]
		public string relativeName { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006B6C File Offset: 0x00004D6C
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00006B74 File Offset: 0x00004D74
		[DataMember]
		public string relativeToName { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006B7D File Offset: 0x00004D7D
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00006B85 File Offset: 0x00004D85
		[DataMember]
		public int align { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006B8E File Offset: 0x00004D8E
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00006B96 File Offset: 0x00004D96
		[DataMember]
		public int marginLeft { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006B9F File Offset: 0x00004D9F
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00006BA7 File Offset: 0x00004DA7
		[DataMember]
		public int marginTop { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006BB0 File Offset: 0x00004DB0
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00006BB8 File Offset: 0x00004DB8
		[DataMember]
		public int marginRight { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00006BC1 File Offset: 0x00004DC1
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00006BC9 File Offset: 0x00004DC9
		[DataMember]
		public int marginDown { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00006BD2 File Offset: 0x00004DD2
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00006BDA File Offset: 0x00004DDA
		[DataMember]
		public int layoutEageType { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00006BE3 File Offset: 0x00004DE3
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00006BEB File Offset: 0x00004DEB
		[DataMember]
		public int layoutNormalHorizontal { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00006BF4 File Offset: 0x00004DF4
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00006BFC File Offset: 0x00004DFC
		[DataMember]
		public int layoutNormalVertical { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00006C05 File Offset: 0x00004E05
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00006C0D File Offset: 0x00004E0D
		[DataMember]
		public int layoutParentHorizontal { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00006C16 File Offset: 0x00004E16
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00006C1E File Offset: 0x00004E1E
		[DataMember]
		public int layoutParentVertical { get; set; }

		// Token: 0x060001A7 RID: 423 RVA: 0x00006C27 File Offset: 0x00004E27
		protected LayoutSurrogate()
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006C2F File Offset: 0x00004E2F
		public LayoutSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00006C43 File Offset: 0x00004E43
		public override void SetValue(object obj)
		{
		}
	}
}
