using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Refactoring
{
	public class GotoDeclarationHandler : CommandHandler
	{
		protected override void Run(object data)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument != null && !(activeDocument.FileName == FilePath.Null))
			{
				object item = CurrentRefactoryOperationsHandler.GetItem(activeDocument, out var _);
				if (item is INamedElement visitable)
				{
					IdeApp.ProjectOperations.JumpToDeclaration(visitable);
				}
				else if (item is IVariable entity)
				{
					IdeApp.ProjectOperations.JumpToDeclaration(entity);
				}
			}
		}
	}
}
