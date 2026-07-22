using System;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000052 RID: 82
	internal class ChangeUnitButton : TriangleComboButton
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00009A6C File Offset: 0x00007C6C
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00009A83 File Offset: 0x00007C83
		public bool IsPercent { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002C6 RID: 710 RVA: 0x00009A8C File Offset: 0x00007C8C
		// (remove) Token: 0x060002C7 RID: 711 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public event EventHandler UnitChanged;

		// Token: 0x060002C8 RID: 712 RVA: 0x00009B04 File Offset: 0x00007D04
		public ChangeUnitButton()
		{
			this.menu = new Menu();
			this.percentMenuItem = new CheckMenuItem("%  " + LanguageInfo.Property_ParentPercentage);
			this.percentMenuItem.ButtonReleaseEvent += this.PercentMenuButtonReleasedHandler;
			this.percentMenuItem.Active = this.IsPercent;
			this.pixelMenuItem = new CheckMenuItem(LanguageInfo.NewFile_Pixel);
			this.pixelMenuItem.ButtonPressEvent += this.PixelMenuButtonReleasedHandler;
			this.pixelMenuItem.Active = !this.IsPercent;
			this.menu.Add(this.percentMenuItem);
			this.menu.Add(this.pixelMenuItem);
			this.menu.ShowAll();
			base.Clicked += this.ButtonClickedHandler;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009BE9 File Offset: 0x00007DE9
		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			this.percentMenuItem.Active = this.IsPercent;
			this.pixelMenuItem.Active = !this.IsPercent;
			this.menu.Popup();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009C20 File Offset: 0x00007E20
		private void PercentMenuButtonReleasedHandler(object o, ButtonReleaseEventArgs args)
		{
			this.IsPercent = true;
			if (this.UnitChanged != null)
			{
				this.UnitChanged(this, new EventArgs());
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009C58 File Offset: 0x00007E58
		private void PixelMenuButtonReleasedHandler(object o, ButtonPressEventArgs args)
		{
			this.IsPercent = false;
			if (this.UnitChanged != null)
			{
				this.UnitChanged(this, new EventArgs());
			}
		}

		// Token: 0x04000131 RID: 305
		private Menu menu;

		// Token: 0x04000132 RID: 306
		private CheckMenuItem percentMenuItem;

		// Token: 0x04000133 RID: 307
		private CheckMenuItem pixelMenuItem;
	}
}
