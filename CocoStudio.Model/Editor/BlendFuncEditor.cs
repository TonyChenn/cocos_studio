using System;
using System.Linq;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000051 RID: 81
	internal class BlendFuncEditor : BaseEditor
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00009430 File Offset: 0x00007630
		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00009444 File Offset: 0x00007644
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00009458 File Offset: 0x00007658
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000946C File Offset: 0x0000766C
		protected override void OnSetSensitive(bool isSensitive)
		{
			Widget widget = this.srcEditor;
			this.dstEditor.Sensitive = isSensitive;
			widget.Sensitive = isSensitive;
			Widget widget2 = this.btnNormal;
			this.btnAdditive.Sensitive = isSensitive;
			widget2.Sensitive = isSensitive;
			this.warningIcon.Visible = !isSensitive;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000094C4 File Offset: 0x000076C4
		protected override Widget OnCreateWidget()
		{
			this.table = new Table(2U, 2U, false);
			this.table.ColumnSpacing = PropertyPadStyle.mainColumnSpacing;
			this.table.RowSpacing = PropertyPadStyle.mainRowSpacing;
			Label label = new Label(LanguageInfo.Animation_Blend_MixSource);
			Label label2 = new Label(LanguageInfo.Animation_Blend_MixAim);
			label.Xalign = (label2.Xalign = 1f);
			label.WidthRequest = (label2.WidthRequest = PropertyPadStyle.propertyLabelWidth);
			this.table.Attach(label, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(label2, 0U, 1U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.srcEditor = new EnumEditorComboBox();
			BorderEventBox borderEventBox = new BorderEventBox();
			borderEventBox.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			borderEventBox.ModifyBg(StateType.Insensitive, WindowStyle.LineDarkColor);
			borderEventBox.Add(this.srcEditor);
			this.dstEditor = new EnumEditorComboBox();
			BorderEventBox borderEventBox2 = new BorderEventBox();
			borderEventBox2.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			borderEventBox2.ModifyBg(StateType.Insensitive, WindowStyle.LineDarkColor);
			borderEventBox2.Add(this.dstEditor);
			this.table.Attach(borderEventBox, 1U, 2U, 0U, 1U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(borderEventBox2, 1U, 2U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.InitEnumEditors();
			this.btnNormal = new Button();
			this.btnAdditive = new Button();
			this.btnNormal.WidthRequest = (this.btnAdditive.WidthRequest = 80);
			this.btnNormal.Label = "NORMAL";
			this.btnAdditive.Label = "ADDITIVE";
			HBox hbox = new HBox();
			hbox.Spacing = 10;
			hbox.PackStart(this.btnNormal, false, false, 0U);
			hbox.PackStart(this.btnAdditive, false, false, 0U);
			this.table.Attach(hbox, 1U, 2U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.warningIcon = new TooltipIcon();
			this.warningIcon.Text = LanguageInfo.Skeleton_BoneSkinBlendInfo;
			hbox.PackStart(this.warningIcon, false, false, 0U);
			base.SetControl();
			this.btnNormal.Clicked += this.normal_Clicked;
			this.btnAdditive.Clicked += this.additive_Clicked;
			this.srcEditor.Changed += this.src_Changed;
			this.dstEditor.Changed += this.dst_Changed;
			this.table.ShowAll();
			return this.table;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00009760 File Offset: 0x00007960
		private void dst_Changed(object sender, EventArgs e)
		{
			string activeText = this.dstEditor.ActiveText;
			this.propertyValue = new BlendFuncValue(this.propertyValue.BlendSrc, (BlendDst)Enum.Parse(typeof(BlendDst), activeText));
			if (!this.activeWithoutDataChange)
			{
				base.UpdatePropertyValue(this.propertyValue, null);
				base.ReportUserData("Blend");
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000097CC File Offset: 0x000079CC
		private void src_Changed(object sender, EventArgs e)
		{
			string activeText = this.srcEditor.ActiveText;
			this.propertyValue = new BlendFuncValue((BlendSrc)Enum.Parse(typeof(BlendSrc), activeText), this.propertyValue.BlendDst);
			if (!this.activeWithoutDataChange)
			{
				base.UpdatePropertyValue(this.propertyValue, null);
				base.ReportUserData("Blend");
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00009837 File Offset: 0x00007A37
		private void additive_Clicked(object sender, EventArgs e)
		{
			this.propertyValue = BlendFuncValue.ADDITIVE;
			base.UpdatePropertyValue(this.propertyValue, null);
			base.ReportUserData("Blend");
			this.activeWithoutDataChange = true;
			this.EnumChangedEditors();
			this.activeWithoutDataChange = false;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00009874 File Offset: 0x00007A74
		private void normal_Clicked(object sender, EventArgs e)
		{
			this.propertyValue = BlendFuncValue.ALPHA_PREMULTIPLIED;
			base.UpdatePropertyValue(this.propertyValue, null);
			base.ReportUserData("Blend");
			this.activeWithoutDataChange = true;
			this.EnumChangedEditors();
			this.activeWithoutDataChange = false;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000098B4 File Offset: 0x00007AB4
		protected override void OnSetControl()
		{
			object obj = base.PropertyItem.Values[0];
			this.propertyValue = (obj as BlendFuncValue);
			this.EnumChangedEditors();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000098E8 File Offset: 0x00007AE8
		private void InitEnumEditor(EnumEditorComboBox enumeditor, Type blendtype)
		{
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			string[] names = Enum.GetNames(blendtype);
			CellRendererText cell = new CellRendererText();
			enumeditor.PackStart(cell, true);
			enumeditor.AddAttribute(cell, "text", 0);
			enumeditor.Model = listStore;
			for (int i = 0; i < names.Count<string>(); i++)
			{
				string text = names[i];
				listStore.AppendValues(new object[]
				{
					text
				});
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00009978 File Offset: 0x00007B78
		private void EnumChangedEditors()
		{
			this.EnumChangedEditor(this.srcEditor, typeof(BlendSrc), this.propertyValue.BlendSrc.ToString());
			this.EnumChangedEditor(this.dstEditor, typeof(BlendDst), this.propertyValue.BlendDst.ToString());
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000099E0 File Offset: 0x00007BE0
		private void EnumChangedEditor(EnumEditorComboBox enumeditor, Type blendtype, string srcname)
		{
			string[] names = Enum.GetNames(blendtype);
			for (int i = 0; i < names.Count<string>(); i++)
			{
				string a = names[i];
				if (a == srcname)
				{
					enumeditor.Active = i;
					break;
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00009A29 File Offset: 0x00007C29
		private void InitEnumEditors()
		{
			this.InitEnumEditor(this.srcEditor, typeof(BlendSrc));
			this.InitEnumEditor(this.dstEditor, typeof(BlendDst));
		}

		// Token: 0x04000129 RID: 297
		private Table table;

		// Token: 0x0400012A RID: 298
		private EnumEditorComboBox srcEditor;

		// Token: 0x0400012B RID: 299
		private EnumEditorComboBox dstEditor;

		// Token: 0x0400012C RID: 300
		private Button btnNormal;

		// Token: 0x0400012D RID: 301
		private Button btnAdditive;

		// Token: 0x0400012E RID: 302
		private BlendFuncValue propertyValue;

		// Token: 0x0400012F RID: 303
		private TooltipIcon warningIcon;

		// Token: 0x04000130 RID: 304
		private bool activeWithoutDataChange = false;
	}
}
