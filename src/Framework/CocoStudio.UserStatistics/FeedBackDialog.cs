using System;
using System.Diagnostics;
using System.IO;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Pango;
using Stetic;

namespace CocoStudio.UserStatistics
{
	public class FeedBackDialog : Dialog
	{
		public string FeedbackInfo { get; private set; }

		public FeedBackDialog()
		{
			this.Build();
			this.buttonOk.Name = "MainButton";
			this.buttonOk.GrabDefault();
			base.WidthRequest = 500;
			base.HeightRequest = 350;
			FontDescription fontDescription = new FontDescription();
			fontDescription.AbsoluteSize = LabelStyleSetting.MainTitleSize;
			fontDescription.Weight = Weight.Bold;
			this.label6.ModifyFont(fontDescription);
			this.label7.ModifyFont(fontDescription);
			base.Title = LanguageInfo.MessageBox_Error;
			this.lab_email = new LabelLinkButton(null);
			this.lab_email.LabelText = "cocostudiosupport@chukong-inc.com";
			this.lab_email.Clicked += this.lab_email_Clicked;
			this.hbox1.Add(this.lab_email);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.lab_email];
			boxChild.Position = 1;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.textview2.WrapMode = Gtk.WrapMode.WordChar;
			this.lab_email.Show();
			Gtk.Window window = MessageService.GetDefaultModalParent();
			if (window == null)
			{
				window = ApplicationCurrent.MainWindow;
			}
			if (window != null && window.Visible)
			{
				this.SetToDialogStyle(window, true, true, true);
				window.Modal = false;
			}
			this.textview2.FocusInEvent += this.textview2_FocusInEvent;
			this.textview2.FocusOutEvent += this.FeedBackDialog_FocusOutEvent;
			this.MutileLanguage();
			this.FeedbackInfo = string.Empty;
		}

		private void FeedBackDialog_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (string.IsNullOrWhiteSpace(this.textview2.Buffer.Text))
			{
				this.textview2.Buffer.Text = LanguageInfo.MessageBox271_FeedBackInfo;
			}
		}

		private void textview2_FocusInEvent(object o, FocusInEventArgs args)
		{
			if (this.textview2.Buffer.Text == LanguageInfo.MessageBox271_FeedBackInfo)
			{
				this.textview2.Buffer.Text = "";
			}
		}

		private void MutileLanguage()
		{
			this.textview2.Buffer.Text = LanguageInfo.MessageBox271_FeedBackInfo;
			this.label6.LabelProp = LanguageInfo.MessageBox271_FeedBackInfo1;
			this.label7.LabelProp = LanguageInfo.MessageBox272_FeedBackInfo2;
			this.checkbutton3.Label = LanguageInfo.MessageBox273_RestartCocos;
			this.label8.LabelProp = LanguageInfo.MessageBox274_FeedBackInfo;
			this.label9.LabelProp = LanguageInfo.MessageBox275_FeedBackInfo;
			this.label11.LabelProp = LanguageInfo.MessageBox276_FeedBackInfo;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			if (LanguageOption.CurrentLanguage == LanguageType.English)
			{
				this.label7.Visible = false;
			}
		}

		private void lab_email_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			string arg = (this.textview2.Buffer.Text == LanguageInfo.MessageBox271_FeedBackInfo) ? "" : this.textview2.Buffer.Text;
			Process.Start(string.Format("mailto:{0}?subject={1}&body={2}", this.lab_email.LabelText, LanguageInfo.MessageBox271_FeedBackSubject, arg));
		}

		protected void HandleButtonOKClicked(object sender, EventArgs e)
		{
			if (this.textview2.Buffer.Text.Length > 160)
			{
				MessageBox.Show("反馈信息不能超过160字.", MessageBoxImage.Warning, null, LanguageInfo.MessageBox_Warning);
			}
			else
			{
				string text = this.textview2.Buffer.Text;
				if (!string.IsNullOrWhiteSpace(text) && text != LanguageInfo.MessageBox271_FeedBackInfo)
				{
					this.FeedbackInfo = text;
				}
				if (this.checkbutton3.Active)
				{
					this.Restart();
				}
				base.Respond(ResponseType.Ok);
			}
		}

		private void Restart()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text = System.IO.Path.Combine(baseDirectory, "CocosStudio.exe");
			if (Platform.IsMac)
			{
				text = "/Applications/Cocos/Cocos Studio 2.app/Contents/MacOS/CocosStudio";
			}
			if (File.Exists(text))
			{
				new Process
				{
					StartInfo = new ProcessStartInfo(text)
				}.Start();
			}
		}

		protected void HandleEntryKeyReleased(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.KP_Enter || args.Event.Key == Gdk.Key.ISO_Enter || args.Event.Key == Gdk.Key.Key_3270_Enter || args.Event.Key == Gdk.Key.Return)
			{
				this.HandleButtonOKClicked(o, args);
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "CocoStudio.UserStatistics.FeedBackDialog";
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 6U;
			this.alignment4 = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment4.Name = "alignment4";
			this.alignment4.LeftPadding = 10U;
			this.alignment4.TopPadding = 10U;
			this.alignment4.RightPadding = 20U;
			this.alignment4.BottomPadding = 10U;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.label6 = new Label();
			this.label6.Name = "label6";
			this.label6.Xalign = 0f;
			this.label6.LabelProp = Catalog.GetString("╮(╯_╰）╭ 哎呀，发生这种状况");
			this.vbox3.Add(this.label6);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox3[this.label6];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.label7 = new Label();
			this.label7.Name = "label7";
			this.label7.LabelProp = Catalog.GetString("我的内心也是崩溃的...");
			this.vbox3.Add(this.label7);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox3[this.label7];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.checkbutton3 = new CheckButton();
			this.checkbutton3.CanFocus = true;
			this.checkbutton3.Name = "checkbutton3";
			this.checkbutton3.Label = Catalog.GetString("重新启动cocos");
			this.checkbutton3.Active = true;
			this.checkbutton3.DrawIndicator = true;
			this.checkbutton3.UseUnderline = true;
			this.vbox3.Add(this.checkbutton3);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox3[this.checkbutton3];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.label8 = new Label();
			this.label8.Name = "label8";
			this.label8.Xalign = 0f;
			this.label8.LabelProp = Catalog.GetString("帮我 改进一下吧~");
			this.vbox3.Add(this.label8);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox3[this.label8];
			boxChild4.Position = 3;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.alignment5 = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment5.Name = "alignment5";
			this.alignment5.LeftPadding = 20U;
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview2 = new TextView();
			this.textview2.Buffer.Text = "喷一喷崩溃是如何发生的吧?(160字以内)";
			this.textview2.CanFocus = true;
			this.textview2.Name = "textview2";
			this.GtkScrolledWindow.Add(this.textview2);
			this.alignment5.Add(this.GtkScrolledWindow);
			this.vbox3.Add(this.alignment5);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox3[this.alignment5];
			boxChild5.Position = 4;
			this.label9 = new Label();
			this.label9.Name = "label9";
			this.label9.Xalign = 0f;
			this.label9.LabelProp = Catalog.GetString("告诉我也是可以的哟:");
			this.vbox3.Add(this.label9);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox3[this.label9];
			boxChild6.Position = 5;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.label11 = new Label();
			this.label11.Name = "label11";
			this.label11.LabelProp = Catalog.GetString("技术支持邮箱:");
			this.hbox1.Add(this.label11);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox1[this.label11];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox3.Add(this.hbox1);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox3[this.hbox1];
			boxChild8.Position = 6;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment4.Add(this.vbox3);
			vbox.Add(this.alignment4);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox[this.alignment4];
			boxChild9.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_occupy = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_occupy.Name = "alignment_occupy";
			this.alignment3 = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment3.Name = "alignment3";
			this.alignment3.LeftPadding = 15U;
			this.alignment3.RightPadding = 18U;
			this.alignment3.BottomPadding = 5U;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 75;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = Catalog.GetString("确定");
			this.alignment3.Add(this.buttonOk);
			this.alignment_occupy.Add(this.alignment3);
			actionArea.Add(this.alignment_occupy);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_occupy];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 510;
			base.DefaultHeight = 276;
			base.Show();
			this.buttonOk.Clicked += this.HandleButtonOKClicked;
		}

		private LabelLinkButton lab_email;

		private Gtk.Alignment alignment4;

		private VBox vbox3;

		private Label label6;

		private Label label7;

		private CheckButton checkbutton3;

		private Label label8;

		private Gtk.Alignment alignment5;

		private ScrolledWindow GtkScrolledWindow;

		private TextView textview2;

		private Label label9;

		private HBox hbox1;

		private Label label11;

		private Gtk.Alignment alignment_occupy;

		private Gtk.Alignment alignment3;

		private Button buttonOk;
	}
}
