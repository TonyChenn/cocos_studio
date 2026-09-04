using System;
using CocoStudio.Model.ViewModel;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200000C RID: 12
	internal class TextEditMenuItem : MenuItem, IObjectMenuItem
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002E2C File Offset: 0x0000102C
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002E08 File Offset: 0x00001008
		public VisualObject TriggerObject
		{
			get
			{
				return this.triggerObject;
			}
			set
			{
				if (this.triggerObject != value)
				{
					this.triggerObject = value;
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E44 File Offset: 0x00001044
		public TextEditMenuItem(string title) : base(title)
		{
			base.ButtonReleaseEvent += this.TextEditMenuItem_ButtonReleaseEvent;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E64 File Offset: 0x00001064
		private void TextEditMenuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			Menu menu = base.Parent as Menu;
			menu.Deactivate();
			this.OpenTextEditWindow();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E8C File Offset: 0x0000108C
		public string OpenTextEditWindow()
		{
			this.TriggerObject.MouseDoubleClick(null);
			return this.TriggerObject.GetType().Name;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002EBB File Offset: 0x000010BB
		public void UpdateMenuItemState()
		{
		}

		// Token: 0x0400001F RID: 31
		private VisualObject triggerObject;
	}
}
