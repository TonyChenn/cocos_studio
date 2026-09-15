using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class ImageViewObjectMenu : NodeObjectMenu
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
				"png",
				"jpg"
			};
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "FileData", LanguageInfo.ContexMenu_SetImageResources);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		public override Type GetObjectType()
		{
			return typeof(ImageViewObject);
		}

		private SetStyleMenuItem menuItemSetStyle;

		private VisualObject triggerbutton;
	}
}
