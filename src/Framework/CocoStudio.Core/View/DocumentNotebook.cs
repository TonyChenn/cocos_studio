using System;
using Gdk;
using MonoDevelop.Components.DockNotebook;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	// Token: 0x02000043 RID: 67
	internal class DocumentNotebook : SdiDragNotebook
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000A34C File Offset: 0x0000854C
		public DocumentNotebook(MainWindow mainWindow)
		{
			base.NavigationButtonsVisible = false;
			base.SwitchPage += mainWindow.OnActiveWindowChanged;
			base.PageRemoved += mainWindow.OnActiveWindowChanged;
			base.PageAdded += mainWindow.OnActiveWindowChanged;
			base.TabClosed += mainWindow.CloseClicked;
			base.TabActivated += delegate(object sender, TabEventArgs e)
			{
				mainWindow.ToggleFullViewMode();
			};
			base.DoPopupMenu = new Action<DockNotebook, int, EventButton>(mainWindow.ShowPopup);
			base.TabsReordered += mainWindow.OnTabsReordered;
		}
	}
}
