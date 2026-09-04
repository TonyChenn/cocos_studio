using System;

namespace Gtk
{
	// Token: 0x0200005E RID: 94
	public static class EntryShellBuilder
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x00008EC4 File Offset: 0x000070C4
		public static EntryShell CreateShell(Entry entry)
		{
			return new EntryShell(entry);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008EE0 File Offset: 0x000070E0
		public static FullEntryShell CreateShell(string title, Entry entry)
		{
			return EntryShellBuilder.CreateShell(title, entry, null, null);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008EFC File Offset: 0x000070FC
		public static FullEntryShell CreateShell(Entry entry, string unit)
		{
			return EntryShellBuilder.CreateShell(null, entry, unit, null);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008F18 File Offset: 0x00007118
		public static FullEntryShell CreateShell(string title, Entry entry, string unit)
		{
			return EntryShellBuilder.CreateShell(title, entry, unit, null);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008F34 File Offset: 0x00007134
		public static FullEntryShell CreateShell(Entry entry, Widget widget)
		{
			return EntryShellBuilder.CreateShell(null, entry, null, widget);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00008F50 File Offset: 0x00007150
		public static FullEntryShell CreateShell(string title, Entry entry, string unit, Widget widget)
		{
			return new FullEntryShell(title, entry, unit, widget);
		}
	}
}
