using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Refactoring
{
	public class OpenFileChange : Change
	{
		public string FileName { get; set; }

		public OpenFileChange(string fileName)
		{
			FileName = fileName;
			base.Description = string.Format(GettextCatalog.GetString("Open file '{0}'"), Path.GetFileName(fileName));
		}

		public override void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx)
		{
			IdeApp.Workbench.OpenDocument(FileName);
		}
	}
}
