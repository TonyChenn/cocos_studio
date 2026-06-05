using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core.AddIns;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A2 RID: 162
	internal class MonoTargetRuntimeFactory : ITargetRuntimeFactory
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x000136EC File Offset: 0x000118EC
		static MonoTargetRuntimeFactory()
		{
			MonoTargetRuntimeFactory.LoadRuntimes();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00013CAC File Offset: 0x00011EAC
		public IEnumerable<TargetRuntime> CreateRuntimes()
		{
			MonoRuntimeInfo currentRuntime = MonoRuntimeInfo.FromCurrentRuntime();
			if (currentRuntime != null)
			{
				yield return new MonoTargetRuntime(currentRuntime);
			}
			if (Platform.IsWindows)
			{
				string progs = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				foreach (string dir in Directory.GetDirectories(progs, "Mono*"))
				{
					MonoRuntimeInfo info = new MonoRuntimeInfo(dir);
					if (info.IsValidRuntime)
					{
						yield return new MonoTargetRuntime(info);
					}
				}
			}
			else if (Platform.IsMac)
			{
				if (!Directory.Exists("/Library/Frameworks/Mono.framework/Versions"))
				{
					goto IL_3B3;
				}
				foreach (string dir2 in Directory.GetDirectories("/Library/Frameworks/Mono.framework/Versions"))
				{
					if (!dir2.EndsWith("/Current", StringComparison.Ordinal) && !(currentRuntime.Prefix == dir2))
					{
						MonoRuntimeInfo info2 = new MonoRuntimeInfo(dir2);
						if (info2.IsValidRuntime)
						{
							yield return new MonoTargetRuntime(info2);
						}
					}
				}
			}
			else
			{
				foreach (string pref in MonoTargetRuntimeFactory.commonLinuxPrefixes)
				{
					if (currentRuntime == null || !(currentRuntime.Prefix == pref))
					{
						MonoRuntimeInfo info3 = new MonoRuntimeInfo(pref);
						if (info3.IsValidRuntime)
						{
							foreach (MonoRuntimeInfo monoRuntimeInfo in MonoTargetRuntimeFactory.customRuntimes)
							{
								if (monoRuntimeInfo.Prefix == info3.Prefix)
								{
									MonoTargetRuntimeFactory.customRuntimes.Remove(monoRuntimeInfo);
									break;
								}
							}
							yield return new MonoTargetRuntime(info3);
						}
					}
				}
			}
			foreach (MonoRuntimeInfo info4 in MonoTargetRuntimeFactory.customRuntimes)
			{
				yield return new MonoTargetRuntime(info4)
				{
					UserDefined = true
				};
			}
			IL_3B3:
			yield break;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00013CCC File Offset: 0x00011ECC
		public static TargetRuntime RegisterRuntime(MonoRuntimeInfo info)
		{
			MonoTargetRuntime monoTargetRuntime = new MonoTargetRuntime(info);
			Runtime.SystemAssemblyService.RegisterRuntime(monoTargetRuntime);
			MonoTargetRuntimeFactory.customRuntimes.Add(info);
			MonoTargetRuntimeFactory.SaveRuntimes();
			return monoTargetRuntime;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00013CFC File Offset: 0x00011EFC
		public static void UnregisterRuntime(MonoTargetRuntime runtime)
		{
			Runtime.SystemAssemblyService.UnregisterRuntime(runtime);
			foreach (MonoRuntimeInfo monoRuntimeInfo in MonoTargetRuntimeFactory.customRuntimes)
			{
				if (monoRuntimeInfo.Prefix == runtime.MonoRuntimeInfo.Prefix)
				{
					MonoTargetRuntimeFactory.customRuntimes.Remove(monoRuntimeInfo);
					break;
				}
			}
			MonoTargetRuntimeFactory.SaveRuntimes();
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00013D80 File Offset: 0x00011F80
		private static void LoadRuntimes()
		{
			if (!File.Exists(MonoTargetRuntimeFactory.configFile))
			{
				return;
			}
			try
			{
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(new DataContext());
				using (StreamReader streamReader = new StreamReader(MonoTargetRuntimeFactory.configFile))
				{
					MonoTargetRuntimeFactory.customRuntimes = (RuntimeCollection)xmlDataSerializer.Deserialize(streamReader, typeof(RuntimeCollection));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error while loading mono-runtimes.xml.", ex);
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00013E04 File Offset: 0x00012004
		private static void SaveRuntimes()
		{
			try
			{
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(new DataContext());
				using (StreamWriter streamWriter = new StreamWriter(MonoTargetRuntimeFactory.configFile))
				{
					xmlDataSerializer.Serialize(streamWriter, MonoTargetRuntimeFactory.customRuntimes);
				}
			}
			catch
			{
			}
		}

		// Token: 0x040001CD RID: 461
		private const string MAC_FRAMEWORK_DIR = "/Library/Frameworks/Mono.framework/Versions";

		// Token: 0x040001CE RID: 462
		private static RuntimeCollection customRuntimes = new RuntimeCollection();

		// Token: 0x040001CF RID: 463
		private static string configFile = UserProfile.Current.ConfigDir.Combine(new string[]
		{
			"mono-runtimes.xml"
		});

		// Token: 0x040001D0 RID: 464
		private static string[] commonLinuxPrefixes = new string[]
		{
			"/usr",
			"/usr/local"
		};
	}
}
