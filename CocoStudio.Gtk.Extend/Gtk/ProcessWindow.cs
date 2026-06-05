using System;
using Gdk;
using GLib;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Ide;
using Stetic;

namespace Gtk
{
	// Token: 0x020000A1 RID: 161
	public class ProcessWindow : Window
	{
		// Token: 0x06000380 RID: 896 RVA: 0x00010D68 File Offset: 0x0000EF68
		public ProcessWindow(string title, bool canCancel, bool showOuput, bool initExpandOutput = true) : base(WindowType.Toplevel)
		{
			this.Build();
			this.parentWindow = MessageService.GetDefaultModalParent();
			this.isParentWndModal = this.parentWindow.Modal;
			this.parentWindow.Modal = false;
			this.isShowOutput = showOuput;
			if (!showOuput)
			{
				base.WidthRequest = 350;
				this.button_cancel.WidthRequest = 65;
				this.vbox_main.Remove(this.expander_output);
			}
			else if (initExpandOutput)
			{
				this.expander_output.Activate();
			}
			if (!canCancel)
			{
				this.hbox_process.Remove(this.button_cancel);
			}
			base.Title = title;
			this.label_topInfo.Text = LanguageInfo.Run_NowRunning;
			this.button_cancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.GtkLabel_showOutput.Text = LanguageInfo.Dialog_New_ShowOuput;
			base.KeyPressEvent += this.KeyPressedHandler;
			base.Destroyed += this.DestroyedHandler;
			this.SetToDialogStyle(this.parentWindow, true, true, true);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		public void StartRunning(CocosMonitor monitor)
		{
			this.monitor = monitor;
			if (this.isShowOutput)
			{
				monitor.OutputUpdated += this.OutputUpdatedHandler;
			}
			base.Show();
			Timeout.Add(10U, () => this.RefreshUI(monitor));
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00010F30 File Offset: 0x0000F130
		private bool RefreshUI(CocosMonitor monitor)
		{
			bool result;
			if (this.hasDestroyed)
			{
				result = false;
			}
			else if (monitor == null || !monitor.HasStarted)
			{
				result = true;
			}
			else if (monitor.IsProcessing)
			{
				this.progressbar.Pulse();
				result = true;
			}
			else
			{
				if (monitor.IsSuccessed)
				{
					this.label_topInfo.Text = LanguageInfo.Run_OperationComplete;
					this.progressbar.Fraction = 1.0;
					this.CloseWindow();
				}
				else
				{
					this.progressbar.Fraction = 0.0;
					this.button_cancel.Label = LanguageInfo.Dialog_ButtonClose;
					if (monitor.IsCancelled)
					{
						this.label_topInfo.Text = LanguageInfo.Run_OperationCanceled;
					}
					else
					{
						this.label_topInfo.Text = LanguageInfo.Run_OperationFinished;
					}
					if (!this.expander_output.Expanded)
					{
						this.expander_output.Activate();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00011044 File Offset: 0x0000F244
		private void CloseWindow()
		{
			if (!this.hasDestroyed)
			{
				this.Destroy();
				this.hasDestroyed = true;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001106C File Offset: 0x0000F26C
		private void ButtonCancelClickedHandler(object sender, EventArgs e)
		{
			this.CloseWindow();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00011118 File Offset: 0x0000F318
		private void OutputUpdatedHandler(object sender, OutputEventArgs e)
		{
			Timeout.Add(0U, delegate
			{
				TextIter endIter = this.textview_output.Buffer.EndIter;
				this.textview_output.Buffer.Insert(ref endIter, e.OutputInfo + "\r\n");
				this.textview_output.ScrollToIter(this.textview_output.Buffer.EndIter, 0.0, false, 0.0, 0.0);
				return false;
			});
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00011150 File Offset: 0x0000F350
		private void KeyPressedHandler(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Key.Escape)
			{
				this.CloseWindow();
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011180 File Offset: 0x0000F380
		private void DestroyedHandler(object sender, EventArgs e)
		{
			if (this.monitor != null)
			{
				if (this.monitor.IsProcessing)
				{
					this.monitor.Cancel();
				}
				if (this.isShowOutput)
				{
					this.monitor.OutputUpdated -= this.OutputUpdatedHandler;
				}
			}
			this.parentWindow.Modal = this.isParentWndModal;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000111F4 File Offset: 0x0000F3F4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 480;
			base.Name = "Gtk.ProcessWindow";
			base.Title = Catalog.GetString("ProcessWindow");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.vbox_main.BorderWidth = 15U;
			this.label_topInfo = new Label();
			this.label_topInfo.Name = "label_topInfo";
			this.label_topInfo.Xalign = 0f;
			this.label_topInfo.LabelProp = Catalog.GetString("正在执行操作，请稍候...");
			this.vbox_main.Add(this.label_topInfo);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.label_topInfo];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.hbox_process = new HBox();
			this.hbox_process.Name = "hbox_process";
			this.hbox_process.Spacing = 6;
			this.button_cancel = new Button();
			this.button_cancel.WidthRequest = 70;
			this.button_cancel.CanFocus = true;
			this.button_cancel.Name = "button_cancel";
			this.button_cancel.UseUnderline = true;
			this.button_cancel.Label = Catalog.GetString("取消");
			this.hbox_process.Add(this.button_cancel);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_process[this.button_cancel];
			boxChild2.PackType = PackType.End;
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.vbox_progressBar = new VBox();
			this.vbox_progressBar.Name = "vbox_progressBar";
			this.alignment_progressTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_progressTop.Name = "alignment_progressTop";
			this.vbox_progressBar.Add(this.alignment_progressTop);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_progressBar[this.alignment_progressTop];
			boxChild3.Position = 0;
			this.progressbar = new ProgressBar();
			this.progressbar.Name = "progressbar";
			this.progressbar.PulseStep = 0.01;
			this.vbox_progressBar.Add(this.progressbar);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_progressBar[this.progressbar];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.alignment_progressBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_progressBottom.Name = "alignment_progressBottom";
			this.vbox_progressBar.Add(this.alignment_progressBottom);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_progressBar[this.alignment_progressBottom];
			boxChild5.Position = 2;
			this.hbox_process.Add(this.vbox_progressBar);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_process[this.vbox_progressBar];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 1;
			this.vbox_main.Add(this.hbox_process);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_main[this.hbox_process];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
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
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.expander_output];
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 480;
			base.DefaultHeight = 116;
			base.Hide();
			this.button_cancel.Clicked += this.ButtonCancelClickedHandler;
		}

		// Token: 0x04000421 RID: 1057
		private Window parentWindow;

		// Token: 0x04000422 RID: 1058
		private bool isParentWndModal;

		// Token: 0x04000423 RID: 1059
		private CocosMonitor monitor;

		// Token: 0x04000424 RID: 1060
		private bool isShowOutput;

		// Token: 0x04000425 RID: 1061
		private bool hasDestroyed = false;

		// Token: 0x04000426 RID: 1062
		private VBox vbox_main;

		// Token: 0x04000427 RID: 1063
		private Label label_topInfo;

		// Token: 0x04000428 RID: 1064
		private HBox hbox_process;

		// Token: 0x04000429 RID: 1065
		private Button button_cancel;

		// Token: 0x0400042A RID: 1066
		private VBox vbox_progressBar;

		// Token: 0x0400042B RID: 1067
		private Alignment alignment_progressTop;

		// Token: 0x0400042C RID: 1068
		private ProgressBar progressbar;

		// Token: 0x0400042D RID: 1069
		private Alignment alignment_progressBottom;

		// Token: 0x0400042E RID: 1070
		private Expander expander_output;

		// Token: 0x0400042F RID: 1071
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x04000430 RID: 1072
		private TextView textview_output;

		// Token: 0x04000431 RID: 1073
		private Label GtkLabel_showOutput;
	}
}
