using System;
using System.Threading;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class GtkConnectionDialog : IConnectionDialog, IDisposable
	{
		private static readonly string DefaultListenMessage = GettextCatalog.GetString("Waiting for debugger to connect...");

		private CancellationTokenSource cts;

		private bool disposed;

		public event EventHandler UserCancelled;

		public void SetMessage(DebuggerStartInfo dsi, string message, bool listening, int attemptNumber)
		{
			if (!disposed && cts == null)
			{
				cts = new CancellationTokenSource();
				Application.Invoke(delegate
				{
					RunDialog(message);
				});
			}
		}

		private void RunDialog(string message)
		{
			if (disposed)
			{
				return;
			}
			string text;
			if (message == null)
			{
				text = GettextCatalog.GetString("Waiting for debugger");
			}
			else
			{
				message = message.Trim();
				int num = message.IndexOfAny(new char[2] { '\n', '\r' });
				if (num > 0)
				{
					text = message.Substring(0, num).Trim();
					message = message.Substring(num).Trim();
				}
				else
				{
					text = message;
					message = null;
				}
			}
			GenericMessage genericMessage = new GenericMessage(text, message, cts.Token);
			genericMessage.Buttons.Add(AlertButton.Cancel);
			genericMessage.DefaultButton = 0;
			MessageService.GenericAlert(genericMessage);
			cts = null;
			if (!disposed && UserCancelled != null)
			{
				UserCancelled(null, null);
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				cts?.Cancel();
			}
		}
	}
}
