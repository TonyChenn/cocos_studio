using System;
using CocoStudio.Basic;
using Gtk;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000014 RID: 20
	public class SystemTrayService
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004DA1 File Offset: 0x00002FA1
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004DA8 File Offset: 0x00002FA8
		public static SystemTrayService Instace { get; private set; } = new SystemTrayService();

		// Token: 0x06000097 RID: 151 RVA: 0x00004DB0 File Offset: 0x00002FB0
		private SystemTrayService()
		{
			this.Initialize();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004DDC File Offset: 0x00002FDC
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

		// Token: 0x0600009A RID: 154 RVA: 0x00004E43 File Offset: 0x00003043
		private void statusIcon_Action(object sender, EventArgs e)
		{
			if (Services.MainWindow.GdkWindow.IsVisible)
			{
				Services.MainWindow.Hide();
				return;
			}
			Services.MainWindow.PresentWindow();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004E6B File Offset: 0x0000306B
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

		// Token: 0x0600009C RID: 156 RVA: 0x00004EA6 File Offset: 0x000030A6
		public void Dispose()
		{
			this.statusIcon.Dispose();
		}

		// Token: 0x0400004A RID: 74
		private string SystemIconFileID = Option.GetEditorResourceFullPath("Cocos_Logo.ico");

		// Token: 0x0400004B RID: 75
		private StatusIconTray statusIcon;
	}
}
