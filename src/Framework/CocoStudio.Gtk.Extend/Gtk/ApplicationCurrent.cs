using System;
using Gdk;

namespace Gtk
{
	public class ApplicationCurrent
	{
		public static Window MainWindow
		{
			get
			{
				return ApplicationCurrent.mainWindow;
			}
			set
			{
				ApplicationCurrent.mainWindow = value;
				KeyboardExtend.MainWindow = value;
			}
		}

		private static Window mainWindow;
	}
}
