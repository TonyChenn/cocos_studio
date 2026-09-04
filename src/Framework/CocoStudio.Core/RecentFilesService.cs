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
	// Token: 0x02000032 RID: 50
	public class RecentFilesService
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00008B74 File Offset: 0x00006D74
		public IEnumerable<CocosItemModel> CocosItemRecordList
		{
			get
			{
				return this.cocosItemRecordList;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00008B8C File Offset: 0x00006D8C
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00008BEC File Offset: 0x00006DEC
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00008C28 File Offset: 0x00006E28
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00008CC4 File Offset: 0x00006EC4
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008CDC File Offset: 0x00006EDC
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00008D04 File Offset: 0x00006F04
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008D1C File Offset: 0x00006F1C
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008D44 File Offset: 0x00006F44
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008D58 File Offset: 0x00006F58
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00008D80 File Offset: 0x00006F80
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00008D94 File Offset: 0x00006F94
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00008DBC File Offset: 0x00006FBC
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00008DD0 File Offset: 0x00006FD0
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00008DF8 File Offset: 0x00006FF8
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

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001E1 RID: 481 RVA: 0x00008E0C File Offset: 0x0000700C
		// (remove) Token: 0x060001E2 RID: 482 RVA: 0x00008E48 File Offset: 0x00007048
		public event EventHandler<RecentDocumentChangeEventArgs> RecentDocumentChanged;

		// Token: 0x060001E4 RID: 484 RVA: 0x00008EC0 File Offset: 0x000070C0
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

		// Token: 0x060001E5 RID: 485 RVA: 0x00008FC4 File Offset: 0x000071C4
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

		// Token: 0x060001E6 RID: 486 RVA: 0x00009020 File Offset: 0x00007220
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

		// Token: 0x060001E7 RID: 487 RVA: 0x00009074 File Offset: 0x00007274
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

		// Token: 0x060001E8 RID: 488 RVA: 0x00009140 File Offset: 0x00007340
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

		// Token: 0x060001E9 RID: 489 RVA: 0x00009190 File Offset: 0x00007390
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

		// Token: 0x060001EA RID: 490 RVA: 0x000092A0 File Offset: 0x000074A0
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

		// Token: 0x060001EB RID: 491 RVA: 0x00009380 File Offset: 0x00007580
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

		// Token: 0x060001EC RID: 492 RVA: 0x00009464 File Offset: 0x00007664
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

		// Token: 0x060001ED RID: 493 RVA: 0x00009510 File Offset: 0x00007710
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

		// Token: 0x060001EE RID: 494 RVA: 0x00009568 File Offset: 0x00007768
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

		// Token: 0x060001EF RID: 495 RVA: 0x000095C4 File Offset: 0x000077C4
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

		// Token: 0x060001F0 RID: 496 RVA: 0x000095FC File Offset: 0x000077FC
		private void SetValueByKey(string key, object value)
		{
			this.dictionary[key] = value;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000960D File Offset: 0x0000780D
		private void OnApplicationQuit(object sender, EventArgs args)
		{
			this.SaveRecord();
		}

		// Token: 0x040000F7 RID: 247
		private const int maxRecordCount = 10;

		// Token: 0x040000F8 RID: 248
		private const string conifgDirName = "RecentRecord";

		// Token: 0x040000F9 RID: 249
		private const string recentConfigFileName = "recent.config";

		// Token: 0x040000FA RID: 250
		private const string recentProjectFileName = "RecentProject.config";

		// Token: 0x040000FB RID: 251
		private const string DefaultFolder = "CocosProjects";

		// Token: 0x040000FC RID: 252
		private static string recentConfigDir = Option.GetUserConfigFileByName("RecentRecord");

		// Token: 0x040000FD RID: 253
		private static string recentConfigFilePath = Path.Combine(RecentFilesService.recentConfigDir, "recent.config");

		// Token: 0x040000FE RID: 254
		private static string recentProjectFilePath = Path.Combine(RecentFilesService.recentConfigDir, "RecentProject.config");

		// Token: 0x040000FF RID: 255
		private Dictionary<string, object> dictionary;

		// Token: 0x04000100 RID: 256
		private List<string> keyList;

		// Token: 0x04000101 RID: 257
		private List<CocosItemModel> cocosItemRecordList;

		// Token: 0x04000102 RID: 258
		private string lastBrowserLocationKey = "RecentLastBrowseDir";

		// Token: 0x04000103 RID: 259
		private string lastCreatePrjDirectoryKey = "RecentLastCreateDir";

		// Token: 0x04000104 RID: 260
		private string cocosCodeIDEDirKey = "RecentCocosCodeIDEDir";

		// Token: 0x04000105 RID: 261
		private string lastAvailableSolutionKey = "LastAvailableSolution";

		// Token: 0x04000106 RID: 262
		private string lastImportLocationKey = "LastImportLocationKey";

		// Token: 0x04000107 RID: 263
		private string lastExportDirKey = "LastExportDir";

		// Token: 0x04000108 RID: 264
		private string lastInstallDirKey = "LastInstallDir";
	}
}
