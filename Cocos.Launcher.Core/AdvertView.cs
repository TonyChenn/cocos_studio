using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using Gtk;
using Xwt.Drawing;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000052 RID: 82
	public class AdvertView : HBox
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000ADF6 File Offset: 0x00008FF6
		public AdvertView()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.Initialize();
				return;
			}
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000AE28 File Offset: 0x00009028
		private void Initialize()
		{
			Advert advertInfo = this.GetAdvertInfo();
			if (advertInfo == null)
			{
				return;
			}
			this.m_Model = advertInfo;
			string targetPath = System.IO.Path.Combine(ConstantConfig.Paths.DownloadDemoPath, "AdvertImage.png");
			this.DownLoadImage(this.m_Model.ImageUrl, targetPath);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000AE70 File Offset: 0x00009070
		private void DownLoadImage(string imageUrl, string targetPath)
		{
			if (this.DeleteFile(targetPath))
			{
				HttpDownload httpDownload = new HttpDownload(imageUrl, targetPath);
				httpDownload.DownloadFinished += this.picdown_HttpDownLoadEndInfoEvent;
				httpDownload.StartDownloadAsync();
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000AEA8 File Offset: 0x000090A8
		private void picdown_HttpDownLoadEndInfoEvent(object sender, DownloadFinishedEventArgs e)
		{
			HttpDownload httpDownload = sender as HttpDownload;
			httpDownload.DownloadFinished -= this.picdown_HttpDownLoadEndInfoEvent;
			string downloadPath = e.DownloadPath;
			if (!e.IsSuccessed && File.Exists(downloadPath))
			{
				this.DeleteFile(downloadPath);
				return;
			}
			this.CreateImageLink(downloadPath);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000AEF8 File Offset: 0x000090F8
		private void CreateImageLink(string imagePath)
		{
			LinkView linkView;
			try
			{
				Xwt.Drawing.Image iconFromFile = ImageIcon.GetIconFromFile(imagePath);
				linkView = new LinkView(iconFromFile, 600, 50);
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("新闻图片加载失败：" + arg);
				return;
			}
			linkView.WidthRequest = 600;
			linkView.HeightRequest = 50;
			linkView.SetTag(this.m_Model.Url);
			linkView.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainTitleColor);
			linkView.LinkClicked += this.linkButton_LinkClicked;
			VBox vbox = new VBox();
			base.PackStart(vbox, true, false, 0U);
			vbox.PackStart(linkView, true, false, 0U);
			base.ShowAll();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000AFAC File Offset: 0x000091AC
		private Advert GetAdvertInfo()
		{
			Advert result = null;
			try
			{
				using (Stream responseOfStream = ConstantConfig.Constant.AdvertisementUrl.GetResponseOfStream("get", ""))
				{
					result = XmlHelper.XMLAction<Advert>.ReadData(responseOfStream);
				}
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("解析新闻xml:" + arg);
			}
			return result;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000B020 File Offset: 0x00009220
		private bool DeleteFile(string filePath)
		{
			bool result = true;
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}
			catch (Exception arg)
			{
				result = false;
				LogConfig.Logger.Error("删除文件失败:" + arg);
			}
			return result;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B06C File Offset: 0x0000926C
		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (e.IsNetworkingSuccessed)
			{
				this.Initialize();
				CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000B092 File Offset: 0x00009292
		private void linkButton_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			WebHelper.OnOpenWeb(sender, e);
			Tracker.Add(ViewRegions.None, "News", "", "");
		}

		// Token: 0x04000107 RID: 263
		private Advert m_Model;
	}
}
