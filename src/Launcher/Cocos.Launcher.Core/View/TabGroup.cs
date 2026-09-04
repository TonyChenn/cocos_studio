using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;

namespace Cocos.Launcher.Core.View
{
	// Token: 0x02000049 RID: 73
	internal class TabGroup : ITabGroup
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00009FCF File Offset: 0x000081CF
		public Widget Content
		{
			get
			{
				return this.tabGroupView;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00009FD7 File Offset: 0x000081D7
		public TabPage LastSelectedTabPage
		{
			get
			{
				return this.lastSelectedTabPage;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600026F RID: 623 RVA: 0x00009FE0 File Offset: 0x000081E0
		// (remove) Token: 0x06000270 RID: 624 RVA: 0x0000A018 File Offset: 0x00008218
		public event EventHandler<EventArgs> SelectedTabChanged;

		// Token: 0x06000271 RID: 625 RVA: 0x0000A04D File Offset: 0x0000824D
		public TabGroup(ITabMain tabMain)
		{
			this.tabMain = tabMain;
			this.tabGroupView = new TabGroupView();
			this.Initialize();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A070 File Offset: 0x00008270
		private void Initialize()
		{
			this.tabPageList = new List<TabPage>();
			IEnumerable<ITabContent> tabContents = TabContentManager.GetTabContents();
			foreach (ITabContent tabContent in tabContents)
			{
				this.AddTab(tabContent);
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A0CC File Offset: 0x000082CC
		public void SwitchTab(SwitchTabInfo switchTabInfo)
		{
			if (this.tabPageList.Count > switchTabInfo.Order)
			{
				TabPage tabPage = this.tabPageList[switchTabInfo.Order];
				this.SwitchTab(tabPage, switchTabInfo);
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A106 File Offset: 0x00008306
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

		// Token: 0x06000275 RID: 629 RVA: 0x0000A134 File Offset: 0x00008334
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

		// Token: 0x06000276 RID: 630 RVA: 0x0000A1A0 File Offset: 0x000083A0
		private void SwitchTabHead(TabPage tabPage)
		{
			ITabHead oldTabHead = null;
			if (this.lastSelectedTabPage != null)
			{
				oldTabHead = this.lastSelectedTabPage.TabHead;
			}
			this.tabGroupView.SwitchTab(oldTabHead, tabPage.TabHead);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A1D8 File Offset: 0x000083D8
		public void AddTab(ITabContent tabContent)
		{
			ITabHead tabHead = this.tabGroupView.AddTab(tabContent);
			tabHead.SelectedChanging += this.TabHead_SelectedChanged;
			tabContent.Initialize(tabHead);
			TabPage item = new TabPage(tabContent, tabHead);
			this.tabPageList.Add(item);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A238 File Offset: 0x00008438
		private void TabHead_SelectedChanged(object sender, SelectedChangingEventArgs e)
		{
			TabPage tabPage = this.tabPageList.FirstOrDefault((TabPage a) => a.TabHead == sender);
			this.SwitchTab(tabPage, new SwitchTabInfo());
		}

		// Token: 0x040000EE RID: 238
		private TabGroupView tabGroupView;

		// Token: 0x040000F0 RID: 240
		private TabPage lastSelectedTabPage;

		// Token: 0x040000F1 RID: 241
		private ITabMain tabMain;

		// Token: 0x040000F2 RID: 242
		private List<TabPage> tabPageList;
	}
}
