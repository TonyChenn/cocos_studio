using System;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TextEditing;

namespace MonoDevelop.Debugger
{
	internal class ExceptionCaughtMessage : IDisposable
	{
		private ExceptionCaughtMiniButton miniButton;

		private ExceptionCaughtDialog dialog;

		private ExceptionCaughtButton button;

		private readonly ExceptionInfo ex;

		public FilePath File { get; private set; }

		public int Line { get; set; }

		public bool IsMinimized => miniButton != null;

		public event EventHandler Closed;

		public ExceptionCaughtMessage(ExceptionInfo val, FilePath file, int line, int col)
		{
			File = file;
			Line = line;
			ex = val;
		}

		public void ShowDialog()
		{
			if (dialog == null)
			{
				dialog = new ExceptionCaughtDialog(ex, this);
				MessageService.ShowCustomDialog(dialog, IdeApp.Workbench.RootWindow);
				dialog = null;
			}
		}

		public void ShowButton()
		{
			if (dialog != null)
			{
				dialog.Destroy();
				dialog = null;
			}
			if (button == null)
			{
				button = new ExceptionCaughtButton(ex, this, File, Line);
				TextEditorService.RegisterExtension(button);
				button.ScrollToView();
			}
			if (miniButton != null)
			{
				miniButton.Dispose();
				miniButton = null;
			}
		}

		public void ShowMiniButton()
		{
			if (dialog != null)
			{
				dialog.Destroy();
				dialog = null;
			}
			if (button != null)
			{
				button.Dispose();
				button = null;
			}
			if (miniButton == null)
			{
				miniButton = new ExceptionCaughtMiniButton(this, File, Line);
				TextEditorService.RegisterExtension(miniButton);
				miniButton.ScrollToView();
			}
		}

		public void Dispose()
		{
			if (dialog != null)
			{
				dialog.Destroy();
				dialog = null;
			}
			if (button != null)
			{
				button.Dispose();
				button = null;
			}
			if (miniButton != null)
			{
				miniButton.Dispose();
				miniButton = null;
			}
			if (Closed != null)
			{
				Closed(this, EventArgs.Empty);
			}
		}

		public void Close()
		{
			ShowButton();
		}
	}
}
