using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class ScrollViewObjectMenu : NodeObjectMenu
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
		}

		public override Type GetObjectType()
		{
			return typeof(ScrollViewObject);
		}

		private VisualObject triggerbutton;
	}
}
