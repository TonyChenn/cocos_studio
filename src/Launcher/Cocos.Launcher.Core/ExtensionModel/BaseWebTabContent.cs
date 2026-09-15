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
	public abstract class BaseWebTabContent : BaseTabContent
	{
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

		public WebView WebView
		{
			get
			{
				return WebView.Instance;
			}
		}

		public BaseWebTabContent()
		{
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
			Services.MainWindow.ShowByTray += this.MainWindow_ShowClick;
		}

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

		protected override void OnDeactivated()
		{
			this.WebView.Deactivated();
			this.box.RemoveAll();
			this.WebView.WebNewWindow -= this.OnWebNewWindow;
			this.WebView.WebNavigating -= this.WebView_WebNavigating;
		}

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

		private void UpdateView()
		{
			if (this.tabHead.IsSelected)
			{
				base.Deactivated();
				base.Activated(this.switchTabInfo);
			}
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			this.UpdateView();
		}

		private void MainWindow_ShowClick(object sender, EventArgs e)
		{
			this.UpdateView();
		}

		protected virtual void OnWebNewWindow(object sender, WebNewWindowEventArgs e)
		{
			WebHelper.OpenWeb(e.Url);
		}

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

		private VBox box;

		protected ITabHead tabHead;

		protected NetWorkErrorView netWorkErrorView;

		protected string initialUrl;

		protected bool isMenuEnabled;

		protected UpdateInfo updateInfo;

		private SwitchTabInfo switchTabInfo;
	}
}
