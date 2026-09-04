using System;
using GLib;
using Gtk;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000006 RID: 6
	internal class LauncherHandler : BaseHandler
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000023E4 File Offset: 0x000005E4
		protected override int GetStartPort()
		{
			return 9010;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000242C File Offset: 0x0000062C
		protected override void OnHandleMessageRecived(object sender, MessageArgs args)
		{
			if (args.Message.Action == Modules.Communal.MutualEditor.Action.Show)
			{
				if (ApplicationCurrent.MainWindow != null)
				{
					GLib.Timeout.Add(0U, delegate
					{
						if (MonoDevelop.Core.Platform.IsMac)
						{
							GtkWorkarounds.GrabDesktopFocus();
						}
						ApplicationCurrent.MainWindow.GrabFocus();
						return false;
					});
				}
			}
		}

		// Token: 0x04000007 RID: 7
		private const int portNumber = 9010;
	}
}
