using System;
using System.Collections.Generic;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000B0 RID: 176
	public class DotNetExecutionCommand : ProcessExecutionCommand
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x000167EC File Offset: 0x000149EC
		public DotNetExecutionCommand()
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000167F4 File Offset: 0x000149F4
		public DotNetExecutionCommand(string command) : base(command)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000167FD File Offset: 0x000149FD
		public DotNetExecutionCommand(string command, string arguments) : base(command, arguments)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00016807 File Offset: 0x00014A07
		public DotNetExecutionCommand(string command, string arguments, string workingDirectory) : base(command, arguments, workingDirectory)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016812 File Offset: 0x00014A12
		public DotNetExecutionCommand(string command, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables) : base(command, arguments, workingDirectory, environmentVariables)
		{
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001681F File Offset: 0x00014A1F
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0001683F File Offset: 0x00014A3F
		public TargetRuntime TargetRuntime
		{
			get
			{
				if (this.targetRuntime == null)
				{
					this.targetRuntime = Runtime.SystemAssemblyService.DefaultRuntime;
				}
				return this.targetRuntime;
			}
			set
			{
				this.targetRuntime = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00016848 File Offset: 0x00014A48
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x00016850 File Offset: 0x00014A50
		public bool DebugMode { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x00016859 File Offset: 0x00014A59
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00016861 File Offset: 0x00014A61
		public string RuntimeArguments { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001686A File Offset: 0x00014A6A
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x00016872 File Offset: 0x00014A72
		public IList<string> UserAssemblyPaths { get; set; }

		// Token: 0x04000207 RID: 519
		private TargetRuntime targetRuntime;
	}
}
