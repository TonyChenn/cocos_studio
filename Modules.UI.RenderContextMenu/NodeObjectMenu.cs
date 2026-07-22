using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000003 RID: 3
	public class NodeObjectMenu : ICustomMenu
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
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

		// Token: 0x06000005 RID: 5 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020A4 File Offset: 0x000002A4
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

		// Token: 0x06000007 RID: 7 RVA: 0x000020DC File Offset: 0x000002DC
		protected virtual void InitMenu()
		{
			this.MenuItemList = new List<MenuItem>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020EA File Offset: 0x000002EA
		protected virtual void InitMenuItemList(List<MenuItem> ItemList)
		{
			this.MenuItemList = new List<MenuItem>();
			ItemList = new List<MenuItem>();
			this.MenuItemList = ItemList;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002108 File Offset: 0x00000308
		public virtual Type GetObjectType()
		{
			return typeof(AbstractNodeObject);
		}

		// Token: 0x04000001 RID: 1
		private const string classNameSuffix = "Menu";

		// Token: 0x04000002 RID: 2
		protected List<MenuItem> MenuItemList;

		// Token: 0x04000003 RID: 3
		private VisualObject triggerbutton;
	}
}
