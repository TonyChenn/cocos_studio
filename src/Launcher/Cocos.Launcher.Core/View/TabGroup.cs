using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;

namespace Cocos.Launcher.Core.View
{
	internal class TabGroup : ITabGroup
	{
		public Widget Content
		{
			get
			{
				return this.tabGroupView;
			}
		}

		public TabPage LastSelectedTabPage
		{
			get
			{
				return this.lastSelectedTabPage;
			}
		}

		public event EventHandler<EventArgs> SelectedTabChanged;

		public TabGroup(ITabMain tabMain)
		{
			this.tabMain = tabMain;
			this.tabGroupView = new TabGroupView();
			this.Initialize();
		}

		private void Initialize()
		{
			this.tabPageList = new List<TabPage>();
			IEnumerable<ITabContent> tabContents = TabContentManager.GetTabContents();
			foreach (ITabContent tabContent in tabContents)
			{
				this.AddTab(tabContent);
			}
		}

		public void SwitchTab(SwitchTabInfo switchTabInfo)
		{
			if (this.tabPageList.Count > switchTabInfo.Order)
			{
				TabPage tabPage = this.tabPageList[switchTabInfo.Order];
				this.SwitchTab(tabPage, switchTabInfo);
			}
		}

		private void SwitchTab(TabPage tabPage, SwitchTabInfo switchTabInfo)
		{
			this.SwitchTabHead(tabPage);
			this.SwitchTabContent(tabPage, switchTabInfo);
			this.lastSelectedTabPage = tabPage;
			if (this.SelectedTabChanged != null)
			{
				this.SelectedTabChanged(this, null);
			}
		}

		private void SwitchTabContent(TabPage tabPage, SwitchTabInfo switchTabInfo)
		{
			if (this.lastSelectedTabPage != null && this.lastSelectedTabPage.TabContent != null)
			{
				this.lastSelectedTabPage.TabContent.Deactivated();
			}
			this.tabMain.ChangeTabContent(tabPage.TabContent);
			if (tabPage != null && tabPage.TabContent != null)
			{
				switchTabInfo.ParentWidget = this.tabMain.ContainerWidget;
				tabPage.TabContent.Activated(switchTabInfo);
			}
		}

		private void SwitchTabHead(TabPage tabPage)
		{
			ITabHead oldTabHead = null;
			if (this.lastSelectedTabPage != null)
			{
				oldTabHead = this.lastSelectedTabPage.TabHead;
			}
			this.tabGroupView.SwitchTab(oldTabHead, tabPage.TabHead);
		}

		public void AddTab(ITabContent tabContent)
		{
			ITabHead tabHead = this.tabGroupView.AddTab(tabContent);
			tabHead.SelectedChanging += this.TabHead_SelectedChanged;
			tabContent.Initialize(tabHead);
			TabPage item = new TabPage(tabContent, tabHead);
			this.tabPageList.Add(item);
		}

		private void TabHead_SelectedChanged(object sender, SelectedChangingEventArgs e)
		{
			TabPage tabPage = this.tabPageList.FirstOrDefault((TabPage a) => a.TabHead == sender);
			this.SwitchTab(tabPage, new SwitchTabInfo());
		}

		private TabGroupView tabGroupView;

		private TabPage lastSelectedTabPage;

		private ITabMain tabMain;

		private List<TabPage> tabPageList;
	}
}
