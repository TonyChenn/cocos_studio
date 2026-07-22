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
	// Token: 0x02000030 RID: 48
	public static class StaticVariable
	{
		// Token: 0x060001CC RID: 460 RVA: 0x00009F0B File Offset: 0x0000810B
		public static string GetResourceID(string iconName)
		{
			return "CocoStudio.DefaultResource.ResourcePanelResource." + iconName;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009F18 File Offset: 0x00008118
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

		// Token: 0x060001CE RID: 462 RVA: 0x00009F60 File Offset: 0x00008160
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

		// Token: 0x060001CF RID: 463 RVA: 0x00009FAC File Offset: 0x000081AC
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

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A088 File Offset: 0x00008288
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

		// Token: 0x060001D1 RID: 465 RVA: 0x0000A1BC File Offset: 0x000083BC
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

		// Token: 0x04000098 RID: 152
		public const string ResourceIDHeader = "CocoStudio.DefaultResource.ResourcePanelResource.";
	}
}
