using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	public class RadioItem
	{
		public string ID { get; private set; }

		public string DisplayName { get; private set; }

		public string Tooltip { get; private set; }

		public XwtImage Icon { get; private set; }

		public RadioItem(string id, string displayName, XwtImage icon, string tooltip = null)
		{
			this.ID = id;
			this.DisplayName = displayName;
			this.Icon = icon;
			this.Tooltip = (tooltip ?? string.Empty);
		}
	}
}
