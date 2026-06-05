using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000CC RID: 204
	internal class IntrumentationMonitor : NullProgressMonitor
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x0001BAFA File Offset: 0x00019CFA
		public IntrumentationMonitor(TimerCounter counter)
		{
			this.counter = counter;
			this.logger.TextWritten += this.HandleLoggerTextWritten;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001BB36 File Offset: 0x00019D36
		private void HandleLoggerTextWritten(string writtenText)
		{
			if (this.timers.Count > 0)
			{
				this.timers.Peek().Trace(writtenText);
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001BB58 File Offset: 0x00019D58
		public override void BeginTask(string name, int totalWork)
		{
			if (!string.IsNullOrEmpty(name))
			{
				ITimeTracker timeTracker = this.counter.BeginTiming(name);
				timeTracker.Trace(name);
				this.timers.Push(timeTracker);
			}
			else
			{
				this.timers.Push(null);
			}
			base.BeginTask(name, totalWork);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001BBA4 File Offset: 0x00019DA4
		public override void BeginStepTask(string name, int totalWork, int stepSize)
		{
			if (!string.IsNullOrEmpty(name))
			{
				ITimeTracker timeTracker = this.counter.BeginTiming(name);
				timeTracker.Trace(name);
				this.timers.Push(timeTracker);
			}
			else
			{
				this.timers.Push(null);
			}
			base.BeginStepTask(name, totalWork, stepSize);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001BBF0 File Offset: 0x00019DF0
		public override void EndTask()
		{
			if (this.timers.Count > 0)
			{
				ITimeTracker timeTracker = this.timers.Pop();
				if (timeTracker != null)
				{
					timeTracker.End();
				}
			}
			base.EndTask();
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001BC26 File Offset: 0x00019E26
		public override TextWriter Log
		{
			get
			{
				return this.logger;
			}
		}

		// Token: 0x04000249 RID: 585
		private TimerCounter counter;

		// Token: 0x0400024A RID: 586
		private Stack<ITimeTracker> timers = new Stack<ITimeTracker>();

		// Token: 0x0400024B RID: 587
		private LogTextWriter logger = new LogTextWriter();
	}
}
