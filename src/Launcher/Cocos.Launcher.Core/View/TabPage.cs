using System;

namespace Cocos.Launcher.Core.View
{
	public class TabPage
	{
		public ITabContent TabContent { get; private set; }

		public ITabHead TabHead { get; private set; }

		public int Order
		{
			get
			{
				return this.TabContent.Order;
			}
		}

		public TabPage(ITabContent content, ITabHead head)
		{
			this.TabContent = content;
			this.TabHead = head;
		}
	}
}
