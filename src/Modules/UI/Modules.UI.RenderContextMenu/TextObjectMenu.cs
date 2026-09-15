using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class TextObjectMenu : NodeObjectMenu
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
					this.menuItemEditText.TriggerObject = this.triggerbutton;
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
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
		}

		public override Type GetObjectType()
		{
			return typeof(TextObject);
		}

		private TextEditMenuItem menuItemEditText;

		private VisualObject triggerbutton;
	}
}
