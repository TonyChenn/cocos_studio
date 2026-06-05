using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000030 RID: 48
	public class NativePlatformExecutionHandler : IExecutionHandler
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000695F File Offset: 0x00004B5F
		public NativePlatformExecutionHandler()
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006967 File Offset: 0x00004B67
		public NativePlatformExecutionHandler(IDictionary<string, string> defaultEnvironmentVariables)
		{
			this.defaultEnvironmentVariables = defaultEnvironmentVariables;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006978 File Offset: 0x00004B78
		public virtual IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			ProcessExecutionCommand processExecutionCommand = (ProcessExecutionCommand)command;
			IDictionary<string, string> dictionary;
			if (this.defaultEnvironmentVariables != null && this.defaultEnvironmentVariables.Count > 0)
			{
				if (processExecutionCommand.EnvironmentVariables.Count == 0)
				{
					dictionary = this.defaultEnvironmentVariables;
					goto IL_83;
				}
				dictionary = new Dictionary<string, string>(this.defaultEnvironmentVariables);
				using (IEnumerator<KeyValuePair<string, string>> enumerator = processExecutionCommand.EnvironmentVariables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> keyValuePair = enumerator.Current;
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
					goto IL_83;
				}
			}
			dictionary = processExecutionCommand.EnvironmentVariables;
			IL_83:
			return Runtime.ProcessService.StartConsoleProcess(processExecutionCommand.Command, processExecutionCommand.Arguments, processExecutionCommand.WorkingDirectory, dictionary, console, null);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006A38 File Offset: 0x00004C38
		public virtual bool CanExecute(ExecutionCommand command)
		{
			return command is NativeExecutionCommand;
		}

		// Token: 0x04000093 RID: 147
		private IDictionary<string, string> defaultEnvironmentVariables;
	}
}
