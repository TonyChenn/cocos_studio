using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MonoDevelop.Core.LogReporting
{
	// Token: 0x02000237 RID: 567
	public abstract class CrashMonitor : ICrashMonitor
	{
		// Token: 0x06001506 RID: 5382 RVA: 0x000562B0 File Offset: 0x000544B0
		public static ICrashMonitor Create(int pid)
		{
			return new MacCrashMonitor(pid);
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06001507 RID: 5383 RVA: 0x000562B8 File Offset: 0x000544B8
		// (remove) Token: 0x06001508 RID: 5384 RVA: 0x000562F0 File Offset: 0x000544F0
		public event EventHandler ApplicationExited;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06001509 RID: 5385 RVA: 0x00056328 File Offset: 0x00054528
		// (remove) Token: 0x0600150A RID: 5386 RVA: 0x00056360 File Offset: 0x00054560
		public event EventHandler<CrashEventArgs> CrashDetected;

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x00056395 File Offset: 0x00054595
		// (set) Token: 0x0600150C RID: 5388 RVA: 0x0005639D File Offset: 0x0005459D
		public int Pid { get; private set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x000563A6 File Offset: 0x000545A6
		// (set) Token: 0x0600150E RID: 5390 RVA: 0x000563AE File Offset: 0x000545AE
		private FileSystemWatcher Watcher { get; set; }

		// Token: 0x0600150F RID: 5391 RVA: 0x000563B7 File Offset: 0x000545B7
		protected CrashMonitor(int pid, string path) : this(pid, path, "")
		{
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00056420 File Offset: 0x00054620
		protected CrashMonitor(int pid, string path, string filter)
		{
			this.Pid = pid;
			this.Watcher = new FileSystemWatcher(path, filter);
			this.Watcher.Created += delegate(object o, FileSystemEventArgs e)
			{
				this.OnCrashDetected(new CrashEventArgs(e.FullPath));
			};
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				Process processById = Process.GetProcessById(this.Pid);
				while (!processById.HasExited)
				{
					Thread.Sleep(1000);
					processById.Refresh();
				}
				Thread.Sleep(5000);
				this.OnApplicationExited();
			});
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0005647E File Offset: 0x0005467E
		protected virtual void OnApplicationExited()
		{
			if (this.ApplicationExited != null)
			{
				this.ApplicationExited(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00056499 File Offset: 0x00054699
		protected virtual void OnCrashDetected(CrashEventArgs e)
		{
			if (this.CrashDetected != null)
			{
				this.CrashDetected(this, e);
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000564B0 File Offset: 0x000546B0
		public void Start()
		{
			this.Watcher.EnableRaisingEvents = true;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000564BE File Offset: 0x000546BE
		public void Stop()
		{
			this.Watcher.EnableRaisingEvents = false;
		}
	}
}
