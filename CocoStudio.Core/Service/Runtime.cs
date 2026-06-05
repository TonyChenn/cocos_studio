using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CocoStudio.Basic;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.Service
{
	// Token: 0x02000036 RID: 54
	public static class Runtime
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000978C File Offset: 0x0000798C
		public static ProcessService ProcessService
		{
			get
			{
				return Runtime.ProcessService;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000097A4 File Offset: 0x000079A4
		public static void Initialize(string configDir, string addinsDir)
		{
			if (!Runtime.initialized)
			{
				Platform.Initialize();
				SynchronizationContext.SetSynchronizationContext(new GtkSynchronizationContext());
				Runtime.MainSynchronizationContext = SynchronizationContext.Current;
				Runtime.InitializeAddins(configDir, addinsDir);
				Runtime.systemAssemblyService = new SystemAssemblyService();
				Runtime.SystemAssemblyService = Runtime.systemAssemblyService;
				Runtime.systemAssemblyService.Initialize();
				Runtime.initialized = true;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000980C File Offset: 0x00007A0C
		private static void InitializeAddins(string configDir, string addinsDir)
		{
			AddinManager.AddinLoadError += Runtime.OnLoadError;
			AddinManager.AddinLoaded += Runtime.OnLoad;
			AddinManager.AddinUnloaded += Runtime.OnUnload;
			bool flag = false;
			do
			{
				try
				{
					AddinManager.Initialize(configDir, addinsDir);
					AddinManager.InitializeDefaultLocalizer(new DefaultAddinLocalizer());
					ProgressStatusMonitor monitor = new ProgressStatusMonitor(new ConsoleProgressFullExceptionMonitor(true, true), 4);
					AddinManager.Registry.Update(monitor);
					IEnumerable<IDisplayBuilder> builder = DisplayBuilderService.GetBuilder<IDisplayBuilder>();
					if (builder == null || builder.Count<IDisplayBuilder>() == 0)
					{
						throw new Exception("Addin update failed.");
					}
					flag = false;
				}
				catch (Exception exception)
				{
					if (!flag && AddinManager.IsInitialized)
					{
						LogConfig.Logger.Error("Addin update failed.", exception);
						try
						{
							AddinManager.Registry.Dispose();
							AddinManager.Shutdown();
						}
						catch (Exception)
						{
						}
						Runtime.DeleteConfigDirectory(configDir);
						flag = true;
					}
					else
					{
						string message = "Failed to start. Some of the assemblies required may miss.";
						LogConfig.Logger.Error(message);
						Environment.Exit(-1);
						flag = false;
					}
				}
				finally
				{
					AddinManager.AddinLoadError -= Runtime.OnLoadError;
					AddinManager.AddinLoaded -= Runtime.OnLoad;
					AddinManager.AddinUnloaded -= Runtime.OnUnload;
				}
			}
			while (flag);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009994 File Offset: 0x00007B94
		private static void OnLoadError(object s, AddinErrorEventArgs args)
		{
			string message = "Add-in error (" + args.AddinId + "): " + args.Message;
			LogConfig.Logger.Error(message, args.Exception);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000099D0 File Offset: 0x00007BD0
		private static void OnLoad(object s, AddinEventArgs args)
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000099D3 File Offset: 0x00007BD3
		private static void OnUnload(object s, AddinEventArgs args)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000099D8 File Offset: 0x00007BD8
		private static void DeleteConfigDirectory(string configDir)
		{
			int i = 0;
			while (i < 3)
			{
				i++;
				try
				{
					configDir.Delete();
					break;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Delete addin configdir failed.", exception);
				}
			}
		}

		// Token: 0x04000114 RID: 276
		private static SystemAssemblyService systemAssemblyService;

		// Token: 0x04000115 RID: 277
		private static bool initialized;
	}
}
