using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace CocoStudio.ControlLib.Windows
{
	// Token: 0x0200000B RID: 11
	public class CoverEachFileDialog : Dialog
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000042 RID: 66 RVA: 0x00003F08 File Offset: 0x00002108
		// (remove) Token: 0x06000043 RID: 67 RVA: 0x00003F44 File Offset: 0x00002144
		public event EventHandler ConfirmClickHandler;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003F80 File Offset: 0x00002180
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003F97 File Offset: 0x00002197
		public bool IsExportCover { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003FA0 File Offset: 0x000021A0
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003FB7 File Offset: 0x000021B7
		public bool IsChangeAll { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003FC0 File Offset: 0x000021C0
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003FD7 File Offset: 0x000021D7
		public bool IsUnCancel { get; set; }

		// Token: 0x0600004A RID: 74 RVA: 0x00003FE0 File Offset: 0x000021E0
		public CoverEachFileDialog()
		{
			this.Build();
			this.ChangeBtnPosion();
			this.buttonOk.Name = "MainButton";
			this.Init();
			this.ReadMultiLanguageConfig();
			this.buttonOk.GrabDefault();
			this.buttonCancel.Clicked += this.buttonCancel_Clicked;
			this.buttonOk.Clicked += this.buttonOk_Clicked;
			base.DeleteEvent += this.CoverEachFileWindow_DeleteEvent;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004074 File Offset: 0x00002274
		private void ChangeBtnPosion()
		{
			if (!Platform.IsMac)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				buttonBoxChild.Position = 0;
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild2.Position = 1;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000040C8 File Offset: 0x000022C8
		private void Init()
		{
			base.AllowGrow = false;
			Rectangle rectangle = new Rectangle(0, 0, 500, 130);
			base.WidthRequest = rectangle.Width;
			base.HeightRequest = rectangle.Height;
			this.SetToDialogStyle(null, true, true, true);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000411C File Offset: 0x0000231C
		private void CoverEachFileWindow_DeleteEvent(object o, DeleteEventArgs e)
		{
			if (!this.IsUnCancel)
			{
				this.IsExportCover = false;
				this.IsChangeAll = true;
				if (this.ConfirmClickHandler != null)
				{
					this.ConfirmClickHandler(this, e);
				}
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004164 File Offset: 0x00002364
		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsExportCover = true;
			this.IsChangeAll = this.checkbutton_All.Active;
			this.IsUnCancel = true;
			if (this.ConfirmClickHandler != null)
			{
				this.ConfirmClickHandler(this, e);
			}
			else
			{
				this.Destroy();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000041BC File Offset: 0x000023BC
		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsExportCover = false;
			this.IsChangeAll = this.checkbutton_All.Active;
			this.IsUnCancel = true;
			if (this.ConfirmClickHandler != null)
			{
				this.ConfirmClickHandler(this, e);
			}
			else
			{
				this.Destroy();
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004214 File Offset: 0x00002414
		public void RefreshMessage(string fileName, int allCount, string existInfo = "")
		{
			if (existInfo == "")
			{
				existInfo = LanguageInfo.CoverEachIsExistFile;
			}
			this.label_Message.Text = string.Format(existInfo, fileName);
			if (allCount > 0)
			{
				this.checkbutton_All.Label = string.Format(LanguageInfo.CoverEachIsConflictFile, allCount);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004278 File Offset: 0x00002478
		private void ReadMultiLanguageConfig()
		{
			base.Title = LanguageInfo.ResourceReplacementWindow;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonYes;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonNo;
			this.checkbutton_All.Label = LanguageInfo.AllReplacement;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000042C8 File Offset: 0x000024C8
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "CocoStudio.ControlLib.Windows.CoverEachFileDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vbox = base.VBox;
			vbox.WidthRequest = 440;
			vbox.HeightRequest = 130;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 72;
			this.vbox2.BorderWidth = 10U;
			this.label_Message = new Label();
			this.label_Message.Name = "label_Message";
			this.label_Message.LabelProp = Catalog.GetString("label1");
			this.vbox2.Add(this.label_Message);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.label_Message];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.checkbutton_All = new CheckButton();
			this.checkbutton_All.CanFocus = true;
			this.checkbutton_All.Name = "checkbutton_All";
			this.checkbutton_All.Label = Catalog.GetString("全部选择");
			this.checkbutton_All.DrawIndicator = true;
			this.checkbutton_All.UseUnderline = true;
			this.vbox2.Add(this.checkbutton_All);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox2[this.checkbutton_All];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			vbox.Add(this.vbox2);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[this.vbox2];
			boxChild3.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonCancel = new Button();
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
			base.DefaultWidth = 440;
			base.DefaultHeight = 130;
			base.Show();
		}

		// Token: 0x04000035 RID: 53
		private VBox vbox2;

		// Token: 0x04000036 RID: 54
		private Label label_Message;

		// Token: 0x04000037 RID: 55
		private CheckButton checkbutton_All;

		// Token: 0x04000038 RID: 56
		private Button buttonCancel;

		// Token: 0x04000039 RID: 57
		private Button buttonOk;
	}
}
