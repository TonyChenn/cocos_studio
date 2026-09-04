using Mono.Debugging.Client;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	public abstract class DebuggerEngineBackend
	{
		public abstract bool CanDebugCommand(ExecutionCommand cmd);

		public virtual bool IsDefaultDebugger(ExecutionCommand cmd)
		{
			return false;
		}

		public abstract DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand cmd);

		public virtual ProcessInfo[] GetAttachableProcesses()
		{
			return new ProcessInfo[0];
		}

		public abstract DebuggerSession CreateSession();
	}
}
