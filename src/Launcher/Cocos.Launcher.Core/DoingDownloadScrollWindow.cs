using System;
using System.Collections.Generic;
using Cocos.Launcher.Control;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000027 RID: 39
	public class DoingDownloadScrollWindow : ScrolledWindow
	{
		// Token: 0x06000164 RID: 356 RVA: 0x00007EB8 File Offset: 0x000060B8
		public DoingDownloadScrollWindow(List<Widget> downloadList)
		{
			this.Initialize(downloadList);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007EC7 File Offset: 0x000060C7
		private void Initialize(List<Widget> downloadList)
		{
			this.InitWidget();
			this.InitEvent();
			this.InitValue(downloadList);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007EDC File Offset: 0x000060DC
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

		// Token: 0x06000167 RID: 359 RVA: 0x00008008 File Offset: 0x00006208
		private void InitEvent()
		{
			this.infoLink.LinkClicked += this.infoLink_LinkClicked;
			this.vbox_all.Removed += this.vbox_all_Removed;
			this.vbox_all.Added += this.vbox_all_Added;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000805C File Offset: 0x0000625C
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

		// Token: 0x06000169 RID: 361 RVA: 0x000080EC File Offset: 0x000062EC
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

		// Token: 0x0600016A RID: 362 RVA: 0x00008125 File Offset: 0x00006325
		private void SetPluginNumber(int num)
		{
			if (this.tabLink != null)
			{
				this.tabLink.SetLableText(LanguageInfo.Launcher_Downloading + "(" + num.ToString() + ")");
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008155 File Offset: 0x00006355
		public void SetTab(LinkView tab, ITabHead tabH = null)
		{
			this.tabLink = tab;
			this.tabHead = tabH;
			this.UpdateHint();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000816C File Offset: 0x0000636C
		public void AddItem(Widget widget)
		{
			this.vbox_all.Add(widget);
			widget.Destroyed += this.widget_Destroyed;
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_all[widget];
			boxChild.Expand = false;
			boxChild.Fill = false;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000081B8 File Offset: 0x000063B8
		private void widget_Destroyed(object sender, EventArgs e)
		{
			Widget widget = sender as Widget;
			widget.Destroyed -= this.widget_Destroyed;
			this.RemoveDoingDownloadItem(widget);
			widget.Dispose();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000081EC File Offset: 0x000063EC
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

		// Token: 0x0600016F RID: 367 RVA: 0x0000821C File Offset: 0x0000641C
		private void vbox_all_Added(object o, AddedArgs args)
		{
			if (this.vbox_all.Children.Length > 0 && this.hbox_hint.Parent != null)
			{
				this.vbox_all.Remove(this.hbox_hint);
			}
			this.UpdateHint();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008252 File Offset: 0x00006452
		private void vbox_all_Removed(object o, RemovedArgs args)
		{
			if (this.vbox_all.Children.Length == 0 && this.hbox_hint.Parent == null)
			{
				this.vbox_all.PackStart(this.hbox_hint, false, false, 0U);
			}
			this.UpdateHint();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000828A File Offset: 0x0000648A
		private void infoLink_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			Services.TabGroupService.SwitchTab(new SwitchTabInfo(2));
		}

		// Token: 0x0400006E RID: 110
		private LinkView tabLink;

		// Token: 0x0400006F RID: 111
		private VBox vbox_all;

		// Token: 0x04000070 RID: 112
		private HBox hbox_hint;

		// Token: 0x04000071 RID: 113
		private LinkView infoLink;

		// Token: 0x04000072 RID: 114
		private ITabHead tabHead;
	}
}
