using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace CocoStudio.ControlLib
{
	public class ExportPlistInfoDialog : Dialog
	{
		public bool IsOk { get; set; }

		public ExportPlistInfoDialog(string exportPath)
		{
			this.Build();
			this.buttonOk.Name = "MainButton";
			this.Init(exportPath);
		}

		public string ExportPath
		{
			get
			{
				return this.exportpath;
			}
			set
			{
				this.entry_Path.Text = value;
				this.exportpath = value;
			}
		}

		private void Init(string exportPath)
		{
			base.AllowGrow = false;
			base.SetSizeRequest(510, 100);
			this.SetToDialogStyle(null, true, true, true);
			this.button_Browse.SetSizeRequest(70, 24);
			this.button_Browse.Clicked += this.button_Browse_Clicked;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.buttonOk.Clicked += this.buttonOk_Clicked;
			this.buttonCancel.Clicked += this.buttonCancel_Clicked;
			this.ExportPath = exportPath;
			this.entry_Path.ActivatesDefault = true;
			this.buttonOk.GrabDefault();
		}

		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsOk = true;
			this.Destroy();
		}

		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsOk = false;
			this.Destroy();
		}

		private void button_Browse_Clicked(object sender, EventArgs e)
		{
			string folder = FileChooserDialogModel.GetBrowseDialogPath("导出路径", false, "", false).Folder;
			if (!string.IsNullOrEmpty(folder))
			{
				this.ExportPath = folder;
			}
			this.buttonOk.GrabFocus();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "CocoStudio.ControlLib.ExportPlistInfoDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.hbox_path = new HBox();
			this.hbox_path.Name = "hbox_path";
			this.hbox_path.Spacing = 6;
			this.hbox_path.BorderWidth = 8U;
			this.label_Path = new Label();
			this.label_Path.Name = "label_Path";
			this.label_Path.LabelProp = Catalog.GetString("导出路径");
			this.hbox_path.Add(this.label_Path);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_path[this.label_Path];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			boxChild.Padding = 5U;
			this.entry_Path = new Entry();
			this.entry_Path.WidthRequest = 370;
			this.entry_Path.CanFocus = true;
			this.entry_Path.Name = "entry_Path";
			this.entry_Path.IsEditable = true;
			this.entry_Path.InvisibleChar = '●';
			this.hbox_path.Add(this.entry_Path);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_path[this.entry_Path];
			boxChild2.Position = 1;
			this.button_Browse = new Button();
			this.button_Browse.CanFocus = true;
			this.button_Browse.Name = "button_Browse";
			this.button_Browse.UseUnderline = true;
			this.button_Browse.Label = Catalog.GetString("   浏览   ");
			this.hbox_path.Add(this.button_Browse);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_path[this.button_Browse];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			vbox.Add(this.hbox_path);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[this.hbox_path];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonOk = new Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			base.AddActionWidget(this.buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			base.AddActionWidget(this.buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 538;
			base.DefaultHeight = 96;
			base.Show();
		}

		private string exportpath;

		private HBox hbox_path;

		private Label label_Path;

		private Entry entry_Path;

		private Button button_Browse;

		private Button buttonOk;

		private Button buttonCancel;
	}
}
