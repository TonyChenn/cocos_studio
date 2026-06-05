using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000027 RID: 39
	public class ProgressTracker
	{
		// Token: 0x06000159 RID: 345 RVA: 0x000065E9 File Offset: 0x000047E9
		public void Reset()
		{
			this.done = false;
			this.tasks.Clear();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006600 File Offset: 0x00004800
		public void BeginTask(string name, int totalWork)
		{
			ProgressTracker.Task task = new ProgressTracker.Task();
			task.Name = name;
			task.TotalWork = totalWork;
			this.tasks.Add(task);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006630 File Offset: 0x00004830
		public void BeginStepTask(string name, int totalWork, int stepSize)
		{
			ProgressTracker.Task task = new ProgressTracker.Task();
			task.StepSize = stepSize;
			task.IsStep = true;
			task.Name = name;
			task.TotalWork = totalWork;
			this.tasks.Add(task);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000666C File Offset: 0x0000486C
		public void EndTask()
		{
			if (this.tasks.Count > 0)
			{
				ProgressTracker.Task lastTask = this.LastTask;
				this.tasks.RemoveAt(this.tasks.Count - 1);
				if (lastTask.IsStep)
				{
					this.Step(lastTask.StepSize);
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000066BC File Offset: 0x000048BC
		public void Step(int work)
		{
			if (this.tasks.Count == 0)
			{
				return;
			}
			ProgressTracker.Task lastTask = this.LastTask;
			lastTask.CurrentWork += work;
			if (lastTask.CurrentWork > lastTask.TotalWork)
			{
				lastTask.CurrentWork = lastTask.TotalWork;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006706 File Offset: 0x00004906
		private ProgressTracker.Task LastTask
		{
			get
			{
				return this.tasks[this.tasks.Count - 1];
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006720 File Offset: 0x00004920
		public string CurrentTask
		{
			get
			{
				if (this.tasks.Count == 0)
				{
					return null;
				}
				return this.LastTask.Name;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000673C File Offset: 0x0000493C
		public double CurrentTaskWork
		{
			get
			{
				if (this.tasks.Count == 0)
				{
					return 0.0;
				}
				return this.LastTask.GetWorkPercent(0.0);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000676C File Offset: 0x0000496C
		public bool UnknownWork
		{
			get
			{
				if (this.tasks.Count == 0)
				{
					return false;
				}
				for (int i = this.tasks.Count - 1; i >= 0; i--)
				{
					if (this.tasks[i].TotalWork != 1)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000067B8 File Offset: 0x000049B8
		public double GlobalWork
		{
			get
			{
				if (this.done)
				{
					return 1.0;
				}
				double num = 0.0;
				for (int i = this.tasks.Count - 1; i >= 0; i--)
				{
					ProgressTracker.Task task = this.tasks[i];
					num = task.GetWorkPercent(num) * (double)task.StepSize;
				}
				return num;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006817 File Offset: 0x00004A17
		public bool InProgress
		{
			get
			{
				return !this.done;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006822 File Offset: 0x00004A22
		public void Done()
		{
			this.done = true;
			this.tasks.Clear();
		}

		// Token: 0x04000088 RID: 136
		private bool done;

		// Token: 0x04000089 RID: 137
		private List<ProgressTracker.Task> tasks = new List<ProgressTracker.Task>();

		// Token: 0x02000028 RID: 40
		private class Task
		{
			// Token: 0x06000166 RID: 358 RVA: 0x00006849 File Offset: 0x00004A49
			public double GetWorkPercent(double part)
			{
				if (this.TotalWork <= 0)
				{
					return 0.0;
				}
				if (this.CurrentWork >= this.TotalWork)
				{
					return 1.0;
				}
				return ((double)this.CurrentWork + part) / (double)this.TotalWork;
			}

			// Token: 0x0400008A RID: 138
			public string Name;

			// Token: 0x0400008B RID: 139
			public int TotalWork;

			// Token: 0x0400008C RID: 140
			public int CurrentWork;

			// Token: 0x0400008D RID: 141
			public int StepSize = 1;

			// Token: 0x0400008E RID: 142
			public bool IsStep;
		}
	}
}
