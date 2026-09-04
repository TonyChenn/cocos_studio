using System;
using CocoStudio.Model;
using Gdk;

namespace Gtk
{
	// Token: 0x02000081 RID: 129
	public static class GtkEventArgsExtend
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000B964 File Offset: 0x00009B64
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

		// Token: 0x060002D9 RID: 729 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		public static MouseButton GetMouseButton(this EventButton eventButton)
		{
			return (MouseButton)eventButton.Button;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000BABC File Offset: 0x00009CBC
		public static PointF GetPoint(this EventButton eventButton)
		{
			return new PointF((float)eventButton.X, (float)eventButton.Y);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public static MouseButton GetMouseButton(this EventMotion args)
		{
			return args.State.GetMouseButton();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000BB04 File Offset: 0x00009D04
		public static PointF GetPoint(this EventMotion eventMotion)
		{
			return new PointF((float)eventMotion.X, (float)eventMotion.Y);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000BB2C File Offset: 0x00009D2C
		public static PointF GetPoint(this EventScroll eventScroll)
		{
			return new PointF((float)eventScroll.X, (float)eventScroll.Y);
		}

		// Token: 0x04000358 RID: 856
		private static uint? firstClickTime = null;

		// Token: 0x04000359 RID: 857
		private static double x = -1.0;

		// Token: 0x0400035A RID: 858
		private static double y = -1.0;
	}
}
