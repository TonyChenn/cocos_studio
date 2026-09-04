using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Commands;

namespace MonoDevelop.Debugger
{
	internal class GlobalRunMethodHandler
	{
		[CommandHandler(ProjectCommands.Run)]
		public void OnRun()
		{
		}

		[CommandUpdateHandler(ProjectCommands.Run)]
		public void OnRunUpdate(CommandInfo cinfo)
		{
			if (!DebuggingService.IsDebuggingSupported)
			{
				cinfo.Visible = false;
			}
			else
			{
				cinfo.Bypass = true;
			}
		}
	}
}
