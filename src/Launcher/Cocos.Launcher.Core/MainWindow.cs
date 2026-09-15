using System;
using System.ComponentModel;
using AppKit;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using Gdk;
using Gtk;
using Modules.Communal.MutualEditor;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Cocos.Launcher.Core
{
	public class MainWindow : Gtk.Window
	{
		public event EventHandler<CancelEventArgs> Closing;

		public event EventHandler<EventArgs> ShowByTray;

		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			Services.MainWindow = this;
			this.Initialize();
			this.InitMenu();
			if (MonoDevelop.Core.Platform.IsMac)
			{
				GtkWorkarounds.GrabDesktopFocus();
			}
			base.Present();
		}

		private void Initialize()
		{
			base.WindowPosition = WindowPosition.Center;
			base.DeleteEvent += this.MainWindow_DeleteEvent;
			base.AllowGrow = true;
			base.Title = "Cocos";
			this.CreateComponents();
			base.ShowAll();
		}

		private void CreateComponents()
		{
			base.WidthRequest = 1100;
			base.HeightRequest = 725;
			EventBox eventBox = new EventBox();
			eventBox.CanFocus = true;
			eventBox.ButtonPressEvent += this.innerBox_ButtonPressEvent;
			VBox vbox = new VBox(false, 0);
			this.CreateWindowBorder(eventBox);
			this.windowTitleHBox = new HBox();
			this.windowTitleHBox.HeightRequest = 25;
			Widget windowTitle = MainPartFactory.GetWindowTitle(this);
			this.windowTitleHBox.PackStart(windowTitle, true, true, 0U);
			vbox.PackStart(this.windowTitleHBox, false, false, 0U);
			this.topHBox = new HBox();
			this.topHBox.HeightRequest = 80;
			Widget topContent = MainPartFactory.GetTopContent();
			this.topHBox.PackStart(topContent, true, true, 0U);
			vbox.PackStart(this.topHBox, false, false, 0U);
			this.tabAreaHBox = new HBox();
			Widget mainContent = MainPartFactory.GetMainContent();
			this.tabAreaHBox.PackStart(mainContent, true, true, 0U);
			vbox.PackStart(this.tabAreaHBox, true, true, 0U);
			eventBox.Add(vbox);
			eventBox.ShowAll();
			((BannerView)topContent).InitDefault();
		}

		private void CreateWindowBorder(EventBox innerBox)
		{
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				base.Decorated = false;
				HBox hbox = new HBox();
				hbox.BorderWidth = 1U;
				hbox.PackStart(innerBox, true, true, 0U);
				EventBox eventBox = new EventBox();
				eventBox.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainRectColor);
				eventBox.Add(hbox);
				base.Add(eventBox);
				eventBox.ShowAll();
				return;
			}
			base.Add(innerBox);
			base.Show();
			NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
			NativeGdkMac.SetNSWindowStyle(base.GdkWindow, style);
		}

		private void InitMenu()
		{
			Services.CommandService.SetMultiRootWindow(this);
			this.MenuManager = MainPartFactory.GetMenuManager();
		}

		protected override void OnFocusGrabbed()
		{
			base.Show();
			base.Present();
			base.OnFocusGrabbed();
		}

		private void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
		{
			base.GdkWindow.Hide();
		}

		protected override bool OnDeleteEvent(Event evnt)
		{
			base.GdkWindow.Hide();
			return true;
		}

		public bool MainWindowQuit()
		{
			if (this.Closing != null)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
				this.Closing(this, cancelEventArgs);
				if (cancelEventArgs.Cancel)
				{
					return false;
				}
			}
			SolutionLockHandler.Instance.ReleaseLock();
			MutualCore.Instance.Dispose();
			UserStatisticsFactory.ExitAll();
			Application.Quit();
			return true;
		}

		private void innerBox_ButtonPressEvent(object sender, ButtonPressEventArgs args)
		{
			EventBox eventBox = sender as EventBox;
			eventBox.HasFocus = true;
		}

		public void PresentWindow()
		{
			base.Show();
			base.Present();
			if (this.ShowByTray != null && MonoDevelop.Core.Platform.IsWindows)
			{
				this.ShowByTray(this, null);
			}
		}

		private HBox windowTitleHBox;

		private HBox topHBox;

		private HBox tabAreaHBox;

		public MenuManager MenuManager;
	}
}
