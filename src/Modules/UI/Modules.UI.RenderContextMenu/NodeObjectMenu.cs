using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	public class NodeObjectMenu : ICustomMenu
	{
		public virtual VisualObject TriggerButton
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

		public virtual List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		public NodeObjectMenu()
		{
			try
			{
				this.InitMenu();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected virtual void InitMenu()
		{
			this.MenuItemList = new List<MenuItem>();
		}

		protected virtual void InitMenuItemList(List<MenuItem> ItemList)
		{
			this.MenuItemList = new List<MenuItem>();
			ItemList = new List<MenuItem>();
			this.MenuItemList = ItemList;
		}

		public virtual Type GetObjectType()
		{
			return typeof(AbstractNodeObject);
		}

		private const string classNameSuffix = "Menu";

		protected List<MenuItem> MenuItemList;

		private VisualObject triggerbutton;
	}
}
