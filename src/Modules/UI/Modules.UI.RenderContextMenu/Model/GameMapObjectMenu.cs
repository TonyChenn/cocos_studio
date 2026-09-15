using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu.Model
{
	[Extension(typeof(ICustomMenu))]
	public class GameMapObjectMenu : NodeObjectMenu
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
					this.menuItemSetStyle.TriggerObject = this.triggerbutton;
				}
			}
		}

		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"tmx"
			};
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "FileData", LanguageInfo.ContexMenu_SetMapFile);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		public override Type GetObjectType()
		{
			return typeof(GameMapObject);
		}

		private SetStyleMenuItem menuItemSetStyle;

		private VisualObject triggerbutton;
	}
}
