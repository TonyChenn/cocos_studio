using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000013 RID: 19
	public class CheckButtonEx : CheckButton
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003F70 File Offset: 0x00002170
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			bool result;
			if (this.keyDown)
			{
				result = true;
			}
			else
			{
				this.keyDown = true;
				result = base.OnKeyPressEvent(evnt);
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003FA4 File Offset: 0x000021A4
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			this.keyDown = false;
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x04000037 RID: 55
		private bool keyDown = false;
	}
}
