using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.CocosAdapter
{
	public class RunInfoDialog : Dialog
	{
		public RunInfoDialog()
		{
			this.Build();
			this.Init();
			this.SetToDialogStyle(null, true, true, true);
		}

		private void Init()
		{
			base.Title = LanguageInfo.MessageBox_Notification;
			this.label_text.Text = LanguageInfo.MessageBox254_DeviceNotDetected;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonOk.Name = "MainButton";
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.MessageBoxIcon.Info.png");
			this.imageBin.SetImageView(icon);
			LabelLinkButton labelLinkButton = new LabelLinkButton(LanguageInfo.Run_LinkCourse);
			this.alignment_labelLink.Add(labelLinkButton);
			labelLinkButton.Show();
			labelLinkButton.URL = LanguageAdapter.GetLocalizedUrl(HelpLinkUrl.AndroidDeviceConnect);
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 360;
			base.HeightRequest = 160;
			base.Name = "Modules.Communal.CocosAdapter.RunInfoDialog";
			base.Title = Catalog.GetString("提示");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.LeftPadding = 24U;
			this.alignment_main.RightPadding = 22U;
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
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_image[this.alignment_image];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.hbox_main.Add(this.vbox_image);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.vbox_image];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.vbox_text = new VBox();
			this.vbox_text.Name = "vbox_text";
			this.vbox_text.Spacing = 6;
			this.alignment_text = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_text.Name = "alignment_text";
			this.alignment_text.TopPadding = 30U;
			this.alignment_text.BottomPadding = 20U;
			this.label_text = new Label();
			this.label_text.WidthRequest = 280;
			this.label_text.Name = "label_text";
			this.label_text.Xalign = 0f;
			this.label_text.LabelProp = Catalog.GetString("未检测到Andorid设备，请连接设备后重试");
			this.label_text.Wrap = true;
			this.alignment_text.Add(this.label_text);
			this.vbox_text.Add(this.alignment_text);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_text[this.alignment_text];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox_labelLink = new HBox();
			this.hbox_labelLink.Name = "hbox_labelLink";
			this.hbox_labelLink.Spacing = 6;
			this.alignment_labelLink = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_labelLink.Name = "alignment_labelLink";
			this.hbox_labelLink.Add(this.alignment_labelLink);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_labelLink[this.alignment_labelLink];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			this.vbox_text.Add(this.hbox_labelLink);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_text[this.hbox_labelLink];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			this.hbox_main.Add(this.vbox_text);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_main[this.vbox_text];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_main.Add(this.hbox_main);
			this.evtbx_bg.Add(this.alignment_main);
			vbox.Add(this.evtbx_bg);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox[this.evtbx_bg];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
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
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 360;
			base.DefaultHeight = 160;
			base.Hide();
		}

		private EventBox evtbx_bg;

		private Alignment alignment_main;

		private HBox hbox_main;

		private VBox vbox_image;

		private Alignment alignment_image;

		private ImageBin imageBin;

		private VBox vbox_text;

		private Alignment alignment_text;

		private Label label_text;

		private HBox hbox_labelLink;

		private Alignment alignment_labelLink;

		private Button buttonOk;
	}
}
