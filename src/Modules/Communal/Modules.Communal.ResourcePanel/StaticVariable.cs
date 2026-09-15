using System;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.ResourcePanel
{
	public static class StaticVariable
	{
		public static string GetResourceID(string iconName)
		{
			return "CocoStudio.DefaultResource.ResourcePanelResource." + iconName;
		}

		public static ResourceFolder GetRootFolder(this Solution solution)
		{
			if (solution != null && solution.RootFolder.Items.Count > 0)
			{
				ResourceGroup resourceGroup = solution.RootFolder.Items[0] as ResourceGroup;
				if (resourceGroup != null)
				{
					return resourceGroup.RootFolder;
				}
			}
			return null;
		}

		public static bool ExecuteCommand(string cmd)
		{
			try
			{
				string text;
				string text2;
				int num;
				Process.SpawnCommandLineSync(cmd, out text, out text2, out num);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Execute cmd failure:" + cmd, exception);
				return false;
			}
			return true;
		}

		internal static ResourceItem CopyScene(ResourceItem parent, ResourceItem sourceFile)
		{
			ResourceItem result;
			try
			{
				FilePath copyNewName = StaticVariable.GetCopyNewName(parent, sourceFile.FullPath);
				if (copyNewName.FileNameWithoutExtension.Length > 50)
				{
					MessageBox.Show(LanguageInfo.MessageBox253_FileNameLengthLimit, MessageBoxImage.Other, null, null);
					result = null;
				}
				else
				{
					FileService.CopyFile(sourceFile.FullPath, copyNewName);
					IProgressMonitor @default = Services.ProgressMonitors.Default;
					CocosItem cocosItem = Services.ProjectsService.ReadCocosItem(@default, copyNewName);
					((ResourceFolder)parent).Items.Add(cocosItem);
					GameFile gameFile = cocosItem.CocosFile as GameFile;
					gameFile.ID = Guid.NewGuid();
					cocosItem.Save(Services.ProgressMonitors.Default);
					result = cocosItem;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error("Copy file failure.", exception);
				result = null;
			}
			return result;
		}

		internal static FilePath GetCopyNewName(ResourceItem parent, FilePath scrFile)
		{
			FilePath filePath = scrFile;
			string extension = scrFile.Extension;
			string fileNameWithoutExtension = scrFile.FileNameWithoutExtension;
			string pattern = "_\\d+$";
			Match match = Regex.Match(fileNameWithoutExtension, pattern);
			if (match.Success)
			{
				string text = match.Value.Substring(1);
				double num = double.Parse(text);
				while (File.Exists(filePath) || StaticVariable.IsExistChild((ResourceFolder)parent, filePath))
				{
					string newValue = match.Value.Replace(text, num.ToString());
					string path = fileNameWithoutExtension.Replace(match.Value, newValue) + extension;
					filePath = Path.Combine(parent.FullPath, path);
					num += 1.0;
				}
			}
			else
			{
				int num2 = 0;
				while (File.Exists(filePath) || StaticVariable.IsExistChild((ResourceFolder)parent, filePath))
				{
					string path2 = string.Concat(new object[]
					{
						fileNameWithoutExtension,
						"_",
						num2,
						extension
					});
					filePath = Path.Combine(parent.FullPath, path2);
					num2++;
				}
			}
			return filePath;
		}

		private static bool IsExistChild(ResourceFolder folder, string filePath)
		{
			if (folder == null || folder.Items == null)
			{
				return false;
			}
			foreach (ResourceItem resourceItem in folder.Items)
			{
				if (resourceItem.FullPath == filePath)
				{
					return true;
				}
			}
			return false;
		}

		public const string ResourceIDHeader = "CocoStudio.DefaultResource.ResourcePanelResource.";
	}
}
