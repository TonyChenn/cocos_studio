using System;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200000E RID: 14
	public interface ITool : IInputEventHandler, IMouseEventHandler, IKeyEventHandler
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600006F RID: 111
		bool HasSeparator { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000070 RID: 112
		Xwt.Drawing.Image Icon { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000071 RID: 113
		string Tooltip { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000072 RID: 114
		Gdk.Key ShortcutKey { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000073 RID: 115
		ToolType Type { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000074 RID: 116
		// (set) Token: 0x06000075 RID: 117
		bool Enabled { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000076 RID: 118
		// (remove) Token: 0x06000077 RID: 119
		event EventHandler EnabledChanged;

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000078 RID: 120
		// (set) Token: 0x06000079 RID: 121
		bool IsSelected { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007A RID: 122
		Widget CustomWidget { get; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600007B RID: 123
		// (remove) Token: 0x0600007C RID: 124
		event EventHandler SelectedChanged;
	}
}
