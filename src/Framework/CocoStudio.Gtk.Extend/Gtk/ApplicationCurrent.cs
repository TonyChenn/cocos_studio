using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000002 RID: 2
	public class ApplicationCurrent
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002067 File Offset: 0x00000267
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

		// Token: 0x04000001 RID: 1
		private static Window mainWindow;
	}
}
