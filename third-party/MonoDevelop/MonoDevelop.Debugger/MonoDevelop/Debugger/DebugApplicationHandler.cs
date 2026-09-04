using System.IO;
using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class DebugApplicationHandler : CommandHandler
	{
		protected override void Run()
		{
			DebugApplicationDialog debugApplicationDialog = new DebugApplicationDialog();
			try
			{
				bool flag;
				while ((flag = MessageService.RunCustomDialog(debugApplicationDialog) == -5) && !Validate(debugApplicationDialog))
				{
				}
				if (flag)
				{
					IdeApp.ProjectOperations.DebugApplication(debugApplicationDialog.SelectedFile, debugApplicationDialog.Arguments, debugApplicationDialog.WorkingDirectory, debugApplicationDialog.EnvironmentVariables);
				}
			}
			finally
			{
				debugApplicationDialog.Destroy();
			}
		}

		private bool Validate(DebugApplicationDialog dlg)
		{
			if (string.IsNullOrEmpty(dlg.SelectedFile))
			{
				MessageService.ShowError(GettextCatalog.GetString("Please select the application to debug"));
				return false;
			}
			if (!File.Exists(dlg.SelectedFile))
			{
				MessageService.ShowError(GettextCatalog.GetString("The file '{0}' does not exist", dlg.SelectedFile));
				return false;
			}
			if (!IdeApp.ProjectOperations.CanDebugFile(dlg.SelectedFile))
			{
				MessageService.ShowError(GettextCatalog.GetString("The file '{0}' can't be debugged", dlg.SelectedFile));
				return false;
			}
			return true;
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.DebugFile);
		}
	}
}
