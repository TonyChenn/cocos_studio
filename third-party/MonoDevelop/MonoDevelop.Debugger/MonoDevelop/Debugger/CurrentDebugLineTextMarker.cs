using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public class CurrentDebugLineTextMarker : DebugTextMarker
	{
		private static readonly Image currentLine = Image.FromResource("gutter-execution-15.png");

		protected override Cairo.Color BackgroundColor => base.Editor.ColorStyle.DebuggerCurrentLineMarker.Color;

		protected override Cairo.Color BorderColor => GetBorderColor(base.Editor.ColorStyle.DebuggerCurrentLineMarker);

		public CurrentDebugLineTextMarker(TextEditor editor)
			: base(editor)
		{
		}

		protected override void SetForegroundColor(ChunkStyle style)
		{
			style.Foreground = base.Editor.ColorStyle.DebuggerCurrentLine.Foreground;
		}

		protected override void DrawMarginIcon(Cairo.Context cr, double x, double y, double size)
		{
			DrawImage(cr, currentLine, x, y, size);
		}
	}
}
