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
	// Token: 0x0200004B RID: 75
	public class MainWindow : Gtk.Window
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600027F RID: 639 RVA: 0x0000A2BC File Offset: 0x000084BC
		// (remove) Token: 0x06000280 RID: 640 RVA: 0x0000A2F4 File Offset: 0x000084F4
		public event EventHandler<CancelEventArgs> Closing;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000281 RID: 641 RVA: 0x0000A32C File Offset: 0x0000852C
		// (remove) Token: 0x06000282 RID: 642 RVA: 0x0000A364 File Offset: 0x00008564
		public event EventHandler<EventArgs> ShowByTray;

		// Token: 0x06000283 RID: 643 RVA: 0x0000A399 File Offset: 0x00008599
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

		// Token: 0x06000284 RID: 644 RVA: 0x0000A3C6 File Offset: 0x000085C6
		private void Initialize()
		{
			base.WindowPosition = WindowPosition.Center;
			base.DeleteEvent += this.MainWindow_DeleteEvent;
			base.AllowGrow = true;
			base.Title = "Cocos";
			this.CreateComponents();
			base.ShowAll();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A400 File Offset: 0x00008600
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

		// Token: 0x06000286 RID: 646 RVA: 0x0000A514 File Offset: 0x00008714
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

		// Token: 0x06000287 RID: 647 RVA: 0x0000A58F File Offset: 0x0000878F
		private void InitMenu()
		{
			Services.CommandService.SetMultiRootWindow(this);
			this.MenuManager = MainPartFactory.GetMenuManager();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A5A7 File Offset: 0x000087A7
		protected override void OnFocusGrabbed()
		{
			base.Show();
			base.Present();
			base.OnFocusGrabbed();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A5BB File Offset: 0x000087BB
		private void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
		{
			base.GdkWindow.Hide();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A5C8 File Offset: 0x000087C8
		protected override bool OnDeleteEvent(Event evnt)
		{
			base.GdkWindow.Hide();
			return true;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A5D8 File Offset: 0x000087D8
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

		// Token: 0x0600028C RID: 652 RVA: 0x0000A62C File Offset: 0x0000882C
		private void innerBox_ButtonPressEvent(object sender, ButtonPressEventArgs args)
		{
			EventBox eventBox = sender as EventBox;
			eventBox.HasFocus = true;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A647 File Offset: 0x00008847
		public void PresentWindow()
		{
			base.Show();
			base.Present();
			if (this.ShowByTray != null && MonoDevelop.Core.Platform.IsWindows)
			{
				this.ShowByTray(this, null);
			}
		}

		// Token: 0x040000F5 RID: 245
		private HBox windowTitleHBox;

		// Token: 0x040000F6 RID: 246
		private HBox topHBox;

		// Token: 0x040000F7 RID: 247
		private HBox tabAreaHBox;

		// Token: 0x040000F8 RID: 248
		public MenuManager MenuManager;
	}
}
