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
	internal class PageManager
	{
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

		public PageManager()
		{
			this.Initialize();
		}

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

		public void SetTabHead(ITabHead tabHead)
		{
			this.TabHead = tabHead;
		}

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

		private void GetPluginInfoFromService()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				HttpSync httpSync = new HttpSync();
				httpSync.OnResived += this.Resived;
				httpSync.GetSyncResponseOfString(ConstantConfig.Constant.ServicePluginInfoUrl, "post", UserJsonInfo.Instance.GetPostDataToString(), null);
			}
		}

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

		public void DownloadItem(string xmlUrl)
		{
		}

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

		public void SetPluginNumber()
		{
			if (this.TabHead != null)
			{
				this.TabHead.SetNumber(this.GetDoingDownloadNum());
			}
		}

		private void DownloadItem_DeleteSelf(object sender, EventArgs e)
		{
			AssetModel assetModel = sender as AssetModel;
			assetModel.DeleteSelf -= this.DownloadItem_DeleteSelf;
			assetModel.DownloadSelf -= this.DownloadItem_DownloadSelf;
			assetModel.DownloadSucceed -= this.DownloadItem_DownloadOver;
			this.RemoveDownloadItem(assetModel.AssetInfo);
			this.SetPluginNumber();
		}

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

		public Dictionary<Plugin, BaseAssetModel> AssetModelList = new Dictionary<Plugin, BaseAssetModel>();

		private FinishDownloadScrollWindow finishWidget;

		private DoingDownloadScrollWindow doingWidget;

		private static PageManager instance;

		public ITabHead TabHead;
	}
}
