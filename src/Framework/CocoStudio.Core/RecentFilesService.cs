using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Lib.Prism;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	public class RecentFilesService
	{
		public IEnumerable<CocosItemModel> CocosItemRecordList
		{
			get
			{
				return this.cocosItemRecordList;
			}
		}

		public string LastBrowserLocation
		{
			get
			{
				string text = (string)this.GetValueByKey(this.lastBrowserLocationKey, string.Empty);
				if (!string.IsNullOrEmpty(text) && !Option.CheckIsReadableDir(text))
				{
					text = string.Empty;
				}
				string result;
				if (string.IsNullOrWhiteSpace(text))
				{
					result = Option.MyDocumentsFolder;
				}
				else
				{
					result = text;
				}
				return result;
			}
			set
			{
				string value2;
				if (File.Exists(value))
				{
					value2 = Path.GetDirectoryName(value);
				}
				else
				{
					value2 = value;
				}
				this.SetValueByKey(this.lastBrowserLocationKey, value2);
				this.SaveRecord();
			}
		}

		public string LastCreatePrjDirectory
		{
			get
			{
				string text = (string)this.GetValueByKey(this.lastCreatePrjDirectoryKey, string.Empty);
				if (!string.IsNullOrEmpty(text) && !Option.CheckIsWritableDir(text))
				{
					text = string.Empty;
				}
				if (string.IsNullOrEmpty(text))
				{
					if (Platform.IsWindows)
					{
						text = Path.Combine(Path.GetDirectoryName(Option.UserCustomerConfigFolder), "CocosProjects");
					}
					else if (Platform.IsMac)
					{
						text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Documents", "CocosProjects");
					}
				}
				return text;
			}
			set
			{
				this.SetValueByKey(this.lastCreatePrjDirectoryKey, value);
				this.SaveRecord();
			}
		}

		public string CocosCodeIDEDir
		{
			get
			{
				return (string)this.GetValueByKey(this.cocosCodeIDEDirKey, string.Empty);
			}
			set
			{
				this.SetValueByKey(this.cocosCodeIDEDirKey, value);
				this.SaveRecord();
			}
		}

		public string LastAvailableSolution
		{
			get
			{
				return this.GetValueByKey(this.lastAvailableSolutionKey, string.Empty).ToString();
			}
			set
			{
				this.SetValueByKey(this.lastAvailableSolutionKey, value);
			}
		}

		public string LastImportLocation
		{
			get
			{
				return this.GetValueByKey(this.lastImportLocationKey, Option.MyDocumentsFolder).ToString();
			}
			set
			{
				this.SetValueByKey(this.lastImportLocationKey, value);
			}
		}

		public string LastExportDir
		{
			get
			{
				return this.GetValueByKey(this.lastExportDirKey, string.Empty).ToString();
			}
			set
			{
				this.SetValueByKey(this.lastExportDirKey, value);
			}
		}

		public string LastInstallDir
		{
			get
			{
				return this.GetValueByKey(this.lastInstallDirKey, string.Empty).ToString();
			}
			set
			{
				this.SetValueByKey(this.lastInstallDirKey, value);
			}
		}

		public event EventHandler<RecentDocumentChangeEventArgs> RecentDocumentChanged;

		public RecentFilesService()
		{
			this.dictionary = new Dictionary<string, object>();
			this.keyList = new List<string>();
			this.keyList.Add(this.lastBrowserLocationKey);
			this.keyList.Add(this.lastCreatePrjDirectoryKey);
			this.keyList.Add(this.cocosCodeIDEDirKey);
			this.keyList.Add(this.lastAvailableSolutionKey);
			this.keyList.Add(this.lastImportLocationKey);
			this.keyList.Add(this.lastInstallDirKey);
			this.CheckAndCreateDir();
			this.InitRecentProject();
			this.InitOtherRecentRecord();
			this.InitEvent();
		}

		private bool CheckAndCreateDir()
		{
			if (!Directory.Exists(RecentFilesService.recentConfigDir))
			{
				try
				{
					Directory.CreateDirectory(RecentFilesService.recentConfigDir);
					return true;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Create recently project record dir failed.", exception);
					return false;
				}
			}
			return true;
		}

		private void InitRecentProject()
		{
			if (File.Exists(RecentFilesService.recentProjectFilePath))
			{
				this.cocosItemRecordList = this.LoadRecentProjectFromFile(RecentFilesService.recentProjectFilePath);
			}
			else
			{
				this.cocosItemRecordList = this.LoadRecentProjectFromFile(RecentFilesService.recentConfigFilePath);
				this.SaveRecentProject(this.cocosItemRecordList);
			}
		}

		private void InitOtherRecentRecord()
		{
			if (File.Exists(RecentFilesService.recentConfigFilePath))
			{
				try
				{
					XElement xelement = XElement.Load(RecentFilesService.recentConfigFilePath);
					foreach (string text in this.keyList)
					{
						XElement xelement2 = xelement.Element(text);
						if (xelement2 != null)
						{
							this.dictionary.Add(text, xelement2.Value);
						}
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Read recently project record file failed.", exception);
				}
			}
		}

		private void InitEvent()
		{
			if (Option.CurrentApp != EnumApp.Launcher)
			{
				if (Services.MainWindow != null)
				{
					IEventAggregator eventsService = Services.EventsService;
					Services.MainWindow.Closed += this.OnApplicationQuit;
				}
			}
		}

		private List<CocosItemModel> LoadRecentProjectFromFile(string filePath)
		{
			List<CocosItemModel> list = new List<CocosItemModel>();
			List<CocosItemModel> result;
			if (!File.Exists(filePath))
			{
				result = list;
			}
			else
			{
				try
				{
					XElement xelement = XElement.Load(filePath);
					IEnumerable<XElement> enumerable = xelement.Elements("ProjectList");
					if (enumerable == null)
					{
						return list;
					}
					foreach (XElement xelement2 in enumerable.Elements<XElement>())
					{
						string value = xelement2.Attribute("LocalPath").Value;
						if (File.Exists(value))
						{
							CocosItemModel item = new CocosItemModel(value);
							list.Add(item);
						}
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("从旧版本配置文件中迁移最近使用项目记录时出错", exception);
				}
				result = list;
			}
			return result;
		}

		private void SaveRecentProject(List<CocosItemModel> projList)
		{
			try
			{
				XElement xelement = new XElement("RecentProjects");
				XElement xelement2 = new XElement("ProjectList");
				xelement.Add(xelement2);
				foreach (CocosItemModel cocosItemModel in projList)
				{
					XElement xelement3 = new XElement("Project");
					xelement3.SetAttributeValue("LocalPath", cocosItemModel.LocalPath);
					xelement2.Add(xelement3);
				}
				xelement.Save(RecentFilesService.recentProjectFilePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("保存最近使用项目列表时出错", exception);
			}
		}

		public void SaveRecord()
		{
			try
			{
				XElement xelement = new XElement("RecentRecord");
				foreach (KeyValuePair<string, object> keyValuePair in this.dictionary)
				{
					xelement.Add(new XElement(keyValuePair.Key, keyValuePair.Value));
				}
				xelement.Save(RecentFilesService.recentConfigFilePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Save recently project record file failed.", exception);
			}
		}

		public void AddProject(string filePath)
		{
			CocosItemModel cocosItemModel = this.cocosItemRecordList.FirstOrDefault((CocosItemModel a) => a.LocalPath.Equals(filePath, StringComparison.CurrentCultureIgnoreCase));
			EnumRecentPrjChangeType changeType = EnumRecentPrjChangeType.Reorder;
			if (cocosItemModel == null)
			{
				cocosItemModel = new CocosItemModel(filePath);
				changeType = EnumRecentPrjChangeType.New;
			}
			this.RefreshFirstItem(this.cocosItemRecordList, cocosItemModel);
			List<CocosItemModel> projList = this.LoadRecentProjectFromFile(RecentFilesService.recentProjectFilePath);
			this.RefreshFirstItem(projList, cocosItemModel);
			this.SaveRecentProject(projList);
			if (this.RecentDocumentChanged != null)
			{
				RecentDocumentChangeEventArgs e = new RecentDocumentChangeEventArgs(cocosItemModel, changeType);
				this.RecentDocumentChanged(this, e);
			}
		}

		private void RefreshFirstItem(List<CocosItemModel> projList, CocosItemModel projItem)
		{
			if (projList != null && projItem != null)
			{
				projList.Remove(projItem);
				projList.Insert(0, projItem);
				if (projList.Count > 10)
				{
					projList.RemoveRange(10, projList.Count - 10);
				}
			}
		}

		public void RemoveCocosItem(CocosItemModel item)
		{
			this.cocosItemRecordList.Remove(item);
			List<CocosItemModel> list = this.LoadRecentProjectFromFile(RecentFilesService.recentProjectFilePath);
			list.Remove(item);
			this.SaveRecentProject(list);
			if (this.RecentDocumentChanged != null)
			{
				RecentDocumentChangeEventArgs e = new RecentDocumentChangeEventArgs(item, EnumRecentPrjChangeType.Remove);
				this.RecentDocumentChanged(this, e);
			}
		}

		private object GetValueByKey(string key, object defaultVal)
		{
			object result;
			if (!this.dictionary.ContainsKey(key))
			{
				result = defaultVal;
			}
			else
			{
				object obj;
				this.dictionary.TryGetValue(key, out obj);
				result = obj;
			}
			return result;
		}

		private void SetValueByKey(string key, object value)
		{
			this.dictionary[key] = value;
		}

		private void OnApplicationQuit(object sender, EventArgs args)
		{
			this.SaveRecord();
		}

		private const int maxRecordCount = 10;

		private const string conifgDirName = "RecentRecord";

		private const string recentConfigFileName = "recent.config";

		private const string recentProjectFileName = "RecentProject.config";

		private const string DefaultFolder = "CocosProjects";

		private static string recentConfigDir = Option.GetUserConfigFileByName("RecentRecord");

		private static string recentConfigFilePath = Path.Combine(RecentFilesService.recentConfigDir, "recent.config");

		private static string recentProjectFilePath = Path.Combine(RecentFilesService.recentConfigDir, "RecentProject.config");

		private Dictionary<string, object> dictionary;

		private List<string> keyList;

		private List<CocosItemModel> cocosItemRecordList;

		private string lastBrowserLocationKey = "RecentLastBrowseDir";

		private string lastCreatePrjDirectoryKey = "RecentLastCreateDir";

		private string cocosCodeIDEDirKey = "RecentCocosCodeIDEDir";

		private string lastAvailableSolutionKey = "LastAvailableSolution";

		private string lastImportLocationKey = "LastImportLocationKey";

		private string lastExportDirKey = "LastExportDir";

		private string lastInstallDirKey = "LastInstallDir";
	}
}
