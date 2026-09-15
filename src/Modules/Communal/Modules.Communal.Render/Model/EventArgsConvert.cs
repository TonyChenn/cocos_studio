using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;

namespace Modules.Communal.Render.Model
{
	public static class EventArgsConvert
	{
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

		public static MouseEventArgs ToMouseEvent(EventCrossing e, ICoordinateMapping coordinateMapping)
		{
			PointF point = new PointF((float)e.X, (float)e.Y);
			if (coordinateMapping != null)
			{
				point = coordinateMapping.ConvertCoordinate(point);
			}
			return new MouseEventArgs(point, MouseButton.None, null);
		}

		public static bool CheckRetval(this SignalArgs args)
		{
			return args.RetVal is bool && (bool)args.RetVal;
		}
	}
}
