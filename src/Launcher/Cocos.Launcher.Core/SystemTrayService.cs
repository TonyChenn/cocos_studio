using System;
using CocoStudio.Basic;
using Gtk;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	public class SystemTrayService
	{
		public static SystemTrayService Instace { get; private set; } = new SystemTrayService();

		private SystemTrayService()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			if (!Platform.IsWindows)
			{
				this.SystemIconFileID = Option.GetEditorResourceFullPath("CocosMac_Logo.ico");
			}
			this.statusIcon = new StatusIconTray();
			this.statusIcon.Tooltip = "Cocos";
			this.statusIcon.IconPath = this.SystemIconFileID;
			this.statusIcon.Action += this.statusIcon_Action;
		}

		private void statusIcon_Action(object sender, EventArgs e)
		{
			if (Services.MainWindow.GdkWindow.IsVisible)
			{
				Services.MainWindow.Hide();
				return;
			}
			Services.MainWindow.PresentWindow();
		}

		public void Start()
		{
			if (Platform.IsWindows)
			{
				this.statusIcon.PopupMenu = SystemTrayMenu.Instance.CreateWinPopupMenu();
				return;
			}
			if (Platform.IsMac)
			{
				this.statusIcon.PopupMenu = SystemTrayMenu.Instance.CreateMacMenu();
			}
		}

		public void Dispose()
		{
			this.statusIcon.Dispose();
		}

		private string SystemIconFileID = Option.GetEditorResourceFullPath("Cocos_Logo.ico");

		private StatusIconTray statusIcon;
	}
}
