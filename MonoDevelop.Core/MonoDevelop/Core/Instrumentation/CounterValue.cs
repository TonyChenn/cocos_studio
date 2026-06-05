using System;
using System.Collections.Generic;
using System.Threading;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000D1 RID: 209
	[Serializable]
	public struct CounterValue
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0001C5FB File Offset: 0x0001A7FB
		internal CounterValue(int value, int totalCount, DateTime timestamp, IDictionary<string, string> metadata)
		{
			this.value = value;
			this.timestamp = timestamp;
			this.totalCount = totalCount;
			this.message = null;
			this.traces = null;
			this.threadId = 0;
			this.change = 0;
			this.metadata = metadata;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001C638 File Offset: 0x0001A838
		internal CounterValue(int value, int totalCount, int change, DateTime timestamp, string message, TimerTraceList traces, IDictionary<string, string> metadata)
		{
			this.value = value;
			this.timestamp = timestamp;
			this.totalCount = totalCount;
			this.message = message;
			this.traces = traces;
			this.change = change;
			this.threadId = Thread.CurrentThread.ManagedThreadId;
			this.metadata = metadata;
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001C68A File Offset: 0x0001A88A
		public DateTime TimeStamp
		{
			get
			{
				return this.timestamp;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001C692 File Offset: 0x0001A892
		public int Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0001C69A File Offset: 0x0001A89A
		public int TotalCount
		{
			get
			{
				return this.totalCount;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0001C6A2 File Offset: 0x0001A8A2
		public int ValueChange
		{
			get
			{
				return this.change;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001C6AA File Offset: 0x0001A8AA
		public int ThreadId
		{
			get
			{
				return this.threadId;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001C6B2 File Offset: 0x0001A8B2
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001C6BA File Offset: 0x0001A8BA
		public bool HasTimerTraces
		{
			get
			{
				return this.traces != null;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
		public IDictionary<string, string> Metadata
		{
			get
			{
				return this.metadata;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
		public TimeSpan Duration
		{
			get
			{
				if (this.traces == null)
				{
					return new TimeSpan(0L);
				}
				return this.traces.TotalTime;
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001C804 File Offset: 0x0001AA04
		public IEnumerable<TimerTrace> GetTimerTraces()
		{
			if (this.traces != null)
			{
				for (TimerTrace trace = this.traces.FirstTrace; trace != null; trace = trace.Next)
				{
					yield return trace;
				}
			}
			yield break;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001C826 File Offset: 0x0001AA26
		internal void UpdateValueIndex(int newIndex)
		{
			if (this.traces != null)
			{
				this.traces.ValueIndex = newIndex;
			}
		}

		// Token: 0x0400025F RID: 607
		private int value;

		// Token: 0x04000260 RID: 608
		private int totalCount;

		// Token: 0x04000261 RID: 609
		private int change;

		// Token: 0x04000262 RID: 610
		private DateTime timestamp;

		// Token: 0x04000263 RID: 611
		private string message;

		// Token: 0x04000264 RID: 612
		private TimerTraceList traces;

		// Token: 0x04000265 RID: 613
		private int threadId;

		// Token: 0x04000266 RID: 614
		private IDictionary<string, string> metadata;
	}
}
