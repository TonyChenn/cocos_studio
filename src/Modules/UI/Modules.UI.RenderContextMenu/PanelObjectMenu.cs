using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000012 RID: 18
	[Extension(typeof(ICustomMenu))]
	public class PanelObjectMenu : NodeObjectMenu
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003284 File Offset: 0x00001484
		// (set) Token: 0x06000060 RID: 96 RVA: 0x0000329C File Offset: 0x0000149C
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

		// Token: 0x06000062 RID: 98 RVA: 0x000032CC File Offset: 0x000014CC
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032E4 File Offset: 0x000014E4
		protected override void InitMenu()
		{
			base.InitMenu();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000032F0 File Offset: 0x000014F0
		public override Type GetObjectType()
		{
			return typeof(PanelObject);
		}

		// Token: 0x04000027 RID: 39
		private VisualObject triggerbutton;
	}
}
