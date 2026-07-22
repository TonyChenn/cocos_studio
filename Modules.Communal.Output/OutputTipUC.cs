using System;
using System.Timers;
using Cairo;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Core;

namespace Modules.Communal.Output
{
	// Token: 0x02000004 RID: 4
	public class OutputTipUC : PopoverWindow
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		public OutputTipUC()
		{
			base.SkipPagerHint = true;
			base.SkipTaskbarHint = true;
			base.AllowGrow = false;
			base.AllowShrink = false;
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.TransientFor = ApplicationCurrent.MainWindow;
			base.Theme.SetFlatColor(new Cairo.Color(0.2549019607843137, 0.2549019607843137, 0.27450980392156865));
			base.Theme.BorderColor = new Cairo.Color(0.3215686274509804, 0.3215686274509804, 0.3215686274509804);
			base.Theme.CornerRadius = 0;
			base.Theme.Padding = 2;
			LogConfig.Output.Output += this.OutputEventHandle;
			LogConfig.TipWithOutConsole.Output += this.OutputEventHandle;
			this.Initialize();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
		private void Initialize()
		{
			this.label = new Label();
			this.label.Wrap = true;
			base.ContentBox.Add(this.label);
			this.label.ShowAll();
			this.timer = new Timer();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Tick);
			if (Platform.IsWindows)
			{
				ApplicationCurrent.MainWindow.WidgetEvent += this.MainWindow_WidgetEvent;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002200 File Offset: 0x00000400
		private void MainWindow_WidgetEvent(object o, WidgetEventArgs args)
		{
			if (args.Event.Type == EventType.Configure)
			{
				if (base.Visible)
				{
					this.ShowTipPopup();
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002263 File Offset: 0x00000463
		private void Timer_Tick(object sender, EventArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.timer.Stop();
				base.Visible = false;
				return false;
			});
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002298 File Offset: 0x00000498
		private void OutputEventHandle(string args)
		{
			Application.Invoke(delegate(object param0, EventArgs param1)
			{
				this.OutputInfoEventHandle(args);
			});
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022D0 File Offset: 0x000004D0
		private void OutputInfoEventHandle(string args)
		{
			base.SetDefaultSize(50, 26);
			base.Resize(50, 26);
			this.label.Text = args;
			this.ShowTipPopup();
			this.timer.Stop();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Start();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002333 File Offset: 0x00000533
		private void ShowTipPopup()
		{
			base.ShowPopup(this.GetParentWidget(), new Gdk.Rectangle(2, 12, 0, 0), PopupPosition.LeftTop);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002350 File Offset: 0x00000550
		private Widget GetParentWidget()
		{
			Widget widget = Services.MainWindow.ActiveWorkbenchWindow as Widget;
			if (widget == null)
			{
				widget = Services.MainWindow.DockNotebook;
			}
			return widget;
		}

		// Token: 0x04000001 RID: 1
		private Timer timer;

		// Token: 0x04000002 RID: 2
		private int defaultShowTime = 5000;

		// Token: 0x04000003 RID: 3
		private Label label;
	}
}
