using System;
using System.Text;
using System.Threading;

namespace Gtk
{
	public class CocosMonitor
	{
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

		public bool HasStarted { get; private set; }

		public bool IsProcessing { get; private set; }

		public bool IsSuccessed { get; private set; }

		public bool IsCancelled
		{
			get
			{
				return this.cts.Token.IsCancellationRequested;
			}
		}

		public event EventHandler<OutputEventArgs> OutputUpdated;

		public event EventHandler<FinishedArgs> Finished;

		public CocosMonitor(bool needSendOutputEvent)
		{
			this.needSendOutputEvent = needSendOutputEvent;
			this.HasStarted = false;
			this.IsSuccessed = false;
			this.IsProcessing = false;
			this.cts = new CancellationTokenSource();
			this.outputBuilder = new StringBuilder();
		}

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

		public void Cancel()
		{
			this.cts.Cancel();
		}

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

		public void Start()
		{
			this.IsSuccessed = false;
			this.IsProcessing = true;
			this.HasStarted = true;
		}

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

		private bool needSendOutputEvent;

		private StringBuilder outputBuilder;

		private CancellationTokenSource cts;
	}
}
