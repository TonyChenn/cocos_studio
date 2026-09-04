using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public class DebugStackLineTextMarker : DebugTextMarker
	{
		private static readonly Image stackLine = Image.FromResource("gutter-stack-15.png");

		protected override Cairo.Color BackgroundColor => base.Editor.ColorStyle.DebuggerStackLineMarker.Color;

		protected override Cairo.Color BorderColor => GetBorderColor(base.Editor.ColorStyle.DebuggerStackLineMarker);

		public DebugStackLineTextMarker(TextEditor editor)
			: base(editor)
		{
		}

		protected override void SetForegroundColor(ChunkStyle style)
		{
			style.Foreground = base.Editor.ColorStyle.DebuggerStackLine.Foreground;
		}

		protected override void DrawMarginIcon(Cairo.Context cr, double x, double y, double size)
		{
			DrawImage(cr, stackLine, x, y, size);
		}
	}
}
