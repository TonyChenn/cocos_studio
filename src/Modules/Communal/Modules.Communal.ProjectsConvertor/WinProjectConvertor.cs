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
	[Extension(Type = typeof(ICommandHandle))]
	public class WinProjectConvertor : ICommandHandle
	{
		public void Initialize()
		{
			GlobalCommand.ImportProjectCmd.Execute += new EventHandler<CommandRunArgs>(this.ImportProjectCmd_Execute);
		}

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

		private void CreateCSDFromJson(IProgressMonitor monitor)
		{
			this.ProjectFilesOp(monitor);
		}

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

		private void EndConvert()
		{
			Services.ProjectsService.CurrentSolution.Save(Services.ProgressMonitors.GetProgressMonitor());
			string messageBox214_ProjectVer1Imported = LanguageInfo.MessageBox214_ProjectVer1Imported;
			LogConfig.Output.Info(messageBox214_ProjectVer1Imported, true);
		}

		private const string AnimationProjType = "animation";

		private const string UIProjType = "ui";

		private const string SceneProjType = "scene";

		private JsonProjType currProjType;

		private string solutionPath = string.Empty;

		private string resourcesDir = string.Empty;

		private string projectName = string.Empty;

		private List<string> jsonFiles = new List<string>();
	}
}
