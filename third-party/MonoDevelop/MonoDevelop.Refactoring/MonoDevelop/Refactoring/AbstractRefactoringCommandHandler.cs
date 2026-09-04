using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Refactoring
{
	public abstract class AbstractRefactoringCommandHandler : CommandHandler
	{
		protected abstract void Run(RefactoringOptions options);

		protected virtual void Update(RefactoringOptions options, CommandInfo info)
		{
		}

		public void UpdateCommandInfo(CommandInfo info)
		{
			Update(info);
		}

		public void Start(object data)
		{
			Run(data);
		}

		private RefactoringOptions CreateOptions()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null)
			{
				return null;
			}
			object item = CurrentRefactoryOperationsHandler.GetItem(activeDocument, out var resolveResult);
			RefactoringOptions refactoringOptions = new RefactoringOptions(activeDocument);
			refactoringOptions.ResolveResult = resolveResult;
			refactoringOptions.SelectedItem = item;
			return refactoringOptions;
		}

		protected override void Update(CommandInfo info)
		{
			base.Update(info);
			RefactoringOptions refactoringOptions = CreateOptions();
			if (refactoringOptions != null)
			{
				Update(refactoringOptions, info);
			}
			else
			{
				info.Bypass = true;
			}
		}

		protected override void Run(object data)
		{
			RefactoringOptions refactoringOptions = CreateOptions();
			if (refactoringOptions != null)
			{
				Run(refactoringOptions);
			}
		}
	}
}
