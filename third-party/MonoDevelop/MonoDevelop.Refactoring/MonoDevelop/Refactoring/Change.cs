using MonoDevelop.Core;

namespace MonoDevelop.Refactoring
{
	public abstract class Change
	{
		public string Description { get; set; }

		public Change()
		{
		}

		public abstract void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx);
	}
}
