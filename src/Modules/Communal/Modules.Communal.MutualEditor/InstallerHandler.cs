using System;
using GLib;
using Gtk;
using MonoDevelop.Core;
using Xwt.GtkBackend;

namespace Modules.Communal.MutualEditor
{
	internal class InstallerHandler : BaseHandler
	{
		protected override int GetStartPort()
		{
			return 9020;
		}

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

		private const int portNumber = 9020;
	}
}
