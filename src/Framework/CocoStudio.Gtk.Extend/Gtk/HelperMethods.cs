using System;
using Cairo;

namespace Gtk
{
	public static class HelperMethods
	{
		public static void SetSourceColor(this Context cr, Color color)
		{
			cr.SetSourceRGBA(color.R, color.G, color.B, color.A);
		}
	}
}
