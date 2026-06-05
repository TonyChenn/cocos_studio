using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mono.Addins;
using MonoDevelop.Core.AddIns;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000013 RID: 19
	public class ProcessService
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003FF6 File Offset: 0x000021F6
		public IDictionary<string, string> EnvironmentVariableOverrides
		{
			get
			{
				if (this.environmentVariableOverrides == null)
				{
					this.environmentVariableOverrides = new Dictionary<string, string>();
				}
				return this.environmentVariableOverrides;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004014 File Offset: 0x00002214
		private void ProcessEnvironmentVariableOverrides(ProcessStartInfo info)
		{
			if (this.environmentVariableOverrides == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this.environmentVariableOverrides)
			{
				if (keyValuePair.Value == null && info.EnvironmentVariables.ContainsKey(keyValuePair.Key))
				{
					info.EnvironmentVariables.Remove(keyValuePair.Key);
				}
				else
				{
					info.EnvironmentVariables[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000040B4 File Offset: 0x000022B4
		internal ProcessService()
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000040DD File Offset: 0x000022DD
		public void SetExternalConsoleHandler(ExternalConsoleHandler handler)
		{
			if (this.externalConsoleHandler != null)
			{
				throw new InvalidOperationException("External console handler already set");
			}
			this.externalConsoleHandler = handler;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000040F9 File Offset: 0x000022F9
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, EventHandler exited)
		{
			return this.StartProcess(command, arguments, workingDirectory, null, null, exited);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004108 File Offset: 0x00002308
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, ProcessEventHandler outputStreamChanged, ProcessEventHandler errorStreamChanged)
		{
			return this.StartProcess(command, arguments, workingDirectory, outputStreamChanged, errorStreamChanged, null);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004118 File Offset: 0x00002318
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, TextWriter outWriter, TextWriter errorWriter, EventHandler exited)
		{
			return this.StartProcess(command, arguments, workingDirectory, outWriter, errorWriter, exited, false);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000412C File Offset: 0x0000232C
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, TextWriter outWriter, TextWriter errorWriter, EventHandler exited, bool redirectStandardInput)
		{
			ProcessEventHandler writeHandler = OutWriter.GetWriteHandler(outWriter);
			ProcessEventHandler writeHandler2 = OutWriter.GetWriteHandler(errorWriter);
			return this.StartProcess(command, arguments, workingDirectory, writeHandler, writeHandler2, exited, redirectStandardInput);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004158 File Offset: 0x00002358
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, ProcessEventHandler outputStreamChanged, ProcessEventHandler errorStreamChanged, EventHandler exited)
		{
			return this.StartProcess(command, arguments, workingDirectory, outputStreamChanged, errorStreamChanged, exited, false);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000416A File Offset: 0x0000236A
		public ProcessWrapper StartProcess(string command, string arguments, string workingDirectory, ProcessEventHandler outputStreamChanged, ProcessEventHandler errorStreamChanged, EventHandler exited, bool redirectStandardInput)
		{
			return this.StartProcess(this.CreateProcessStartInfo(command, arguments, workingDirectory, redirectStandardInput), outputStreamChanged, errorStreamChanged, exited);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004184 File Offset: 0x00002384
		public ProcessWrapper StartProcess(ProcessStartInfo startInfo, TextWriter outWriter, TextWriter errorWriter, EventHandler exited)
		{
			ProcessEventHandler writeHandler = OutWriter.GetWriteHandler(outWriter);
			ProcessEventHandler writeHandler2 = OutWriter.GetWriteHandler(errorWriter);
			return this.StartProcess(startInfo, writeHandler, writeHandler2, exited);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000041E8 File Offset: 0x000023E8
		public ProcessWrapper StartProcess(ProcessStartInfo startInfo, ProcessEventHandler outputStreamChanged, ProcessEventHandler errorStreamChanged, EventHandler exited)
		{
			if (startInfo == null)
			{
				throw new ArgumentException("startInfo");
			}
			ProcessWrapper p = new ProcessWrapper();
			if (outputStreamChanged != null)
			{
				startInfo.RedirectStandardOutput = true;
				p.OutputStreamChanged += outputStreamChanged;
			}
			if (errorStreamChanged != null)
			{
				startInfo.RedirectStandardError = true;
				p.ErrorStreamChanged += errorStreamChanged;
			}
			startInfo.CreateNoWindow = true;
			p.StartInfo = startInfo;
			this.ProcessEnvironmentVariableOverrides(p.StartInfo);
			if (exited != null)
			{
				OperationHandler handler = null;
				handler = delegate(IAsyncOperation op)
				{
					op.Completed -= handler;
					exited(p, EventArgs.Empty);
				};
				((IAsyncOperation)p).Completed += handler;
			}
			Counters.ProcessesStarted = ++Counters.ProcessesStarted;
			p.Start();
			return p;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000042CC File Offset: 0x000024CC
		public ProcessStartInfo CreateProcessStartInfo(string command, string arguments, string workingDirectory, bool redirectStandardInput)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command.Length == 0)
			{
				throw new ArgumentException("command");
			}
			ProcessStartInfo processStartInfo;
			if (string.IsNullOrEmpty(arguments))
			{
				processStartInfo = new ProcessStartInfo(command);
			}
			else
			{
				processStartInfo = new ProcessStartInfo(command, arguments);
			}
			if (workingDirectory != null && workingDirectory.Length > 0)
			{
				processStartInfo.WorkingDirectory = workingDirectory;
			}
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardInput = redirectStandardInput;
			processStartInfo.UseShellExecute = false;
			return processStartInfo;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004346 File Offset: 0x00002546
		public IProcessAsyncOperation StartConsoleProcess(string command, string arguments, string workingDirectory, IConsole console, EventHandler exited)
		{
			return this.StartConsoleProcess(command, arguments, workingDirectory, null, console, exited);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004384 File Offset: 0x00002584
		public IProcessAsyncOperation StartConsoleProcess(string command, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables, IConsole console, EventHandler exited)
		{
			if ((console == null || console is ExternalConsole) && this.externalConsoleHandler != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (environmentVariables != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in environmentVariables)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				if (this.environmentVariableOverrides != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in this.environmentVariableOverrides)
					{
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				IProcessAsyncOperation p = this.externalConsoleHandler(command, arguments, workingDirectory, dictionary, GettextCatalog.GetString("{0} External Console", BrandingService.ApplicationName), console != null && !console.CloseOnDispose);
				if (p != null)
				{
					if (exited != null)
					{
						p.Completed += delegate(IAsyncOperation param0)
						{
							exited(p, EventArgs.Empty);
						};
					}
					Counters.ProcessesStarted = ++Counters.ProcessesStarted;
					return p;
				}
				LoggingService.LogError("Could not create external console for command: " + command + " " + arguments);
			}
			ProcessStartInfo processStartInfo = this.CreateProcessStartInfo(command, arguments, workingDirectory, false);
			if (environmentVariables != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair3 in environmentVariables)
				{
					processStartInfo.EnvironmentVariables[keyValuePair3.Key] = keyValuePair3.Value;
				}
			}
			ProcessWrapper processWrapper = this.StartProcess(processStartInfo, console.Out, console.Error, null);
			new ProcessMonitor(console, processWrapper, exited);
			return processWrapper;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000045A0 File Offset: 0x000027A0
		public IExecutionHandler GetDefaultExecutionHandler(ExecutionCommand command)
		{
			if (this.executionHandlers == null)
			{
				this.executionHandlers = new List<ExtensionNode>();
				AddinManager.AddExtensionNodeHandler("/MonoDevelop/Core/ExecutionHandlers", new ExtensionNodeEventHandler(this.OnExtensionChange));
			}
			foreach (ExtensionNode extensionNode in this.executionHandlers)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)extensionNode;
				IExecutionHandler executionHandler = (IExecutionHandler)typeExtensionNode.GetInstance(typeof(IExecutionHandler));
				if (executionHandler.CanExecute(command))
				{
					return executionHandler;
				}
			}
			return null;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004640 File Offset: 0x00002840
		public ProcessExecutionCommand CreateCommand(string file)
		{
			foreach (ICommandFactory commandFactory in AddinManager.GetExtensionObjects<ICommandFactory>("/MonoDevelop/Core/CommandFactories"))
			{
				ProcessExecutionCommand processExecutionCommand = commandFactory.CreateCommand(file);
				if (processExecutionCommand != null)
				{
					return processExecutionCommand;
				}
			}
			return new NativeExecutionCommand(file);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000048C4 File Offset: 0x00002AC4
		public IEnumerable<IExecutionModeSet> GetExecutionModes()
		{
			yield return this.defaultExecutionModeSet;
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/Core/ExecutionModes"))
			{
				ExtensionNode node = (ExtensionNode)obj;
				if (node is ExecutionModeSetNode)
				{
					yield return (ExecutionModeSetNode)node;
				}
				else if (!(node is ExecutionModeNode))
				{
					yield return (IExecutionModeSet)((TypeExtensionNode)node).GetInstance(typeof(IExecutionModeSet));
				}
			}
			yield break;
		}

		/// <summary>
		/// Returns the debug execution mode set
		/// </summary>
		/// <remarks>The returned mode set can be used to run applications in debug mode</remarks>
		// Token: 0x06000094 RID: 148 RVA: 0x000048E4 File Offset: 0x00002AE4
		public IExecutionModeSet GetDebugExecutionMode()
		{
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/Core/ExecutionModes"))
			{
				ExtensionNode extensionNode = (ExtensionNode)obj;
				if (extensionNode.Id == "MonoDevelop.Debugger")
				{
					return (IExecutionModeSet)((TypeExtensionNode)extensionNode).GetInstance(typeof(IExecutionModeSet));
				}
			}
			return null;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000496C File Offset: 0x00002B6C
		public IExecutionHandler DefaultExecutionHandler
		{
			get
			{
				return this.defaultExecutionHandler;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004974 File Offset: 0x00002B74
		public IExecutionMode DefaultExecutionMode
		{
			get
			{
				return this.defaultExecutionMode;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000497C File Offset: 0x00002B7C
		private void OnExtensionChange(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				this.executionHandlers.Add(args.ExtensionNode);
				return;
			}
			this.executionHandlers.Remove(args.ExtensionNode);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000049AC File Offset: 0x00002BAC
		private ProcessHostController GetHost(string id, bool shared, IExecutionHandler executionHandler)
		{
			if (!shared)
			{
				return new ProcessHostController(id, 0U, executionHandler);
			}
			ProcessHostController result;
			lock (this)
			{
				if (this.externalProcess == null)
				{
					this.externalProcess = new ProcessHostController("SharedHostProcess", 10000U, null);
				}
				result = this.externalProcess;
			}
			return result;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004A14 File Offset: 0x00002C14
		public IDisposable CreateExternalProcessObject(Type type)
		{
			return this.CreateExternalProcessObject(type, true, null);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004A1F File Offset: 0x00002C1F
		private void CheckRemoteType(Type type)
		{
			if (!typeof(IDisposable).IsAssignableFrom(type))
			{
				throw new ArgumentException("The remote object type must implement IDisposable", "type");
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004A44 File Offset: 0x00002C44
		public IDisposable CreateExternalProcessObject(Type type, bool shared, IList<string> userAssemblyPaths = null)
		{
			this.CheckRemoteType(type);
			ProcessHostController host = this.GetHost(type.ToString(), shared, null);
			return (IDisposable)host.CreateInstance(type.Assembly.Location, type.FullName, this.GetRequiredAddins(type), userAssemblyPaths);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004A8B File Offset: 0x00002C8B
		public IDisposable CreateExternalProcessObject(Type type, TargetRuntime runtime)
		{
			return this.CreateExternalProcessObject(type, runtime.GetExecutionHandler(), null);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004A9B File Offset: 0x00002C9B
		public IDisposable CreateExternalProcessObject(Type type, IExecutionHandler executionHandler, IList<string> userAssemblyPaths = null)
		{
			this.CheckRemoteType(type);
			return (IDisposable)this.GetHost(type.ToString(), false, executionHandler).CreateInstance(type.Assembly.Location, type.FullName, this.GetRequiredAddins(type), userAssemblyPaths);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004AD5 File Offset: 0x00002CD5
		public IDisposable CreateExternalProcessObject(string assemblyPath, string typeName, bool shared, params string[] requiredAddins)
		{
			return (IDisposable)this.GetHost(typeName, shared, null).CreateInstance(assemblyPath, typeName, requiredAddins, null);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004AEF File Offset: 0x00002CEF
		public IDisposable CreateExternalProcessObject(string assemblyPath, string typeName, IExecutionHandler executionHandler, params string[] requiredAddins)
		{
			return (IDisposable)this.GetHost(typeName, false, executionHandler).CreateInstance(assemblyPath, typeName, requiredAddins, null);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004B0C File Offset: 0x00002D0C
		public bool IsValidForRemoteHosting(IExecutionHandler handler)
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			text = Path.Combine(text, "mdhost.exe");
			return handler.CanExecute(new DotNetExecutionCommand(text));
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004B44 File Offset: 0x00002D44
		private string[] GetRequiredAddins(Type type)
		{
			if (type.IsDefined(typeof(AddinDependencyAttribute), true))
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(AddinDependencyAttribute), true);
				string[] array = new string[customAttributes.Length];
				for (int i = 0; i < customAttributes.Length; i++)
				{
					array[i] = ((AddinDependencyAttribute)customAttributes[i]).Addin;
				}
				return array;
			}
			return null;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004BA0 File Offset: 0x00002DA0
		internal void Dispose()
		{
			RemotingService.Dispose();
		}

		// Token: 0x04000045 RID: 69
		private const string ExecutionModesExtensionPath = "/MonoDevelop/Core/ExecutionModes";

		// Token: 0x04000046 RID: 70
		private ProcessHostController externalProcess;

		// Token: 0x04000047 RID: 71
		private List<ExtensionNode> executionHandlers;

		// Token: 0x04000048 RID: 72
		private DefaultExecutionModeSet defaultExecutionModeSet = new DefaultExecutionModeSet();

		// Token: 0x04000049 RID: 73
		private IExecutionHandler defaultExecutionHandler = new DefaultExecutionHandler();

		// Token: 0x0400004A RID: 74
		private IExecutionMode defaultExecutionMode = new DefaultExecutionMode();

		// Token: 0x0400004B RID: 75
		private ExternalConsoleHandler externalConsoleHandler;

		// Token: 0x0400004C RID: 76
		private Dictionary<string, string> environmentVariableOverrides;

		// Token: 0x02000014 RID: 20
		public class ExecutionModeReference
		{
			// Token: 0x060000A3 RID: 163 RVA: 0x00004BA7 File Offset: 0x00002DA7
			public ExecutionModeReference(IExecutionModeSet mset, IExecutionMode mode)
			{
				this.mset = mset;
				this.mode = mode;
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00004BC0 File Offset: 0x00002DC0
			public override bool Equals(object obj)
			{
				ProcessService.ExecutionModeReference executionModeReference = obj as ProcessService.ExecutionModeReference;
				return executionModeReference != null && executionModeReference.mset == this.mset && executionModeReference.mode.Name == this.mode.Name;
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x00004C04 File Offset: 0x00002E04
			public override int GetHashCode()
			{
				return this.mset.GetHashCode() + this.mode.Name.GetHashCode();
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004C22 File Offset: 0x00002E22
			public IExecutionMode ExecutionMode
			{
				get
				{
					return this.mode;
				}
			}

			// Token: 0x0400004D RID: 77
			private IExecutionModeSet mset;

			// Token: 0x0400004E RID: 78
			private IExecutionMode mode;
		}
	}
}
