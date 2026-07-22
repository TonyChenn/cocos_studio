using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using EditorCommon.JsonModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.ProjectsConvertor
{
	// Token: 0x02000043 RID: 67
	[Extension(Type = typeof(ICommandHandle))]
	public class WinProjectConvertor : ICommandHandle
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x0000A772 File Offset: 0x00008972
		public void Initialize()
		{
			GlobalCommand.ImportProjectCmd.Execute += new EventHandler<CommandRunArgs>(this.ImportProjectCmd_Execute);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000A78C File Offset: 0x0000898C
		private void ImportProjectCmd_Execute(object sender, EventArgs e)
		{
			string lastBrowserLocation = Services.RecentFileService.LastBrowserLocation;
			string[] fileTypes = new string[]
			{
				"animation",
				"ui",
				"scene"
			};
			string fileName = FileChooserDialogModel.GetOpenFilePath(fileTypes, LanguageInfo.OpenProect, false, lastBrowserLocation).FileName;
			this.jsonFiles.Clear();
			this.PreDealPath(fileName);
			this.resourcesDir = Path.Combine(this.solutionPath, "CocosStudio".ToLower());
			this.ConvertProject(fileName, this.resourcesDir);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000A814 File Offset: 0x00008A14
		protected void PreDealPath(string filepath)
		{
			string text = Path.GetExtension(filepath);
			text = text.Substring(1);
			if (text.Equals("animation", StringComparison.OrdinalIgnoreCase))
			{
				this.currProjType = JsonProjType.animation;
			}
			else if (text.Equals("ui", StringComparison.OrdinalIgnoreCase))
			{
				this.currProjType = JsonProjType.ui;
			}
			else if (text.Equals("scene", StringComparison.OrdinalIgnoreCase))
			{
				this.currProjType = JsonProjType.scene;
			}
			this.solutionPath = Services.ProjectOperations.CurrentSelectedSolution.BaseDirectory;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
			this.projectName = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000AD1C File Offset: 0x00008F1C
		private async void ConvertProject(string filepath, string resPath)
		{
			if (!Directory.Exists(resPath))
			{
				Directory.CreateDirectory(resPath);
			}
			XmlDocument document = new XmlDocument();
			document.Load(filepath);
			XmlNode root = document.DocumentElement;
			XmlNode node = null;
			string oldprojdir = Path.GetDirectoryName(filepath);
			node = root.SelectSingleNode("Name");
			this.projectName = node.InnerText;
			node = root.SelectSingleNode("JsonFileName");
			string jsonname = node.InnerText;
			string oldresdir = string.Empty;
			string relativeRes = null;
			node = root.SelectSingleNode("ResRelativePath");
			if (node != null)
			{
				relativeRes = node.InnerText;
			}
			string jsondir = string.Empty;
			if (!string.IsNullOrEmpty(relativeRes))
			{
				oldresdir = Path.Combine(oldprojdir, "..", "..", "assets");
				if (this.currProjType == JsonProjType.scene)
				{
					jsondir = Path.Combine(oldresdir, "publish");
				}
				else
				{
					jsondir = Path.Combine(oldprojdir, "Json");
				}
			}
			else
			{
				oldresdir = Path.Combine(oldprojdir, "Resources");
				jsondir = Path.Combine(oldprojdir, "Json");
			}
			node = root.SelectSingleNode("JsonList");
			string outsidejsonpath = Path.Combine(jsondir, jsonname);
			if (!File.Exists(outsidejsonpath) && File.Exists(Path.Combine(oldresdir, jsonname)))
			{
				outsidejsonpath = Path.Combine(oldresdir, jsonname);
			}
			if (File.Exists(outsidejsonpath))
			{
				this.jsonFiles.Add(outsidejsonpath);
			}
			foreach (object obj in node.SelectNodes("string"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				string item = Path.Combine(jsondir, xmlNode.InnerText);
				if (!this.jsonFiles.Contains(item))
				{
					this.jsonFiles.Add(item);
				}
			}
			JsonFileHelp.SetOldResDir(oldresdir);
			List<string> oldfiles = new List<string>();
			DirectoryInfo dirinfo = new DirectoryInfo(oldresdir);
			foreach (DirectoryInfo directoryInfo in dirinfo.GetDirectories())
			{
				oldfiles.Add(directoryInfo.FullName);
			}
			foreach (FileInfo fileInfo in dirinfo.GetFiles())
			{
				oldfiles.Add(fileInfo.FullName);
			}
			ResourceFolder rootfolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			new List<ResourceItem>();
			await Services.ProjectOperations.ImportResourcesAsync(rootfolder, oldfiles, new Action<IProgressMonitor>(this.CreateCSDFromJson));
			this.EndConvert();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000AD66 File Offset: 0x00008F66
		private void CreateCSDFromJson(IProgressMonitor monitor)
		{
			this.ProjectFilesOp(monitor);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000AD70 File Offset: 0x00008F70
		private void ProjectFilesOp(IProgressMonitor monitor)
		{
			List<ResourceItem> list = new List<ResourceItem>();
			string empty = string.Empty;
			foreach (string text in this.jsonFiles)
			{
				JsonFileHelp.SetOldToNewRelativeResDir(text, "");
				ProjectsConvertorHelper.Clear();
				CocosItem cocosItem = ProjectsConvertorHelper.BuildCSD(text, this.resourcesDir, this.currProjType, true);
				string text2 = string.Format("{0} ", text);
				if (cocosItem != null)
				{
					list.Add(cocosItem);
					text2 += LanguageInfo.Dialog_Import_Success;
				}
				else
				{
					text2 += LanguageInfo.Dialog_Import_Failed;
				}
				LogConfig.Output.Info(text2, true);
			}
			ResourceGroup currentResourceGroup = Services.ProjectOperations.CurrentResourceGroup;
			AddResourcesArgs payload = new AddResourcesArgs(currentResourceGroup.RootFolder, list, true);
			Services.EventsService.GetEvent<AddResourcesEvent>().Publish(payload);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000AE58 File Offset: 0x00009058
		private void EndConvert()
		{
			Services.ProjectsService.CurrentSolution.Save(Services.ProgressMonitors.GetProgressMonitor());
			string messageBox214_ProjectVer1Imported = LanguageInfo.MessageBox214_ProjectVer1Imported;
			LogConfig.Output.Info(messageBox214_ProjectVer1Imported, true);
		}

		// Token: 0x040001D7 RID: 471
		private const string AnimationProjType = "animation";

		// Token: 0x040001D8 RID: 472
		private const string UIProjType = "ui";

		// Token: 0x040001D9 RID: 473
		private const string SceneProjType = "scene";

		// Token: 0x040001DA RID: 474
		private JsonProjType currProjType;

		// Token: 0x040001DB RID: 475
		private string solutionPath = string.Empty;

		// Token: 0x040001DC RID: 476
		private string resourcesDir = string.Empty;

		// Token: 0x040001DD RID: 477
		private string projectName = string.Empty;

		// Token: 0x040001DE RID: 478
		private List<string> jsonFiles = new List<string>();
	}
}
