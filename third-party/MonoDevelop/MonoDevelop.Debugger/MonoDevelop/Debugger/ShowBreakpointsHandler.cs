using System.Linq;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	internal class ShowBreakpointsHandler : CommandHandler
	{
		protected override void Run()
		{
			IdeApp.Workbench.Pads.FirstOrDefault((Pad p) => p.Id == "MonoDevelop.Debugger.BreakpointPad")?.BringToFront();
		}
	}
}
