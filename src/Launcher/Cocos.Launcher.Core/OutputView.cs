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
	public class OutputView : PopoverWindow
	{
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

		private void InitTime()
		{
			this.timer = new Timer();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Tick);
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
			this.label.Text = args;
			base.Visible = false;
			base.ShowPopup(this.parentView, new Gdk.Rectangle(0, 15, 0, 0), PopupPosition.Left);
			base.Visible = true;
			this.timer.Stop();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Start();
		}

		private void MainWindow_WidgetEvent(object o, WidgetEventArgs args)
		{
			if (args.Event.Type == EventType.Configure && base.Visible)
			{
				base.ShowPopup(this.parentView, new Gdk.Rectangle(0, 15, 0, 0), PopupPosition.Left);
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

		internal void SetParentWidget(Widget hBox)
		{
			this.parentView = hBox;
		}

		public Timer timer;

		private int defaultShowTime = 3000;

		private Label label;

		private Widget parentView;
	}
}
