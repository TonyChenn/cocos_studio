using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class ParticleObjectMenu : NodeObjectMenu
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
					this.customMenuItems.ForEach(delegate(IObjectMenuItem item)
					{
						item.TriggerObject = this.triggerbutton;
					});
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
			this.customMenuItems.ForEach(delegate(IObjectMenuItem item)
			{
				item.UpdateMenuItemState();
			});
		}

		protected override void InitMenu()
		{
			base.InitMenu();
			SetStyleMenuItem item = new SetStyleMenuItem(new string[]
			{
				"plist"
			}, "FileData", LanguageInfo.ContexMenu_SetSpriteFile);
			this.customMenuItems.Add(item);
			this.MenuItemList.Add(item);
		}

		public override Type GetObjectType()
		{
			return typeof(ParticleObject);
		}

		private List<IObjectMenuItem> customMenuItems = new List<IObjectMenuItem>();

		private VisualObject triggerbutton;
	}
}
