using System;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Projects
{
	// Token: 0x0200014A RID: 330
	[Serializable]
	public sealed class ExecutionContext
	{
		// Token: 0x06000C4D RID: 3149 RVA: 0x0002D799 File Offset: 0x0002B999
		public ExecutionContext(IExecutionMode executionMode, IConsoleFactory consoleFactory, ExecutionTarget target) : this(executionMode.ExecutionHandler, consoleFactory, target)
		{
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0002D7AC File Offset: 0x0002B9AC
		public ExecutionContext(IExecutionHandler executionHandler, IConsoleFactory consoleFactory, ExecutionTarget target)
		{
			ITargetedExecutionHandler targetedExecutionHandler = executionHandler as ITargetedExecutionHandler;
			if (targetedExecutionHandler != null)
			{
				target = (targetedExecutionHandler.Target ?? target);
			}
			this.executionHandler = executionHandler;
			this.consoleFactory = consoleFactory;
			this.executionTarget = target;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0002D7EB File Offset: 0x0002B9EB
		public IExecutionHandler ExecutionHandler
		{
			get
			{
				return this.executionHandler;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0002D7F3 File Offset: 0x0002B9F3
		public ExecutionTarget ExecutionTarget
		{
			get
			{
				return this.executionTarget;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x0002D7FB File Offset: 0x0002B9FB
		public IConsoleFactory ConsoleFactory
		{
			get
			{
				return this.consoleFactory;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0002D803 File Offset: 0x0002BA03
		public IConsoleFactory ExternalConsoleFactory
		{
			get
			{
				return MonoDevelop.Core.Execution.ExternalConsoleFactory.Instance;
			}
		}

		// Token: 0x040003A7 RID: 935
		private IExecutionHandler executionHandler;

		// Token: 0x040003A8 RID: 936
		private IConsoleFactory consoleFactory;

		// Token: 0x040003A9 RID: 937
		private ExecutionTarget executionTarget;
	}
}
