using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000031 RID: 49
	public class MonoPlatformExecutionHandler : NativePlatformExecutionHandler
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00006A43 File Offset: 0x00004C43
		public MonoPlatformExecutionHandler()
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006A56 File Offset: 0x00004C56
		public MonoPlatformExecutionHandler(string monoPath, IDictionary<string, string> defaultEnvironmentVariables) : base(defaultEnvironmentVariables)
		{
			this.monoPath = monoPath;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006A74 File Offset: 0x00004C74
		public override IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			DotNetExecutionCommand dotNetExecutionCommand = (DotNetExecutionCommand)command;
			string arg = string.IsNullOrEmpty(dotNetExecutionCommand.RuntimeArguments) ? "--debug" : dotNetExecutionCommand.RuntimeArguments;
			string arguments = string.Format("{2} \"{0}\" {1}", dotNetExecutionCommand.Command, dotNetExecutionCommand.Arguments, arg);
			NativeExecutionCommand command2 = new NativeExecutionCommand(this.monoPath, arguments, dotNetExecutionCommand.WorkingDirectory, dotNetExecutionCommand.EnvironmentVariables);
			return base.Execute(command2, console);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006ADC File Offset: 0x00004CDC
		public override bool CanExecute(ExecutionCommand command)
		{
			return command is DotNetExecutionCommand;
		}

		// Token: 0x04000094 RID: 148
		private string monoPath = "mono";
	}
}
