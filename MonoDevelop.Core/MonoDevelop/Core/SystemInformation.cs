using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Addins;

namespace MonoDevelop.Core
{
	// Token: 0x0200022D RID: 557
	public abstract class SystemInformation
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x000558D4 File Offset: 0x00053AD4
		// (set) Token: 0x060014D6 RID: 5334 RVA: 0x000558DB File Offset: 0x00053ADB
		private static SystemInformation Instance { get; set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x000558E4 File Offset: 0x00053AE4
		public static string InstallationUuid
		{
			get
			{
				return PropertyService.Get<string>("MonoDevelop.Core.InstallUuid", Guid.NewGuid().ToString());
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0005590E File Offset: 0x00053B0E
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x00055915 File Offset: 0x00053B15
		public static string SessionUuid { get; private set; }

		// Token: 0x060014DA RID: 5338 RVA: 0x0005591D File Offset: 0x00053B1D
		internal SystemInformation()
		{
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00055928 File Offset: 0x00053B28
		static SystemInformation()
		{
			if (Platform.IsMac)
			{
				SystemInformation.Instance = new MacSystemInformation();
			}
			else if (Platform.IsWindows)
			{
				SystemInformation.Instance = new WindowsSystemInformation();
			}
			else
			{
				SystemInformation.Instance = new LinuxSystemInformation();
			}
			SystemInformation.SessionUuid = DateTime.UtcNow.Ticks.ToString();
		}

		// Token: 0x060014DC RID: 5340
		internal abstract void AppendOperatingSystem(StringBuilder sb);

		// Token: 0x060014DD RID: 5341 RVA: 0x00055DF0 File Offset: 0x00053FF0
		private IEnumerable<ISystemInformationProvider> InternalGetDescription()
		{
			foreach (ISystemInformationProvider info in AddinManager.GetExtensionObjects<ISystemInformationProvider>("/MonoDevelop/Core/SystemInformation", false))
			{
				yield return info;
			}
			StringBuilder sb = new StringBuilder();
			FilePath biFile = Assembly.GetEntryAssembly().Location.ParentDirectory.Combine(new string[]
			{
				"buildinfo"
			});
			if (File.Exists(biFile))
			{
				string[] array = (from l in File.ReadAllLines(biFile)
				select l.Trim() into l
				where !string.IsNullOrEmpty(l)
				select l).ToArray<string>();
				if (array.Length > 0)
				{
					foreach (string value in array)
					{
						sb.AppendLine(value);
					}
				}
			}
			biFile = Assembly.GetEntryAssembly().Location.ParentDirectory.Combine(new string[]
			{
				"buildinfo_xamarin"
			});
			if (File.Exists(biFile))
			{
				string[] array3 = (from l in File.ReadAllLines(biFile)
				select l.Trim() into l
				where !string.IsNullOrEmpty(l)
				select l).ToArray<string>();
				if (array3.Length > 0)
				{
					sb.Append("Xamarin addins: ");
					foreach (string value2 in array3)
					{
						sb.AppendLine(value2);
					}
				}
			}
			if (sb.Length == 0)
			{
				sb.AppendLine("Build information unavailable");
			}
			yield return new SystemInformationSection
			{
				Title = "Build Information",
				Description = sb.ToString()
			};
			sb.Clear();
			this.AppendOperatingSystem(sb);
			yield return new SystemInformationSection
			{
				Title = "Operating System",
				Description = sb.ToString()
			};
			yield break;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00055E4C File Offset: 0x0005404C
		internal static string GetReleaseId()
		{
			FilePath filePath = Assembly.GetEntryAssembly().Location.ParentDirectory.Combine(new string[]
			{
				"buildinfo"
			});
			if (File.Exists(filePath))
			{
				string[] array = (from l in File.ReadAllLines(filePath)
				select l.Split(new char[]
				{
					':'
				})).FirstOrDefault((string[] a) => a.Length > 1 && a[0].Trim() == "Release ID");
				if (array != null)
				{
					return array[1].Trim();
				}
			}
			return null;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00055EF6 File Offset: 0x000540F6
		public static IEnumerable<ISystemInformationProvider> GetDescription()
		{
			return SystemInformation.Instance.InternalGetDescription();
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00055F04 File Offset: 0x00054104
		public static string GetTextDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ISystemInformationProvider systemInformationProvider in SystemInformation.GetDescription())
			{
				stringBuilder.Append("=== ").Append(systemInformationProvider.Title.Trim()).Append(" ===\n\n");
				stringBuilder.Append(systemInformationProvider.Description.Trim());
				stringBuilder.Append("\n\n");
			}
			return stringBuilder.ToString();
		}
	}
}
