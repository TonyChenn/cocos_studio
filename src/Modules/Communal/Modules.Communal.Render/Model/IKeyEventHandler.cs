using System;
using Gtk;

namespace Modules.Communal.Render.Model
{
	public interface IKeyEventHandler
	{
		void OnKeyDown(KeyPressEventArgs args);

		void OnKeyUp(KeyReleaseEventArgs args);
	}
}
