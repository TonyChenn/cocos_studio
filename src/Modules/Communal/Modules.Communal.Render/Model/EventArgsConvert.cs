using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000009 RID: 9
	public static class EventArgsConvert
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00005768 File Offset: 0x00003968
		public static MouseEventArgs ToMouseEvent(EventButton e, ICoordinateMapping coordinateMapping)
		{
			MouseButton mouseButton = e.GetMouseButton();
			PointF point = e.GetPoint();
			if (coordinateMapping != null)
			{
				point = coordinateMapping.ConvertCoordinate(point);
			}
			return new MouseEventArgs(point, mouseButton, null);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000057A0 File Offset: 0x000039A0
		public static MouseEventArgs ToMouseEvent(EventMotion e, ICoordinateMapping coordinateMapping)
		{
			MouseButton mouseButton = e.GetMouseButton();
			PointF point = e.GetPoint();
			if (coordinateMapping != null)
			{
				point = coordinateMapping.ConvertCoordinate(point);
			}
			return new MouseEventArgs(point, mouseButton, null);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000057D8 File Offset: 0x000039D8
		public static MouseEventArgs ToMouseEvent(EventCrossing e, ICoordinateMapping coordinateMapping)
		{
			PointF point = new PointF((float)e.X, (float)e.Y);
			if (coordinateMapping != null)
			{
				point = coordinateMapping.ConvertCoordinate(point);
			}
			return new MouseEventArgs(point, MouseButton.None, null);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005818 File Offset: 0x00003A18
		public static bool CheckRetval(this SignalArgs args)
		{
			return args.RetVal is bool && (bool)args.RetVal;
		}
	}
}
