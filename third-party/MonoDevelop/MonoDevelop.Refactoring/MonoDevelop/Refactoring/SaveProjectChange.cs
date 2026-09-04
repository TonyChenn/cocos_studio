using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public class SaveProjectChange : Change
	{
		public Project Project { get; set; }

		public SaveProjectChange(Project project)
		{
			Project = project;
			base.Description = string.Format(GettextCatalog.GetString("Save project {0}"), project.Name);
		}

		public override void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx)
		{
			IdeApp.ProjectOperations.Save(Project);
		}
	}
}
