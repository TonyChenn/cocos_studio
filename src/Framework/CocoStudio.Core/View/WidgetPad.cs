using System;
using Gtk;

namespace CocoStudio.Core.View
{
	// Token: 0x02000055 RID: 85
	public static class WidgetPad
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0000ECDC File Offset: 0x0000CEDC
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
