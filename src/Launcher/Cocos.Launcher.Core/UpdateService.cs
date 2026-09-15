using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;

namespace Cocos.Launcher.Core
{
	public class UpdateService : IUpdateService
	{
		public event EventHandler<EventArgs> UpdateChanged;

		public UpdateInfo StoreUpdateInfo
		{
			get
			{
				return this.storeUpdateInfo;
			}
			set
			{
				this.storeUpdateInfo = value;
			}
		}

		public UpdateInfo TutorialsUpdateInfo
		{
			get
			{
				return this.tutorialsUpdateInfo;
			}
			set
			{
				this.tutorialsUpdateInfo = value;
			}
		}

		public static UpdateService Instance
		{
			get
			{
				if (UpdateService.instance == null)
				{
					UpdateService.instance = new UpdateService();
				}
				return UpdateService.instance;
			}
		}

		public UpdateService()
		{
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			this.Load();
			if (this.updateLocalInfo == null || this.updateHttpsInfo == null)
			{
				return;
			}
			this.StoreUpdateInfo = new UpdateInfo(this.updateLocalInfo.ToolsTime, this.updateHttpsInfo.ToolsTime);
			this.TutorialsUpdateInfo = new UpdateInfo(this.updateLocalInfo.TutorialTime, this.updateHttpsInfo.TutorialTime);
			if (this.UpdateChanged != null)
			{
				this.UpdateChanged(this, e);
			}
		}

		private void Load()
		{
			try
			{
				if (File.Exists(ConstantConfig.Paths.UpdateIdentifyXmlPath))
				{
					using (FileStream fileStream = File.Open(ConstantConfig.Paths.UpdateIdentifyXmlPath, FileMode.Open))
					{
						this.updateLocalInfo = XmlHelper.XMLAction<UpdateIdentify>.ReadData(fileStream);
						goto IL_45;
					}
				}
				this.updateLocalInfo = new UpdateIdentify();
				IL_45:
				using (Stream responseOfStream = ConstantConfig.Constant.UpdateIdentifyXmlUrl.GetResponseOfStream("get", ""))
				{
					this.updateHttpsInfo = XmlHelper.XMLAction<UpdateIdentify>.ReadData(responseOfStream);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("小红点更新失败：" + ex.ToString());
			}
		}

		public void Save()
		{
			this.updateLocalInfo.ToolsTime = this.StoreUpdateInfo.LocalTime;
			this.updateLocalInfo.TutorialTime = this.TutorialsUpdateInfo.LocalTime;
			XmlHelper.SaveXml(ConstantConfig.Paths.UpdateIdentifyXmlPath, this.updateLocalInfo);
		}

		private UpdateInfo storeUpdateInfo;

		private UpdateInfo tutorialsUpdateInfo;

		private UpdateIdentify updateLocalInfo;

		private UpdateIdentify updateHttpsInfo;

		private static UpdateService instance;
	}
}
