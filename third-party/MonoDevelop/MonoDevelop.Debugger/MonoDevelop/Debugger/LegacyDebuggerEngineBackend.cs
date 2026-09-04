using Mono.Debugging.Client;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	internal class LegacyDebuggerEngineBackend : DebuggerEngineBackend
	{
		private IDebuggerEngine engine;

		public LegacyDebuggerEngineBackend(IDebuggerEngine engine)
		{
			this.engine = engine;
		}

		public override bool CanDebugCommand(ExecutionCommand cmd)
		{
			return engine.CanDebugCommand(cmd);
		}

		public override bool IsDefaultDebugger(ExecutionCommand cmd)
		{
			return false;
		}

		public override DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand cmd)
		{
			return engine.CreateDebuggerStartInfo(cmd);
		}

		public override ProcessInfo[] GetAttachableProcesses()
		{
			return engine.GetAttachableProcesses();
		}

		public override DebuggerSession CreateSession()
		{
			return engine.CreateSession();
		}
	}
}
