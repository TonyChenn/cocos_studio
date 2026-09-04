using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000A RID: 10
	[Extension(typeof(ICustomMenu))]
	public class TextAtlasObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002BE0 File Offset: 0x00000DE0
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002BF8 File Offset: 0x00000DF8
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
					this.menuItemEditText.TriggerObject = this.triggerbutton;
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002C4C File Offset: 0x00000E4C
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C64 File Offset: 0x00000E64
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"png",
				"jpg"
			};
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "LabelAtlasFileImage_CNB", LanguageInfo.ContexMenu_SetLabelStyle);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CDC File Offset: 0x00000EDC
		public override Type GetObjectType()
		{
			return typeof(TextAtlasObject);
		}

		// Token: 0x04000019 RID: 25
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x0400001A RID: 26
		private TextEditMenuItem menuItemEditText;

		// Token: 0x0400001B RID: 27
		private VisualObject triggerbutton;
	}
}
