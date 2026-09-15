using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Projects
{
	internal class SolutionUpgraderManager
	{
		static SolutionUpgraderManager()
		{
			SolutionUpgraderManager.LoadSolutionUpgraders();
		}

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

		private static List<ISolutionUpgrader> solutionUpgraderList;
	}
}
