using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger
{
	internal class DebugHandler : CommandHandler
	{
		internal static IBuildTarget GetRunTarget()
		{
			if (IdeApp.ProjectOperations.CurrentSelectedSolution == null || IdeApp.ProjectOperations.CurrentSelectedSolution.StartupItem == null)
			{
				return IdeApp.ProjectOperations.CurrentSelectedBuildTarget;
			}
			return IdeApp.ProjectOperations.CurrentSelectedSolution.StartupItem;
		}

		internal static void BuildAndDebug()
		{
			if (!DebuggingService.IsDebuggingSupported && !IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted)
			{
				StopHandler.StopBuildOperations();
				IdeApp.ProjectOperations.CurrentRunOperation.WaitForCompleted();
			}
			if (IdeApp.Workspace.IsOpen)
			{
				IBuildTarget it = GetRunTarget();
				IAsyncOperation op = IdeApp.ProjectOperations.CheckAndBuildForExecute(it);
				op.Completed += delegate
				{
					if (op.Success)
					{
						ExecuteSolution(it);
					}
				};
				return;
			}
			Document doc = IdeApp.Workbench.ActiveDocument;
			if (doc == null)
			{
				return;
			}
			if (!IdeApp.Preferences.BuildBeforeExecuting)
			{
				ExecuteDocument(doc);
				return;
			}
			doc.Save();
			IAsyncOperation docOp = doc.Build();
			docOp.Completed += delegate
			{
				if ((!docOp.SuccessWithWarnings || IdeApp.Preferences.RunWithWarnings) && docOp.Success)
				{
					ExecuteDocument(doc);
				}
			};
		}

		private static void ExecuteSolution(IBuildTarget target)
		{
			if (IdeApp.ProjectOperations.CanDebug(target))
			{
				IdeApp.ProjectOperations.Debug(target);
			}
			else
			{
				IdeApp.ProjectOperations.Execute(target);
			}
		}

		private static void ExecuteDocument(Document doc)
		{
			if (doc.CanDebug())
			{
				doc.Debug();
			}
			else
			{
				doc.Run();
			}
		}

		protected override void Run()
		{
			if (DebuggingService.IsPaused)
			{
				DebuggingService.Resume();
			}
			else
			{
				BuildAndDebug();
			}
		}

		protected override void Update(CommandInfo info)
		{
			if (DebuggingService.IsRunning)
			{
				info.Enabled = false;
				return;
			}
			if (DebuggingService.IsPaused)
			{
				info.Enabled = true;
				info.Text = GettextCatalog.GetString("_Continue Debugging");
				info.Description = GettextCatalog.GetString("Continue the execution of the application");
				return;
			}
			if (!DebuggingService.IsDebuggingSupported)
			{
				info.Text = (IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted ? GettextCatalog.GetString("Start Without Debugging") : GettextCatalog.GetString("Restart Without Debugging"));
				info.Icon = "gtk-execute";
			}
			if (IdeApp.Workspace.IsOpen)
			{
				IBuildTarget runTarget = GetRunTarget();
				bool flag = runTarget != null && (IdeApp.ProjectOperations.CanDebug(runTarget) || (!DebuggingService.IsDebuggingSupported && IdeApp.ProjectOperations.CanExecute(runTarget)));
				info.Enabled = flag && (IdeApp.ProjectOperations.CurrentRunOperation.IsCompleted || !DebuggingService.IsDebuggingSupported);
			}
			else
			{
				Document activeDocument = IdeApp.Workbench.ActiveDocument;
				info.Enabled = activeDocument != null && activeDocument.IsBuildTarget && (activeDocument.CanRun() || activeDocument.CanDebug());
			}
		}
	}
}
