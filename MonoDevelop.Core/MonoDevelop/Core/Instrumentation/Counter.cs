using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000D0 RID: 208
	[Serializable]
	public class Counter : MarshalByRefObject
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001BD49 File Offset: 0x00019F49
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001BD51 File Offset: 0x00019F51
		public bool StoreValues
		{
			get
			{
				return this.storeValues;
			}
			set
			{
				this.storeValues = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001BD5A File Offset: 0x00019F5A
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001BD62 File Offset: 0x00019F62
		internal List<InstrumentationConsumer> Handlers
		{
			get
			{
				InstrumentationService.InitializeHandlers();
				return this.handlers;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001BD6F File Offset: 0x00019F6F
		internal void UpdateStatus()
		{
			InstrumentationService.InitializeHandlers();
			this.enabled = (InstrumentationService.Enabled || this.Handlers.Count > 0);
			this.storeValues = InstrumentationService.Enabled;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		internal Counter(string name, CounterCategory category)
		{
			this.name = name;
			this.category = category;
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001BDF6 File Offset: 0x00019FF6
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001BDFE File Offset: 0x00019FFE
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001BE10 File Offset: 0x0001A010
		public string Id
		{
			get
			{
				return this.id ?? this.Name;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001BE19 File Offset: 0x0001A019
		public CounterCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001BE21 File Offset: 0x0001A021
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001BE29 File Offset: 0x0001A029
		public TimeSpan Resolution
		{
			get
			{
				return this.resolution;
			}
			set
			{
				this.resolution = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001BE32 File Offset: 0x0001A032
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001BE3A File Offset: 0x0001A03A
		public bool LogMessages
		{
			get
			{
				return this.logMessages;
			}
			set
			{
				this.logMessages = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001BE43 File Offset: 0x0001A043
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001BE4C File Offset: 0x0001A04C
		public int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				lock (this.values)
				{
					if (value > this.count)
					{
						this.totalCount += value - this.count;
					}
					this.count = value;
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
			internal set
			{
				this.disposed = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001BEBD File Offset: 0x0001A0BD
		public int TotalCount
		{
			get
			{
				return this.totalCount;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001BEC5 File Offset: 0x0001A0C5
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001BECD File Offset: 0x0001A0CD
		public CounterDisplayMode DisplayMode
		{
			get
			{
				return this.displayMode;
			}
			set
			{
				this.displayMode = value;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public IEnumerable<CounterValue> GetValues()
		{
			IEnumerable<CounterValue> result;
			lock (this.values)
			{
				result = new List<CounterValue>(this.values);
			}
			return result;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001BF20 File Offset: 0x0001A120
		public IEnumerable<CounterValue> GetValuesAfter(DateTime time)
		{
			List<CounterValue> list = new List<CounterValue>();
			lock (this.values)
			{
				for (int i = this.values.Count - 1; i >= 0; i--)
				{
					CounterValue item = this.values[i];
					if (!(item.TimeStamp > time))
					{
						break;
					}
					list.Add(item);
				}
			}
			list.Reverse();
			return list;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001BFA4 File Offset: 0x0001A1A4
		public IEnumerable<CounterValue> GetValuesBetween(DateTime startTime, DateTime endTime)
		{
			List<CounterValue> list = new List<CounterValue>();
			lock (this.values)
			{
				if (this.values.Count == 0 || startTime > this.values[this.values.Count - 1].TimeStamp)
				{
					return list;
				}
				for (int i = 0; i < this.values.Count; i++)
				{
					CounterValue item = this.values[i];
					if (item.TimeStamp > endTime)
					{
						break;
					}
					if (item.TimeStamp >= startTime)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001C070 File Offset: 0x0001A270
		public CounterValue GetValueAt(DateTime time)
		{
			lock (this.values)
			{
				if (this.values.Count == 0 || time < this.values[0].TimeStamp)
				{
					return new CounterValue(0, 0, time, null);
				}
				if (time >= this.values[this.values.Count - 1].TimeStamp)
				{
					return this.values[this.values.Count - 1];
				}
				for (int i = 0; i < this.values.Count; i++)
				{
					if (this.values[i].TimeStamp > time)
					{
						return this.values[i - 1];
					}
				}
			}
			return new CounterValue(0, 0, time, null);
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001C178 File Offset: 0x0001A378
		public CounterValue LastValue
		{
			get
			{
				CounterValue result;
				lock (this.values)
				{
					if (this.values.Count > 0)
					{
						result = this.values[this.values.Count - 1];
					}
					else
					{
						result = new CounterValue(0, 0, DateTime.MinValue, null);
					}
				}
				return result;
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001C1EC File Offset: 0x0001A3EC
		internal int StoreValue(string message, TimeCounter timer, IDictionary<string, string> metadata)
		{
			DateTime now = DateTime.Now;
			if (this.resolution.Ticks != 0L && now - this.lastValueTime < this.resolution)
			{
				return -1;
			}
			CounterValue counterValue = new CounterValue(this.count, this.totalCount, this.count - this.lastStoredCount, now, message, (timer != null) ? timer.TraceList : null, metadata);
			this.lastStoredCount = this.count;
			if (this.storeValues)
			{
				this.values.Add(counterValue);
			}
			if (this.Handlers.Count > 0)
			{
				if (timer != null)
				{
					using (List<InstrumentationConsumer>.Enumerator enumerator = this.handlers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							InstrumentationConsumer instrumentationConsumer = enumerator.Current;
							IDisposable disposable = instrumentationConsumer.BeginTimer((TimerCounter)this, counterValue);
							if (disposable != null)
							{
								timer.AddHandlerTracker(disposable);
							}
						}
						goto IL_115;
					}
				}
				foreach (InstrumentationConsumer instrumentationConsumer2 in this.handlers)
				{
					instrumentationConsumer2.ConsumeValue(this, counterValue);
				}
			}
			IL_115:
			return this.values.Count - 1;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001C338 File Offset: 0x0001A538
		internal void RemoveValue(int index)
		{
			lock (this.values)
			{
				this.values.RemoveAt(index);
				for (int i = index; i < this.values.Count; i++)
				{
					this.values[i].UpdateValueIndex(i);
				}
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001C3AC File Offset: 0x0001A5AC
		public void Inc()
		{
			this.Inc(1, null);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001C3B6 File Offset: 0x0001A5B6
		public void Inc(string message)
		{
			this.Inc(1, message);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001C3C0 File Offset: 0x0001A5C0
		public void Inc(int n)
		{
			this.Inc(n, null);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001C3CA File Offset: 0x0001A5CA
		public void Inc(int n, string message)
		{
			this.Inc(n, message, null);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001C3D5 File Offset: 0x0001A5D5
		public void Inc(string message, IDictionary<string, string> metadata)
		{
			this.Inc(1, message, metadata);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
		public void Inc(IDictionary<string, string> metadata)
		{
			this.Inc(1, null, metadata);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001C3EC File Offset: 0x0001A5EC
		public void Inc(int n, string message, IDictionary<string, string> metadata)
		{
			if (this.enabled)
			{
				lock (this.values)
				{
					this.count += n;
					this.totalCount += n;
					this.StoreValue(message, null, metadata);
				}
			}
			if (this.logMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001C468 File Offset: 0x0001A668
		public void Dec()
		{
			this.Dec(1);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001C471 File Offset: 0x0001A671
		public void Dec(string message)
		{
			this.Dec(1, message);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001C47B File Offset: 0x0001A67B
		public void Dec(int n)
		{
			this.Dec(n, null);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001C485 File Offset: 0x0001A685
		public void Dec(int n, string message)
		{
			this.Dec(n, message, null);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001C490 File Offset: 0x0001A690
		public void Dec(int n, string message, IDictionary<string, string> metadata)
		{
			if (this.enabled)
			{
				lock (this.values)
				{
					this.count -= n;
					this.StoreValue(message, null, metadata);
				}
			}
			if (this.logMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		public void SetValue(int value)
		{
			this.SetValue(value, null);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001C506 File Offset: 0x0001A706
		public void SetValue(int value, string message)
		{
			this.SetValue(value, message, null);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001C514 File Offset: 0x0001A714
		public void SetValue(int value, string message, IDictionary<string, string> metadata)
		{
			if (this.enabled)
			{
				lock (this.values)
				{
					this.count = value;
					this.StoreValue(message, null, metadata);
				}
			}
			if (this.logMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001C578 File Offset: 0x0001A778
		public static Counter operator ++(Counter c)
		{
			c.Inc(1, null);
			return c;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001C583 File Offset: 0x0001A783
		public static Counter operator --(Counter c)
		{
			c.Dec(1, null);
			return c;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001C58E File Offset: 0x0001A78E
		public MemoryProbe CreateMemoryProbe()
		{
			return new MemoryProbe(this);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001C598 File Offset: 0x0001A798
		public virtual void Trace(string message)
		{
			if (this.enabled)
			{
				lock (this.values)
				{
					this.StoreValue(message, null, null);
				}
			}
			if (this.logMessages && message != null)
			{
				InstrumentationService.LogMessage(message);
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001C5F8 File Offset: 0x0001A7F8
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x04000250 RID: 592
		internal int count;

		// Token: 0x04000251 RID: 593
		internal int totalCount;

		// Token: 0x04000252 RID: 594
		private int lastStoredCount;

		// Token: 0x04000253 RID: 595
		private string name;

		// Token: 0x04000254 RID: 596
		private bool logMessages;

		// Token: 0x04000255 RID: 597
		private CounterCategory category;

		// Token: 0x04000256 RID: 598
		protected List<CounterValue> values = new List<CounterValue>();

		// Token: 0x04000257 RID: 599
		private TimeSpan resolution = TimeSpan.FromMilliseconds(0.0);

		// Token: 0x04000258 RID: 600
		private DateTime lastValueTime = DateTime.MinValue;

		// Token: 0x04000259 RID: 601
		private CounterDisplayMode displayMode;

		// Token: 0x0400025A RID: 602
		private bool disposed;

		// Token: 0x0400025B RID: 603
		private bool storeValues;

		// Token: 0x0400025C RID: 604
		private bool enabled;

		// Token: 0x0400025D RID: 605
		private string id;

		// Token: 0x0400025E RID: 606
		private List<InstrumentationConsumer> handlers = new List<InstrumentationConsumer>();
	}
}
