using System;
using System.Text;
using System.Threading;

namespace Gtk
{
	// Token: 0x02000068 RID: 104
	public class CocosMonitor
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00009EA0 File Offset: 0x000080A0
		public string FullOutputInfo
		{
			get
			{
				string result;
				if (this.outputBuilder == null)
				{
					result = "";
				}
				else
				{
					result = this.outputBuilder.ToString();
				}
				return result;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009ED8 File Offset: 0x000080D8
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00009EEF File Offset: 0x000080EF
		public bool HasStarted { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00009EF8 File Offset: 0x000080F8
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00009F0F File Offset: 0x0000810F
		public bool IsProcessing { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009F18 File Offset: 0x00008118
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00009F2F File Offset: 0x0000812F
		public bool IsSuccessed { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009F38 File Offset: 0x00008138
		public bool IsCancelled
		{
			get
			{
				return this.cts.Token.IsCancellationRequested;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000246 RID: 582 RVA: 0x00009F60 File Offset: 0x00008160
		// (remove) Token: 0x06000247 RID: 583 RVA: 0x00009F9C File Offset: 0x0000819C
		public event EventHandler<OutputEventArgs> OutputUpdated;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000248 RID: 584 RVA: 0x00009FD8 File Offset: 0x000081D8
		// (remove) Token: 0x06000249 RID: 585 RVA: 0x0000A014 File Offset: 0x00008214
		public event EventHandler<FinishedArgs> Finished;

		// Token: 0x0600024A RID: 586 RVA: 0x0000A050 File Offset: 0x00008250
		public CocosMonitor(bool needSendOutputEvent)
		{
			this.needSendOutputEvent = needSendOutputEvent;
			this.HasStarted = false;
			this.IsSuccessed = false;
			this.IsProcessing = false;
			this.cts = new CancellationTokenSource();
			this.outputBuilder = new StringBuilder();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A090 File Offset: 0x00008290
		public void Reset(bool needSendOutputEvent)
		{
			this.needSendOutputEvent = needSendOutputEvent;
			this.HasStarted = false;
			this.IsSuccessed = false;
			this.IsProcessing = false;
			this.outputBuilder.Clear();
			if (this.cts != null)
			{
				this.cts.Dispose();
			}
			this.cts = new CancellationTokenSource();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A0ED File Offset: 0x000082ED
		public void Cancel()
		{
			this.cts.Cancel();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A0FC File Offset: 0x000082FC
		public void SendInfo(string info)
		{
			this.outputBuilder.Append(info + "\r\n");
			if (this.needSendOutputEvent)
			{
				if (this.OutputUpdated != null)
				{
					this.OutputUpdated(this, new OutputEventArgs(info));
				}
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A150 File Offset: 0x00008350
		public void Start()
		{
			this.IsSuccessed = false;
			this.IsProcessing = true;
			this.HasStarted = true;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A16C File Offset: 0x0000836C
		public void Finish(bool isSuccess)
		{
			this.IsSuccessed = isSuccess;
			this.IsProcessing = false;
			if (this.Finished != null)
			{
				FinishedArgs e = new FinishedArgs(isSuccess);
				this.Finished(this, e);
			}
		}

		// Token: 0x0400031C RID: 796
		private bool needSendOutputEvent;

		// Token: 0x0400031D RID: 797
		private StringBuilder outputBuilder;

		// Token: 0x0400031E RID: 798
		private CancellationTokenSource cts;
	}
}
