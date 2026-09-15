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
	public class OutputTipUC : PopoverWindow
	{
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

		private void Timer_Tick(object sender, EventArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.timer.Stop();
				base.Visible = false;
				return false;
			});
		}

		private void OutputEventHandle(string args)
		{
			Application.Invoke(delegate(object param0, EventArgs param1)
			{
				this.OutputInfoEventHandle(args);
			});
		}

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

		private void ShowTipPopup()
		{
			base.ShowPopup(this.GetParentWidget(), new Gdk.Rectangle(2, 12, 0, 0), PopupPosition.LeftTop);
		}

		private Widget GetParentWidget()
		{
			Widget widget = Services.MainWindow.ActiveWorkbenchWindow as Widget;
			if (widget == null)
			{
				widget = Services.MainWindow.DockNotebook;
			}
			return widget;
		}

		private Timer timer;

		private int defaultShowTime = 5000;

		private Label label;
	}
}
