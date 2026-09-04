using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000013 RID: 19
	[Extension(typeof(ICustomMenu))]
	public class ParticleObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000330C File Offset: 0x0000150C
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003324 File Offset: 0x00001524
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

		// Token: 0x06000068 RID: 104 RVA: 0x00003380 File Offset: 0x00001580
		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000339F File Offset: 0x0000159F
		private void UpdateCheckItemState()
		{
			this.customMenuItems.ForEach(delegate(IObjectMenuItem item)
			{
				item.UpdateMenuItemState();
			});
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000033D0 File Offset: 0x000015D0
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

		// Token: 0x0600006B RID: 107 RVA: 0x00003420 File Offset: 0x00001620
		public override Type GetObjectType()
		{
			return typeof(ParticleObject);
		}

		// Token: 0x04000028 RID: 40
		private List<IObjectMenuItem> customMenuItems = new List<IObjectMenuItem>();

		// Token: 0x04000029 RID: 41
		private VisualObject triggerbutton;
	}
}
