using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003D RID: 61
	public class UpdateService : IUpdateService
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000211 RID: 529 RVA: 0x00009208 File Offset: 0x00007408
		// (remove) Token: 0x06000212 RID: 530 RVA: 0x00009240 File Offset: 0x00007440
		public event EventHandler<EventArgs> UpdateChanged;

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00009275 File Offset: 0x00007475
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000927D File Offset: 0x0000747D
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00009286 File Offset: 0x00007486
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000928E File Offset: 0x0000748E
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00009297 File Offset: 0x00007497
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

		// Token: 0x06000218 RID: 536 RVA: 0x000092AF File Offset: 0x000074AF
		public UpdateService()
		{
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000092D0 File Offset: 0x000074D0
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

		// Token: 0x0600021A RID: 538 RVA: 0x0000934C File Offset: 0x0000754C
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

		// Token: 0x0600021B RID: 539 RVA: 0x00009418 File Offset: 0x00007618
		public void Save()
		{
			this.updateLocalInfo.ToolsTime = this.StoreUpdateInfo.LocalTime;
			this.updateLocalInfo.TutorialTime = this.TutorialsUpdateInfo.LocalTime;
			XmlHelper.SaveXml(ConstantConfig.Paths.UpdateIdentifyXmlPath, this.updateLocalInfo);
		}

		// Token: 0x040000CF RID: 207
		private UpdateInfo storeUpdateInfo;

		// Token: 0x040000D0 RID: 208
		private UpdateInfo tutorialsUpdateInfo;

		// Token: 0x040000D1 RID: 209
		private UpdateIdentify updateLocalInfo;

		// Token: 0x040000D2 RID: 210
		private UpdateIdentify updateHttpsInfo;

		// Token: 0x040000D3 RID: 211
		private static UpdateService instance;
	}
}
