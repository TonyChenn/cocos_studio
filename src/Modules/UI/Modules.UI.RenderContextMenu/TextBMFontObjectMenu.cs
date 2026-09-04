using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000B RID: 11
	[Extension(typeof(ICustomMenu))]
	public class TextBMFontObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002CF8 File Offset: 0x00000EF8
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002D10 File Offset: 0x00000F10
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

		// Token: 0x0600003A RID: 58 RVA: 0x00002D64 File Offset: 0x00000F64
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D7C File Offset: 0x00000F7C
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"fnt"
			};
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "LabelBMFontFile_CNB", LanguageInfo.ContexMenu_SetCustomFontRec);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002DEC File Offset: 0x00000FEC
		public override Type GetObjectType()
		{
			return typeof(TextBMFontObject);
		}

		// Token: 0x0400001C RID: 28
		private SetStyleMenuItem menuItemSetStyle;

		// Token: 0x0400001D RID: 29
		private TextEditMenuItem menuItemEditText;

		// Token: 0x0400001E RID: 30
		private VisualObject triggerbutton;
	}
}
