using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000004 RID: 4
	[Extension(typeof(ICustomMenu))]
	public class ButtonObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002124 File Offset: 0x00000324
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000213C File Offset: 0x0000033C
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
					this.menuItemNormal.TriggerObject = this.triggerbutton;
					this.menuItemPressed.TriggerObject = this.triggerbutton;
					this.menuItemDisable.TriggerObject = this.triggerbutton;
					this.menuItemEditText.TriggerObject = this.triggerbutton;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021B4 File Offset: 0x000003B4
		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D4 File Offset: 0x000003D4
		private void UpdateCheckItemState()
		{
			IDisplayState displayState = this.TriggerButton as IDisplayState;
			if (displayState != null)
			{
				this.menuItemNormalStatus.Active = !displayState.DisplayState;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002210 File Offset: 0x00000410
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"png",
				"jpg"
			};
			this.menuItemNormal = new SetStyleMenuItem(filetype, "NormalFileData", LanguageInfo.Display_NormalState);
			this.menuItemPressed = new SetStyleMenuItem(filetype, "PressedFileData", LanguageInfo.Display_BtnDown);
			this.menuItemDisable = new SetStyleMenuItem(filetype, "DisabledFileData", LanguageInfo.Display_Disable);
			Menu menu = new Menu();
			menu.Add(this.menuItemNormal);
			menu.Add(this.menuItemPressed);
			menu.Add(this.menuItemDisable);
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
			MenuItem menuItem = new MenuItem(LanguageInfo.ContexMenu_setButtonStyle);
			menuItem.Submenu = menu;
			this.MenuItemList.Add(menuItem);
			this.MenuItemList.Add(new SeparatorMenuItem());
			this.menuItemNormalStatus = new CheckMenuItem(LanguageInfo.Display_MakeDisabled);
			this.menuItemNormalStatus.Toggled += this.menuItemNormalStatus_Click;
			this.MenuItemList.Add(this.menuItemNormalStatus);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000233C File Offset: 0x0000053C
		private void menuItemNormalStatus_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("status changed", null))
			{
				IDisplayState displayState = this.TriggerButton as IDisplayState;
				if (displayState != null)
				{
					displayState.DisplayState = !this.menuItemNormalStatus.Active;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023A8 File Offset: 0x000005A8
		public override Type GetObjectType()
		{
			return typeof(ButtonObject);
		}

		// Token: 0x04000004 RID: 4
		private VisualObject triggerbutton;

		// Token: 0x04000005 RID: 5
		private TextEditMenuItem menuItemEditText;

		// Token: 0x04000006 RID: 6
		private SetStyleMenuItem menuItemNormal;

		// Token: 0x04000007 RID: 7
		private SetStyleMenuItem menuItemPressed;

		// Token: 0x04000008 RID: 8
		private SetStyleMenuItem menuItemDisable;

		// Token: 0x04000009 RID: 9
		private CheckMenuItem menuItemNormalStatus;
	}
}
