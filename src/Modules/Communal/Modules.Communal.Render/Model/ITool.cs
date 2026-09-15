using System;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	public interface ITool : IInputEventHandler, IMouseEventHandler, IKeyEventHandler
	{
		bool HasSeparator { get; }

		Xwt.Drawing.Image Icon { get; }

		string Tooltip { get; }

		Gdk.Key ShortcutKey { get; }

		ToolType Type { get; }

		bool Enabled { get; set; }

		event EventHandler EnabledChanged;

		bool IsSelected { get; set; }

		Widget CustomWidget { get; }

		event EventHandler SelectedChanged;
	}
}
