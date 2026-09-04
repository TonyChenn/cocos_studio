using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger
{
	internal class DebugEntryHandler : CommandHandler
	{
		protected override void Run()
		{
			IBuildTarget entry = IdeApp.ProjectOperations.CurrentSelectedBuildTarget;
			IAsyncOperation op = IdeApp.ProjectOperations.CheckAndBuildForExecute(entry);
			op.Completed += delegate
			{
				if (op.Success)
				{
					IdeApp.ProjectOperations.Debug(entry);
				}
			};
		}

		protected override void Update(CommandInfo info)
		{
			IBuildTarget currentSelectedBuildTarget = IdeApp.ProjectOperations.CurrentSelectedBuildTarget;
			info.Enabled = currentSelectedBuildTarget != null && !(currentSelectedBuildTarget is Workspace) && IdeApp.ProjectOperations.CanDebug(currentSelectedBuildTarget) && IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted;
		}
	}
}
