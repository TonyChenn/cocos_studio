using System;
using Gdk;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000013 RID: 19
	public class EnumEditorComboBox : ComboBox
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00003524 File Offset: 0x00001724
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

		// Token: 0x06000081 RID: 129 RVA: 0x00003558 File Offset: 0x00001758
		protected override bool OnScrollEvent(EventScroll evnt)
		{
			return base.IsFocus && base.OnScrollEvent(evnt);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003580 File Offset: 0x00001780
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1U)
			{
				this.isPress = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000035B0 File Offset: 0x000017B0
		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			this.isPress = false;
			return base.OnFocusOutEvent(evnt);
		}

		// Token: 0x0400001D RID: 29
		private bool isPress = false;
	}
}
