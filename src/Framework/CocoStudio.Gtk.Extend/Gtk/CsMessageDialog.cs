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
	public class CsMessageDialog : Dialog
	{
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

		public CsMessageDialog()
		{
			throw new Exception("请使用消息框的有参构造");
		}

		public CsMessageDialog(string info, ButtonText btnText, Window parentWnd, MessageBoxImage image, EnumMainButton mainBtn, string title)
		{
			this.Build();
			this.Info = info;
			this.InitImage(image);
			this.InitButton(btnText, mainBtn);
			this.InitStyle(parentWnd, title);
		}

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

		protected void OnButtonClicked(object sender, EventArgs e)
		{
			IDialogButton dialogButton = sender as IDialogButton;
			ResponseType buttonType = (ResponseType)dialogButton.ButtonType;
			base.Respond(buttonType);
		}

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

		private void HandleDialogSizeAllocated(object o, SizeAllocatedArgs args)
		{
			base.VBox.BorderWidth = 1U;
		}

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

		private MessageBoxButton btnsType;

		private EnumMainButton mainButton;

		private VBox vbox_window;

		private Alignment alignment_title;

		private EventBox evtbx_bg;

		private Alignment alignment_main;

		private VBox vbox_main;

		private HBox hbox_main;

		private VBox vbox_image;

		private Alignment alignment_image;

		private ImageBin imageBin;

		private VBox vbox_text;

		private Alignment alignment_text;

		private Label label_text;

		private Alignment alignment_bottom;

		private HBox hbox_bottom;

		private Alignment alignment_btn1;

		private Alignment alignment_btn2;

		private Alignment alignment_btn3;

		private Alignment alignment_occupy;
	}
}
