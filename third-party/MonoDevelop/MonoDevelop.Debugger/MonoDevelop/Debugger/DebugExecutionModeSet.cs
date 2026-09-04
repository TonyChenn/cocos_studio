using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	public class DebugExecutionModeSet : IExecutionModeSet
	{
		public string Name => GettextCatalog.GetString("Debug");

		public IEnumerable<IExecutionMode> ExecutionModes
		{
			get
			{
				try
				{
					DebuggerEngine[] debuggerEngines = DebuggingService.GetDebuggerEngines();
					foreach (DebuggerEngine engine in debuggerEngines)
					{
						yield return new ExecutionMode(engine.Name, engine.Name, new InternalDebugExecutionHandler(engine));
					}
				}
				finally
				{
				}
			}
		}
	}
}
