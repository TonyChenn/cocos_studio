using System;
using System.Threading;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public class FindReferencesHandler : CommandHandler
	{
		public static void FindRefs(object obj)
		{
			ISearchProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(bringToFront: true, focusPad: true);
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					foreach (MemberReference item in ReferenceFinder.FindReferences(solution, obj, searchForAllOverloads: false, ReferenceFinder.RefactoryScope.Unknown, monitor))
					{
						monitor.ReportResult(item);
					}
				}
				catch (Exception ex)
				{
					if (monitor != null)
					{
						monitor.ReportError("Error finding references", ex);
					}
					else
					{
						LoggingService.LogError("Error finding references", ex);
					}
				}
				finally
				{
					if (monitor != null)
					{
						monitor.Dispose();
					}
				}
			});
		}

		protected override void Run(object data)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument != null && !(activeDocument.FileName == FilePath.Null))
			{
				object item = CurrentRefactoryOperationsHandler.GetItem(activeDocument, out var _);
				if (item != null)
				{
					FindRefs(item);
				}
			}
		}
	}
}
