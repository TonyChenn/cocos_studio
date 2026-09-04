using System;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	internal class StatusBarConnectionDialog : IConnectionDialog, IDisposable
	{
		public event EventHandler UserCancelled;

		public void SetMessage(DebuggerStartInfo dsi, string message, bool listening, int attemptNumber)
		{
			Application.Invoke(delegate
			{
				IdeApp.Workbench.StatusBar.ShowMessage(MonoDevelop.Ide.Gui.Stock.StatusConnecting, message);
			});
		}

		public void Dispose()
		{
			Application.Invoke(delegate
			{
				IdeApp.Workbench.StatusBar.ShowReady();
			});
		}
	}
}
