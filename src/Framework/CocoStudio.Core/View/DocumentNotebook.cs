using System;
using Gdk;
using MonoDevelop.Components.DockNotebook;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	internal class DocumentNotebook : SdiDragNotebook
	{
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
