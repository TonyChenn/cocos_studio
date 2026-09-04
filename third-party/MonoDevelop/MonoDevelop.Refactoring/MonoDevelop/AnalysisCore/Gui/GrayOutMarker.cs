using System.Collections.Generic;
using Cairo;
using Mono.TextEditor;
using MonoDevelop.Debugger;
using Pango;

namespace MonoDevelop.AnalysisCore.Gui
{
	internal class GrayOutMarker : ResultMarker, IChunkMarker
	{
		public GrayOutMarker(Result result, TextSegment segment)
			: base(result, segment)
		{
		}

		public override void Draw(TextEditor editor, Cairo.Context cr, Layout layout, bool selected, int startOffset, int endOffset, double y, double startXPos, double endXPos)
		{
		}

		void IChunkMarker.TransformChunks(List<Chunk> chunks)
		{
			int offset = base.Segment.Offset;
			int endOffset = base.Segment.EndOffset;
			for (int i = 0; i < chunks.Count; i++)
			{
				Chunk chunk = chunks[i];
				if (chunk.EndOffset < offset || endOffset <= chunk.Offset)
				{
					continue;
				}
				if (chunk.Offset == offset && chunk.EndOffset == endOffset)
				{
					break;
				}
				if (chunk.Offset <= offset && chunk.EndOffset >= endOffset)
				{
					if (offset - chunk.Offset > 0)
					{
						Chunk chunk2 = new Chunk(chunk.Offset, offset - chunk.Offset, chunk.Style);
						chunks.Insert(i, chunk2);
						chunk.Offset += chunk2.Length;
						chunk.Length -= chunk2.Length;
						chunk = chunk2;
					}
					if (endOffset < chunk.EndOffset)
					{
						Chunk chunk3 = new Chunk(chunk.Offset, endOffset - chunk.Offset, chunk.Style);
						chunks.Insert(i, chunk3);
						chunk.Offset += chunk3.Length;
						chunk.Length -= chunk3.Length;
					}
				}
			}
		}

		void IChunkMarker.ChangeForeColor(TextEditor editor, Chunk chunk, ref Cairo.Color color)
		{
			if (!DebuggingService.IsDebugging)
			{
				int offset = base.Segment.Offset;
				int endOffset = base.Segment.EndOffset;
				if (chunk.EndOffset > offset && endOffset > chunk.Offset)
				{
					Cairo.Color background = editor.ColorStyle.PlainText.Background;
					double num = 0.6;
					color = new Cairo.Color(color.R * num + background.R * (1.0 - num), color.G * num + background.G * (1.0 - num), color.B * num + background.B * (1.0 - num));
				}
			}
		}
	}
}
