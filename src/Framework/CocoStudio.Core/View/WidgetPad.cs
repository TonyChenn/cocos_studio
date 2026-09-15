using System;
using Gtk;

namespace CocoStudio.Core.View
{
	public static class WidgetPad
	{
		public static Widget CurrentWidget(this Pad pad)
		{
			Widget result;
			if (pad == null || pad.Window == null || pad.Window.Content == null)
			{
				result = null;
			}
			else
			{
				result = pad.Window.Content.Control;
			}
			return result;
		}
	}
}
