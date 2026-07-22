using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000D RID: 13
	[Extension(typeof(ICustomMenu))]
	public class TextObjectMenu : NodeObjectMenu
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002ED8 File Offset: 0x000010D8
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
					this.menuItemEditText.TriggerObject = this.triggerbutton;
				}
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F1C File Offset: 0x0000111C
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002F34 File Offset: 0x00001134
		protected override void InitMenu()
		{
			base.InitMenu();
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002F60 File Offset: 0x00001160
		public override Type GetObjectType()
		{
			return typeof(TextObject);
		}

		// Token: 0x04000020 RID: 32
		private TextEditMenuItem menuItemEditText;

		// Token: 0x04000021 RID: 33
		private VisualObject triggerbutton;
	}
}
