using System;
using GLib;
using Gtk;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000005 RID: 5
	internal class InstallerHandler : BaseHandler
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002338 File Offset: 0x00000538
		protected override int GetStartPort()
		{
			return 9020;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002390 File Offset: 0x00000590
		protected override void OnHandleMessageRecived(object sender, MessageArgs args)
		{
			if (args.Message.Action == Modules.Communal.MutualEditor.Action.Show)
			{
				GLib.Timeout.Add(0U, delegate
				{
					if (ApplicationCurrent.MainWindow != null)
					{
						if (MonoDevelop.Core.Platform.IsMac)
						{
							GtkWorkarounds.GrabDesktopFocus();
						}
						ApplicationCurrent.MainWindow.Present();
					}
					return false;
				});
			}
		}

		// Token: 0x04000005 RID: 5
		private const int portNumber = 9020;
	}
}
