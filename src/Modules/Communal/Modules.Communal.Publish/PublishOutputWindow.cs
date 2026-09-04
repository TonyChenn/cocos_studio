using System;
using System.Threading;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.Publish
{
	// Token: 0x02000009 RID: 9
	public class PublishOutputWindow : Gtk.Window
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000035D0 File Offset: 0x000017D0
		public PublishOutputWindow(CreateParams createPrams) : base(Gtk.WindowType.Toplevel)
		{
			base.TypeHint = WindowTypeHint.Dialog;
			this.Build();
			this.SetToDialogStyle(ApplicationCurrent.MainWindow, false, true, true);
			this.prms = createPrams;
			base.DeleteEvent += this.PublishOutputWindow_DeleteEvent;
			this.SetMultiLanguageInfo();
			System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(this.StartCreating));
			thread.Start();
			System.Action refreshAction = delegate()
			{
				GLib.Timeout.Add(10U, new TimeoutHandler(this.RefreshControls));
			};
			refreshAction.BeginInvoke(null, null);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003651 File Offset: 0x00001851
		private void PublishOutputWindow_DeleteEvent(object o, DeleteEventArgs args)
		{
			this.OnBtnCancelClicked(this, new EventArgs());
			args.RetVal = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000366C File Offset: 0x0000186C
		private void SetMultiLanguageInfo()
		{
			base.Title = LanguageInfo.Dialog_NewProject;
			this.label_info.Text = LanguageInfo.Dialog_New_NewPrj + this.prms.ProjName;
			this.button_cancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.progressbar_main.Text = LanguageInfo.Dialgo_New_NowCreating;
			this.GtkLabel_showOutput.Text = LanguageInfo.Dialog_New_ShowOuput;
			this.WriteLineToTextView("Start creating");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000036DF File Offset: 0x000018DF
		private void StartCreating()
		{
			this.monitor = new CocosMonitor(true);
			this.monitor.OutputUpdated += this.OnOutputUpdated;
			Cocos2dxServices.CreateServices.CreateCocosSolution(this.prms, this.monitor);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000371C File Offset: 0x0000191C
		private void WriteLineToTextView(string newTxt)
		{
			GLib.Timeout.Add(0U, delegate
			{
				TextIter endIter = this.textview_output.Buffer.EndIter;
				this.textview_output.Buffer.Insert(ref endIter, newTxt + "\r\n");
				return false;
			});
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003750 File Offset: 0x00001950
		private bool RefreshControls()
		{
			if (this.monitor == null || !this.monitor.HasStarted)
			{
				return true;
			}
			if (this.monitor.IsProcessing)
			{
				this.progressbar_main.Pulse();
				return true;
			}
			this.monitor.OutputUpdated -= this.OnOutputUpdated;
			if (this.monitor.IsSuccessed)
			{
				this.progressbar_main.Fraction = 1.0;
			}
			else
			{
				this.progressbar_main.Fraction = 0.0;
			}
			if (this.monitor.IsCancelled)
			{
				this.progressbar_main.Text = LanguageInfo.Dialog_New_Abort;
				this.WriteLineToTextView(LanguageInfo.Dialog_New_Abort);
			}
			else if (this.monitor.IsSuccessed)
			{
				this.progressbar_main.Text = LanguageInfo.Dialog_New_CreateSuccess;
				this.WriteLineToTextView(LanguageInfo.Dialog_New_CreateSuccess);
			}
			else
			{
				this.progressbar_main.Text = LanguageInfo.Dialog_New_CreateFailed;
				this.WriteLineToTextView(LanguageInfo.Dialog_New_CreateFailed);
			}
			this.button_cancel.Label = LanguageInfo.Dialog_ButtonClose;
			this.button_cancel.Sensitive = true;
			if (this.monitor.IsSuccessed)
			{
				this.OnBtnCancelClicked(this, new EventArgs());
			}
			else if (!this.expander_output.Expanded)
			{
				this.expander_output.Activate();
			}
			return false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000389C File Offset: 0x00001A9C
		private void OnOutputUpdated(object sender, OutputEventArgs e)
		{
			this.WriteLineToTextView(e.OutputInfo);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000038AA File Offset: 0x00001AAA
		protected void OnBtnCancelClicked(object sender, EventArgs e)
		{
			if (this.monitor.IsProcessing)
			{
				this.button_cancel.Sensitive = false;
				this.monitor.Cancel();
				return;
			}
			this.Destroy();
			bool isSuccessed = this.monitor.IsSuccessed;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000038E4 File Offset: 0x00001AE4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.Publish.PublishOutputWindow";
			base.Title = Catalog.GetString("PublishOutputWindow");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.vbox_main.BorderWidth = 15U;
			this.label_info = new Label();
			this.label_info.Name = "label_info";
			this.label_info.Xalign = 0f;
			this.label_info.LabelProp = Catalog.GetString("发布项目XXX");
			this.vbox_main.Add(this.label_info);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.label_info];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.hbox_top = new HBox();
			this.hbox_top.HeightRequest = 26;
			this.hbox_top.Name = "hbox_top";
			this.hbox_top.Spacing = 6;
			this.progressbar_main = new ProgressBar();
			this.progressbar_main.WidthRequest = 400;
			this.progressbar_main.Name = "progressbar_main";
			this.progressbar_main.Text = Catalog.GetString("正在发布");
			this.progressbar_main.PulseStep = 0.01;
			this.hbox_top.Add(this.progressbar_main);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_top[this.progressbar_main];
			boxChild2.Position = 0;
			this.button_cancel = new Button();
			this.button_cancel.WidthRequest = 65;
			this.button_cancel.CanFocus = true;
			this.button_cancel.Name = "button_cancel";
			this.button_cancel.UseUnderline = true;
			this.button_cancel.Label = Catalog.GetString("取消");
			this.hbox_top.Add(this.button_cancel);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_top[this.button_cancel];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox_main.Add(this.hbox_top);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.hbox_top];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.expander_output = new Expander(null);
			this.expander_output.CanFocus = true;
			this.expander_output.Name = "expander_output";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.HeightRequest = 180;
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview_output = new TextView();
			this.textview_output.CanFocus = true;
			this.textview_output.Name = "textview_output";
			this.textview_output.Editable = false;
			this.textview_output.WrapMode = WrapMode.Word;
			this.GtkScrolledWindow.Add(this.textview_output);
			this.expander_output.Add(this.GtkScrolledWindow);
			this.GtkLabel_showOutput = new Label();
			this.GtkLabel_showOutput.Name = "GtkLabel_showOutput";
			this.GtkLabel_showOutput.LabelProp = Catalog.GetString(" 显示完整输出窗口");
			this.GtkLabel_showOutput.UseUnderline = true;
			this.expander_output.LabelWidget = this.GtkLabel_showOutput;
			this.vbox_main.Add(this.expander_output);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.expander_output];
			boxChild5.Position = 2;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 501;
			base.DefaultHeight = 108;
			base.Show();
			this.button_cancel.Clicked += this.OnBtnCancelClicked;
		}

		// Token: 0x04000016 RID: 22
		private CreateParams prms;

		// Token: 0x04000017 RID: 23
		private CocosMonitor monitor;

		// Token: 0x04000018 RID: 24
		private VBox vbox_main;

		// Token: 0x04000019 RID: 25
		private Label label_info;

		// Token: 0x0400001A RID: 26
		private HBox hbox_top;

		// Token: 0x0400001B RID: 27
		private ProgressBar progressbar_main;

		// Token: 0x0400001C RID: 28
		private Button button_cancel;

		// Token: 0x0400001D RID: 29
		private Expander expander_output;

		// Token: 0x0400001E RID: 30
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x0400001F RID: 31
		private TextView textview_output;

		// Token: 0x04000020 RID: 32
		private Label GtkLabel_showOutput;
	}
}
