using System.Collections.Generic;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger
{
	public static class Extensions
	{
		public static bool CanDebug(this ProjectOperations opers, IBuildTarget entry)
		{
			ExecutionContext context = new ExecutionContext(DebuggingService.GetExecutionHandler(), IdeApp.Workbench.ProgressMonitors, IdeApp.Workspace.ActiveExecutionTarget);
			return opers.CanExecute(entry, context);
		}

		public static IAsyncOperation Debug(this ProjectOperations opers, IBuildTarget entry)
		{
			if (opers.CurrentRunOperation != null && !opers.CurrentRunOperation.IsCompleted)
			{
				return opers.CurrentRunOperation;
			}
			ExecutionContext context = new ExecutionContext(DebuggingService.GetExecutionHandler(), IdeApp.Workbench.ProgressMonitors, IdeApp.Workspace.ActiveExecutionTarget);
			IAsyncOperation asyncOperation = opers.Execute(entry, context);
			SwitchToDebugLayout(asyncOperation);
			return asyncOperation;
		}

		public static bool CanDebugFile(this ProjectOperations opers, string file)
		{
			ExecutionContext context = new ExecutionContext(DebuggingService.GetExecutionHandler(), IdeApp.Workbench.ProgressMonitors, IdeApp.Workspace.ActiveExecutionTarget);
			return opers.CanExecuteFile(file, context);
		}

		public static IAsyncOperation DebugFile(this ProjectOperations opers, string file)
		{
			ExecutionContext context = new ExecutionContext(DebuggingService.GetExecutionHandler(), IdeApp.Workbench.ProgressMonitors, IdeApp.Workspace.ActiveExecutionTarget);
			return opers.ExecuteFile(file, context);
		}

		public static IAsyncOperation DebugApplication(this ProjectOperations opers, string executableFile, string args, string workingDir, IDictionary<string, string> envVars)
		{
			if (opers.CurrentRunOperation != null && !opers.CurrentRunOperation.IsCompleted)
			{
				return opers.CurrentRunOperation;
			}
			string oldLayout = IdeApp.Workbench.CurrentLayout;
			IdeApp.Workbench.CurrentLayout = "Debug";
			IProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetRunProgressMonitor();
			IProcessAsyncOperation processAsyncOperation = DebuggingService.Run(executableFile, args, workingDir, envVars, (IConsole)monitor);
			processAsyncOperation.Completed += delegate
			{
				monitor.Dispose();
				Application.Invoke(delegate
				{
					IdeApp.Workbench.CurrentLayout = oldLayout;
				});
			};
			opers.CurrentRunOperation = monitor.AsyncOperation;
			return opers.CurrentRunOperation;
		}

		public static IAsyncOperation AttachToProcess(this ProjectOperations opers, DebuggerEngine debugger, ProcessInfo proc)
		{
			if (opers.CurrentRunOperation != null && !opers.CurrentRunOperation.IsCompleted)
			{
				return opers.CurrentRunOperation;
			}
			IAsyncOperation asyncOperation = DebuggingService.AttachToProcess(debugger, proc);
			SwitchToDebugLayout(asyncOperation);
			opers.CurrentRunOperation = asyncOperation;
			return opers.CurrentRunOperation;
		}

		public static IAsyncOperation Debug(this Document doc)
		{
			return IdeApp.ProjectOperations.DebugFile(doc.FileName);
		}

		public static bool CanDebug(this Document doc)
		{
			if (doc.FileName != FilePath.Null)
			{
				return IdeApp.ProjectOperations.CanDebugFile(doc.FileName);
			}
			return false;
		}

		private static void SwitchToDebugLayout(IAsyncOperation oper)
		{
			string oldLayout = IdeApp.Workbench.CurrentLayout;
			IdeApp.Workbench.CurrentLayout = "Debug";
			oper.Completed += delegate
			{
				DispatchService.GuiDispatch(delegate
				{
					IdeApp.Workbench.CurrentLayout = oldLayout;
				});
			};
		}
	}
}
