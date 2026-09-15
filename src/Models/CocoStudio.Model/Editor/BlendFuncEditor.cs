using System;
using System.Linq;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class BlendFuncEditor : BaseEditor
	{
		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

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

		private void additive_Clicked(object sender, EventArgs e)
		{
			this.propertyValue = BlendFuncValue.ADDITIVE;
			base.UpdatePropertyValue(this.propertyValue, null);
			base.ReportUserData("Blend");
			this.activeWithoutDataChange = true;
			this.EnumChangedEditors();
			this.activeWithoutDataChange = false;
		}

		private void normal_Clicked(object sender, EventArgs e)
		{
			this.propertyValue = BlendFuncValue.ALPHA_PREMULTIPLIED;
			base.UpdatePropertyValue(this.propertyValue, null);
			base.ReportUserData("Blend");
			this.activeWithoutDataChange = true;
			this.EnumChangedEditors();
			this.activeWithoutDataChange = false;
		}

		protected override void OnSetControl()
		{
			object obj = base.PropertyItem.Values[0];
			this.propertyValue = (obj as BlendFuncValue);
			this.EnumChangedEditors();
		}

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

		private void EnumChangedEditors()
		{
			this.EnumChangedEditor(this.srcEditor, typeof(BlendSrc), this.propertyValue.BlendSrc.ToString());
			this.EnumChangedEditor(this.dstEditor, typeof(BlendDst), this.propertyValue.BlendDst.ToString());
		}

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

		private void InitEnumEditors()
		{
			this.InitEnumEditor(this.srcEditor, typeof(BlendSrc));
			this.InitEnumEditor(this.dstEditor, typeof(BlendDst));
		}

		private Table table;

		private EnumEditorComboBox srcEditor;

		private EnumEditorComboBox dstEditor;

		private Button btnNormal;

		private Button btnAdditive;

		private BlendFuncValue propertyValue;

		private TooltipIcon warningIcon;

		private bool activeWithoutDataChange = false;
	}
}
