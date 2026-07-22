using System;
using Gtk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200000C RID: 12
	public interface IKeyEventHandler
	{
		// Token: 0x0600006D RID: 109
		void OnKeyDown(KeyPressEventArgs args);

		// Token: 0x0600006E RID: 110
		void OnKeyUp(KeyReleaseEventArgs args);
	}
}
