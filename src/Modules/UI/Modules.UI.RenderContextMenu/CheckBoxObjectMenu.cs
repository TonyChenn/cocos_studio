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
	// Token: 0x02000005 RID: 5
	[Extension(typeof(ICustomMenu))]
	public class CheckBoxObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000023C4 File Offset: 0x000005C4
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000023DC File Offset: 0x000005DC
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

		// Token: 0x06000015 RID: 21 RVA: 0x00002468 File Offset: 0x00000668
		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002488 File Offset: 0x00000688
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

		// Token: 0x06000017 RID: 23 RVA: 0x000024F8 File Offset: 0x000006F8
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

		// Token: 0x06000018 RID: 24 RVA: 0x0000269C File Offset: 0x0000089C
		private void menuItemSelected_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("play Particle", null))
			{
				PropertyInfo property = this.TriggerButton.GetType().GetProperty("CheckedState");
				property.SetValue(this.TriggerButton, !this.menuItemSelected.Active, null);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002714 File Offset: 0x00000914
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

		// Token: 0x0600001A RID: 26 RVA: 0x00002780 File Offset: 0x00000980
		public override Type GetObjectType()
		{
			return typeof(CheckBoxObject);
		}

		// Token: 0x0400000A RID: 10
		private CheckMenuItem menuItemSelected;

		// Token: 0x0400000B RID: 11
		private MenuItem menuItemSetCheckBoxStyle;

		// Token: 0x0400000C RID: 12
		private SetStyleMenuItem menuItemNormal;

		// Token: 0x0400000D RID: 13
		private SetStyleMenuItem menuItemPressed;

		// Token: 0x0400000E RID: 14
		private SetStyleMenuItem menuItemDisable;

		// Token: 0x0400000F RID: 15
		private SetStyleMenuItem menuItemSelectedNormal;

		// Token: 0x04000010 RID: 16
		private SetStyleMenuItem menuItemSelectedDisable;

		// Token: 0x04000011 RID: 17
		private CheckMenuItem menuItemSetDisable;

		// Token: 0x04000012 RID: 18
		private VisualObject triggerbutton;
	}
}
