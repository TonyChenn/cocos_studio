using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000E RID: 14
	[Extension(typeof(ICustomMenu))]
	public class ListViewObjectMenu : NodeObjectMenu
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002F7C File Offset: 0x0000117C
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002F94 File Offset: 0x00001194
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

		// Token: 0x0600004C RID: 76 RVA: 0x00002FC4 File Offset: 0x000011C4
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002FDC File Offset: 0x000011DC
		protected override void InitMenu()
		{
			base.InitMenu();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002FE8 File Offset: 0x000011E8
		public override Type GetObjectType()
		{
			return typeof(ListViewObject);
		}

		// Token: 0x04000022 RID: 34
		private VisualObject triggerbutton;
	}
}
