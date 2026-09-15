using System;
using Cocos.Launcher.Control;
using Gtk;

namespace Cocos.Launcher.Core
{
	public class TabGroupView : EventBox
	{
		internal TabGroupView()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
			this.fullBox = new VBox();
			this.fullBox.WidthRequest = 140;
			this.fullBox.Spacing = 10;
			base.Add(this.fullBox);
			base.ShowAll();
		}

		public ITabHead AddTab(ITabContent tabContent)
		{
			TabHeadView tabHeadView = new TabHeadView();
			this.fullBox.PackStart(tabHeadView, false, false, 0U);
			this.fullBox.ShowAll();
			return tabHeadView;
		}

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

		private VBox fullBox;
	}
}
