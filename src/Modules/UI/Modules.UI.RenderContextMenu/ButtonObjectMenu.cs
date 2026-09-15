using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class ButtonObjectMenu : NodeObjectMenu
	{
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

		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		private void UpdateCheckItemState()
		{
			IDisplayState displayState = this.TriggerButton as IDisplayState;
			if (displayState != null)
			{
				this.menuItemNormalStatus.Active = !displayState.DisplayState;
			}
		}

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

		public override Type GetObjectType()
		{
			return typeof(ButtonObject);
		}

		private VisualObject triggerbutton;

		private TextEditMenuItem menuItemEditText;

		private SetStyleMenuItem menuItemNormal;

		private SetStyleMenuItem menuItemPressed;

		private SetStyleMenuItem menuItemDisable;

		private CheckMenuItem menuItemNormalStatus;
	}
}
