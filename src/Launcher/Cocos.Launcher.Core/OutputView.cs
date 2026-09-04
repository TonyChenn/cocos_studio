using System;
using System.Timers;
using Cairo;
using Cocos.Launcher.Control;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003E RID: 62
	public class OutputView : PopoverWindow
	{
		// Token: 0x0600021C RID: 540 RVA: 0x00009468 File Offset: 0x00007668
		public OutputView()
		{
			base.SkipPagerHint = true;
			base.SkipTaskbarHint = true;
			base.AllowGrow = false;
			base.AllowShrink = false;
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.TransientFor = Services.MainWindow;
			base.ShowArrow = false;
			this.Initialize();
			Services.OutputService.Output += this.OutputEventHandle;
			this.InitTime();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000094E0 File Offset: 0x000076E0
		private void Initialize()
		{
			base.SetSizeRequest(960, 30);
			base.Theme.CornerRadius = 0;
			base.Theme.SetFlatColor(new Cairo.Color(1.0, 0.9450980392156862, 0.8));
			base.Theme.BorderColor = new Cairo.Color(1.0, 0.9450980392156862, 0.8);
			HBox hbox = new HBox();
			hbox.Spacing = 8;
			Alignment widget = new Alignment(0.5f, 0.5f, 1f, 1f);
			Alignment widget2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.label = new Label();
			this.label.ModifyFg(StateType.Normal, ConstantConfig.Colors.outputColor1);
			this.label.SetFontSize(14.0);
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.outputIcon.png"));
			hbox.Add(widget);
			hbox.PackStart(imageBin, false, false, 0U);
			hbox.PackStart(this.label, false, false, 0U);
			hbox.Add(widget2);
			base.ContentBox.Add(hbox);
			hbox.ShowAll();
			if (Platform.IsWindows)
			{
				ApplicationCurrent.MainWindow.WidgetEvent += this.MainWindow_WidgetEvent;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000963E File Offset: 0x0000783E
		private void InitTime()
		{
			this.timer = new Timer();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Tick);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009690 File Offset: 0x00007890
		private void OutputEventHandle(string args)
		{
			Application.Invoke(delegate(object param0, EventArgs param1)
			{
				this.OutputInfoEventHandle(args);
			});
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000096C4 File Offset: 0x000078C4
		private void OutputInfoEventHandle(string args)
		{
			this.label.Text = args;
			base.Visible = false;
			base.ShowPopup(this.parentView, new Gdk.Rectangle(0, 15, 0, 0), PopupPosition.Left);
			base.Visible = true;
			this.timer.Stop();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Start();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000972A File Offset: 0x0000792A
		private void MainWindow_WidgetEvent(object o, WidgetEventArgs args)
		{
			if (args.Event.Type == EventType.Configure && base.Visible)
			{
				base.ShowPopup(this.parentView, new Gdk.Rectangle(0, 15, 0, 0), PopupPosition.Left);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000976F File Offset: 0x0000796F
		private void Timer_Tick(object sender, EventArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.timer.Stop();
				base.Visible = false;
				return false;
			});
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00009784 File Offset: 0x00007984
		internal void SetParentWidget(Widget hBox)
		{
			this.parentView = hBox;
		}

		// Token: 0x040000D5 RID: 213
		public Timer timer;

		// Token: 0x040000D6 RID: 214
		private int defaultShowTime = 3000;

		// Token: 0x040000D7 RID: 215
		private Label label;

		// Token: 0x040000D8 RID: 216
		private Widget parentView;
	}
}
