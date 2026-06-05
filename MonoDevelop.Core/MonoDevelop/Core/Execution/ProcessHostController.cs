using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using Mono.Addins;
using Mono.Addins.Setup;
using MonoDevelop.Core.Logging;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000D RID: 13
	[DesignerCategory("Code")]
	internal class ProcessHostController : MarshalByRefObject, IProcessHostController
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00003431 File Offset: 0x00001631
		static ProcessHostController()
		{
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object s, ResolveEventArgs args)
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (assembly.GetName().FullName == args.Name)
					{
						return assembly;
					}
				}
				return null;
			};
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000345C File Offset: 0x0000165C
		public ProcessHostController(string id, uint stopDelay, IExecutionHandler executionHandlerFactory)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = "?";
			}
			this.id = id;
			this.stopDelay = stopDelay;
			this.executionHandlerFactory = executionHandlerFactory;
			this.timer = new System.Timers.Timer();
			this.timer.AutoReset = false;
			this.timer.Elapsed += this.WaitTimeout;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000034FC File Offset: 0x000016FC
		public void Start(IList<string> userAssemblyPaths = null)
		{
			lock (this)
			{
				if (!this.starting)
				{
					this.starting = true;
					this.exitRequestEvent.Reset();
					RemotingService.RegisterRemotingChannel();
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					ObjRef graph = RemotingServices.Marshal(this);
					MemoryStream memoryStream = new MemoryStream();
					binaryFormatter.Serialize(memoryStream, graph);
					string value = Convert.ToBase64String(memoryStream.ToArray());
					string text = null;
					if (this.executionHandlerFactory == null)
					{
						this.executionHandlerFactory = Runtime.SystemAssemblyService.CurrentRuntime.GetExecutionHandler();
					}
					try
					{
						string text2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
						text2 = Path.Combine(text2, "mdhost.exe");
						text = Path.GetTempFileName();
						StreamWriter streamWriter = new StreamWriter(text);
						streamWriter.WriteLine(value);
						streamWriter.WriteLine(Process.GetCurrentProcess().Id);
						streamWriter.WriteLine(Runtime.SystemAssemblyService.CurrentRuntime.RuntimeId);
						streamWriter.WriteLine(2);
						streamWriter.WriteLine(typeof(AddinManager).Assembly.Location);
						streamWriter.WriteLine(typeof(SetupService).Assembly.Location);
						streamWriter.Close();
						string arguments = string.Format("{0} \"{1}\"", this.id, text);
						DotNetExecutionCommand dotNetExecutionCommand = new DotNetExecutionCommand(text2, arguments, AppDomain.CurrentDomain.BaseDirectory);
						if (userAssemblyPaths != null)
						{
							dotNetExecutionCommand.UserAssemblyPaths = userAssemblyPaths;
						}
						dotNetExecutionCommand.DebugMode = this.isDebugMode;
						ProcessHostConsole console = new ProcessHostConsole();
						this.process = this.executionHandlerFactory.Execute(dotNetExecutionCommand, console);
						Counters.ExternalHostProcesses = ++Counters.ExternalHostProcesses;
						this.process.Completed += this.ProcessExited;
					}
					catch (Exception ex)
					{
						if (text != null)
						{
							try
							{
								File.Delete(text);
							}
							catch
							{
							}
						}
						LoggingService.LogError(ex.ToString());
						throw;
					}
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003728 File Offset: 0x00001928
		private bool isDebugMode
		{
			get
			{
				return Environment.StackTrace.IndexOf("ProcessHostController.cs:") > -1;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000373C File Offset: 0x0000193C
		private void ProcessExited(IAsyncOperation oper)
		{
			lock (this)
			{
				Counters.ExternalHostProcesses = --Counters.ExternalHostProcesses;
				foreach (object proxy in this.remoteObjects)
				{
					RemotingService.UnregisterMethodCallback(proxy, "Dispose");
				}
				this.remoteObjects.Clear();
				this.exitedEvent.Set();
				this.exitRequestEvent.Set();
				if (oper == this.process)
				{
					this.runningEvent.Reset();
					this.processHost = null;
					this.process = null;
					this.references = 0;
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003818 File Offset: 0x00001A18
		public object CreateInstance(Type type, string[] addins, IList<string> userAssemblyPaths = null)
		{
			lock (this)
			{
				this.references++;
				if (this.processHost == null)
				{
					this.Start(userAssemblyPaths);
				}
			}
			if (!this.runningEvent.WaitOne(15000, false))
			{
				this.references--;
				throw new ApplicationException("Couldn't create a remote process.");
			}
			object result;
			try
			{
				if (addins != null && addins.Length > 0)
				{
					this.processHost.LoadAddins(addins);
				}
				RemotingService.RegisterAssemblyForSimpleResolve(type.Assembly.GetName().Name);
				object obj = this.processHost.CreateInstance(type);
				RemotingService.RegisterMethodCallback(obj, "Dispose", new CallingMethodCallback(this.RemoteProcessObjectDisposing), null);
				RemotingService.RegisterMethodCallback(obj, "Shutdown", new CallingMethodCallback(this.RemoteProcessObjectShuttingDown), null);
				this.remoteObjects.Add(obj);
				Counters.ExternalObjects = ++Counters.ExternalObjects;
				result = obj;
			}
			catch
			{
				this.ReleaseInstance(null);
				throw;
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003934 File Offset: 0x00001B34
		public object CreateInstance(string assemblyPath, string typeName, string[] addins, IList<string> userAssemblyPaths = null)
		{
			lock (this)
			{
				this.references++;
				if (this.processHost == null)
				{
					this.Start(userAssemblyPaths);
				}
			}
			if (!this.runningEvent.WaitOne(15000, false))
			{
				this.references--;
				throw new ApplicationException("Couldn't create a remote process.");
			}
			object result;
			try
			{
				if (addins != null && addins.Length > 0)
				{
					this.processHost.LoadAddins(addins);
				}
				RemotingService.RegisterAssemblyForSimpleResolve(Path.GetFileNameWithoutExtension(assemblyPath));
				object obj = this.processHost.CreateInstance(assemblyPath, typeName);
				RemotingService.RegisterMethodCallback(obj, "Dispose", new CallingMethodCallback(this.RemoteProcessObjectDisposing), null);
				RemotingService.RegisterMethodCallback(obj, "Shutdown", new CallingMethodCallback(this.RemoteProcessObjectShuttingDown), null);
				this.remoteObjects.Add(obj);
				Counters.ExternalObjects = ++Counters.ExternalObjects;
				result = obj;
			}
			catch
			{
				this.ReleaseInstance(null);
				throw;
			}
			return result;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003AA0 File Offset: 0x00001CA0
		private IMethodReturnMessage RemoteProcessObjectDisposing(object obj, IMethodCallMessage msg)
		{
			ThreadPool.QueueUserWorkItem(delegate(object param0)
			{
				try
				{
					this.processHost.DisposeObject((IDisposable)obj);
				}
				catch
				{
				}
				this.ReleaseInstance(obj);
			});
			return new ReturnMessage(null, null, 0, msg.LogicalCallContext, msg);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003B14 File Offset: 0x00001D14
		private IMethodReturnMessage RemoteProcessObjectShuttingDown(object obj, IMethodCallMessage msg)
		{
			ThreadPool.QueueUserWorkItem(delegate(object param0)
			{
				try
				{
					this.process.Cancel();
				}
				catch
				{
				}
			});
			return new ReturnMessage(null, null, 0, msg.LogicalCallContext, msg);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003B37 File Offset: 0x00001D37
		public void ReleaseInstance(object obj)
		{
			this.ReleaseInstance(obj, 2000);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003B48 File Offset: 0x00001D48
		public void ReleaseInstance(object proc, int shutdownTimeout)
		{
			Counters.ExternalObjects = --Counters.ExternalObjects;
			if (this.processHost == null)
			{
				return;
			}
			lock (this)
			{
				for (int i = 0; i < this.remoteObjects.Count; i++)
				{
					if (this.remoteObjects[i] == proc)
					{
						this.remoteObjects.RemoveAt(i);
						break;
					}
				}
				this.references--;
				if (this.references == 0)
				{
					this.lastReleaseTime = DateTime.Now;
					if (!this.stopping)
					{
						this.stopping = true;
						this.shutdownTimeout = shutdownTimeout;
						if (this.stopDelay == 0U)
						{
							this.timer.Interval = 1000.0;
							this.timer.Enabled = true;
						}
						else
						{
							this.timer.Interval = this.stopDelay;
							this.timer.Enabled = true;
						}
					}
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003C48 File Offset: 0x00001E48
		private void WaitTimeout(object sender, ElapsedEventArgs args)
		{
			try
			{
				IProcessAsyncOperation processAsyncOperation;
				lock (this)
				{
					if (this.references > 0)
					{
						this.stopping = false;
						return;
					}
					uint num = (uint)(DateTime.Now - this.lastReleaseTime).TotalMilliseconds;
					if (num < this.stopDelay)
					{
						this.timer.Interval = this.stopDelay - num;
						this.timer.Enabled = true;
						return;
					}
					this.runningEvent.Reset();
					this.exitedEvent.Reset();
					this.exitRequestEvent.Set();
					processAsyncOperation = this.process;
					this.processHost = null;
					this.process = null;
					this.stopping = false;
				}
				if (!this.exitedEvent.WaitOne(this.shutdownTimeout, false))
				{
					try
					{
						processAsyncOperation.Cancel();
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(ex.ToString());
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003D64 File Offset: 0x00001F64
		public void RegisterHost(IProcessHost processHost)
		{
			lock (this)
			{
				this.processHost = processHost;
				this.runningEvent.Set();
				this.starting = false;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public void WaitForExit()
		{
			this.exitRequestEvent.WaitOne();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003DC2 File Offset: 0x00001FC2
		public ILogger GetLogger()
		{
			return LoggingService.RemoteLogger;
		}

		// Token: 0x04000032 RID: 50
		private int references;

		// Token: 0x04000033 RID: 51
		private uint stopDelay;

		// Token: 0x04000034 RID: 52
		private DateTime lastReleaseTime;

		// Token: 0x04000035 RID: 53
		private bool starting;

		// Token: 0x04000036 RID: 54
		private bool stopping;

		// Token: 0x04000037 RID: 55
		private IProcessAsyncOperation process;

		// Token: 0x04000038 RID: 56
		private System.Timers.Timer timer;

		// Token: 0x04000039 RID: 57
		private string id;

		// Token: 0x0400003A RID: 58
		private IExecutionHandler executionHandlerFactory;

		// Token: 0x0400003B RID: 59
		private int shutdownTimeout = 2000;

		// Token: 0x0400003C RID: 60
		private IProcessHost processHost;

		// Token: 0x0400003D RID: 61
		private ManualResetEvent runningEvent = new ManualResetEvent(false);

		// Token: 0x0400003E RID: 62
		private ManualResetEvent exitRequestEvent = new ManualResetEvent(false);

		// Token: 0x0400003F RID: 63
		private ManualResetEvent exitedEvent = new ManualResetEvent(false);

		// Token: 0x04000040 RID: 64
		private List<object> remoteObjects = new List<object>();
	}
}
