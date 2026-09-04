using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu.Model
{
	// Token: 0x02000006 RID: 6
	[Extension(typeof(ICustomMenu))]
	public class GameMapObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000279C File Offset: 0x0000099C
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000027B4 File Offset: 0x000009B4
		public override VisualObject TriggerButton
		{
			get
			{
				return this.triggerbutton;
			}
			set
			{
				if (this.triggerbutton != value)
				{
					this.triggerbutton = value;
					this.menuItemSetStyle.TriggerObject = this.triggerbutton;
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027F8 File Offset: 0x000009F8
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002810 File Offset: 0x00000A10
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"tmx"
			};
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "FileData", LanguageInfo.ContexMenu_SetMapFile);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002860 File Offset: 0x00000A60
		public override Type GetObjectType()
		{
			return typeof(GameMapObject);
		}

		// Token: 0x04000013 RID: 19
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x04000014 RID: 20
		private VisualObject triggerbutton;
	}
}
