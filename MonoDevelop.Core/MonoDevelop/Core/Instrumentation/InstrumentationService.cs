using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Mono.Addins;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000CB RID: 203
	public static class InstrumentationService
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001B000 File Offset: 0x00019200
		static InstrumentationService()
		{
			InstrumentationService.counters = new Dictionary<string, Counter>();
			InstrumentationService.categories = new List<CounterCategory>();
			InstrumentationService.startTime = DateTime.Now;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001B038 File Offset: 0x00019238
		internal static void InitializeHandlers()
		{
			if (!InstrumentationService.handlersLoaded && AddinManager.IsInitialized)
			{
				lock (InstrumentationService.counters)
				{
					InstrumentationService.handlersLoaded = true;
					AddinManager.AddExtensionNodeHandler(typeof(InstrumentationConsumer), new ExtensionNodeEventHandler(InstrumentationService.HandleInstrumentationHandlerExtension));
				}
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001B0A0 File Offset: 0x000192A0
		private static void UpdateCounterStatus()
		{
			lock (InstrumentationService.counters)
			{
				foreach (Counter counter in InstrumentationService.counters.Values)
				{
					counter.UpdateStatus();
				}
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001B120 File Offset: 0x00019320
		private static void HandleInstrumentationHandlerExtension(object sender, ExtensionNodeEventArgs args)
		{
			InstrumentationConsumer consumer = (InstrumentationConsumer)args.ExtensionObject;
			if (args.Change == ExtensionChange.Add)
			{
				InstrumentationService.RegisterInstrumentationConsumer(consumer);
				return;
			}
			InstrumentationService.UnregisterInstrumentationConsumer(consumer);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001B150 File Offset: 0x00019350
		public static void RegisterInstrumentationConsumer(InstrumentationConsumer consumer)
		{
			lock (InstrumentationService.counters)
			{
				InstrumentationService.handlers.Add(consumer);
				foreach (Counter counter in InstrumentationService.counters.Values)
				{
					if (consumer.SupportsCounter(counter))
					{
						counter.Handlers.Add(consumer);
					}
				}
			}
			InstrumentationService.UpdateCounterStatus();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001B1F0 File Offset: 0x000193F0
		public static void UnregisterInstrumentationConsumer(InstrumentationConsumer consumer)
		{
			lock (InstrumentationService.counters)
			{
				InstrumentationService.handlers.Remove(consumer);
				foreach (Counter counter in InstrumentationService.counters.Values)
				{
					counter.Handlers.Remove(consumer);
				}
			}
			InstrumentationService.UpdateCounterStatus();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001B288 File Offset: 0x00019488
		public static int PublishService()
		{
			RemotingService.RegisterRemotingChannel();
			TcpChannel tcpChannel = (TcpChannel)ChannelServices.GetChannel("tcp");
			Uri uri = new Uri(tcpChannel.GetUrlsForUri("test")[0]);
			InstrumentationService.publicPort = uri.Port;
			InstrumentationServiceBackend obj = new InstrumentationServiceBackend();
			RemotingServices.Marshal(obj, "InstrumentationService");
			return InstrumentationService.publicPort;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001B2E0 File Offset: 0x000194E0
		public static void StartMonitor()
		{
			if (InstrumentationService.publicPort == -1)
			{
				throw new InvalidOperationException("Service not published");
			}
			if (Platform.IsMac)
			{
				FilePath filePath = PropertyService.EntryAssemblyPath.ParentDirectory.ParentDirectory.ParentDirectory.ParentDirectory.Combine(new string[]
				{
					"MacOS"
				}).Combine(new string[]
				{
					"MDMonitor.app"
				});
				if (Directory.Exists(filePath))
				{
					ProcessStartInfo startInfo = new ProcessStartInfo("open", string.Format("-n '{0}' --args -c localhost:{1} ", filePath, InstrumentationService.publicPort))
					{
						UseShellExecute = false
					};
					Process.Start(startInfo);
					return;
				}
			}
			string file = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mdmonitor.exe");
			string arguments = "-c localhost:" + InstrumentationService.publicPort;
			Runtime.SystemAssemblyService.CurrentRuntime.ExecuteAssembly(file, arguments);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001B410 File Offset: 0x00019610
		public static void StartAutoSave(string file, int interval)
		{
			InstrumentationService.autoSaveInterval = interval;
			InstrumentationService.autoSaveThread = new Thread(delegate()
			{
				InstrumentationService.AutoSave(file, interval);
			});
			InstrumentationService.autoSaveThread.IsBackground = true;
			InstrumentationService.autoSaveThread.Start();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001B467 File Offset: 0x00019667
		public static void Stop()
		{
			InstrumentationService.stopping = true;
			if (InstrumentationService.autoSaveThread != null)
			{
				InstrumentationService.autoSaveThread.Join(InstrumentationService.autoSaveInterval * 3);
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001B488 File Offset: 0x00019688
		private static void AutoSave(string file, int interval)
		{
			while (!InstrumentationService.stopping)
			{
				Thread.Sleep(interval);
				try
				{
					lock (InstrumentationService.counters)
					{
						InstrumentationServiceData instrumentationServiceData = new InstrumentationServiceData();
						instrumentationServiceData.EndTime = DateTime.Now;
						instrumentationServiceData.StartTime = InstrumentationService.StartTime;
						instrumentationServiceData.Counters = InstrumentationService.counters;
						instrumentationServiceData.Categories = InstrumentationService.categories;
						FilePath filePath = file + ".tmp";
						using (Stream stream = File.OpenWrite(filePath))
						{
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							binaryFormatter.Serialize(stream, instrumentationServiceData);
						}
						FileService.SystemRename(filePath, file);
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Instrumentation service data could not be saved", ex);
				}
			}
			InstrumentationService.autoSaveThread = null;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001B580 File Offset: 0x00019780
		public static IInstrumentationService GetRemoteService(string hostAndPort)
		{
			return (IInstrumentationService)Activator.GetObject(typeof(IInstrumentationService), "tcp://" + hostAndPort + "/InstrumentationService");
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001B5A8 File Offset: 0x000197A8
		public static IInstrumentationService LoadServiceDataFromFile(string file)
		{
			IInstrumentationService result;
			using (Stream stream = File.OpenRead(file))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				IInstrumentationService instrumentationService = binaryFormatter.Deserialize(stream) as IInstrumentationService;
				if (instrumentationService == null)
				{
					throw new Exception("Invalid instrumentation service data file");
				}
				result = instrumentationService;
			}
			return result;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001B5FC File Offset: 0x000197FC
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001B603 File Offset: 0x00019803
		public static bool Enabled
		{
			get
			{
				return InstrumentationService.enabled;
			}
			set
			{
				if (InstrumentationService.enabled == value)
				{
					return;
				}
				InstrumentationService.enabled = value;
				InstrumentationService.UpdateCounterStatus();
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001B619 File Offset: 0x00019819
		public static DateTime StartTime
		{
			get
			{
				return InstrumentationService.startTime;
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001B620 File Offset: 0x00019820
		public static Counter CreateCounter(string name)
		{
			return InstrumentationService.CreateCounter(name, null);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001B629 File Offset: 0x00019829
		public static Counter CreateCounter(string name, string category)
		{
			return InstrumentationService.CreateCounter(name, category, false);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001B633 File Offset: 0x00019833
		public static Counter CreateCounter(string name, string category, bool logMessages)
		{
			return InstrumentationService.CreateCounter(name, category, logMessages, null, false);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001B63F File Offset: 0x0001983F
		public static Counter CreateCounter(string name, string category = null, bool logMessages = false, string id = null)
		{
			return InstrumentationService.CreateCounter(name, category, logMessages, id, false);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001B64C File Offset: 0x0001984C
		private static Counter CreateCounter(string name, string category, bool logMessages, string id, bool isTimer)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Counters must have a Name");
			}
			InstrumentationService.InitializeHandlers();
			if (category == null)
			{
				category = "Global";
			}
			Counter result;
			lock (InstrumentationService.counters)
			{
				CounterCategory counterCategory = InstrumentationService.GetCategory(category);
				if (counterCategory == null)
				{
					counterCategory = new CounterCategory(category);
					InstrumentationService.categories.Add(counterCategory);
				}
				Counter counter = isTimer ? new TimerCounter(name, counterCategory) : new Counter(name, counterCategory);
				counter.Id = id;
				counter.LogMessages = logMessages;
				counterCategory.AddCounter(counter);
				Counter counter2;
				if (InstrumentationService.counters.TryGetValue(name, out counter2))
				{
					counter2.Disposed = true;
				}
				InstrumentationService.counters[name] = counter;
				foreach (InstrumentationConsumer instrumentationConsumer in InstrumentationService.handlers)
				{
					if (instrumentationConsumer.SupportsCounter(counter))
					{
						counter.Handlers.Add(instrumentationConsumer);
					}
				}
				counter.UpdateStatus();
				result = counter;
			}
			return result;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001B770 File Offset: 0x00019970
		public static MemoryProbe CreateMemoryProbe(string name)
		{
			return InstrumentationService.CreateMemoryProbe(name, null);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001B77C File Offset: 0x0001997C
		public static MemoryProbe CreateMemoryProbe(string name, string category)
		{
			if (!InstrumentationService.enabled)
			{
				return null;
			}
			Counter c;
			lock (InstrumentationService.counters)
			{
				if (!InstrumentationService.counters.TryGetValue(name, out c))
				{
					c = InstrumentationService.CreateCounter(name, category);
				}
			}
			return new MemoryProbe(c);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001B7DC File Offset: 0x000199DC
		public static TimerCounter CreateTimerCounter(string name)
		{
			return InstrumentationService.CreateTimerCounter(name, null);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001B7E5 File Offset: 0x000199E5
		public static TimerCounter CreateTimerCounter(string name, string category)
		{
			return InstrumentationService.CreateTimerCounter(name, category, 0.0, false);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001B7F8 File Offset: 0x000199F8
		public static TimerCounter CreateTimerCounter(string name, string category, double minSeconds, bool logMessages)
		{
			return InstrumentationService.CreateTimerCounter(name, category, minSeconds, logMessages, null);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001B804 File Offset: 0x00019A04
		public static TimerCounter CreateTimerCounter(string name, string category = null, double minSeconds = 0.0, bool logMessages = false, string id = null)
		{
			TimerCounter timerCounter = (TimerCounter)InstrumentationService.CreateCounter(name, category, logMessages, id, true);
			timerCounter.DisplayMode = CounterDisplayMode.Line;
			timerCounter.LogMessages = logMessages;
			timerCounter.MinSeconds = minSeconds;
			return timerCounter;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001B838 File Offset: 0x00019A38
		public static IEnumerable<Counter> GetCounters()
		{
			IEnumerable<Counter> result;
			lock (InstrumentationService.counters)
			{
				result = new List<Counter>(InstrumentationService.counters.Values);
			}
			return result;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001B884 File Offset: 0x00019A84
		public static Counter GetCounter(string name)
		{
			Counter result;
			lock (InstrumentationService.counters)
			{
				Counter counter;
				if (InstrumentationService.counters.TryGetValue(name, out counter))
				{
					result = counter;
				}
				else
				{
					counter = new Counter(name, null);
					InstrumentationService.counters[name] = counter;
					result = counter;
				}
			}
			return result;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		public static CounterCategory GetCategory(string name)
		{
			CounterCategory result;
			lock (InstrumentationService.counters)
			{
				foreach (CounterCategory counterCategory in InstrumentationService.categories)
				{
					if (counterCategory.Name == name)
					{
						return counterCategory;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001B974 File Offset: 0x00019B74
		public static IEnumerable<CounterCategory> GetCategories()
		{
			IEnumerable<CounterCategory> result;
			lock (InstrumentationService.counters)
			{
				result = new List<CounterCategory>(InstrumentationService.categories);
			}
			return result;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001B9BC File Offset: 0x00019BBC
		internal static void LogMessage(string message)
		{
			InstrumentationService.IsLoggingMessage = true;
			try
			{
				LoggingService.LogInfo(message);
			}
			finally
			{
				InstrumentationService.IsLoggingMessage = false;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001B9F0 File Offset: 0x00019BF0
		public static void Dump()
		{
			foreach (CounterCategory counterCategory in InstrumentationService.categories)
			{
				Console.WriteLine(counterCategory.Name);
				Console.WriteLine(new string('-', counterCategory.Name.Length));
				Console.WriteLine();
				foreach (Counter counter in counterCategory.Counters)
				{
					Console.WriteLine("{0,-6} {1,-6} : {2}", counter.Count, counter.TotalCount, counter.Name);
				}
				Console.WriteLine();
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001BAC8 File Offset: 0x00019CC8
		public static IProgressMonitor GetInstrumentedMonitor(IProgressMonitor monitor, TimerCounter counter)
		{
			if (InstrumentationService.enabled)
			{
				AggregatedProgressMonitor aggregatedProgressMonitor = new AggregatedProgressMonitor(monitor, new IProgressMonitor[0]);
				aggregatedProgressMonitor.AddSlaveMonitor(new IntrumentationMonitor(counter), MonitorAction.WriteLog | MonitorAction.Tasks);
				return aggregatedProgressMonitor;
			}
			return monitor;
		}

		// Token: 0x0400023E RID: 574
		private static Dictionary<string, Counter> counters;

		// Token: 0x0400023F RID: 575
		private static List<CounterCategory> categories;

		// Token: 0x04000240 RID: 576
		private static bool enabled = true;

		// Token: 0x04000241 RID: 577
		private static DateTime startTime;

		// Token: 0x04000242 RID: 578
		private static int publicPort = -1;

		// Token: 0x04000243 RID: 579
		private static Thread autoSaveThread;

		// Token: 0x04000244 RID: 580
		private static bool stopping;

		// Token: 0x04000245 RID: 581
		private static int autoSaveInterval;

		// Token: 0x04000246 RID: 582
		private static List<InstrumentationConsumer> handlers = new List<InstrumentationConsumer>();

		// Token: 0x04000247 RID: 583
		private static bool handlersLoaded;

		// Token: 0x04000248 RID: 584
		[ThreadStatic]
		internal static bool IsLoggingMessage;
	}
}
