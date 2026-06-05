using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Mono.Addins;
using Mono.Addins.Setup;
using Mono.Unix.Native;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Core.Setup;

namespace MonoDevelop.Core
{
	// Token: 0x02000005 RID: 5
	public static class Runtime
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000029C0 File Offset: 0x00000BC0
		public static void GetAddinRegistryLocation(out string configDir, out string addinsDir, out string databaseDir)
		{
			string text = Environment.GetEnvironmentVariable("MONODEVELOP_DEV_CONFIG");
			if (text != null && text.Length == 0)
			{
				text = null;
			}
			string text2 = Environment.GetEnvironmentVariable("MONODEVELOP_DEV_ADDINS");
			if (text2 != null && text2.Length == 0)
			{
				text2 = null;
			}
			configDir = (text ?? UserProfile.Current.ConfigDir);
			string text3;
			if ((text3 = text2) == null)
			{
				text3 = UserProfile.Current.LocalInstallDir.Combine(new string[]
				{
					"Addins"
				});
			}
			addinsDir = text3;
			databaseDir = (text2 ?? UserProfile.Current.CacheDir);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002A8C File Offset: 0x00000C8C
		public static void Initialize(bool updateAddinRegistry)
		{
			if (Runtime.initialized)
			{
				return;
			}
			Counters.RuntimeInitialization.BeginTiming();
			Runtime.SetupInstrumentation();
			Platform.Initialize();
			if (SynchronizationContext.Current == null)
			{
				SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
			}
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				if (sslPolicyErrors == SslPolicyErrors.None)
				{
					return true;
				}
				if (sender is WebRequest)
				{
					sender = ((WebRequest)sender).RequestUri.Host;
				}
				return WebCertificateService.GetIsCertificateTrusted(sender as string, certificate.GetPublicKeyString());
			}));
			AddinManager.AddinLoadError += Runtime.OnLoadError;
			AddinManager.AddinLoaded += Runtime.OnLoad;
			AddinManager.AddinUnloaded += Runtime.OnUnload;
			try
			{
				Counters.RuntimeInitialization.Trace("Initializing Addin Manager");
				string configDir;
				string addinsDir;
				string databaseDir;
				Runtime.GetAddinRegistryLocation(out configDir, out addinsDir, out databaseDir);
				AddinManager.Initialize(configDir, addinsDir, databaseDir);
				AddinManager.InitializeDefaultLocalizer(new DefaultAddinLocalizer());
				if (updateAddinRegistry)
				{
					AddinManager.Registry.Update(null);
				}
				Runtime.setupService = new AddinSetupService(AddinManager.Registry);
				Counters.RuntimeInitialization.Trace("Initialized Addin Manager");
				PropertyService.Initialize();
				WebRequestHelper.Initialize();
				WebRequestHelper.SetRequestHandler(new Func<Func<HttpWebRequest>, Action<HttpWebRequest>, CancellationToken, HttpWebResponse>(WebRequestHelper.GetResponse));
				if (UserDataMigrationService.HasSource)
				{
					Counters.RuntimeInitialization.Trace("Migrating User Data from MD " + UserDataMigrationService.SourceVersion);
					UserDataMigrationService.StartMigration();
				}
				Runtime.RegisterAddinRepositories();
				Counters.RuntimeInitialization.Trace("Initializing Assembly Service");
				Runtime.systemAssemblyService = new SystemAssemblyService();
				Runtime.systemAssemblyService.Initialize();
				Runtime.initialized = true;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				AddinManager.AddinLoadError -= Runtime.OnLoadError;
				AddinManager.AddinLoaded -= Runtime.OnLoad;
				AddinManager.AddinUnloaded -= Runtime.OnUnload;
			}
			finally
			{
				Counters.RuntimeInitialization.EndTiming();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C80 File Offset: 0x00000E80
		private static void RegisterAddinRepositories()
		{
			List<string> list = (from UpdateLevel v in Enum.GetValues(typeof(UpdateLevel))
			select Runtime.setupService.GetMainRepositoryUrl(v)).ToList<string>();
			RepositoryRegistry repositories = Runtime.setupService.Repositories;
			foreach (AddinRepository addinRepository in repositories.GetRepositories())
			{
				if (addinRepository.Url.StartsWith("http://go-mono.com/md/") || addinRepository.Url.StartsWith("http://monodevelop.com/files/addins/") || (addinRepository.Url.StartsWith("http://addins.monodevelop.com/") && !list.Contains(addinRepository.Url)))
				{
					repositories.RemoveRepository(addinRepository.Url);
				}
			}
			if (!Runtime.setupService.IsMainRepositoryRegistered(UpdateLevel.Stable))
			{
				Runtime.setupService.RegisterMainRepository(UpdateLevel.Stable, true);
				Runtime.setupService.RegisterMainRepository(UpdateLevel.Beta, true);
			}
			if (!Runtime.setupService.IsMainRepositoryRegistered(UpdateLevel.Beta))
			{
				Runtime.setupService.RegisterMainRepository(UpdateLevel.Beta, false);
			}
			if (!Runtime.setupService.IsMainRepositoryRegistered(UpdateLevel.Alpha))
			{
				Runtime.setupService.RegisterMainRepository(UpdateLevel.Alpha, false);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D9C File Offset: 0x00000F9C
		internal static string GetRepoUrl(string quality)
		{
			string text;
			if (Platform.IsWindows)
			{
				text = "Win32";
			}
			else if (Platform.IsMac)
			{
				text = "Mac";
			}
			else
			{
				text = "Linux";
			}
			return string.Concat(new string[]
			{
				"http://addins.monodevelop.com/",
				quality,
				"/",
				text,
				"/",
				AddinManager.CurrentAddin.Version,
				"/main.mrep"
			});
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002E24 File Offset: 0x00001024
		private static void SetupInstrumentation()
		{
			InstrumentationService.Enabled = PropertyService.Get<bool>("MonoDevelop.EnableInstrumentation", false);
			if (InstrumentationService.Enabled)
			{
				LoggingService.LogInfo("Instrumentation Service started");
				try
				{
					int num = InstrumentationService.PublishService();
					LoggingService.LogInfo("Instrumentation available at port " + num);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Instrumentation service could not be published", ex);
				}
			}
			PropertyService.AddPropertyHandler("MonoDevelop.EnableInstrumentation", delegate(object param0, PropertyChangedEventArgs param1)
			{
				InstrumentationService.Enabled = PropertyService.Get<bool>("MonoDevelop.EnableInstrumentation", false);
			});
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002EB4 File Offset: 0x000010B4
		private static void OnLoadError(object s, AddinErrorEventArgs args)
		{
			string message = "Add-in error (" + args.AddinId + "): " + args.Message;
			LoggingService.LogError(message, args.Exception);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002EEC File Offset: 0x000010EC
		private static void OnLoad(object s, AddinEventArgs args)
		{
			Counters.AddinsLoaded.Inc("Add-in loaded: " + args.AddinId, new Dictionary<string, string>
			{
				{
					"AddinId",
					args.AddinId
				}
			});
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F2B File Offset: 0x0000112B
		private static void OnUnload(object s, AddinEventArgs args)
		{
			Counters.AddinsLoaded.Dec("Add-in unloaded: " + args.AddinId);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002F47 File Offset: 0x00001147
		internal static bool Initialized
		{
			get
			{
				return Runtime.initialized;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F50 File Offset: 0x00001150
		public static void Shutdown()
		{
			if (!Runtime.initialized)
			{
				return;
			}
			if (Runtime.ShuttingDown != null)
			{
				Runtime.ShuttingDown(null, EventArgs.Empty);
			}
			PropertyService.SaveProperties();
			if (Runtime.processService != null)
			{
				Runtime.processService.Dispose();
				Runtime.processService = null;
			}
			Runtime.initialized = false;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002F9E File Offset: 0x0000119E
		public static ProcessService ProcessService
		{
			get
			{
				if (Runtime.processService == null)
				{
					Runtime.processService = new ProcessService();
				}
				return Runtime.processService;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002FB6 File Offset: 0x000011B6
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002FBD File Offset: 0x000011BD
		public static SystemAssemblyService SystemAssemblyService
		{
			get
			{
				return Runtime.systemAssemblyService;
			}
			set
			{
				Runtime.systemAssemblyService = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002FC5 File Offset: 0x000011C5
		public static AddinSetupService AddinSetupService
		{
			get
			{
				return Runtime.setupService;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002FCC File Offset: 0x000011CC
		public static ApplicationService ApplicationService
		{
			get
			{
				if (Runtime.applicationService == null)
				{
					Runtime.applicationService = new ApplicationService();
				}
				return Runtime.applicationService;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002FE4 File Offset: 0x000011E4
		public static Version Version
		{
			get
			{
				if (Runtime.version == null)
				{
					Runtime.version = new Version("5.4.0");
					string releaseId = SystemInformation.GetReleaseId();
					if (releaseId != null && releaseId.Length >= 9)
					{
						int val;
						int.TryParse(releaseId.Substring(releaseId.Length - 4), out val);
						Runtime.version = new Version(Math.Max(Runtime.version.Major, 0), Math.Max(Runtime.version.Minor, 0), Math.Max(Runtime.version.Build, 0), Math.Max(val, 0));
					}
				}
				return Runtime.version;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000307C File Offset: 0x0000127C
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000308C File Offset: 0x0000128C
		public static SynchronizationContext MainSynchronizationContext
		{
			get
			{
				return Runtime.mainSynchronizationContext ?? SynchronizationContext.Current;
			}
			set
			{
				if (Runtime.mainSynchronizationContext != null && value != null)
				{
					throw new InvalidOperationException("The main synchronization context has already been set");
				}
				Runtime.mainSynchronizationContext = value;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000030AC File Offset: 0x000012AC
		public static void SetProcessName(string name)
		{
			if (!Platform.IsMac && !Platform.IsWindows)
			{
				try
				{
					Runtime.unixSetProcessName(name);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error setting process name", ex);
				}
			}
		}

		// Token: 0x06000030 RID: 48
		[DllImport("libc")]
		private static extern int prctl(int option, byte[] arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5);

		// Token: 0x06000031 RID: 49
		[DllImport("libc")]
		private static extern void setproctitle(byte[] fmt, byte[] str_arg);

		// Token: 0x06000032 RID: 50 RVA: 0x000030F0 File Offset: 0x000012F0
		private static void unixSetProcessName(string name)
		{
			try
			{
				if (Runtime.prctl(15, Encoding.ASCII.GetBytes(name + "\0"), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0)
				{
					throw new ApplicationException("Error setting process name: " + Stdlib.GetLastError());
				}
			}
			catch (EntryPointNotFoundException)
			{
				try
				{
					Runtime.setproctitle(Encoding.ASCII.GetBytes("%s\0"), Encoding.ASCII.GetBytes(name + "\0"));
				}
				catch (EntryPointNotFoundException)
				{
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000033 RID: 51 RVA: 0x00003194 File Offset: 0x00001394
		// (remove) Token: 0x06000034 RID: 52 RVA: 0x000031C8 File Offset: 0x000013C8
		public static event EventHandler ShuttingDown;

		// Token: 0x04000011 RID: 17
		private static ProcessService processService;

		// Token: 0x04000012 RID: 18
		private static SystemAssemblyService systemAssemblyService;

		// Token: 0x04000013 RID: 19
		private static AddinSetupService setupService;

		// Token: 0x04000014 RID: 20
		private static ApplicationService applicationService;

		// Token: 0x04000015 RID: 21
		private static bool initialized;

		// Token: 0x04000016 RID: 22
		private static SynchronizationContext mainSynchronizationContext;

		// Token: 0x04000017 RID: 23
		private static Version version;
	}
}
