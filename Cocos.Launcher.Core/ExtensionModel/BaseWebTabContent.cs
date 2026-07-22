using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Gtk.Web;

namespace Cocos.Launcher.Core.ExtensionModel
{
	// Token: 0x02000008 RID: 8
	public abstract class BaseWebTabContent : BaseTabContent
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000024B6 File Offset: 0x000006B6
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000024DD File Offset: 0x000006DD
		public override Widget Content
		{
			get
			{
				if (this.box == null)
				{
					this.box = new VBox();
					this.box.CanFocus = true;
				}
				return this.box;
			}
			protected set
			{
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000024DF File Offset: 0x000006DF
		public WebView WebView
		{
			get
			{
				return WebView.Instance;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024E6 File Offset: 0x000006E6
		public BaseWebTabContent()
		{
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
			Services.MainWindow.ShowByTray += this.MainWindow_ShowClick;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000251C File Offset: 0x0000071C
		protected override void OnActivated(SwitchTabInfo switchTabInfo)
		{
			this.switchTabInfo = switchTabInfo;
			this.AddChildContent();
			this.RemoveRed();
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.WebView.Activated(switchTabInfo.ParentWidget);
				string url = string.IsNullOrEmpty(switchTabInfo.Url) ? this.initialUrl : switchTabInfo.Url;
				this.WebView.Url = url;
				this.WebView.IsMenuEnabled = this.isMenuEnabled;
			}
			this.WebView.WebNavigating += this.WebView_WebNavigating;
			this.WebView.WebNewWindow += this.OnWebNewWindow;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025C4 File Offset: 0x000007C4
		protected override void OnDeactivated()
		{
			this.WebView.Deactivated();
			this.box.RemoveAll();
			this.WebView.WebNewWindow -= this.OnWebNewWindow;
			this.WebView.WebNavigating -= this.WebView_WebNavigating;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002618 File Offset: 0x00000818
		protected override void OnSearch(string url)
		{
			string postDataToString = UserJsonInfo.Instance.GetPostDataToString();
			if (postDataToString != null)
			{
				this.WebView.PostData(postDataToString);
			}
			this.WebView.Url = url;
			this.Content.HasFocus = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002658 File Offset: 0x00000858
		private void AddChildContent()
		{
			if (this.box == null)
			{
				return;
			}
			if (this.box.Children.Length > 0)
			{
				return;
			}
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.box.Add(this.WebView.GtkWidget);
			}
			else
			{
				this.box.Add(this.netWorkErrorView);
			}
			this.box.ShowAll();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000026BF File Offset: 0x000008BF
		private void RemoveRed()
		{
			if (this.tabHead.IsShowRed)
			{
				this.tabHead.IsShowRed = false;
				if (this.updateInfo != null)
				{
					this.updateInfo.Update();
					Services.UpdateService.Save();
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000026F7 File Offset: 0x000008F7
		private void UpdateView()
		{
			if (this.tabHead.IsSelected)
			{
				base.Deactivated();
				base.Activated(this.switchTabInfo);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002718 File Offset: 0x00000918
		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			this.UpdateView();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002720 File Offset: 0x00000920
		private void MainWindow_ShowClick(object sender, EventArgs e)
		{
			this.UpdateView();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002728 File Offset: 0x00000928
		protected virtual void OnWebNewWindow(object sender, WebNewWindowEventArgs e)
		{
			WebHelper.OpenWeb(e.Url);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002744 File Offset: 0x00000944
		private void WebView_WebNavigating(object sender, WebNavigatingEventArgs e)
		{
			try
			{
				string text = this.switchTabInfo.Url = e.Url;
				string[] source = text.Split(new char[]
				{
					'&'
				});
				List<string> list = (from a in source
				where a.Contains("goal=")
				select a).ToList<string>();
				if (list.Count > 0)
				{
					string value = list[0].Replace("goal=", "");
					DataType dataType = (DataType)Enum.Parse(typeof(DataType), value, true);
					if (dataType != (DataType)this.Order)
					{
						SwitchTabInfo switchTabInfo = new SwitchTabInfo();
						switchTabInfo.Order = (int)dataType;
						switchTabInfo.Url = text;
						Services.TabGroupService.SwitchTab(switchTabInfo);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("解析WebNavigating事件错误：", exception);
			}
		}

		// Token: 0x0400000C RID: 12
		private VBox box;

		// Token: 0x0400000D RID: 13
		protected ITabHead tabHead;

		// Token: 0x0400000E RID: 14
		protected NetWorkErrorView netWorkErrorView;

		// Token: 0x0400000F RID: 15
		protected string initialUrl;

		// Token: 0x04000010 RID: 16
		protected bool isMenuEnabled;

		// Token: 0x04000011 RID: 17
		protected UpdateInfo updateInfo;

		// Token: 0x04000012 RID: 18
		private SwitchTabInfo switchTabInfo;
	}
}
