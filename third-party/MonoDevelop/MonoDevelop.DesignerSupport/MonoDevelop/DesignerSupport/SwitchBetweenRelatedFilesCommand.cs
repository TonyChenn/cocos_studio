using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public class SwitchBetweenRelatedFilesCommand : CommandHandler
	{
		private IEnumerable<ProjectFile> GetFileGroup(ProjectFile currentFile)
		{
			ProjectFile parent = currentFile.DependsOnFile ?? currentFile;
			IList<ProjectFile> children = parent.DependentChildren;
			if (children == null)
			{
				yield break;
			}
			yield return parent;
			foreach (ProjectFile c in children)
			{
				if (!CodeBehind.IsDesignerFile(c.FilePath))
				{
					yield return c;
				}
			}
		}

		protected override void Run()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			ProjectFile projectFile = activeDocument.Project.GetProjectFile(activeDocument.FileName);
			List<ProjectFile> list = GetFileGroup(projectFile).ToList();
			for (int i = 0; i < list.Count; i++)
			{
				if (projectFile.Equals(list[i]))
				{
					IdeApp.Workbench.OpenDocument(list[(i + 1) % list.Count].FilePath, bringToFront: true);
				}
			}
		}

		protected override void Update(CommandInfo info)
		{
			bool enabled = (info.Visible = false);
			info.Enabled = enabled;
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument != null && activeDocument.Project != null)
			{
				ProjectFile projectFile = activeDocument.Project.GetProjectFile(activeDocument.FileName);
				bool enabled2 = (info.Visible = projectFile != null && GetFileGroup(projectFile).Count() > 1);
				info.Enabled = enabled2;
			}
		}
	}
}
