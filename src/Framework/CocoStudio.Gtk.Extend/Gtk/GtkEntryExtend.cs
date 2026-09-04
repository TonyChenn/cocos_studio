using System;

namespace Gtk
{
	// Token: 0x0200001E RID: 30
	public static class GtkEntryExtend
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00005A47 File Offset: 0x00003C47
		public static void SelectAll(this Entry entry)
		{
			entry.SelectRegion(0, entry.Text.Length);
		}
	}
}
