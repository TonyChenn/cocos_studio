using System;
using Mono.Debugging.Client;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	[Obsolete("This interface is going to be removed. Please use MonoDevelop.Debugger.DebuggerEngineBackend")]
	public interface IDebuggerEngine
	{
		bool CanDebugCommand(ExecutionCommand cmd);

		DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand cmd);

		ProcessInfo[] GetAttachableProcesses();

		DebuggerSession CreateSession();
	}
}
