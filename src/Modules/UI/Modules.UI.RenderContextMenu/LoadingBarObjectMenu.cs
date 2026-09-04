using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000F RID: 15
	[Extension(typeof(ICustomMenu))]
	public class LoadingBarObjectMenu : NodeObjectMenu
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003004 File Offset: 0x00001204
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000301C File Offset: 0x0000121C
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

		// Token: 0x06000052 RID: 82 RVA: 0x00003060 File Offset: 0x00001260
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003078 File Offset: 0x00001278
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"png",
				"jpg"
			};
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "ImageFileData", LanguageInfo.ContexMenu_SetLoadingBarStyle);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030D0 File Offset: 0x000012D0
		public override Type GetObjectType()
		{
			return typeof(LoadingBarObject);
		}

		// Token: 0x04000023 RID: 35
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x04000024 RID: 36
		private VisualObject triggerbutton;
	}
}
