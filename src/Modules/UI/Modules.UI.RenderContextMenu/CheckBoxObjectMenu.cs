using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class CheckBoxObjectMenu : NodeObjectMenu
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
					this.menuItemSelectedNormal.TriggerObject = this.triggerbutton;
					this.menuItemSelectedDisable.TriggerObject = this.triggerbutton;
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
			PropertyInfo property = this.TriggerButton.GetType().GetProperty("CheckedState");
			bool active = (bool)property.GetValue(this.TriggerButton, null);
			this.menuItemSelected.Active = active;
			IDisplayState displayState = this.TriggerButton as IDisplayState;
			if (displayState != null)
			{
				this.menuItemSetDisable.Active = !displayState.DisplayState;
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
			this.menuItemSelected = new CheckMenuItem(LanguageInfo.ContexMenu_selected);
			this.menuItemSelected.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.menuItemSelected_Click);
			this.menuItemNormal = new SetStyleMenuItem(filetype, "NormalBackFileData", LanguageInfo.ContexMenu_BackgroundNormal);
			this.menuItemPressed = new SetStyleMenuItem(filetype, "PressedBackFileData", LanguageInfo.ContexMenu_BackgroundPressed);
			this.menuItemDisable = new SetStyleMenuItem(filetype, "DisableBackFileData", LanguageInfo.ContexMenu_BackgroundDisabled);
			this.menuItemSelectedNormal = new SetStyleMenuItem(filetype, "NodeNormalFileData", LanguageInfo.ContexMenu_CheckNormal);
			this.menuItemSelectedDisable = new SetStyleMenuItem(filetype, "NodeDisableFileData", LanguageInfo.ContexMenu_CheckDisabled);
			Menu menu = new Menu();
			menu.Add(this.menuItemNormal);
			menu.Add(this.menuItemPressed);
			menu.Add(this.menuItemDisable);
			menu.Add(new SeparatorMenuItem());
			menu.Add(this.menuItemSelectedNormal);
			menu.Add(this.menuItemSelectedDisable);
			this.menuItemSetCheckBoxStyle = new MenuItem(LanguageInfo.ContexMenu_CheckboxStyle);
			this.menuItemSetCheckBoxStyle.Submenu = menu;
			this.menuItemSetDisable = new CheckMenuItem(LanguageInfo.Display_MakeDisabled);
			this.menuItemSetDisable.Toggled += this.menuItemSetDisable_Click;
			this.MenuItemList.Add(this.menuItemSelected);
			this.MenuItemList.Add(this.menuItemSetCheckBoxStyle);
			this.MenuItemList.Add(new SeparatorMenuItem());
			this.MenuItemList.Add(this.menuItemSetDisable);
		}

		private void menuItemSelected_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("play Particle", null))
			{
				PropertyInfo property = this.TriggerButton.GetType().GetProperty("CheckedState");
				property.SetValue(this.TriggerButton, !this.menuItemSelected.Active, null);
			}
		}

		private void menuItemSetDisable_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("status changed", null))
			{
				IDisplayState displayState = this.TriggerButton as IDisplayState;
				if (displayState != null)
				{
					displayState.DisplayState = !this.menuItemSetDisable.Active;
				}
			}
		}

		public override Type GetObjectType()
		{
			return typeof(CheckBoxObject);
		}

		private CheckMenuItem menuItemSelected;

		private MenuItem menuItemSetCheckBoxStyle;

		private SetStyleMenuItem menuItemNormal;

		private SetStyleMenuItem menuItemPressed;

		private SetStyleMenuItem menuItemDisable;

		private SetStyleMenuItem menuItemSelectedNormal;

		private SetStyleMenuItem menuItemSelectedDisable;

		private CheckMenuItem menuItemSetDisable;

		private VisualObject triggerbutton;
	}
}
