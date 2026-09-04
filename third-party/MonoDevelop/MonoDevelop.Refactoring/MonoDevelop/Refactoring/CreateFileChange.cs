using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Refactoring
{
	public class CreateFileChange : Change
	{
		public string FileName { get; set; }

		public string Content { get; set; }

		public CreateFileChange(string fileName, string content)
		{
			FileName = fileName;
			Content = content;
			base.Description = string.Format(GettextCatalog.GetString("Create file '{0}'"), Path.GetFileName(fileName));
		}

		public override void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx)
		{
			File.WriteAllText(FileName, Content);
			rctx.Document.Project.AddFile(FileName);
			IdeApp.ProjectOperations.Save(rctx.Document.Project);
		}
	}
}
