using System;
using CocoStudio.Model;
using Gdk;

namespace Gtk
{
	public static class GtkEventArgsExtend
	{
		public static bool IsDoubleClick(this EventButton eventbutton, uint buttontype = 1U)
		{
			bool result = false;
			if (eventbutton.Button == buttontype)
			{
				if (GtkEventArgsExtend.firstClickTime == null)
				{
					GtkEventArgsExtend.firstClickTime = new uint?(eventbutton.Time);
					GtkEventArgsExtend.x = eventbutton.X;
					GtkEventArgsExtend.y = eventbutton.Y;
				}
				else
				{
					uint time = eventbutton.Time;
					if (time - GtkEventArgsExtend.firstClickTime < 500U && Math.Abs(eventbutton.X - GtkEventArgsExtend.x) < 0.3 && Math.Abs(eventbutton.Y - GtkEventArgsExtend.y) < 0.3)
					{
						result = true;
						GtkEventArgsExtend.firstClickTime = null;
					}
					else
					{
						GtkEventArgsExtend.firstClickTime = new uint?(time);
						GtkEventArgsExtend.x = eventbutton.X;
						GtkEventArgsExtend.y = eventbutton.Y;
					}
				}
			}
			else
			{
				GtkEventArgsExtend.firstClickTime = null;
			}
			return result;
		}

		public static MouseButton GetMouseButton(this EventButton eventButton)
		{
			return (MouseButton)eventButton.Button;
		}

		public static PointF GetPoint(this EventButton eventButton)
		{
			return new PointF((float)eventButton.X, (float)eventButton.Y);
		}

		public static MouseButton GetMouseButton(this EventMotion args)
		{
			return args.State.GetMouseButton();
		}

		public static PointF GetPoint(this EventMotion eventMotion)
		{
			return new PointF((float)eventMotion.X, (float)eventMotion.Y);
		}

		public static PointF GetPoint(this EventScroll eventScroll)
		{
			return new PointF((float)eventScroll.X, (float)eventScroll.Y);
		}

		private static uint? firstClickTime = null;

		private static double x = -1.0;

		private static double y = -1.0;
	}
}
