using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public class BreakpointTextMarker : DebugTextMarker
	{
		private static readonly Image breakpoint = Image.FromResource("gutter-breakpoint-15.png");

		private static readonly Image tracepoint = Image.FromResource("gutter-tracepoint-15.png");

		public bool IsTracepoint { get; private set; }

		protected override Cairo.Color BackgroundColor => base.Editor.ColorStyle.BreakpointMarker.Color;

		protected override Cairo.Color BorderColor => GetBorderColor(base.Editor.ColorStyle.BreakpointMarker);

		public BreakpointTextMarker(TextEditor editor, bool tracepoint)
			: base(editor)
		{
			IsTracepoint = tracepoint;
		}

		protected override void SetForegroundColor(ChunkStyle style)
		{
			style.Foreground = base.Editor.ColorStyle.BreakpointText.Foreground;
		}

		protected override void DrawMarginIcon(Cairo.Context cr, double x, double y, double size)
		{
			DrawImage(cr, IsTracepoint ? tracepoint : breakpoint, x, y, size);
		}
	}
}
