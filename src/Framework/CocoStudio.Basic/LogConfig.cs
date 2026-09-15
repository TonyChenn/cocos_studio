using System;

namespace CocoStudio.Basic
{
	public static class LogConfig
	{
		public static ICSLog Logger { get; private set; } = new CSLogger(false);

		public static ICSLog Output { get; private set; } = new CSLogger(true);

		public static ICSLog OutputWithoutTip { get; private set; } = new CSLogger(true);

		public static ICSLog TipWithOutConsole { get; private set; } = new CSLogger(false);

		public static bool IsLogActivated { get; set; } = true;
	}
}
