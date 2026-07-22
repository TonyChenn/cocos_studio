using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000002 RID: 2
	public class CocosCodeIDEDialog : Dialog
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CocosCodeIDEDialog()
		{
			this.Build();
			this.Init();
			base.ShowAll();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
		private void Init()
		{
			this.SetToDialogStyle(null, true, true, true);
			this.buttonOk.GrabFocus();
			this.buttonOk.Name = "MainButton";
			this.label_content.WidthRequest = 270;
			base.Title = LanguageInfo.MessageBox_Notification;
			this.label_content.Text = LanguageInfo.MessageBox181_CodeIDENotInstalled;
			this.button_already.Label = LanguageInfo.Dialog_Publish_HaveInstalledZip;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonYes;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonNo;
			if (Platform.IsWindows)
			{
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.buttonOk];
				Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.buttonCancel];
				boxChild.Position = 1;
				boxChild2.Position = 0;
				return;
			}
			this.hbox2.Remove(this.button_already);
			this.button_already.Clicked -= this.OnButtonAlreadyClicked;
			this.button_already = null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000216C File Offset: 0x0000036C
		protected void OnButtonAlreadyClicked(object sender, EventArgs e)
		{
			this.Destroy();
			SelectPathDialog selectPathDialog = new SelectPathDialog();
			selectPathDialog.Run();
			selectPathDialog.Destroy();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002192 File Offset: 0x00000392
		protected void OnButtonOKClicked(object sender, EventArgs e)
		{
			CocosCodeIDEService.Instance.DownloadCocosCodeIDE();
			this.Destroy();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021A4 File Offset: 0x000003A4
		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021AC File Offset: 0x000003AC
		protected void OnDialogKeyPressed(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return)
			{
				this.OnButtonCancelClicked(o, args);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021C8 File Offset: 0x000003C8
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 300;
			base.HeightRequest = 150;
			base.Name = "Modules.Communal.CocosCodeIDE.CocosCodeIDEDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.vbox_main.BorderWidth = 12U;
			this.vbox_top = new VBox();
			this.vbox_top.Name = "vbox_top";
			this.vbox_top.Spacing = 6;
			this.vbox_main.Add(this.vbox_top);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.vbox_top];
			boxChild.Position = 0;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 6;
			this.hbox_left = new HBox();
			this.hbox_left.Name = "hbox_left";
			this.hbox_left.Spacing = 6;
			this.hbox_main.Add(this.hbox_left);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.hbox_left];
			boxChild2.Position = 0;
			this.label_content = new Label();
			this.label_content.Name = "label_content";
			this.label_content.LabelProp = Catalog.GetString("您还没有安装Cocos Code IDE，是否前往下载？");
			this.label_content.Wrap = true;
			this.hbox_main.Add(this.label_content);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_main[this.label_content];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox_right = new HBox();
			this.hbox_right.Name = "hbox_right";
			this.hbox_right.Spacing = 6;
			this.hbox_main.Add(this.hbox_right);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_main[this.hbox_right];
			boxChild4.Position = 2;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.vbox_bottom = new VBox();
			this.vbox_bottom.Name = "vbox_bottom";
			this.vbox_bottom.Spacing = 6;
			this.vbox_main.Add(this.vbox_bottom);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_main[this.vbox_bottom];
			boxChild6.Position = 2;
			vbox.Add(this.vbox_main);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox[this.vbox_main];
			boxChild7.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.Edge;
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.button_already = new Button();
			this.button_already.CanFocus = true;
			this.button_already.Name = "button_already";
			this.button_already.UseUnderline = true;
			this.button_already.Label = Catalog.GetString("我已安装ZipFile版");
			this.hbox2.Add(this.button_already);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox2[this.button_already];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			actionArea.Add(this.hbox2);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.hbox2];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 60;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.hbox1.Add(this.buttonOk);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox1[this.buttonOk];
			boxChild9.PackType = PackType.End;
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 60;
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.hbox1.Add(this.buttonCancel);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox1[this.buttonCancel];
			boxChild10.PackType = PackType.End;
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			actionArea.Add(this.hbox1);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.hbox1];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 300;
			base.DefaultHeight = 150;
			base.Show();
			base.KeyPressEvent += this.OnDialogKeyPressed;
			this.button_already.Clicked += this.OnButtonAlreadyClicked;
			this.buttonCancel.Clicked += this.OnButtonCancelClicked;
			this.buttonOk.Clicked += this.OnButtonOKClicked;
		}

		// Token: 0x04000001 RID: 1
		private VBox vbox_main;

		// Token: 0x04000002 RID: 2
		private VBox vbox_top;

		// Token: 0x04000003 RID: 3
		private HBox hbox_main;

		// Token: 0x04000004 RID: 4
		private HBox hbox_left;

		// Token: 0x04000005 RID: 5
		private Label label_content;

		// Token: 0x04000006 RID: 6
		private HBox hbox_right;

		// Token: 0x04000007 RID: 7
		private VBox vbox_bottom;

		// Token: 0x04000008 RID: 8
		private HBox hbox2;

		// Token: 0x04000009 RID: 9
		private Button button_already;

		// Token: 0x0400000A RID: 10
		private HBox hbox1;

		// Token: 0x0400000B RID: 11
		private Button buttonOk;

		// Token: 0x0400000C RID: 12
		private Button buttonCancel;
	}
}
