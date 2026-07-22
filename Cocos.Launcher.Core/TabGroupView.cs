using System;
using Cocos.Launcher.Control;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200004F RID: 79
	public class TabGroupView : EventBox
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0000A781 File Offset: 0x00008981
		internal TabGroupView()
		{
			this.Initialize();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A790 File Offset: 0x00008990
		private void Initialize()
		{
			base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
			this.fullBox = new VBox();
			this.fullBox.WidthRequest = 140;
			this.fullBox.Spacing = 10;
			base.Add(this.fullBox);
			base.ShowAll();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A7E8 File Offset: 0x000089E8
		public ITabHead AddTab(ITabContent tabContent)
		{
			TabHeadView tabHeadView = new TabHeadView();
			this.fullBox.PackStart(tabHeadView, false, false, 0U);
			this.fullBox.ShowAll();
			return tabHeadView;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A818 File Offset: 0x00008A18
		public void SwitchTab(ITabHead oldTabHead, ITabHead newTabHead)
		{
			TabHeadView tabHeadView = oldTabHead as TabHeadView;
			if (tabHeadView != null)
			{
				tabHeadView.IsSelected = false;
			}
			tabHeadView = (newTabHead as TabHeadView);
			if (tabHeadView != null)
			{
				tabHeadView.IsSelected = true;
			}
		}

		// Token: 0x04000100 RID: 256
		private VBox fullBox;
	}
}
