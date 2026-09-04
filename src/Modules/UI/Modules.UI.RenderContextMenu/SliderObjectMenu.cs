using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000025 RID: 37
	[Extension(typeof(ICustomMenu))]
	public class SliderObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006F78 File Offset: 0x00005178
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00006F90 File Offset: 0x00005190
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
					this.menuItemBackground.TriggerObject = this.triggerbutton;
					this.menuItemInnerSliderStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodeNormalStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodePressedStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodeDisabledStyle.TriggerObject = this.triggerbutton;
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000701C File Offset: 0x0000521C
		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000703C File Offset: 0x0000523C
		private void UpdateCheckItemState()
		{
			IDisplayState displayState = this.TriggerButton as IDisplayState;
			if (displayState != null)
			{
				this.menuItemSetDisable.Active = !displayState.DisplayState;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007078 File Offset: 0x00005278
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"png",
				"jpg"
			};
			this.menuItemBackground = new SetStyleMenuItem(filetype, "BackGroundData", LanguageInfo.ContexMenu_BackgroundStyle);
			this.menuItemInnerSliderStyle = new SetStyleMenuItem(filetype, "ProgressBarData", LanguageInfo.ContexMenu_InnerSliderStyle);
			this.menuItemNodeNormalStyle = new SetStyleMenuItem(filetype, "BallNormalData", LanguageInfo.ContexMenu_NodeNormalStyle);
			this.menuItemNodePressedStyle = new SetStyleMenuItem(filetype, "BallPressedData", LanguageInfo.ContexMenu_NodePressedStyle);
			this.menuItemNodeDisabledStyle = new SetStyleMenuItem(filetype, "BallDisabledData", LanguageInfo.ContexMenu_NodeDisabledStyle);
			Menu menu = new Menu();
			menu.Add(this.menuItemBackground);
			menu.Add(this.menuItemInnerSliderStyle);
			menu.Add(new SeparatorMenuItem());
			menu.Add(this.menuItemNodeNormalStyle);
			menu.Add(this.menuItemNodePressedStyle);
			menu.Add(this.menuItemNodeDisabledStyle);
			this.menuItemSetStyle = new MenuItem(LanguageInfo.ContexMenu_SetSliderStyle);
			this.menuItemSetStyle.Submenu = menu;
			this.menuItemSetDisable = new CheckMenuItem(LanguageInfo.Display_MakeDisabled);
			this.menuItemSetDisable.Toggled += this.menuItemSetDisable_Click;
			this.MenuItemList.Add(this.menuItemSetStyle);
			this.MenuItemList.Add(new SeparatorMenuItem());
			this.MenuItemList.Add(this.menuItemSetDisable);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000071E4 File Offset: 0x000053E4
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

		// Token: 0x0600011C RID: 284 RVA: 0x00007250 File Offset: 0x00005450
		public override Type GetObjectType()
		{
			return typeof(SliderObject);
		}

		// Token: 0x04000088 RID: 136
		private MenuItem menuItemSetStyle;

		// Token: 0x04000089 RID: 137
		private SetStyleMenuItem menuItemBackground;

		// Token: 0x0400008A RID: 138
		private SetStyleMenuItem menuItemInnerSliderStyle;

		// Token: 0x0400008B RID: 139
		private SetStyleMenuItem menuItemNodeNormalStyle;

		// Token: 0x0400008C RID: 140
		private SetStyleMenuItem menuItemNodePressedStyle;

		// Token: 0x0400008D RID: 141
		private SetStyleMenuItem menuItemNodeDisabledStyle;

		// Token: 0x0400008E RID: 142
		private CheckMenuItem menuItemSetDisable;

		// Token: 0x0400008F RID: 143
		private VisualObject triggerbutton;
	}
}
