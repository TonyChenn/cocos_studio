using System;
using GLib;

namespace CocoStudio.EngineAdapterWrap
{
	public class GtkInvokeHelp
	{
		public static void BeginInvoke(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			Timeout.Add(500U, delegate
			{
				action();
				return false;
			});
		}

		private const int timeSpan = 500;
	}
}
