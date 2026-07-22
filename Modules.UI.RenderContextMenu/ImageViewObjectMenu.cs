using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000007 RID: 7
	[Extension(typeof(ICustomMenu))]
	public class ImageViewObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000287C File Offset: 0x00000A7C
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002894 File Offset: 0x00000A94
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

		// Token: 0x06000024 RID: 36 RVA: 0x000028D8 File Offset: 0x00000AD8
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000028F0 File Offset: 0x00000AF0
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

		// Token: 0x06000026 RID: 38 RVA: 0x00002948 File Offset: 0x00000B48
		public override Type GetObjectType()
		{
			return typeof(ImageViewObject);
		}

		// Token: 0x04000015 RID: 21
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x04000016 RID: 22
		private VisualObject triggerbutton;
	}
}
