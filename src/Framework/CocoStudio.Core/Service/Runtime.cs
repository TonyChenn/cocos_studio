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
	public static class Runtime
	{
		public static ProcessService ProcessService
		{
			get
			{
				return MonoDevelop.Core.Runtime.ProcessService;
			}
		}

		public static void Initialize(string configDir, string addinsDir)
		{
			if (!Runtime.initialized)
			{
				Platform.Initialize();
				SynchronizationContext.SetSynchronizationContext(new GtkSynchronizationContext());
				MonoDevelop.Core.Runtime.MainSynchronizationContext = SynchronizationContext.Current;
				Runtime.InitializeAddins(configDir, addinsDir);
				Runtime.systemAssemblyService = new SystemAssemblyService();
				MonoDevelop.Core.Runtime.SystemAssemblyService = Runtime.systemAssemblyService;
				Runtime.systemAssemblyService.Initialize();
				Runtime.initialized = true;
			}
		}

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

		private static void OnLoadError(object s, AddinErrorEventArgs args)
		{
			string message = "Add-in error (" + args.AddinId + "): " + args.Message;
			LogConfig.Logger.Error(message, args.Exception);
		}

		private static void OnLoad(object s, AddinEventArgs args)
		{
		}

		private static void OnUnload(object s, AddinEventArgs args)
		{
		}

		private static void DeleteConfigDirectory(string configDir)
		{
			int i = 0;
			while (i < 3)
			{
				i++;
				try
				{
					System.IO.Directory.Delete(configDir, true);
					break;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Delete addin configdir failed.", exception);
				}
			}
		}

		private static SystemAssemblyService systemAssemblyService;

		private static bool initialized;
	}
}
