using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class AttachToProcessHandler : CommandHandler
	{
		protected override void Run()
		{
			AttachToProcessDialog attachToProcessDialog = new AttachToProcessDialog();
			try
			{
				if (MessageService.RunCustomDialog(attachToProcessDialog) == -5)
				{
					IdeApp.ProjectOperations.AttachToProcess(attachToProcessDialog.SelectedDebugger, attachToProcessDialog.SelectedProcess);
				}
			}
			finally
			{
				attachToProcessDialog.Destroy();
			}
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Attaching);
		}
	}
}
