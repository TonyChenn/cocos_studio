using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000C9 RID: 201
	public static class RemotingService
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x0001AD90 File Offset: 0x00018F90
		public static void RegisterRemotingChannel()
		{
			if (!RemotingService.channelRegistered)
			{
				RemotingService.channelRegistered = true;
				IDictionary dictionary = new Hashtable();
				dictionary["includeVersions"] = false;
				dictionary["strictBinding"] = false;
				IChannel channel = ChannelServices.GetChannel("ipc");
				if (channel != null)
				{
					LoggingService.LogFatalError("IPC channel already registered. An add-in may have registered it");
					throw new InvalidOperationException("IPC channel already registered. An add-in may have registered it.");
				}
				BinaryServerFormatterSinkProvider binaryServerFormatterSinkProvider = new BinaryServerFormatterSinkProvider(dictionary, null);
				binaryServerFormatterSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
				DisposerFormatterSinkProvider disposerFormatterSinkProvider = new DisposerFormatterSinkProvider();
				disposerFormatterSinkProvider.Next = new BinaryClientFormatterSinkProvider(dictionary, null);
				RemotingService.unixRemotingFile = Path.GetTempFileName();
				IDictionary dictionary2 = new Hashtable();
				dictionary2["portName"] = Path.GetFileName(RemotingService.unixRemotingFile);
				ChannelServices.RegisterChannel(new IpcChannel(dictionary2, disposerFormatterSinkProvider, binaryServerFormatterSinkProvider), false);
				channel = ChannelServices.GetChannel("tcp");
				if (channel != null)
				{
					LoggingService.LogFatalError("TCP channel already registered. An add-in may have registered it");
					throw new InvalidOperationException("TCP channel already registered. An add-in may have registered it.");
				}
				binaryServerFormatterSinkProvider = new BinaryServerFormatterSinkProvider(dictionary, null);
				binaryServerFormatterSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
				disposerFormatterSinkProvider = new DisposerFormatterSinkProvider();
				disposerFormatterSinkProvider.Next = new BinaryClientFormatterSinkProvider(dictionary, null);
				dictionary2 = new Hashtable();
				dictionary2["port"] = 0;
				dictionary2["rejectRemoteRequests"] = true;
				ChannelServices.RegisterChannel(new TcpChannel(dictionary2, disposerFormatterSinkProvider, binaryServerFormatterSinkProvider), false);
				if (Platform.IsWindows)
				{
					AppDomain.CurrentDomain.AssemblyResolve += delegate(object s, ResolveEventArgs args)
					{
						if (!RemotingService.simpleResolveAssemblies.Contains(args.Name))
						{
							return null;
						}
						foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
						{
							if (assembly.GetName().FullName == args.Name || args.Name == assembly.GetName().Name)
							{
								Console.WriteLine(Environment.StackTrace);
								return assembly;
							}
						}
						return null;
					};
				}
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001AEFD File Offset: 0x000190FD
		public static void RegisterMethodCallback(object proxy, string method, CallingMethodCallback calling, CalledMethodCallback called)
		{
			RemotingService.RegisterMethodCallback(proxy, method, calling, called, -1);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001AF0C File Offset: 0x0001910C
		public static void RegisterMethodCallback(object proxy, string method, CallingMethodCallback calling, CalledMethodCallback called, int timeout)
		{
			string objectUri = RemotingServices.GetObjectUri((MarshalByRefObject)proxy);
			RemotingService.CallbackData callbackData = new RemotingService.CallbackData();
			callbackData.Target = proxy;
			callbackData.Calling = calling;
			callbackData.Called = called;
			callbackData.Timeout = timeout;
			callbackData.Method = method;
			RemotingService.callbacks[objectUri + " " + method] = callbackData;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001AF68 File Offset: 0x00019168
		public static void UnregisterMethodCallback(object proxy, string method)
		{
			string objectUri = RemotingServices.GetObjectUri((MarshalByRefObject)proxy);
			RemotingService.callbacks.Remove(objectUri + " " + method);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001AF98 File Offset: 0x00019198
		internal static void RegisterAssemblyForSimpleResolve(string name)
		{
			RemotingService.simpleResolveAssemblies.Add(name);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001AFA8 File Offset: 0x000191A8
		internal static RemotingService.CallbackData GetCallbackData(string uri, string method)
		{
			RemotingService.CallbackData result;
			RemotingService.callbacks.TryGetValue(uri + " " + method, out result);
			return result;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001AFCF File Offset: 0x000191CF
		internal static void Dispose()
		{
			if (RemotingService.unixRemotingFile != null)
			{
				File.Delete(RemotingService.unixRemotingFile);
			}
		}

		// Token: 0x04000234 RID: 564
		private static string unixRemotingFile;

		// Token: 0x04000235 RID: 565
		private static Dictionary<string, RemotingService.CallbackData> callbacks = new Dictionary<string, RemotingService.CallbackData>();

		// Token: 0x04000236 RID: 566
		private static bool channelRegistered;

		// Token: 0x04000237 RID: 567
		private static HashSet<string> simpleResolveAssemblies = new HashSet<string>();

		// Token: 0x020000CA RID: 202
		internal class CallbackData
		{
			// Token: 0x04000239 RID: 569
			public object Target;

			// Token: 0x0400023A RID: 570
			public int Timeout;

			// Token: 0x0400023B RID: 571
			public string Method;

			// Token: 0x0400023C RID: 572
			public CallingMethodCallback Calling;

			// Token: 0x0400023D RID: 573
			public CalledMethodCallback Called;
		}
	}
}
