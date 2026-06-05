using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000AF RID: 175
	public class ProcessExecutionCommand : ExecutionCommand
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x0001672E File Offset: 0x0001492E
		public ProcessExecutionCommand() : this(null)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00016737 File Offset: 0x00014937
		public ProcessExecutionCommand(string command) : this(command, "", ".", null)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001674B File Offset: 0x0001494B
		public ProcessExecutionCommand(string command, string arguments) : this(command, arguments, ".", null)
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001675B File Offset: 0x0001495B
		public ProcessExecutionCommand(string command, string arguments, string workingDirectory) : this(command, arguments, workingDirectory, null)
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00016767 File Offset: 0x00014967
		public ProcessExecutionCommand(string command, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables)
		{
			this.Command = command;
			this.Arguments = arguments;
			this.WorkingDirectory = workingDirectory;
			this.environmentVariables = environmentVariables;
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001678C File Offset: 0x0001498C
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x00016794 File Offset: 0x00014994
		public string Command { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0001679D File Offset: 0x0001499D
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x000167AE File Offset: 0x000149AE
		public string Arguments
		{
			get
			{
				return this.arguments ?? "";
			}
			set
			{
				this.arguments = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x000167B7 File Offset: 0x000149B7
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x000167BF File Offset: 0x000149BF
		public string WorkingDirectory { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000167C8 File Offset: 0x000149C8
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x000167E3 File Offset: 0x000149E3
		public IDictionary<string, string> EnvironmentVariables
		{
			get
			{
				if (this.environmentVariables == null)
				{
					this.environmentVariables = new Dictionary<string, string>();
				}
				return this.environmentVariables;
			}
			set
			{
				this.environmentVariables = value;
			}
		}

		// Token: 0x04000203 RID: 515
		private IDictionary<string, string> environmentVariables;

		// Token: 0x04000204 RID: 516
		private string arguments;
	}
}
