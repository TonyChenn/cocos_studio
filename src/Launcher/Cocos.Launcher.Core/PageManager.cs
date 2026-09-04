using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using GLib;
using Gtk;
using Modules.Communal.CocoaChina;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000020 RID: 32
	internal class PageManager
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006D01 File Offset: 0x00004F01
		public FinishDownloadScrollWindow FinishWidget
		{
			get
			{
				if (this.finishWidget == null)
				{
					this.finishWidget = new FinishDownloadScrollWindow(this.GetFinishWidgetList());
				}
				return this.finishWidget;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006D22 File Offset: 0x00004F22
		public DoingDownloadScrollWindow DoingWidget
		{
			get
			{
				if (this.doingWidget == null)
				{
					this.doingWidget = new DoingDownloadScrollWindow(this.GetDoingWidgetList());
				}
				return this.doingWidget;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006D43 File Offset: 0x00004F43
		public static PageManager Instance
		{
			get
			{
				if (PageManager.instance == null)
				{
					PageManager.instance = new PageManager();
				}
				return PageManager.instance;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006D5B File Offset: 0x00004F5B
		public PageManager()
		{
			this.Initialize();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006DAC File Offset: 0x00004FAC
		private void Initialize()
		{
			List<Plugin> downloadedList = this.GetDownloadedList();
			foreach (Plugin model in downloadedList)
			{
				this.AddDownloadItem(model, false);
			}
			Task.Run(delegate()
			{
				try
				{
					this.GetPluginInfoFromService();
				}
				catch (Exception exception)
				{
					LogConfig.Output.Error("与服务器交互失败:", exception);
				}
			});
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006E18 File Offset: 0x00005018
		public void SetTabHead(ITabHead tabHead)
		{
			this.TabHead = tabHead;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006E24 File Offset: 0x00005024
		private List<Widget> GetFinishWidgetList()
		{
			List<Widget> list = new List<Widget>();
			foreach (AssetModel assetModel in this.AssetModelList.Values)
			{
				if (assetModel.AssetInfo.PluginFraction == 100f)
				{
					FinishDownloadItemView item = new FinishDownloadItemView(assetModel);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006E9C File Offset: 0x0000509C
		private List<Widget> GetDoingWidgetList()
		{
			List<Widget> list = new List<Widget>();
			foreach (AssetModel assetModel in this.AssetModelList.Values)
			{
				if (assetModel.AssetInfo.PluginFraction < 100f)
				{
					DoingDownloadItemView item = new DoingDownloadItemView(assetModel);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006F14 File Offset: 0x00005114
		private void RemoveDownloadItem(Plugin info)
		{
			if (!this.AssetModelList.ContainsKey(info))
			{
				return;
			}
			try
			{
				if (File.Exists(this.AssetModelList[info].AssetInfo.PluginPath))
				{
					File.Delete(this.AssetModelList[info].AssetInfo.PluginPath);
				}
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("文件删除失败" + arg);
			}
			this.AssetModelList.Remove(info);
			this.SavePluginListInfo();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006FA8 File Offset: 0x000051A8
		private void GetPluginInfoFromService()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				HttpSync httpSync = new HttpSync();
				httpSync.OnResived += this.Resived;
				httpSync.GetSyncResponseOfString(ConstantConfig.Constant.ServicePluginInfoUrl, "post", UserJsonInfo.Instance.GetPostDataToString(), null);
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007004 File Offset: 0x00005204
		private void Resived(object sender, HttpSync.HttpSyncArgs e)
		{
			try
			{
				HttpSync httpSync = sender as HttpSync;
				httpSync.OnResived -= this.Resived;
				int num = int.Parse(Login.GetJsonAnalysis(e.Message, "Res"));
				UserJsonInfo userJsonInfo = null;
				if (num > 0)
				{
					userJsonInfo = JsonHelper.Parse<UserJsonInfo>(e.Message);
				}
				if (userJsonInfo != null)
				{
					this.UpdateFinishDownload(userJsonInfo);
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000707C File Offset: 0x0000527C
		private void UpdateFinishDownload(UserJsonInfo serviceUserInfo)
		{
			if (serviceUserInfo.Tools == null)
			{
				return;
			}
			foreach (ServicePluginInfo servicePluginInfo in serviceUserInfo.Tools)
			{
				foreach (AssetModel assetModel in this.AssetModelList.Values)
				{
					Plugin assetInfo = assetModel.AssetInfo;
					if (servicePluginInfo.Type == assetInfo.PluginType && assetInfo.PluginFraction == 100f)
					{
						assetModel.SetServiceInfo(servicePluginInfo);
						break;
					}
				}
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007144 File Offset: 0x00005344
		public void DownloadItem(string xmlUrl, int x, int y)
		{
			bool flag = false;
			Plugin pluginInfoFromUrl = this.GetPluginInfoFromUrl(xmlUrl, out flag);
			if (pluginInfoFromUrl == null)
			{
				return;
			}
			if (flag)
			{
				this.HandleItem(pluginInfoFromUrl, x, y);
				return;
			}
			if (!Option.CheckIsWritableDir(Option.UserConfig.CocosStorePath))
			{
				MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.Launcher_IllegalPath, MessageBoxButton.YesNo, MessageBoxImage.Question, Services.MainWindow, EnumMainButton.Yes, null);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					Commands.SettingCmd.RaiseExecute("Download");
				}
				return;
			}
			this.AddDownloadItem(pluginInfoFromUrl, x, y);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000071B4 File Offset: 0x000053B4
		private void HandleItem(Plugin model, int x, int y)
		{
			if (!this.AssetModelList.ContainsKey(model))
			{
				return;
			}
			if (model.PluginFraction != 100f)
			{
				Services.OutputService.Info(LanguageInfo.Launcher_IsDownloading);
				this.AssetModelList[model].StartDownload();
				return;
			}
			if (model.Action == ActionType.update.ToString())
			{
				DownloadAnimation.Instance.StartDownloadAnimation(x, y);
				this.AssetModelList[model].Update(model);
				return;
			}
			if (model.Action == ActionType.open.ToString())
			{
				this.AssetModelList[model].Open();
				return;
			}
			if (model.Action == ActionType.install.ToString())
			{
				this.AssetModelList[model].Install(false);
				return;
			}
			Services.TabGroupService.SwitchTab(new SwitchTabInfo(3));
			DownloadService.Instance.SwitchPageOne(false);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000072B4 File Offset: 0x000054B4
		private void AddDownloadItem(Plugin pluginmodel, int x, int y)
		{
			DownloadAnimation.Instance.StartDownloadAnimation(x, y);
			this.AddDownloadItem(pluginmodel, true);
			Tracker.Add(ViewRegions.None, "CocosStore", pluginmodel.PluginName, "");
			this.SavePluginListInfo();
			GLib.Timeout.Add(1000U, delegate
			{
				this.SetPluginNumber();
				return false;
			});
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000730C File Offset: 0x0000550C
		public bool HaveLoading()
		{
			foreach (AssetModel assetModel in this.AssetModelList.Values)
			{
				if (assetModel.AssetInfo.IsLoading)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007374 File Offset: 0x00005574
		public void DownloadItem(string xmlUrl)
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007378 File Offset: 0x00005578
		public bool AddDownloadItem(Plugin model, bool start = true)
		{
			if (this.AssetModelList.ContainsKey(model))
			{
				return false;
			}
			BaseAssetModel assetModel = AssetModelFactory.Instance.GetAssetModel(model);
			if (assetModel == null)
			{
				return false;
			}
			assetModel.DownloadSucceed += this.DownloadItem_DownloadOver;
			assetModel.DeleteSelf += this.DownloadItem_DeleteSelf;
			assetModel.DownloadSelf += this.DownloadItem_DownloadSelf;
			try
			{
				if (start)
				{
					assetModel.StartDownload();
				}
				else
				{
					model.IsLoading = false;
				}
			}
			catch (Exception message)
			{
				LogConfig.Output.Error(message);
				return false;
			}
			this.AssetModelList.Add(model, assetModel);
			if (model.PluginFraction != 100f && this.doingWidget != null)
			{
				this.doingWidget.AddItem(new DoingDownloadItemView(assetModel));
			}
			else if (model.PluginFraction == 100f && this.finishWidget != null)
			{
				this.finishWidget.AddItem(new FinishDownloadItemView(assetModel));
			}
			return true;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007470 File Offset: 0x00005670
		public void SetPluginNumber()
		{
			if (this.TabHead != null)
			{
				this.TabHead.SetNumber(this.GetDoingDownloadNum());
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000748C File Offset: 0x0000568C
		private void DownloadItem_DeleteSelf(object sender, EventArgs e)
		{
			AssetModel assetModel = sender as AssetModel;
			assetModel.DeleteSelf -= this.DownloadItem_DeleteSelf;
			assetModel.DownloadSelf -= this.DownloadItem_DownloadSelf;
			assetModel.DownloadSucceed -= this.DownloadItem_DownloadOver;
			this.RemoveDownloadItem(assetModel.AssetInfo);
			this.SetPluginNumber();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000074E8 File Offset: 0x000056E8
		private void DownloadItem_DownloadOver(object sender, DownloadSucceedEventArgs e)
		{
			AssetModel model = sender as AssetModel;
			if (File.Exists(e.TargetPath))
			{
				if (this.finishWidget != null)
				{
					this.finishWidget.AddItem(new FinishDownloadItemView(model));
				}
				this.SavePluginListInfo();
			}
			this.SetPluginNumber();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007530 File Offset: 0x00005730
		private void DownloadItem_DownloadSelf(object sender, PluginDownloadEventArgs e)
		{
			AssetModel assetModel = sender as AssetModel;
			assetModel.StartDownload();
			if (this.doingWidget != null)
			{
				this.doingWidget.AddItem(new DoingDownloadItemView(assetModel));
			}
			this.SavePluginListInfo();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000756C File Offset: 0x0000576C
		private List<Plugin> GetDownloadedList()
		{
			List<Plugin> result = new List<Plugin>();
			try
			{
				if (File.Exists(ConstantConfig.Paths.PluginListInfoFile))
				{
					result = XmlHelper.XMLAction<List<Plugin>>.ReadData(ConstantConfig.Paths.PluginListInfoFile);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("初始化本地插件信息失败: ", exception);
			}
			return result;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000075C8 File Offset: 0x000057C8
		public void SavePluginListInfo()
		{
			try
			{
				if (File.Exists(ConstantConfig.Paths.PluginListInfoFile))
				{
					File.Delete(ConstantConfig.Paths.PluginListInfoFile);
				}
				List<Plugin> list = new List<Plugin>();
				foreach (Plugin item in this.AssetModelList.Keys)
				{
					list.Add(item);
				}
				XmlHelper.SaveXml(ConstantConfig.Paths.PluginListInfoFile, list);
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("保存下载插件列表失败:" + arg);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000767C File Offset: 0x0000587C
		public int GetDoingDownloadNum()
		{
			int num = 0;
			foreach (Plugin plugin in this.AssetModelList.Keys)
			{
				if (plugin.PluginFraction != 100f)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000076E4 File Offset: 0x000058E4
		public Plugin GetPluginInfoFromUrl(string url, out bool have)
		{
			have = false;
			Plugin result;
			try
			{
				Plugin plugin;
				using (Stream responseOfStream = url.GetResponseOfStream("get", ""))
				{
					StreamReader streamReader = new StreamReader(responseOfStream, Encoding.UTF8);
					string text = streamReader.ReadLine();
					if (!text.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
					{
						WebHelper.OpenWeb(url);
						return null;
					}
					responseOfStream.Position = 0L;
					plugin = XmlHelper.XMLAction<Plugin>.ReadData(responseOfStream);
				}
				if (plugin.OpenType == OperationType.link.ToString())
				{
					WebHelper.OpenWeb(plugin.PluginUrl);
					result = null;
				}
				else if (string.IsNullOrEmpty(plugin.PluginUrl))
				{
					result = null;
				}
				else
				{
					foreach (Plugin plugin2 in this.AssetModelList.Keys)
					{
						if (plugin2.PluginType == plugin.PluginType)
						{
							plugin2.PluginVersion = plugin.PluginVersion;
							plugin2.PluginUrl = plugin.PluginUrl;
							plugin2.Action = plugin.Action;
							have = true;
							return plugin2;
						}
					}
					result = plugin;
				}
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("解析网络链接失败" + url + arg);
				result = null;
			}
			return result;
		}

		// Token: 0x04000066 RID: 102
		public Dictionary<Plugin, BaseAssetModel> AssetModelList = new Dictionary<Plugin, BaseAssetModel>();

		// Token: 0x04000067 RID: 103
		private FinishDownloadScrollWindow finishWidget;

		// Token: 0x04000068 RID: 104
		private DoingDownloadScrollWindow doingWidget;

		// Token: 0x04000069 RID: 105
		private static PageManager instance;

		// Token: 0x0400006A RID: 106
		public ITabHead TabHead;
	}
}
