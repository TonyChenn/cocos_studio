using System;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	public class DebuggerEngine
	{
		private DebuggerEngineBackend engine;

		private DebuggerEngineExtensionNode node;

		private bool gotEngine;

		public string Id => node.Id;

		public string Name => node.Name;

		public DebuggerFeatures SupportedFeatures { get; private set; }

		internal DebuggerEngine(DebuggerEngineExtensionNode node)
		{
			this.node = node;
			string[] supportedFeatures = node.SupportedFeatures;
			foreach (string text in supportedFeatures)
			{
				try
				{
					object obj = Enum.Parse(typeof(DebuggerFeatures), text, ignoreCase: true);
					if (obj != null)
					{
						SupportedFeatures |= (DebuggerFeatures)obj;
					}
				}
				catch
				{
					LoggingService.LogError("Invalid feature '" + text + "' in debugger engine node (" + node.Addin.Id + ")");
				}
			}
		}

		private void LoadEngine()
		{
			if (!gotEngine)
			{
				gotEngine = true;
				object instance = node.GetInstance();
				if (instance is IDebuggerEngine debuggerEngine)
				{
					engine = new LegacyDebuggerEngineBackend(debuggerEngine);
				}
				else
				{
					engine = (DebuggerEngineBackend)node.GetInstance();
				}
			}
		}

		public bool CanDebugCommand(ExecutionCommand cmd)
		{
			LoadEngine();
			if (engine != null)
			{
				return engine.CanDebugCommand(cmd);
			}
			return false;
		}

		public bool IsDefaultDebugger(ExecutionCommand cmd)
		{
			LoadEngine();
			if (engine != null)
			{
				return engine.IsDefaultDebugger(cmd);
			}
			return false;
		}

		public DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand cmd)
		{
			LoadEngine();
			return engine.CreateDebuggerStartInfo(cmd);
		}

		public ProcessInfo[] GetAttachableProcesses()
		{
			LoadEngine();
			if (engine == null)
			{
				return new ProcessInfo[0];
			}
			return engine.GetAttachableProcesses();
		}

		public DebuggerSession CreateSession()
		{
			LoadEngine();
			return engine.CreateSession();
		}
	}
}
