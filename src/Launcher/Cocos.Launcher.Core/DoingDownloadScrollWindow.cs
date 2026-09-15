using System;
using System.Collections.Generic;
using Cocos.Launcher.Control;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class DoingDownloadScrollWindow : ScrolledWindow
	{
		public DoingDownloadScrollWindow(List<Widget> downloadList)
		{
			this.Initialize(downloadList);
		}

		private void Initialize(List<Widget> downloadList)
		{
			this.InitWidget();
			this.InitEvent();
			this.InitValue(downloadList);
		}

		private void InitWidget()
		{
			base.Name = "GtkScrolledWindow";
			base.HeightRequest = 500;
			base.ShadowType = ShadowType.None;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			this.vbox_all = new VBox();
			this.vbox_all.Spacing = 30;
			viewport.Add(this.vbox_all);
			base.Add(viewport);
			this.hbox_hint = new HBox();
			this.hbox_hint.Spacing = 0;
			Label label = new Label();
			label.Text = LanguageInfo.Launcher_DownloadHint1;
			label.HeightRequest = 30;
			label.Xalign = 1f;
			label.Yalign = 1f;
			label.SetFontSize(14.0);
			this.infoLink = new LinkView();
			this.infoLink.SetLinkText(LanguageInfo.Launcher_DownloadHint2);
			this.infoLink.SetLableTextHeight(30);
			this.infoLink.SetLabelAlign(0f, 1f);
			this.infoLink.SetFontSize(14.0);
			this.hbox_hint.PackStart(label, false, false, 0U);
			this.hbox_hint.PackStart(this.infoLink, false, false, 0U);
		}

		private void InitEvent()
		{
			this.infoLink.LinkClicked += this.infoLink_LinkClicked;
			this.vbox_all.Removed += this.vbox_all_Removed;
			this.vbox_all.Added += this.vbox_all_Added;
		}

		private void InitValue(List<Widget> downloadList)
		{
			foreach (Widget widget in downloadList)
			{
				this.vbox_all.Add(widget);
				Box.BoxChild boxChild = (Box.BoxChild)this.vbox_all[widget];
				boxChild.Expand = false;
				boxChild.Fill = false;
			}
			if (downloadList.Count == 0)
			{
				this.vbox_all.PackStart(this.hbox_hint, false, false, 0U);
			}
		}

		public void UpdateHint()
		{
			int pluginNumber;
			if (this.hbox_hint.Parent != null)
			{
				pluginNumber = 0;
			}
			else
			{
				pluginNumber = this.vbox_all.Children.Length;
			}
			this.SetPluginNumber(pluginNumber);
			base.ShowAll();
		}

		private void SetPluginNumber(int num)
		{
			if (this.tabLink != null)
			{
				this.tabLink.SetLableText(LanguageInfo.Launcher_Downloading + "(" + num.ToString() + ")");
			}
		}

		public void SetTab(LinkView tab, ITabHead tabH = null)
		{
			this.tabLink = tab;
			this.tabHead = tabH;
			this.UpdateHint();
		}

		public void AddItem(Widget widget)
		{
			this.vbox_all.Add(widget);
			widget.Destroyed += this.widget_Destroyed;
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_all[widget];
			boxChild.Expand = false;
			boxChild.Fill = false;
		}

		private void widget_Destroyed(object sender, EventArgs e)
		{
			Widget widget = sender as Widget;
			widget.Destroyed -= this.widget_Destroyed;
			this.RemoveDoingDownloadItem(widget);
			widget.Dispose();
		}

		public void RemoveDoingDownloadItem(Widget widget)
		{
			try
			{
				this.vbox_all.Remove(widget);
			}
			catch (Exception)
			{
			}
		}

		private void vbox_all_Added(object o, AddedArgs args)
		{
			if (this.vbox_all.Children.Length > 0 && this.hbox_hint.Parent != null)
			{
				this.vbox_all.Remove(this.hbox_hint);
			}
			this.UpdateHint();
		}

		private void vbox_all_Removed(object o, RemovedArgs args)
		{
			if (this.vbox_all.Children.Length == 0 && this.hbox_hint.Parent == null)
			{
				this.vbox_all.PackStart(this.hbox_hint, false, false, 0U);
			}
			this.UpdateHint();
		}

		private void infoLink_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			Services.TabGroupService.SwitchTab(new SwitchTabInfo(2));
		}

		private LinkView tabLink;

		private VBox vbox_all;

		private HBox hbox_hint;

		private LinkView infoLink;

		private ITabHead tabHead;
	}
}
