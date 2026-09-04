using MonoDevelop.Components.Commands;

namespace MonoDevelop.Refactoring.Rename
{
	public class RenameHandler : AbstractRefactoringCommandHandler
	{
		protected override void Update(RefactoringOptions options, CommandInfo info)
		{
			RenameRefactoring renameRefactoring = new RenameRefactoring();
			if (!renameRefactoring.IsValid(options))
			{
				info.Bypass = true;
			}
		}

		protected override void Run(RefactoringOptions options)
		{
			RenameRefactoring renameRefactoring = new RenameRefactoring();
			if (renameRefactoring.IsValid(options))
			{
				renameRefactoring.Run(options);
			}
		}
	}
}
