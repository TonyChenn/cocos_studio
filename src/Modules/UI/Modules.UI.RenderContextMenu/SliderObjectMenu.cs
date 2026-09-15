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
	public class SliderObjectMenu : NodeObjectMenu
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
					this.menuItemBackground.TriggerObject = this.triggerbutton;
					this.menuItemInnerSliderStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodeNormalStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodePressedStyle.TriggerObject = this.triggerbutton;
					this.menuItemNodeDisabledStyle.TriggerObject = this.triggerbutton;
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
			return typeof(SliderObject);
		}

		private MenuItem menuItemSetStyle;

		private SetStyleMenuItem menuItemBackground;

		private SetStyleMenuItem menuItemInnerSliderStyle;

		private SetStyleMenuItem menuItemNodeNormalStyle;

		private SetStyleMenuItem menuItemNodePressedStyle;

		private SetStyleMenuItem menuItemNodeDisabledStyle;

		private CheckMenuItem menuItemSetDisable;

		private VisualObject triggerbutton;
	}
}
