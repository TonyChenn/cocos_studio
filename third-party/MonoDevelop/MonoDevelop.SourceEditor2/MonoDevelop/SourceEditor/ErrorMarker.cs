using Cairo;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;

namespace MonoDevelop.SourceEditor
{
	internal class ErrorMarker : UnderlineMarker
	{
		public Error Info { get; private set; }

		public ErrorMarker(TextDocument doc, Error info, DocumentLine line)
		{
			Info = info;
			base.LineSegment = line;
			base.Wave = true;
			base.StartCol = Info.Region.BeginColumn;
			if (line != null)
			{
				int offset = line.Offset;
				if (offset + base.StartCol - 1 >= 0)
				{
					while (base.StartCol < line.Length)
					{
						char charAt = doc.GetCharAt(offset + base.StartCol - 1);
						if (!char.IsWhiteSpace(charAt))
						{
							break;
						}
						base.StartCol++;
					}
				}
			}
			if (Info.Region.EndColumn > base.StartCol)
			{
				base.EndCol = Info.Region.EndColumn;
				return;
			}
			if (line == null)
			{
				base.EndCol = base.StartCol + 1;
				return;
			}
			int num = line.Offset + base.StartCol - 1;
			int i;
			for (i = num + 1; i < line.EndOffset; i++)
			{
				char charAt2 = doc.GetCharAt(i);
				if (!char.IsLetterOrDigit(charAt2) && charAt2 != '_')
				{
					break;
				}
			}
			base.EndCol = Info.Region.BeginColumn + i - num + 1;
		}

		public override void Draw(TextEditor editor, Context cr, double y, LineMetrics metrics)
		{
			base.Color = ((Info.ErrorType == ErrorType.Warning) ? editor.ColorStyle.UnderlineWarning.Color : editor.ColorStyle.UnderlineError.Color);
			base.Draw(editor, cr, y, metrics);
		}
	}
}
