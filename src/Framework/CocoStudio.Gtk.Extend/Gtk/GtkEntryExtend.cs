using System;

namespace Gtk
{
	public static class GtkEntryExtend
	{
		public static void SelectAll(this Entry entry)
		{
			entry.SelectRegion(0, entry.Text.Length);
		}
	}
}
