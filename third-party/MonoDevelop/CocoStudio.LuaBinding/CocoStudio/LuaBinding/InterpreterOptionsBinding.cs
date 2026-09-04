using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;

namespace CocoStudio.LuaBinding
{
	public class InterpreterOptionsBinding : ItemOptionsPanel
	{
		private InterpreterOptions panel;

		public override Widget CreatePanelWidget()
		{
			panel = new InterpreterOptions();
			panel.LuaDefault = PropertyService.Get<string>("Lua.DefaultInterpreterPath");
			panel.Lua51 = PropertyService.Get<string>("Lua.51InterpreterPath");
			panel.Lua52 = PropertyService.Get<string>("Lua.52InterpreterPath");
			panel.LuaJIT = PropertyService.Get<string>("Lua.JITInterpreterPath");
			return panel;
		}

		public override void ApplyChanges()
		{
			PropertyService.Set("Lua.DefaultInterpreterPath", panel.LuaDefault);
			PropertyService.Set("Lua.51InterpreterPath", panel.Lua51);
			PropertyService.Set("Lua.52InterpreterPath", panel.Lua52);
			PropertyService.Set("Lua.JITInterpreterPath", panel.LuaJIT);
		}
	}
}
