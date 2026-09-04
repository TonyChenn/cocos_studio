using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000021 RID: 33
	[Extension(typeof(ICustomMenu))]
	public class ScrollViewObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00006D64 File Offset: 0x00004F64
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00006D7C File Offset: 0x00004F7C
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

		// Token: 0x06000105 RID: 261 RVA: 0x00006DAC File Offset: 0x00004FAC
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006DC4 File Offset: 0x00004FC4
		protected override void InitMenu()
		{
			base.InitMenu();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public override Type GetObjectType()
		{
			return typeof(ScrollViewObject);
		}

		// Token: 0x04000076 RID: 118
		private VisualObject triggerbutton;
	}
}
