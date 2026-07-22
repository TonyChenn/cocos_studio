using System;
using System.ComponentModel;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.DefaultResource;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000023 RID: 35
	[ToolboxItem(true)]
	public class OutputViewWidget : Bin
	{
		// Token: 0x06000109 RID: 265 RVA: 0x000089E1 File Offset: 0x00006BE1
		public OutputViewWidget(Dialog parentDlg, string fullOutput)
		{
			this.Build();
			this.parentDialog = parentDlg;
			this.InitWidget(fullOutput);
			this.InitStyle();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008A04 File Offset: 0x00006C04
		private void InitWidget(string output)
		{
			this.label_text.Text = LanguageInfo.MessageBox227_FailedToCreateSolution;
			this.checkbutton_output.Label = LanguageInfo.Dialog_New_ShowOuput;
			if (!string.IsNullOrEmpty(output))
			{
				this.textview_output.Buffer.Text = output;
			}
			this.textview_output.WrapMode = WrapMode.WordChar;
			this.alignment_output.Remove(this.vbox_output);
			this.checkbutton_output.Toggled += this.CheckButtonToggledHandler;
			Widget widget;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				GeneralLauncherButton generalLauncherButton = new GeneralLauncherButton();
				generalLauncherButton.Text = LanguageInfo.Dialog_ButtonOK;
				generalLauncherButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonOkClickedHandler);
				generalLauncherButton.SetButtonStyle(true);
				widget = generalLauncherButton;
			}
			else
			{
				Button button = new Button(LanguageInfo.Dialog_ButtonOK);
				button.Clicked += this.ButtonOkClickedHandler;
				button.Name = "MainButton";
				widget = button;
			}
			this.alignment_btn.Add(widget);
			widget.Show();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008AF4 File Offset: 0x00006CF4
		private void InitStyle()
		{
			this.GtkScrolledWindow.BorderWidth = 1U;
			Gdk.Color color;
			string resourceID;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				color = new Gdk.Color(187, 187, 187);
				resourceID = "CocoStudio.DefaultResource.Images.MessageBoxIcon.LauncherWarning.png";
			}
			else
			{
				color = new Gdk.Color(29, 29, 31);
				resourceID = "CocoStudio.DefaultResource.Images.MessageBoxIcon.Warning.png";
			}
			this.eventbox_textViewBg.ModifyBg(StateType.Normal, color);
			this.eventbox_seperator.ModifyBg(StateType.Normal, color);
			Stream resourceStream = Resources.GetResourceStream(resourceID);
			if (resourceStream != null)
			{
				Xwt.Drawing.Image imageView = Xwt.Drawing.Image.FromStream(resourceStream);
				this.imageBin.SetImageView(imageView);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008B7D File Offset: 0x00006D7D
		private void CheckButtonToggledHandler(object sender, EventArgs e)
		{
			if (!this.checkbutton_output.Active)
			{
				this.alignment_output.RemoveChild();
				return;
			}
			this.alignment_output.RemoveChild();
			this.alignment_output.Add(this.vbox_output);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008BB4 File Offset: 0x00006DB4
		private void ButtonOkClickedHandler(object sender, EventArgs e)
		{
			this.parentDialog.Respond(ResponseType.Ok);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008BC4 File Offset: 0x00006DC4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 400;
			base.Name = "Modules.Communal.NewSolution.OutputViewWidget";
			this.vbox_dialog = new VBox();
			this.vbox_dialog.Name = "vbox_dialog";
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
			this.label_text = new Label();
			this.label_text.WidthRequest = 275;
			this.label_text.Name = "label_text";
			this.label_text.Xalign = 0f;
			this.label_text.LabelProp = Catalog.GetString("项目创建失败");
			this.label_text.Wrap = true;
			this.alignment_text.Add(this.label_text);
			this.vbox_text.Add(this.alignment_text);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_text[this.alignment_text];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox_main.Add(this.vbox_text);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_main[this.vbox_text];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_buttons = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_buttons.Name = "alignment_buttons";
			this.alignment_buttons.BottomPadding = 10U;
			this.hbox_buttons = new HBox();
			this.hbox_buttons.Name = "hbox_buttons";
			this.hbox_buttons.Spacing = 10;
			this.checkbutton_output = new CheckButton();
			this.checkbutton_output.CanFocus = true;
			this.checkbutton_output.Name = "checkbutton_output";
			this.checkbutton_output.Label = Catalog.GetString("显示详细输出信息");
			this.checkbutton_output.DrawIndicator = true;
			this.checkbutton_output.UseUnderline = true;
			this.hbox_buttons.Add(this.checkbutton_output);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_buttons[this.checkbutton_output];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			this.alignment_btn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btn.WidthRequest = 80;
			this.alignment_btn.HeightRequest = 26;
			this.alignment_btn.Name = "alignment_btn";
			this.hbox_buttons.Add(this.alignment_btn);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_buttons[this.alignment_btn];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			this.alignment_buttons.Add(this.hbox_buttons);
			this.vbox_main.Add(this.alignment_buttons);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.alignment_buttons];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			this.vbox_dialog.Add(this.alignment_main);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_dialog[this.alignment_main];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.alignment_output = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_output.Name = "alignment_output";
			this.alignment_output.LeftPadding = 8U;
			this.alignment_output.RightPadding = 8U;
			this.alignment_output.BottomPadding = 10U;
			this.vbox_output = new VBox();
			this.vbox_output.Name = "vbox_output";
			this.vbox_output.Spacing = 6;
			this.eventbox_seperator = new EventBox();
			this.eventbox_seperator.HeightRequest = 1;
			this.eventbox_seperator.Name = "eventbox_seperator";
			this.vbox_output.Add(this.eventbox_seperator);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_output[this.eventbox_seperator];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			this.alignment_textView = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_textView.Name = "alignment_textView";
			this.eventbox_textViewBg = new EventBox();
			this.eventbox_textViewBg.Name = "eventbox_textViewBg";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.HeightRequest = 180;
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview_output = new TextView();
			this.textview_output.CanFocus = true;
			this.textview_output.Name = "textview_output";
			this.GtkScrolledWindow.Add(this.textview_output);
			this.eventbox_textViewBg.Add(this.GtkScrolledWindow);
			this.alignment_textView.Add(this.eventbox_textViewBg);
			this.vbox_output.Add(this.alignment_textView);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_output[this.alignment_textView];
			boxChild11.Position = 1;
			this.alignment_output.Add(this.vbox_output);
			this.vbox_dialog.Add(this.alignment_output);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_dialog[this.alignment_output];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			base.Add(this.vbox_dialog);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x040000D1 RID: 209
		private Dialog parentDialog;

		// Token: 0x040000D2 RID: 210
		private VBox vbox_dialog;

		// Token: 0x040000D3 RID: 211
		private Alignment alignment_main;

		// Token: 0x040000D4 RID: 212
		private VBox vbox_main;

		// Token: 0x040000D5 RID: 213
		private HBox hbox_main;

		// Token: 0x040000D6 RID: 214
		private VBox vbox_image;

		// Token: 0x040000D7 RID: 215
		private Alignment alignment_image;

		// Token: 0x040000D8 RID: 216
		private ImageBin imageBin;

		// Token: 0x040000D9 RID: 217
		private VBox vbox_text;

		// Token: 0x040000DA RID: 218
		private Alignment alignment_text;

		// Token: 0x040000DB RID: 219
		private Label label_text;

		// Token: 0x040000DC RID: 220
		private Alignment alignment_buttons;

		// Token: 0x040000DD RID: 221
		private HBox hbox_buttons;

		// Token: 0x040000DE RID: 222
		private CheckButton checkbutton_output;

		// Token: 0x040000DF RID: 223
		private Alignment alignment_btn;

		// Token: 0x040000E0 RID: 224
		private Alignment alignment_output;

		// Token: 0x040000E1 RID: 225
		private VBox vbox_output;

		// Token: 0x040000E2 RID: 226
		private EventBox eventbox_seperator;

		// Token: 0x040000E3 RID: 227
		private Alignment alignment_textView;

		// Token: 0x040000E4 RID: 228
		private EventBox eventbox_textViewBg;

		// Token: 0x040000E5 RID: 229
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x040000E6 RID: 230
		private TextView textview_output;
	}
}
