using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	public class SwitchTabInfo
	{
		public int Order { get; set; }

		public string Url { get; set; }

		public Widget ParentWidget { get; set; }

		public SwitchTabInfo()
		{
		}

		public SwitchTabInfo(int order)
		{
			this.Order = order;
		}
	}
}
