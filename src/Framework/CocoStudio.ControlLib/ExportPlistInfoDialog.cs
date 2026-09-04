using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace CocoStudio.ControlLib
{
	// Token: 0x0200000D RID: 13
	public class ExportPlistInfoDialog : Dialog
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000519C File Offset: 0x0000339C
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000051B3 File Offset: 0x000033B3
		public bool IsOk { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x000051BC File Offset: 0x000033BC
		public ExportPlistInfoDialog(string exportPath)
		{
			this.Build();
			this.buttonOk.Name = "MainButton";
			this.Init(exportPath);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000051E8 File Offset: 0x000033E8
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00005200 File Offset: 0x00003400
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

		// Token: 0x06000064 RID: 100 RVA: 0x00005224 File Offset: 0x00003424
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

		// Token: 0x06000065 RID: 101 RVA: 0x000052EE File Offset: 0x000034EE
		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsOk = true;
			this.Destroy();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005300 File Offset: 0x00003500
		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsOk = false;
			this.Destroy();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005314 File Offset: 0x00003514
		private void button_Browse_Clicked(object sender, EventArgs e)
		{
			string folder = FileChooserDialogModel.GetBrowseDialogPath("导出路径", false, "", false).Folder;
			if (!string.IsNullOrEmpty(folder))
			{
				this.ExportPath = folder;
			}
			this.buttonOk.GrabFocus();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000535C File Offset: 0x0000355C
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

		// Token: 0x04000053 RID: 83
		private string exportpath;

		// Token: 0x04000054 RID: 84
		private HBox hbox_path;

		// Token: 0x04000055 RID: 85
		private Label label_Path;

		// Token: 0x04000056 RID: 86
		private Entry entry_Path;

		// Token: 0x04000057 RID: 87
		private Button button_Browse;

		// Token: 0x04000058 RID: 88
		private Button buttonOk;

		// Token: 0x04000059 RID: 89
		private Button buttonCancel;
	}
}
