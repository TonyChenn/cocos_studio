using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200002C RID: 44
	public class AskDownloadSimulatorDialog : Dialog
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00008675 File Offset: 0x00006875
		public bool IsChecked
		{
			get
			{
				return this.checkbutton_neverShow.Active;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008684 File Offset: 0x00006884
		public AskDownloadSimulatorDialog(bool isDownload)
		{
			this.Build();
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Simulator.Logo.png");
			Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.MessageBoxIcon.Question.png");
			this.imagebin_logo.SetImageView(icon);
			this.imageBin_question.SetImageView(icon2);
			this.checkbutton_neverShow.Label = LanguageInfo.Run_DontShowAgain;
			this.label_introduce.Text = LanguageInfo.Run_SimulatorItrdct;
			this.label_introduce.SetFontSize(11.0);
			if (LanguageOption.CurrentLanguage == LanguageType.English)
			{
				this.label_introduce.WidthRequest = 310;
				this.label_text.WidthRequest = 340;
			}
			this.button_yes.Name = "MainButton";
			this.button_yes.Clicked += this.ButtonYesClickedHandler;
			this.button_no.Clicked += this.ButtonNoClickedHandler;
			if (isDownload)
			{
				this.checkbutton_neverShow.Hide();
				this.button_yes.WidthRequest = (this.button_no.WidthRequest = -1);
				this.button_yes.HeightRequest = (this.button_no.HeightRequest = 27);
				base.Title = LanguageInfo.Run_DownloadSimulator;
				this.label_text.Text = LanguageInfo.MessageBox269_WetherDownSimulator;
				this.button_yes.Label = string.Format(" {0} ", LanguageInfo.Run_DownloadInStore);
				this.button_no.Label = string.Format(" {0} ", LanguageInfo.Run_RunOnDefaultBrowser);
			}
			else
			{
				this.checkbutton_neverShow.Show();
				if (MonoDevelop.Core.Platform.IsWindows)
				{
					this.hbox_bottom.Remove(this.button_no);
					this.hbox_bottom.Remove(this.button_yes);
					this.hbox_bottom.PackEnd(this.button_no, false, false, 0U);
					this.hbox_bottom.PackEnd(this.button_yes, false, false, 0U);
				}
				base.Title = "Cocos Web Simulator";
				this.label_text.Text = LanguageInfo.MessageBox270_WetherUseSimulator;
				this.button_yes.Label = LanguageInfo.Dialog_ButtonYes;
				this.button_no.Label = LanguageInfo.Dialog_ButtonNo;
			}
			this.button_yes.HasFocus = true;
			this.SetToDialogStyle(ApplicationCurrent.MainWindow, true, true, true);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000088AF File Offset: 0x00006AAF
		private void ButtonYesClickedHandler(object sender, EventArgs e)
		{
			base.Respond(ResponseType.Yes);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000088B9 File Offset: 0x00006AB9
		private void ButtonNoClickedHandler(object sender, EventArgs e)
		{
			base.Respond(ResponseType.No);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000088C4 File Offset: 0x00006AC4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.CocosAdapter.AskDownloadSimulatorDialog";
			base.Title = Catalog.GetString("下载Cocos Web Simulator");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.BorderWidth = 15U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 20;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.vbox_image = new VBox();
			this.vbox_image.HeightRequest = 80;
			this.vbox_image.Name = "vbox_image";
			this.vbox_image.Spacing = 6;
			this.alignment_image = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.Name = "alignment_image";
			this.alignment_image.TopPadding = 25U;
			this.alignment_image.RightPadding = 16U;
			this.imageBin_question = new ImageBin();
			this.imageBin_question.WidthRequest = 22;
			this.imageBin_question.HeightRequest = 22;
			this.imageBin_question.Events = EventMask.ButtonPressMask;
			this.imageBin_question.Name = "imageBin_question";
			this.alignment_image.Add(this.imageBin_question);
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
			this.label_text.WidthRequest = 300;
			this.label_text.Name = "label_text";
			this.label_text.Xalign = 0f;
			this.label_text.LabelProp = Catalog.GetString("当前项目可以使用Cocos Web Simulator运行。是否要到商店中下载Cocos Web Simulator?");
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
			this.hbox_bottom = new HBox();
			this.hbox_bottom.Name = "hbox_bottom";
			this.hbox_bottom.Spacing = 10;
			this.checkbutton_neverShow = new CheckButton();
			this.checkbutton_neverShow.CanFocus = true;
			this.checkbutton_neverShow.Name = "checkbutton_neverShow";
			this.checkbutton_neverShow.Label = Catalog.GetString("不再提示");
			this.checkbutton_neverShow.DrawIndicator = true;
			this.checkbutton_neverShow.UseUnderline = true;
			this.hbox_bottom.Add(this.checkbutton_neverShow);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottom[this.checkbutton_neverShow];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			this.button_yes = new Button();
			this.button_yes.WidthRequest = 75;
			this.button_yes.CanFocus = true;
			this.button_yes.Name = "button_yes";
			this.button_yes.UseUnderline = true;
			this.button_yes.Label = Catalog.GetString("是");
			this.hbox_bottom.Add(this.button_yes);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottom[this.button_yes];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.button_no = new Button();
			this.button_no.WidthRequest = 75;
			this.button_no.CanFocus = true;
			this.button_no.Name = "button_no";
			this.button_no.UseUnderline = true;
			this.button_no.Label = Catalog.GetString("否");
			this.hbox_bottom.Add(this.button_no);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_bottom[this.button_no];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.vbox_main.Add(this.hbox_bottom);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_main[this.hbox_bottom];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.hseparator2 = new HSeparator();
			this.hseparator2.Name = "hseparator2";
			this.vbox_main.Add(this.hseparator2);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_main[this.hseparator2];
			boxChild10.Position = 2;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.hbox_introduce = new HBox();
			this.hbox_introduce.Name = "hbox_introduce";
			this.hbox_introduce.Spacing = 6;
			this.imagebin_logo = new ImageBin();
			this.imagebin_logo.WidthRequest = 80;
			this.imagebin_logo.HeightRequest = 80;
			this.imagebin_logo.Events = EventMask.ButtonPressMask;
			this.imagebin_logo.Name = "imagebin_logo";
			this.hbox_introduce.Add(this.imagebin_logo);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_introduce[this.imagebin_logo];
			boxChild11.Position = 0;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.label_introduce = new Label();
			this.label_introduce.WidthRequest = 250;
			this.label_introduce.Name = "label_introduce";
			this.label_introduce.LabelProp = Catalog.GetString("Cocos Web Simulator是cocos对Web开发者提供的快速HTML5游戏预览和代码调试增强工具，可进行场景预览、场景节点树查看、代码断点与调试。");
			this.label_introduce.Wrap = true;
			this.hbox_introduce.Add(this.label_introduce);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.hbox_introduce[this.label_introduce];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.vbox_main.Add(this.hbox_introduce);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.vbox_main[this.hbox_introduce];
			boxChild13.Position = 3;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild14.Position = 0;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_dummyBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_dummyBtn.Name = "alignment_dummyBtn";
			actionArea.Add(this.alignment_dummyBtn);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_dummyBtn];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 390;
			base.DefaultHeight = 321;
			this.checkbutton_neverShow.Hide();
			actionArea.Hide();
			base.Hide();
		}

		// Token: 0x040000A9 RID: 169
		private Alignment alignment_main;

		// Token: 0x040000AA RID: 170
		private VBox vbox_main;

		// Token: 0x040000AB RID: 171
		private HBox hbox_main;

		// Token: 0x040000AC RID: 172
		private VBox vbox_image;

		// Token: 0x040000AD RID: 173
		private Alignment alignment_image;

		// Token: 0x040000AE RID: 174
		private ImageBin imageBin_question;

		// Token: 0x040000AF RID: 175
		private VBox vbox_text;

		// Token: 0x040000B0 RID: 176
		private Alignment alignment_text;

		// Token: 0x040000B1 RID: 177
		private Label label_text;

		// Token: 0x040000B2 RID: 178
		private HBox hbox_bottom;

		// Token: 0x040000B3 RID: 179
		private CheckButton checkbutton_neverShow;

		// Token: 0x040000B4 RID: 180
		private Button button_yes;

		// Token: 0x040000B5 RID: 181
		private Button button_no;

		// Token: 0x040000B6 RID: 182
		private HSeparator hseparator2;

		// Token: 0x040000B7 RID: 183
		private HBox hbox_introduce;

		// Token: 0x040000B8 RID: 184
		private ImageBin imagebin_logo;

		// Token: 0x040000B9 RID: 185
		private Label label_introduce;

		// Token: 0x040000BA RID: 186
		private Alignment alignment_dummyBtn;
	}
}
