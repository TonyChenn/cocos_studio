using MonoDevelop.Core.Execution;

namespace CocoStudio.LuaBinding
{
	public class LuaExecutionCommand : NativeExecutionCommand
	{
		public LuaConfiguration Configuration { get; set; }

		public LuaExecutionCommand(string command)
			: base(command)
		{
		}
	}
}
