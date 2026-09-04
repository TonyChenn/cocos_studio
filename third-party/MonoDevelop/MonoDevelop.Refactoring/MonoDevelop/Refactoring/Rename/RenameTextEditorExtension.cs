using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.Refactoring.Rename
{
	public class RenameTextEditorExtension : TextEditorExtension
	{
		[CommandUpdateHandler(EditCommands.Rename)]
		public void RenameCommand_Update(CommandInfo ci)
		{
			new RenameHandler().UpdateCommandInfo(ci);
		}

		[CommandHandler(EditCommands.Rename)]
		public void RenameCommand()
		{
			new RenameHandler().Start(base.Editor);
		}
	}
}
