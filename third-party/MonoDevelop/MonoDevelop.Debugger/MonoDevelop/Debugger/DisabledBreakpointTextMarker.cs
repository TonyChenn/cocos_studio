using Cairo;
using Mono.TextEditor;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public class DisabledBreakpointTextMarker : DebugTextMarker
	{
		private static readonly Image breakpoint = Image.FromResource("gutter-breakpoint-disabled-15.png");

		private static readonly Image tracepoint = Image.FromResource("gutter-tracepoint-disabled-15.png");

		public bool IsTracepoint { get; private set; }

		protected override Cairo.Color BackgroundColor => base.Editor.ColorStyle.BreakpointMarkerDisabled.Color;

		protected override Cairo.Color BorderColor => GetBorderColor(base.Editor.ColorStyle.BreakpointMarkerDisabled);

		public DisabledBreakpointTextMarker(TextEditor editor, bool tracepoint)
			: base(editor)
		{
			IsTracepoint = tracepoint;
		}

		protected override void DrawMarginIcon(Cairo.Context cr, double x, double y, double size)
		{
			DrawImage(cr, IsTracepoint ? tracepoint : breakpoint, x, y, size);
		}
	}
}
