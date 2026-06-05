using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000026 RID: 38
	internal class SolutionUpgraderManager
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00004341 File Offset: 0x00002541
		static SolutionUpgraderManager()
		{
			SolutionUpgraderManager.LoadSolutionUpgraders();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004348 File Offset: 0x00002548
		private static void LoadSolutionUpgraders()
		{
			try
			{
				SolutionUpgraderManager.solutionUpgraderList = new List<ISolutionUpgrader>();
				ISolutionUpgrader[] extensionObjects = AddinManager.GetExtensionObjects<ISolutionUpgrader>();
				foreach (ISolutionUpgrader item in extensionObjects)
				{
					SolutionUpgraderManager.solutionUpgraderList.Add(item);
				}
				SolutionUpgraderManager.solutionUpgraderList.Sort(UpgraderComparer.Instance);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("加载项目升级器时出错", exception);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000043C0 File Offset: 0x000025C0
		public static bool Upgrade(string filePath)
		{
			bool result = false;
			try
			{
				if (!Path.GetExtension(filePath).Equals(".csd", StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				XElement xelement = XElement.Load(filePath);
				XElement xelement2 = xelement.Element("PropertyGroup");
				string value = xelement2.Attribute("Version").Value;
				Version version = Version.Parse(value);
				foreach (ISolutionUpgrader solutionUpgrader in SolutionUpgraderManager.solutionUpgraderList)
				{
					if (version.CompareTo(solutionUpgrader.Version) <= 0 && solutionUpgrader.Upgrade(filePath))
					{
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Upgrade solution failed.", exception);
			}
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000044A4 File Offset: 0x000026A4
		public static bool Upgrade(Solution solution)
		{
			bool result = false;
			foreach (ISolutionUpgrader solutionUpgrader in SolutionUpgraderManager.solutionUpgraderList)
			{
				if (solution.Version.CompareTo(solutionUpgrader.Version) <= 0 && solutionUpgrader.Upgrade(solution))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0400003B RID: 59
		private static List<ISolutionUpgrader> solutionUpgraderList;
	}
}
