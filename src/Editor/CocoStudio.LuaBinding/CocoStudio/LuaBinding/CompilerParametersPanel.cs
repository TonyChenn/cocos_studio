using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;

namespace CocoStudio.LuaBinding
{
	internal class CompilerParametersPanel : MultiConfigItemOptionsPanel
	{
		private CompilerParametersPanelWidget widget;

		public override Widget CreatePanelWidget()
		{
			return widget = new CompilerParametersPanelWidget();
		}

		public override void LoadConfigData()
		{
			LuaConfiguration luaConfiguration = base.CurrentConfiguration as LuaConfiguration;
			widget.DefaultFile = luaConfiguration.MainFile;
			widget.LangVersion = luaConfiguration.LangVersion;
		}

		public override void ApplyChanges()
		{
			LuaConfiguration luaConfiguration = base.CurrentConfiguration as LuaConfiguration;
			luaConfiguration.MainFile = widget.DefaultFile;
			luaConfiguration.LangVersion = widget.LangVersion;
		}
	}
}
