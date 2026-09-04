using System;
using System.Diagnostics;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x0200000C RID: 12
	public class SelectPathDialog : Dialog
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002EB0 File Offset: 0x000010B0
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.HeightRequest = 120;
			base.Name = "Modules.Communal.CocosCodeIDE.SelectPathDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.vbox_main.BorderWidth = 12U;
			this.vbox_up = new VBox();
			this.vbox_up.Name = "vbox_up";
			this.vbox_up.Spacing = 6;
			this.vbox_main.Add(this.vbox_up);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.vbox_up];
			boxChild.Position = 0;
			this.hbox_left = new HBox();
			this.hbox_left.Name = "hbox_left";
			this.hbox_left.Spacing = 6;
			this.selectpathwidget2 = new SelectPathWidget(false);
			this.selectpathwidget2.Events = EventMask.ButtonPressMask;
			this.selectpathwidget2.Name = "selectpathwidget2";
			this.hbox_left.Add(this.selectpathwidget2);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_left[this.selectpathwidget2];
			boxChild2.Position = 0;
			this.vbox_main.Add(this.hbox_left);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_main[this.hbox_left];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox_down = new VBox();
			this.vbox_down.Name = "vbox_down";
			this.vbox_down.Spacing = 6;
			this.vbox_main.Add(this.vbox_down);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.vbox_down];
			boxChild4.Position = 2;
			vbox.Add(this.vbox_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox[this.vbox_main];
			boxChild5.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 60;
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			base.AddActionWidget(this.buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 60;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			base.AddActionWidget(this.buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 374;
			base.DefaultHeight = 120;
			base.Show();
			base.KeyPressEvent += this.OnDialogKeyPressed;
			this.buttonOk.Clicked += this.OnBtnOKClicked;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003283 File Offset: 0x00001483
		public SelectPathDialog()
		{
			this.Build();
			this.Init();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003298 File Offset: 0x00001498
		private void Init()
		{
			this.SetToDialogStyle(null, true, true, true);
			if (this.selectpathwidget2 == null)
			{
				this.selectpathwidget2 = new SelectPathWidget(false);
				this.hbox_left.Add(this.selectpathwidget2);
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox_left[this.selectpathwidget2];
				boxChild.Position = 0;
				this.selectpathwidget2.ShowAll();
			}
			this.selectpathwidget2.SetEntryEnable(false);
			this.selectpathwidget2.PathSelected += this.OnPathSet;
			base.Title = LanguageInfo.Dialog_Publish_SelectCodeIDE;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonOk.Name = "MainButton";
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.buttonOk.GrabFocus();
			this.buttonOk.Sensitive = false;
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild.Position = 0;
				buttonBoxChild2.Position = 1;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000033B2 File Offset: 0x000015B2
		private void OnPathSet(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.selectpathwidget2.FilePath))
			{
				this.buttonOk.Sensitive = true;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000033D4 File Offset: 0x000015D4
		protected void OnBtnOKClicked(object sender, EventArgs e)
		{
			string filePath = this.selectpathwidget2.FilePath;
			if (string.IsNullOrEmpty(filePath))
			{
				MessageBox.Show(LanguageInfo.Dialog_Publish_SelectCodeIDE, MessageBoxImage.Other, null, null);
				return;
			}
			Services.RecentFileService.CocosCodeIDEDir = filePath;
			try
			{
				Process.Start(filePath);
			}
			catch
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox180_FailedToOpenCodeIDE);
			}
			this.Destroy();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003440 File Offset: 0x00001640
		protected void OnDialogKeyPressed(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return)
			{
				this.Destroy();
			}
		}

		// Token: 0x0400001F RID: 31
		private VBox vbox_main;

		// Token: 0x04000020 RID: 32
		private VBox vbox_up;

		// Token: 0x04000021 RID: 33
		private HBox hbox_left;

		// Token: 0x04000022 RID: 34
		private SelectPathWidget selectpathwidget2;

		// Token: 0x04000023 RID: 35
		private VBox vbox_down;

		// Token: 0x04000024 RID: 36
		private Button buttonCancel;

		// Token: 0x04000025 RID: 37
		private Button buttonOk;
	}
}
