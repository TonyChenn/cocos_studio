using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	[ToolboxItem(true)]
	public class IOSWidget : Bin, IProjectSettingWidget
	{
		public IOSWidget()
		{
			this.Build();
			this.InitControl();
			this.InitMultiLanuage();
		}

		private void InitControl()
		{
			this.packageParams = PackageServices.Instance.PackageParams;
			this.entry_tag.Text = this.packageParams.iOS_Target;
			List<string> list = PackageServices.Instance.GetiOSBundleIDlist();
			foreach (string text in list)
			{
				this.combobox_certificate.AppendText(text);
			}
			if (list.Count > 0)
			{
				if (list.Contains(this.packageParams.iOS_BundleID))
				{
					this.combobox_certificate.Active = list.IndexOf(this.packageParams.iOS_BundleID);
					return;
				}
				this.combobox_certificate.Active = 0;
			}
		}

		private void InitMultiLanuage()
		{
			this.label_certificate.Text = LanguageInfo.Package_iOS_Certificate;
			this.label_tag.Text = LanguageInfo.Package_iOS_Tag;
			this.GtkLabel_publish.Text = " " + LanguageInfo.Package_iOS_Publish + " ";
		}

		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.iOS;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.Package_iOS_Setting;
			}
		}

		public void ApplySetting()
		{
			if (this.combobox_certificate.ActiveText != null)
			{
				this.packageParams.iOS_BundleID = this.combobox_certificate.ActiveText;
			}
			this.packageParams.iOS_Target = this.entry_tag.Text;
		}

		public bool CanApply(out string output)
		{
			if (string.IsNullOrWhiteSpace(this.entry_tag.Text))
			{
				output = LanguageInfo.MessageBox243_enterTarget;
				this.entry_tag.HasFocus = true;
				return false;
			}
			output = "";
			return true;
		}

		public Widget GetWidget()
		{
			return this;
		}

		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.IOSWidget";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 10;
			this.frame_publish = new Frame();
			this.frame_publish.Name = "frame_publish";
			this.GtkAlignment_publish = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_publish.Name = "GtkAlignment_publish";
			this.GtkAlignment_publish.LeftPadding = 15U;
			this.GtkAlignment_publish.TopPadding = 10U;
			this.GtkAlignment_publish.RightPadding = 10U;
			this.GtkAlignment_publish.BottomPadding = 10U;
			this.GtkAlignment_publish.BorderWidth = 5U;
			this.table_publish = new Table(2U, 2U, false);
			this.table_publish.Name = "table_publish";
			this.table_publish.RowSpacing = 15U;
			this.table_publish.ColumnSpacing = 12U;
			this.combobox_certificate = ComboBox.NewText();
			this.combobox_certificate.Name = "combobox_certificate";
			this.table_publish.Add(this.combobox_certificate);
			Table.TableChild tableChild = (Table.TableChild)this.table_publish[this.combobox_certificate];
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.YOptions = AttachOptions.Fill;
			this.entry_tag = new Entry();
			this.entry_tag.CanFocus = true;
			this.entry_tag.Name = "entry_tag";
			this.entry_tag.IsEditable = true;
			this.entry_tag.InvisibleChar = '●';
			this.table_publish.Add(this.entry_tag);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_publish[this.entry_tag];
			tableChild2.TopAttach = 1U;
			tableChild2.BottomAttach = 2U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			this.label_certificate = new Label();
			this.label_certificate.Name = "label_certificate";
			this.label_certificate.Xalign = 1f;
			this.label_certificate.LabelProp = Catalog.GetString("iOS证书");
			this.table_publish.Add(this.label_certificate);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_publish[this.label_certificate];
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			this.label_tag = new Label();
			this.label_tag.Name = "label_tag";
			this.label_tag.Xalign = 1f;
			this.label_tag.LabelProp = Catalog.GetString("标签");
			this.table_publish.Add(this.label_tag);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_publish[this.label_tag];
			tableChild4.TopAttach = 1U;
			tableChild4.BottomAttach = 2U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.GtkAlignment_publish.Add(this.table_publish);
			this.frame_publish.Add(this.GtkAlignment_publish);
			this.GtkLabel_publish = new Label();
			this.GtkLabel_publish.Name = "GtkLabel_publish";
			this.GtkLabel_publish.LabelProp = Catalog.GetString(" 发布设置 ");
			this.GtkLabel_publish.UseMarkup = true;
			this.frame_publish.LabelWidget = this.GtkLabel_publish;
			this.vbox_main.Add(this.frame_publish);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.frame_publish];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private PackageParams packageParams;

		private Alignment alignment_main;

		private VBox vbox_main;

		private Frame frame_publish;

		private Alignment GtkAlignment_publish;

		private Table table_publish;

		private ComboBox combobox_certificate;

		private Entry entry_tag;

		private Label label_certificate;

		private Label label_tag;

		private Label GtkLabel_publish;
	}
}
