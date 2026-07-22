using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000026 RID: 38
	[Extension(typeof(ICustomMenu))]
	public class SpriteObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000726C File Offset: 0x0000546C
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00007284 File Offset: 0x00005484
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

		// Token: 0x06000120 RID: 288 RVA: 0x000072C8 File Offset: 0x000054C8
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000072E0 File Offset: 0x000054E0
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"png",
				"jpg"
			};
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "FileData", LanguageInfo.ContexMenu_SetImageResources);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007338 File Offset: 0x00005538
		public override Type GetObjectType()
		{
			return typeof(SpriteObject);
		}

		// Token: 0x04000090 RID: 144
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x04000091 RID: 145
		private VisualObject triggerbutton;
	}
}
