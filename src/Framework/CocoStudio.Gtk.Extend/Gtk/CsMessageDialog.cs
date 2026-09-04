using System;
using System.IO;
using AppKit;
using CocoStudio.Basic;
using CocoStudio.DefaultResource;
using Gdk;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x0200009B RID: 155
	public class CsMessageDialog : Dialog
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000E500 File Offset: 0x0000C700
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000E51D File Offset: 0x0000C71D
		public string Info
		{
			get
			{
				return this.label_text.Text;
			}
			set
			{
				this.label_text.Text = value;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E52D File Offset: 0x0000C72D
		public CsMessageDialog()
		{
			throw new Exception("请使用消息框的有参构造");
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E541 File Offset: 0x0000C741
		public CsMessageDialog(string info, ButtonText btnText, Window parentWnd, MessageBoxImage image, EnumMainButton mainBtn, string title)
		{
			this.Build();
			this.Info = info;
			this.InitImage(image);
			this.InitButton(btnText, mainBtn);
			this.InitStyle(parentWnd, title);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000E578 File Offset: 0x0000C778
		private void InitStyle(Window parentWnd, string title)
		{
			this.label_text.SetFontSize(12.0);
			if (Platform.IsMac)
			{
				this.alignment_text.TopPadding = 31U;
			}
			else if (Platform.IsWindows)
			{
				this.alignment_text.TopPadding = 29U;
			}
			base.Title = title;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.label_text.SetFontSize(14.0);
				CustomTitleBar customTitleBar = new CustomTitleBar();
				customTitleBar.CloseClicked += this.OnCustumTitleClose;
				customTitleBar.HeightRequest = 26;
				customTitleBar.SetParentWindow(this);
				customTitleBar.Title = title;
				this.alignment_title.Add(customTitleBar);
				customTitleBar.Show();
				if (parentWnd.Visible)
				{
					this.CenterToParentWindow(parentWnd);
					base.TransientFor = parentWnd;
				}
				if (Platform.IsMac)
				{
					base.VBox.BorderWidth = 0U;
					NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
					if (base.GdkWindow == null)
					{
						base.Show();
					}
					NativeGdkMac.SetNSWindowStyle(base.GdkWindow, style);
				}
				else if (Platform.IsWindows)
				{
					base.Decorated = false;
					base.ModifyBg(StateType.Normal, new Gdk.Color(187, 187, 187));
					base.SizeAllocated += this.HandleDialogSizeAllocated;
				}
			}
			else if (parentWnd.Visible)
			{
				this.SetToDialogStyle(parentWnd, true, true, true);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000E714 File Offset: 0x0000C914
		private void InitImage(MessageBoxImage imageType)
		{
			string text;
			switch (imageType)
			{
			case MessageBoxImage.Info:
			case MessageBoxImage.Warning:
			case MessageBoxImage.Question:
				text = imageType.ToString();
				break;
			case MessageBoxImage.Error:
				text = MessageBoxImage.Warning.ToString();
				break;
			default:
				text = MessageBoxImage.Info.ToString();
				break;
			}
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				text = "Launcher" + text;
			}
			string resourceID = string.Format("CocoStudio.DefaultResource.Images.MessageBoxIcon.{0}.png", text);
			Stream resourceStream = Resources.GetResourceStream(resourceID);
			if (resourceStream != null)
			{
				Xwt.Drawing.Image imageView = Xwt.Drawing.Image.FromStream(resourceStream);
				this.imageBin.SetImageView(imageView);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		private void InitButton(ButtonText btnText, EnumMainButton mainBtn)
		{
			this.btnsType = btnText.ButtonType;
			this.mainButton = mainBtn;
			IDialogButton dialogButton = null;
			IDialogButton dialogButton2 = null;
			IDialogButton dialogButton3 = null;
			Alignment alignment = null;
			Alignment alignment2 = null;
			Alignment alignment3 = null;
			switch (this.btnsType)
			{
			case MessageBoxButton.Yes:
				dialogButton = this.CreateButtonInstance(btnText.YesBtnText, EnumMainButton.Yes);
				this.alignment_btn1.Add(dialogButton.GetWidget());
				alignment = this.alignment_btn1;
				dialogButton.GetWidget().Show();
				break;
			case MessageBoxButton.YesNo:
				dialogButton = this.CreateButtonInstance(btnText.YesBtnText, EnumMainButton.Yes);
				dialogButton2 = this.CreateButtonInstance(btnText.NoBtnText, EnumMainButton.No);
				if (Platform.IsMac)
				{
					this.alignment_btn1.Add(dialogButton.GetWidget());
					alignment = this.alignment_btn1;
					this.alignment_btn2.Add(dialogButton2.GetWidget());
					alignment2 = this.alignment_btn2;
				}
				else if (Platform.IsWindows)
				{
					this.alignment_btn1.Add(dialogButton2.GetWidget());
					alignment2 = this.alignment_btn1;
					this.alignment_btn2.Add(dialogButton.GetWidget());
					alignment = this.alignment_btn2;
				}
				dialogButton.GetWidget().Show();
				dialogButton2.GetWidget().Show();
				break;
			case MessageBoxButton.YesNoCancel:
				dialogButton = this.CreateButtonInstance(btnText.YesBtnText, EnumMainButton.Yes);
				dialogButton2 = this.CreateButtonInstance(btnText.NoBtnText, EnumMainButton.No);
				dialogButton3 = this.CreateButtonInstance(btnText.CancelBtnText, EnumMainButton.Cancel);
				if (Platform.IsMac)
				{
					this.alignment_btn1.Add(dialogButton.GetWidget());
					alignment = this.alignment_btn1;
					this.alignment_btn2.Add(dialogButton3.GetWidget());
					alignment3 = this.alignment_btn2;
					this.alignment_btn3.Add(dialogButton2.GetWidget());
					alignment2 = this.alignment_btn3;
				}
				else if (Platform.IsWindows)
				{
					this.alignment_btn1.Add(dialogButton3.GetWidget());
					alignment3 = this.alignment_btn1;
					this.alignment_btn2.Add(dialogButton2.GetWidget());
					alignment2 = this.alignment_btn2;
					this.alignment_btn3.Add(dialogButton.GetWidget());
					alignment = this.alignment_btn3;
				}
				dialogButton.GetWidget().Show();
				dialogButton2.GetWidget().Show();
				dialogButton3.GetWidget().Show();
				break;
			}
			if (Option.CurrentApp == EnumApp.Studio)
			{
				if (btnText.IsYesBtnAutoSize && alignment != null)
				{
					alignment.WidthRequest = -1;
				}
				if (btnText.IsNoBtnAutoSize && alignment2 != null)
				{
					alignment2.WidthRequest = -1;
				}
				if (btnText.IsCancelBtnAutoSize && alignment3 != null)
				{
					alignment3.WidthRequest = -1;
				}
			}
			base.CanFocus = true;
			base.HasFocus = true;
			IDialogButton dialogButton4 = null;
			switch (mainBtn)
			{
			case EnumMainButton.No:
				dialogButton4 = dialogButton2;
				break;
			case EnumMainButton.Yes:
				dialogButton4 = dialogButton;
				break;
			case EnumMainButton.Cancel:
				dialogButton4 = dialogButton3;
				break;
			}
			if (dialogButton4 != null)
			{
				this.SetToMainButtonStyle(dialogButton4.GetWidget());
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000EAE0 File Offset: 0x0000CCE0
		private IDialogButton CreateButtonInstance(string text, EnumMainButton type)
		{
			IDialogButton dialogButton;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				dialogButton = new GeneralLauncherButton();
			}
			else
			{
				dialogButton = new CsMessageBoxButton();
			}
			dialogButton.Text = text;
			dialogButton.ButtonType = type;
			dialogButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnButtonClicked);
			return dialogButton;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000EB34 File Offset: 0x0000CD34
		private void SetToMainButtonStyle(Widget widgetButton)
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				GeneralLauncherButton generalLauncherButton = widgetButton as GeneralLauncherButton;
				generalLauncherButton.SetButtonStyle(true);
			}
			else
			{
				widgetButton.Name = "MainButton";
			}
			widgetButton.CanFocus = true;
			widgetButton.HasFocus = true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000EB84 File Offset: 0x0000CD84
		protected void OnButtonClicked(object sender, EventArgs e)
		{
			IDialogButton dialogButton = sender as IDialogButton;
			ResponseType buttonType = (ResponseType)dialogButton.ButtonType;
			base.Respond(buttonType);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EBA8 File Offset: 0x0000CDA8
		private void OnCustumTitleClose(object sender, EventArgs args)
		{
			if (this.btnsType == MessageBoxButton.YesNo)
			{
				base.Respond(ResponseType.No);
			}
			else
			{
				base.Respond(ResponseType.Cancel);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000EBDE File Offset: 0x0000CDDE
		private void HandleDialogSizeAllocated(object o, SizeAllocatedArgs args)
		{
			base.VBox.BorderWidth = 1U;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 360;
			base.Name = "Gtk.CsMessageDialog";
			base.Title = Catalog.GetString("提示");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			this.vbox_window = new VBox();
			this.vbox_window.Name = "vbox_window";
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.vbox_window.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_window[this.alignment_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.LeftPadding = 24U;
			this.alignment_main.RightPadding = 22U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.vbox_image = new VBox();
			this.vbox_image.HeightRequest = 102;
			this.vbox_image.Name = "vbox_image";
			this.vbox_image.Spacing = 6;
			this.alignment_image = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.Name = "alignment_image";
			this.alignment_image.TopPadding = 25U;
			this.alignment_image.RightPadding = 16U;
			this.imageBin = new ImageBin();
			this.imageBin.WidthRequest = 22;
			this.imageBin.HeightRequest = 22;
			this.imageBin.Events = EventMask.ButtonPressMask;
			this.imageBin.Name = "imageBin";
			this.alignment_image.Add(this.imageBin);
			this.vbox_image.Add(this.alignment_image);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_image[this.alignment_image];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.hbox_main.Add(this.vbox_image);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_main[this.vbox_image];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox_text = new VBox();
			this.vbox_text.Name = "vbox_text";
			this.vbox_text.Spacing = 6;
			this.alignment_text = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_text.Name = "alignment_text";
			this.alignment_text.TopPadding = 30U;
			this.label_text = new Label();
			this.label_text.WidthRequest = 275;
			this.label_text.Name = "label_text";
			this.label_text.Xalign = 0f;
			this.label_text.LabelProp = Catalog.GetString("在这里显示想要提示的内容信息");
			this.label_text.Wrap = true;
			this.alignment_text.Add(this.label_text);
			this.vbox_text.Add(this.alignment_text);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_text[this.alignment_text];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.hbox_main.Add(this.vbox_text);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_main[this.vbox_text];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_bottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottom.Name = "alignment_bottom";
			this.alignment_bottom.BottomPadding = 10U;
			this.hbox_bottom = new HBox();
			this.hbox_bottom.Name = "hbox_bottom";
			this.hbox_bottom.Spacing = 10;
			this.alignment_btn1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btn1.WidthRequest = 80;
			this.alignment_btn1.HeightRequest = 26;
			this.alignment_btn1.Name = "alignment_btn1";
			this.hbox_bottom.Add(this.alignment_btn1);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottom[this.alignment_btn1];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			this.alignment_btn2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btn2.WidthRequest = 80;
			this.alignment_btn2.HeightRequest = 26;
			this.alignment_btn2.Name = "alignment_btn2";
			this.hbox_bottom.Add(this.alignment_btn2);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_bottom[this.alignment_btn2];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_btn3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btn3.WidthRequest = 80;
			this.alignment_btn3.HeightRequest = 26;
			this.alignment_btn3.Name = "alignment_btn3";
			this.hbox_bottom.Add(this.alignment_btn3);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_bottom[this.alignment_btn3];
			boxChild9.PackType = PackType.End;
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.alignment_bottom.Add(this.hbox_bottom);
			this.vbox_main.Add(this.alignment_bottom);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_main[this.alignment_bottom];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			this.evtbx_bg.Add(this.alignment_main);
			this.vbox_window.Add(this.evtbx_bg);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_window[this.evtbx_bg];
			boxChild11.Position = 1;
			vbox.Add(this.vbox_window);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox[this.vbox_window];
			boxChild12.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.HeightRequest = 0;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_occupy = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_occupy.Name = "alignment_occupy";
			actionArea.Add(this.alignment_occupy);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_occupy];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 360;
			base.DefaultHeight = 172;
			actionArea.Hide();
			base.Hide();
		}

		// Token: 0x040003E7 RID: 999
		private MessageBoxButton btnsType;

		// Token: 0x040003E8 RID: 1000
		private EnumMainButton mainButton;

		// Token: 0x040003E9 RID: 1001
		private VBox vbox_window;

		// Token: 0x040003EA RID: 1002
		private Alignment alignment_title;

		// Token: 0x040003EB RID: 1003
		private EventBox evtbx_bg;

		// Token: 0x040003EC RID: 1004
		private Alignment alignment_main;

		// Token: 0x040003ED RID: 1005
		private VBox vbox_main;

		// Token: 0x040003EE RID: 1006
		private HBox hbox_main;

		// Token: 0x040003EF RID: 1007
		private VBox vbox_image;

		// Token: 0x040003F0 RID: 1008
		private Alignment alignment_image;

		// Token: 0x040003F1 RID: 1009
		private ImageBin imageBin;

		// Token: 0x040003F2 RID: 1010
		private VBox vbox_text;

		// Token: 0x040003F3 RID: 1011
		private Alignment alignment_text;

		// Token: 0x040003F4 RID: 1012
		private Label label_text;

		// Token: 0x040003F5 RID: 1013
		private Alignment alignment_bottom;

		// Token: 0x040003F6 RID: 1014
		private HBox hbox_bottom;

		// Token: 0x040003F7 RID: 1015
		private Alignment alignment_btn1;

		// Token: 0x040003F8 RID: 1016
		private Alignment alignment_btn2;

		// Token: 0x040003F9 RID: 1017
		private Alignment alignment_btn3;

		// Token: 0x040003FA RID: 1018
		private Alignment alignment_occupy;
	}
}
