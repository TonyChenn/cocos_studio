using System;
using System.ComponentModel;
using Gdk;

namespace Gtk
{
	// Token: 0x02000016 RID: 22
	[ToolboxItem(true)]
	public class ComBoxEx : ComboBox
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000044D0 File Offset: 0x000026D0
		protected override void OnFocusGrabbed()
		{
			if (this.isPress)
			{
				base.CanFocus = true;
			}
			else
			{
				base.CanFocus = false;
			}
			base.OnFocusGrabbed();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004504 File Offset: 0x00002704
		protected override bool OnScrollEvent(EventScroll evnt)
		{
			return base.IsFocus && base.OnScrollEvent(evnt);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000452C File Offset: 0x0000272C
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1U)
			{
				this.isPress = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000455C File Offset: 0x0000275C
		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			this.isPress = false;
			return base.OnFocusOutEvent(evnt);
		}

		// Token: 0x0400003E RID: 62
		private bool isPress = false;
	}
}
