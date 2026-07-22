using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000021 RID: 33
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelSurrogate : WidgetSurrogate
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000069E4 File Offset: 0x00004BE4
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000069EC File Offset: 0x00004BEC
		[DataMember]
		[DefaultValue("微软雅黑")]
		public string fontName { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000069F5 File Offset: 0x00004BF5
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000069FD File Offset: 0x00004BFD
		[DataMember]
		public ResourceDataSurrogate fontFile { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006A06 File Offset: 0x00004C06
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00006A0E File Offset: 0x00004C0E
		[DefaultValue(20)]
		[DataMember]
		public int fontSize { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006A17 File Offset: 0x00004C17
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00006A1F File Offset: 0x00004C1F
		[DefaultValue("Text Label")]
		[DataMember]
		public string text { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006A28 File Offset: 0x00004C28
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00006A30 File Offset: 0x00004C30
		[DataMember]
		public float areaWidth { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00006A39 File Offset: 0x00004C39
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00006A41 File Offset: 0x00004C41
		[DataMember]
		public float areaHeight { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006A4A File Offset: 0x00004C4A
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006A52 File Offset: 0x00004C52
		[DataMember]
		public int hAlignment { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006A5B File Offset: 0x00004C5B
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006A63 File Offset: 0x00004C63
		[DataMember]
		public int vAlignment { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006A6C File Offset: 0x00004C6C
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006A74 File Offset: 0x00004C74
		[DataMember]
		public bool touchScaleEnable { get; set; }

		// Token: 0x06000189 RID: 393 RVA: 0x00006A7D File Offset: 0x00004C7D
		protected LabelSurrogate()
		{
			this.fontSize = 20;
			this.fontName = "";
			this.text = "Text Label";
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextObjectData textObjectData = obj as TextObjectData;
			textObjectData.FlipX = this.flipX;
			textObjectData.FlipY = this.flipY;
			textObjectData.FontSize = this.fontSize;
			textObjectData.LabelText = this.text;
			textObjectData.Size = new SizeF(this.width, this.height);
			textObjectData.TouchScaleChangeAble = this.touchScaleEnable;
			textObjectData.HorizontalAlignmentType = (TextHorizontalType)this.hAlignment;
			textObjectData.VerticalAlignmentType = (TextVerticalType)this.vAlignment;
			textObjectData.IsCustomSize = !this.ignoreSize;
		}
	}
}
