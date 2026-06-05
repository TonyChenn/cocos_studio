using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000E0 RID: 224
	internal class TimeCounter : ITimeTracker, IDisposable
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x00020596 File Offset: 0x0001E796
		internal TimeCounter(TimerCounter counter)
		{
			this.counter = counter;
			this.traceList = new TimerTraceList();
			this.Begin();
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000205B8 File Offset: 0x0001E7B8
		public void AddHandlerTracker(IDisposable t)
		{
			if (this.linkedTrackers == null)
			{
				this.linkedTrackers = t;
				return;
			}
			if (!(this.linkedTrackers is List<IDisposable>))
			{
				List<IDisposable> list = new List<IDisposable>();
				list.Add((IDisposable)this.linkedTrackers);
				list.Add(t);
				return;
			}
			((List<IDisposable>)this.linkedTrackers).Add(t);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00020612 File Offset: 0x0001E812
		internal TimerTraceList TraceList
		{
			get
			{
				return this.traceList;
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002061C File Offset: 0x0001E81C
		public void Trace(string message)
		{
			TimerTrace timerTrace = new TimerTrace();
			timerTrace.Timestamp = DateTime.Now;
			timerTrace.Message = message;
			if (this.lastTrace == null)
			{
				this.lastTrace = (this.traceList.FirstTrace = timerTrace);
			}
			else
			{
				this.lastTrace.Next = timerTrace;
				this.lastTrace = timerTrace;
			}
			this.traceList.TotalTime = timerTrace.Timestamp - this.traceList.FirstTrace.Timestamp;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00020699 File Offset: 0x0001E899
		internal void Begin()
		{
			this.begin = DateTime.Now;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000206A8 File Offset: 0x0001E8A8
		public void End()
		{
			if (this.counter == null)
			{
				Console.WriteLine("Timer already finished");
				return;
			}
			this.traceList.TotalTime = DateTime.Now - this.begin;
			if (this.traceList.TotalTime.TotalSeconds < this.counter.MinSeconds)
			{
				this.counter.RemoveValue(this.traceList.ValueIndex);
			}
			else
			{
				this.counter.AddTime(this.traceList.TotalTime);
			}
			this.counter = null;
			if (this.linkedTrackers is List<IDisposable>)
			{
				using (List<IDisposable>.Enumerator enumerator = ((List<IDisposable>)this.linkedTrackers).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDisposable disposable = enumerator.Current;
						disposable.Dispose();
					}
					return;
				}
			}
			if (this.linkedTrackers != null)
			{
				((IDisposable)this.linkedTrackers).Dispose();
			}
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000207A4 File Offset: 0x0001E9A4
		void IDisposable.Dispose()
		{
			this.End();
		}

		// Token: 0x04000285 RID: 645
		private DateTime begin;

		// Token: 0x04000286 RID: 646
		private TimerTraceList traceList;

		// Token: 0x04000287 RID: 647
		private TimerTrace lastTrace;

		// Token: 0x04000288 RID: 648
		private TimerCounter counter;

		// Token: 0x04000289 RID: 649
		private object linkedTrackers;
	}
}
