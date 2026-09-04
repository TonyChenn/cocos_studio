using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000011 RID: 17
	[Extension(typeof(ICustomMenu))]
	public class PageViewObjectMenu : NodeObjectMenu
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000031FC File Offset: 0x000013FC
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003214 File Offset: 0x00001414
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

		// Token: 0x0600005C RID: 92 RVA: 0x00003244 File Offset: 0x00001444
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000325C File Offset: 0x0000145C
		protected override void InitMenu()
		{
			base.InitMenu();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003268 File Offset: 0x00001468
		public override Type GetObjectType()
		{
			return typeof(PageViewObject);
		}

		// Token: 0x04000026 RID: 38
		private VisualObject triggerbutton;
	}
}
