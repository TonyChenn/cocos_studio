using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200002D RID: 45
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class TextFieldSurrogate : WidgetSurrogate
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000084E3 File Offset: 0x000066E3
		// (set) Token: 0x06000334 RID: 820 RVA: 0x000084EB File Offset: 0x000066EB
		[DefaultValue("微软雅黑")]
		[DataMember]
		public string fontName { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000084F4 File Offset: 0x000066F4
		// (set) Token: 0x06000336 RID: 822 RVA: 0x000084FC File Offset: 0x000066FC
		[DataMember]
		public ResourceDataSurrogate fontFile { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00008505 File Offset: 0x00006705
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000850D File Offset: 0x0000670D
		[DataMember]
		[DefaultValue(20)]
		public int fontSize { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00008516 File Offset: 0x00006716
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000851E File Offset: 0x0000671E
		[DataMember]
		[DefaultValue("Text Field")]
		public string text { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00008527 File Offset: 0x00006727
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000852F File Offset: 0x0000672F
		[DefaultValue("input words here")]
		[DataMember]
		public string placeHolder { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00008538 File Offset: 0x00006738
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00008540 File Offset: 0x00006740
		[DataMember]
		public bool passwordEnable { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00008549 File Offset: 0x00006749
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00008551 File Offset: 0x00006751
		[DefaultValue("*")]
		[DataMember]
		public string passwordStyleText { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000855A File Offset: 0x0000675A
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00008562 File Offset: 0x00006762
		[DataMember]
		public bool maxLengthEnable { get; set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000856B File Offset: 0x0000676B
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00008573 File Offset: 0x00006773
		[DataMember]
		[DefaultValue(10)]
		public int maxLength { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000857C File Offset: 0x0000677C
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00008584 File Offset: 0x00006784
		[DataMember]
		public float areaWidth { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000858D File Offset: 0x0000678D
		// (set) Token: 0x06000348 RID: 840 RVA: 0x00008595 File Offset: 0x00006795
		[DataMember]
		public float areaHeight { get; set; }

		// Token: 0x06000349 RID: 841 RVA: 0x0000859E File Offset: 0x0000679E
		protected TextFieldSurrogate()
		{
			this.InitDefalutValue();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000085AC File Offset: 0x000067AC
		private void InitDefalutValue()
		{
			this.fontName = "";
			this.fontSize = 20;
			this.text = "Text Field";
			this.passwordStyleText = "*";
			this.maxLength = 10;
			this.placeHolder = "input words here";
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000085EC File Offset: 0x000067EC
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextFieldObjectData textFieldObjectData = obj as TextFieldObjectData;
			textFieldObjectData.FontSize = this.fontSize;
			textFieldObjectData.LabelText = this.text;
			textFieldObjectData.Size = new SizeF(this.width, this.height);
			if (this.placeHolder != null)
			{
				textFieldObjectData.PlaceHolderText = this.placeHolder;
			}
			else
			{
				textFieldObjectData.PlaceHolderText = "input words here";
			}
			textFieldObjectData.MaxLengthEnable = this.maxLengthEnable;
			textFieldObjectData.MaxLengthText = this.maxLength;
			textFieldObjectData.PasswordEnable = this.passwordEnable;
			textFieldObjectData.PasswordStyleText = this.passwordStyleText;
		}
	}
}
