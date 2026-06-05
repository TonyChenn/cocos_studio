using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000FA RID: 250
	[Serializable]
	public class TimerCounter : Counter
	{
		// Token: 0x060008CA RID: 2250 RVA: 0x00022F2A File Offset: 0x0002112A
		public TimerCounter(string name, CounterCategory category) : base(name, category)
		{
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00022F3F File Offset: 0x0002113F
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00022F47 File Offset: 0x00021147
		public double MinSeconds
		{
			get
			{
				return this.minSeconds;
			}
			set
			{
				this.minSeconds = value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00022F50 File Offset: 0x00021150
		public TimeSpan TotalTime
		{
			get
			{
				return this.totalTime;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00022F58 File Offset: 0x00021158
		public TimeSpan AverageTime
		{
			get
			{
				if (this.totalCountWithTime <= 0)
				{
					return TimeSpan.FromTicks(0L);
				}
				return new TimeSpan(this.totalTime.Ticks / (long)this.totalCountWithTime);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00022F83 File Offset: 0x00021183
		public TimeSpan MinTime
		{
			get
			{
				if (this.totalCountWithTime <= 0)
				{
					return TimeSpan.Zero;
				}
				return this.minTime;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00022F9A File Offset: 0x0002119A
		public TimeSpan MaxTime
		{
			get
			{
				return this.maxTime;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00022FA2 File Offset: 0x000211A2
		public int CountWithDuration
		{
			get
			{
				return this.totalCountWithTime;
			}
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00022FAC File Offset: 0x000211AC
		internal void AddTime(TimeSpan time)
		{
			lock (this.values)
			{
				this.totalCountWithTime++;
				this.totalTime += time;
				if (time < this.minTime)
				{
					this.minTime = time;
				}
				if (time > this.maxTime)
				{
					this.maxTime = time;
				}
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00023030 File Offset: 0x00021230
		public override void Trace(string message)
		{
			if (base.Enabled)
			{
				if (this.lastTimer != null)
				{
					this.lastTimer.Trace(message);
				}
				else
				{
					lock (this.values)
					{
						base.StoreValue(message, null, null);
					}
				}
			}
			if (base.LogMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000230A4 File Offset: 0x000212A4
		public ITimeTracker BeginTiming()
		{
			return this.BeginTiming(null, null);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000230AE File Offset: 0x000212AE
		public ITimeTracker BeginTiming(string message)
		{
			return this.BeginTiming(message, null);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000230B8 File Offset: 0x000212B8
		public ITimeTracker BeginTiming(IDictionary<string, string> metadata)
		{
			return this.BeginTiming(null, metadata);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000230C4 File Offset: 0x000212C4
		public ITimeTracker BeginTiming(string message, IDictionary<string, string> metadata)
		{
			ITimeTracker result;
			if (!base.Enabled)
			{
				result = TimerCounter.dummyTimer;
			}
			else
			{
				TimeCounter timeCounter = new TimeCounter(this);
				lock (this.values)
				{
					result = (this.lastTimer = timeCounter);
					this.count++;
					this.totalCount++;
					int valueIndex = base.StoreValue(message, this.lastTimer, metadata);
					this.lastTimer.TraceList.ValueIndex = valueIndex;
				}
			}
			if (base.LogMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
			return result;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00023174 File Offset: 0x00021374
		public void EndTiming()
		{
			if (base.Enabled && this.lastTimer != null)
			{
				this.lastTimer.End();
			}
		}

		// Token: 0x040002C3 RID: 707
		[NonSerialized]
		private TimeCounter lastTimer;

		// Token: 0x040002C4 RID: 708
		private double minSeconds;

		// Token: 0x040002C5 RID: 709
		private TimeSpan totalTime;

		// Token: 0x040002C6 RID: 710
		private int totalCountWithTime;

		// Token: 0x040002C7 RID: 711
		private TimeSpan minTime = TimeSpan.MaxValue;

		// Token: 0x040002C8 RID: 712
		private TimeSpan maxTime;

		// Token: 0x040002C9 RID: 713
		private static ITimeTracker dummyTimer = new DummyTimerCounter();
	}
}
