using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	internal class FileUpgraderManager
	{
		static FileUpgraderManager()
		{
			FileUpgraderManager.LoadFileUpgraders();
		}

		private static void LoadFileUpgraders()
		{
			try
			{
				FileUpgraderManager.fileUpgraderList = new List<IFileUpgrader>();
				IFileUpgrader[] extensionObjects = AddinManager.GetExtensionObjects<IFileUpgrader>();
				foreach (IFileUpgrader item in extensionObjects)
				{
					FileUpgraderManager.fileUpgraderList.Add(item);
				}
				FileUpgraderManager.fileUpgraderList.Sort(UpgraderComparer.Instance);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("加载Project升级器时出错", exception);
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
				foreach (IFileUpgrader fileUpgrader in FileUpgraderManager.fileUpgraderList)
				{
					if (version.CompareTo(fileUpgrader.Version) <= 0 && fileUpgrader.Upgrade(filePath))
					{
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Upgrade file failed.", exception);
			}
			return result;
		}

		public static bool Upgrade(CocosFile file)
		{
			bool result = false;
			try
			{
				Version version = Version.Parse(file.Version);
				foreach (IFileUpgrader fileUpgrader in FileUpgraderManager.fileUpgraderList)
				{
					if (version.CompareTo(fileUpgrader.Version) <= 0 && fileUpgrader.Upgrade(file))
					{
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Upgrade file failed.", exception);
			}
			return result;
		}

		private static List<IFileUpgrader> fileUpgraderList;
	}
}
