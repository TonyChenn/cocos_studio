using System;
using GLib;
using Gtk;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Modules.Communal.MutualEditor
{
	internal class LauncherHandler : BaseHandler
	{
		protected override int GetStartPort()
		{
			return 9010;
		}

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

		private const int portNumber = 9010;
	}
}
