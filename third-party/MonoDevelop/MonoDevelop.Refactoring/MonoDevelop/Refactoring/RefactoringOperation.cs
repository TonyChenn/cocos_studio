using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Refactoring
{
	public abstract class RefactoringOperation
	{
		public string Name { get; set; }

		public bool IsBreakingAPI { get; set; }

		public virtual string AccelKey => "";

		public virtual string GetMenuDescription(RefactoringOptions options)
		{
			return Name;
		}

		public virtual bool IsValid(RefactoringOptions options)
		{
			return true;
		}

		public virtual List<Change> PerformChanges(RefactoringOptions options, object properties)
		{
			throw new NotImplementedException();
		}

		public virtual void Run(RefactoringOptions options)
		{
			List<Change> changes = PerformChanges(options, null);
			IProgressMonitor backgroundProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetBackgroundProgressMonitor(Name, null);
			RefactoringService.AcceptChanges(backgroundProgressMonitor, changes);
		}
	}
}
