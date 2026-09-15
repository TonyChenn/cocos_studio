using System;
using Gtk;

namespace Stetic
{
	internal class Gui
	{
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		private static bool initialized;
	}
}
