using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public class RenameFileChange : Change
	{
		public string OldName { get; set; }

		public string NewName { get; set; }

		public RenameFileChange(string oldName, string newName)
		{
			if (oldName == null)
			{
				throw new ArgumentNullException("oldName");
			}
			if (newName == null)
			{
				throw new ArgumentNullException("newName");
			}
			OldName = oldName;
			NewName = newName;
			base.Description = string.Format(GettextCatalog.GetString("Rename file '{0}' to '{1}'"), Path.GetFileName(oldName), Path.GetFileName(newName));
		}

		public override void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx)
		{
			if (rctx == null)
			{
				throw new ArgumentNullException("rctx");
			}
			FileService.RenameFile(OldName, NewName);
			if (IdeApp.ProjectOperations.CurrentSelectedSolution == null)
			{
				return;
			}
			foreach (Project allProject in IdeApp.ProjectOperations.CurrentSelectedSolution.GetAllProjects())
			{
				if (allProject.GetProjectFile(NewName) != null)
				{
					IdeApp.ProjectOperations.Save(allProject);
				}
			}
		}
	}
}
