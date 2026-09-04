using System;
using System.Linq;
using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public abstract class DebugTextMarker : MarginMarker
	{
		protected abstract Cairo.Color BackgroundColor { get; }

		protected abstract Cairo.Color BorderColor { get; }

		protected TextEditor Editor { get; private set; }

		protected DebugTextMarker(TextEditor editor)
		{
			Editor = editor;
		}

		protected Cairo.Color GetBorderColor(AmbientColor color)
		{
			if (color.HasBorderColor)
			{
				return color.BorderColor;
			}
			return color.Color;
		}

		public override bool CanDrawBackground(Margin margin)
		{
			return margin is TextViewMargin;
		}

		public override bool CanDrawForeground(Margin margin)
		{
			return margin is IconMargin;
		}

		public override bool DrawBackground(TextEditor editor, Cairo.Context cr, double y, LineMetrics metrics)
		{
			if (base.LineSegment != null && base.LineSegment.Markers.Any((TextLineMarker m) => m != this && m is IExtendingTextLineMarker))
			{
				return false;
			}
			int num = 4;
			double r = editor.LineHeight / 2.0 - 1.0;
			double num2 = metrics.TextRenderEndPosition - metrics.TextRenderStartPosition;
			if (num2 > 0.0)
			{
				cr.LineWidth = 1.0;
				cr.RoundedRectangle(metrics.TextRenderStartPosition, Math.Floor(y) + 0.5, num2 + (double)num, metrics.LineHeight - 1.0, r);
				cr.SetSourceColor(BackgroundColor);
				cr.FillPreserve();
				cr.SetSourceColor(BorderColor);
				cr.Stroke();
			}
			return base.DrawBackground(editor, cr, y, metrics);
		}

		public override void DrawForeground(TextEditor editor, Cairo.Context cr, MarginDrawMetrics metrics)
		{
			double width = metrics.Margin.Width;
			double lineWidth = cr.LineWidth;
			double x = Math.Floor(metrics.Margin.XOffset - lineWidth / 2.0);
			double y = Math.Floor(metrics.Y + (metrics.Height - width) / 2.0);
			DrawMarginIcon(cr, x, y, width);
		}

		protected virtual void SetForegroundColor(ChunkStyle style)
		{
		}

		public override ChunkStyle GetStyle(ChunkStyle baseStyle)
		{
			if (baseStyle == null)
			{
				return null;
			}
			ChunkStyle chunkStyle = new ChunkStyle(baseStyle);
			SetForegroundColor(chunkStyle);
			return chunkStyle;
		}

		protected void DrawImage(Cairo.Context cr, Image image, double x, double y, double size)
		{
			double num = size / 2.0 - image.Width / 2.0 + 0.5;
			double num2 = size / 2.0 - image.Height / 2.0 + 0.5;
			cr.DrawImage(Editor, image, Math.Round(x + num), Math.Round(y + num2));
		}

		protected virtual void DrawMarginIcon(Cairo.Context cr, double x, double y, double size)
		{
		}
	}
}
