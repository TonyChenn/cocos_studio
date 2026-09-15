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
	public class AdvertView : HBox
	{
		public AdvertView()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.Initialize();
				return;
			}
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

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

		private void DownLoadImage(string imageUrl, string targetPath)
		{
			if (this.DeleteFile(targetPath))
			{
				HttpDownload httpDownload = new HttpDownload(imageUrl, targetPath);
				httpDownload.DownloadFinished += this.picdown_HttpDownLoadEndInfoEvent;
				httpDownload.StartDownloadAsync();
			}
		}

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

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (e.IsNetworkingSuccessed)
			{
				this.Initialize();
				CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
			}
		}

		private void linkButton_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			WebHelper.OnOpenWeb(sender, e);
			Tracker.Add(ViewRegions.None, "News", "", "");
		}

		private Advert m_Model;
	}
}
