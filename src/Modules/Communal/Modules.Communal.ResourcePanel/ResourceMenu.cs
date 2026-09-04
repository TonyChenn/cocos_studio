using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.UserStatistics;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002F RID: 47
	public class ResourceMenu
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00009E4C File Offset: 0x0000804C
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00009E53 File Offset: 0x00008053
		public static Menu ContextMenu { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009E5B File Offset: 0x0000805B
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00009E62 File Offset: 0x00008062
		public static Menu AdditionMenu { get; private set; }

		// Token: 0x060001C9 RID: 457 RVA: 0x00009E6A File Offset: 0x0000806A
		static ResourceMenu()
		{
			ResourceMenu.InitMenu();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009E74 File Offset: 0x00008074
		public static void InitMenu()
		{
			if (ResourceMenu.ContextMenu == null)
			{
				CommandEntrySet ce = Services.CommandService.CreateCommandEntrySet("/CocoStudio/Ide/ResourcePanel/Menu/Context");
				ResourceMenu.ContextMenu = (MenuCreator.CreateMenuItem(ce).Submenu as Menu);
				(ResourceMenu.ContextMenu as CommandMenu).CommandSource = ViewRegions.ResourcePanel;
			}
			if (ResourceMenu.AdditionMenu == null)
			{
				CommandEntrySet ce2 = Services.CommandService.CreateCommandEntrySet("/CocoStudio/Ide/ResourcePanel/Menu/Addition");
				ResourceMenu.AdditionMenu = (MenuCreator.CreateMenuItem(ce2).Submenu as Menu);
				(ResourceMenu.AdditionMenu as CommandMenu).CommandSource = ViewRegions.ResourcePanelAdditionMenu;
			}
		}
	}
}
