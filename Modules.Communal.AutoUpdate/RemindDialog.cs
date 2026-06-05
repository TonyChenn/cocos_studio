using System;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000013 RID: 19
	public class RemindDialog : Dialog
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004D43 File Offset: 0x00002F43
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004D4B File Offset: 0x00002F4B
		internal EnumRemindType RemindType { get; private set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D54 File Offset: 0x00002F54
		public RemindDialog(Gtk.Window parentWnd)
		{
			this.Build();
			this.RemindType = EnumRemindType.Always;
			this.SetToDialogStyle(parentWnd, true, true, true);
			this.InitDisplayText();
			this.parentWindow = parentWnd;
			this.parentWndModal = parentWnd.Modal;
			this.parentWindow.Modal = false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004DA4 File Offset: 0x00002FA4
		private void InitDisplayText()
		{
			base.Title = LanguageInfo.AutoUpdate_Remind;
			this.GtkLabel_remind.Text = " " + LanguageInfo.AutoUpdate_RemindType + " ";
			this.radiobutton_always.Label = LanguageInfo.AutoUpdate_AlwaysRemind;
			this.radiobutton_later.Label = LanguageInfo.AutoUpdate_RemindLater;
			this.radiobutton_skip.Label = LanguageInfo.AutoUpdate_Skip;
			this.buttonOk.Name = "MainButton";
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				buttonBoxChild2.Position = 0;
				buttonBoxChild.Position = 1;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004E7C File Offset: 0x0000307C
		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			if (this.radiobutton_always.Active)
			{
				this.RemindType = EnumRemindType.Always;
			}
			else if (this.radiobutton_later.Active)
			{
				this.RemindType = EnumRemindType.Later;
			}
			else if (this.radiobutton_skip.Active)
			{
				this.RemindType = EnumRemindType.Skip;
			}
			this.parentWindow.Modal = this.parentWndModal;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004EDC File Offset: 0x000030DC
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.AutoUpdate.RemindDialog";
			base.Title = Catalog.GetString("更新提醒");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.TopPadding = 10U;
			this.frame_main = new Frame();
			this.frame_main.WidthRequest = 300;
			this.frame_main.Name = "frame_main";
			this.frame_main.BorderWidth = 12U;
			this.GtkAlignment_frame = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_frame.Name = "GtkAlignment_frame";
			this.GtkAlignment_frame.LeftPadding = 12U;
			this.vbox_frame = new VBox();
			this.vbox_frame.Name = "vbox_frame";
			this.vbox_frame.Spacing = 6;
			this.vbox_frame.BorderWidth = 12U;
			this.radiobutton_always = new RadioButton(Catalog.GetString("总是提醒"));
			this.radiobutton_always.CanFocus = true;
			this.radiobutton_always.Name = "radiobutton_always";
			this.radiobutton_always.DrawIndicator = true;
			this.radiobutton_always.UseUnderline = true;
			this.radiobutton_always.Group = new SList(IntPtr.Zero);
			this.vbox_frame.Add(this.radiobutton_always);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_frame[this.radiobutton_always];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.radiobutton_later = new RadioButton(Catalog.GetString("稍后提醒"));
			this.radiobutton_later.CanFocus = true;
			this.radiobutton_later.Name = "radiobutton_later";
			this.radiobutton_later.DrawIndicator = true;
			this.radiobutton_later.UseUnderline = true;
			this.radiobutton_later.Group = this.radiobutton_always.Group;
			this.vbox_frame.Add(this.radiobutton_later);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_frame[this.radiobutton_later];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.radiobutton_skip = new RadioButton(Catalog.GetString("跳过本次更新"));
			this.radiobutton_skip.CanFocus = true;
			this.radiobutton_skip.Name = "radiobutton_skip";
			this.radiobutton_skip.DrawIndicator = true;
			this.radiobutton_skip.UseUnderline = true;
			this.radiobutton_skip.Group = this.radiobutton_always.Group;
			this.vbox_frame.Add(this.radiobutton_skip);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_frame[this.radiobutton_skip];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.GtkAlignment_frame.Add(this.vbox_frame);
			this.frame_main.Add(this.GtkAlignment_frame);
			this.GtkLabel_remind = new Label();
			this.GtkLabel_remind.Name = "GtkLabel_remind";
			this.GtkLabel_remind.LabelProp = Catalog.GetString(" 自动更新提醒方式 ");
			this.GtkLabel_remind.UseMarkup = true;
			this.frame_main.LabelWidget = this.GtkLabel_remind;
			this.alignment_main.Add(this.frame_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonCancel = new Button();
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
			base.DefaultWidth = 304;
			base.DefaultHeight = 187;
			base.Show();
			this.buttonOk.Clicked += this.HandleButtonOKClicked;
		}

		// Token: 0x04000054 RID: 84
		private bool parentWndModal;

		// Token: 0x04000055 RID: 85
		private Gtk.Window parentWindow;

		// Token: 0x04000056 RID: 86
		private Alignment alignment_main;

		// Token: 0x04000057 RID: 87
		private Frame frame_main;

		// Token: 0x04000058 RID: 88
		private Alignment GtkAlignment_frame;

		// Token: 0x04000059 RID: 89
		private VBox vbox_frame;

		// Token: 0x0400005A RID: 90
		private RadioButton radiobutton_always;

		// Token: 0x0400005B RID: 91
		private RadioButton radiobutton_later;

		// Token: 0x0400005C RID: 92
		private RadioButton radiobutton_skip;

		// Token: 0x0400005D RID: 93
		private Label GtkLabel_remind;

		// Token: 0x0400005E RID: 94
		private Button buttonCancel;

		// Token: 0x0400005F RID: 95
		private Button buttonOk;
	}
}
