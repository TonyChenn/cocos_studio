using System;

namespace Gtk
{
	public static class EntryShellBuilder
	{
		public static EntryShell CreateShell(Entry entry)
		{
			return new EntryShell(entry);
		}

		public static FullEntryShell CreateShell(string title, Entry entry)
		{
			return EntryShellBuilder.CreateShell(title, entry, null, null);
		}

		public static FullEntryShell CreateShell(Entry entry, string unit)
		{
			return EntryShellBuilder.CreateShell(null, entry, unit, null);
		}

		public static FullEntryShell CreateShell(string title, Entry entry, string unit)
		{
			return EntryShellBuilder.CreateShell(title, entry, unit, null);
		}

		public static FullEntryShell CreateShell(Entry entry, Widget widget)
		{
			return EntryShellBuilder.CreateShell(null, entry, null, widget);
		}

		public static FullEntryShell CreateShell(string title, Entry entry, string unit, Widget widget)
		{
			return new FullEntryShell(title, entry, unit, widget);
		}
	}
}
