using System;
using Cairo;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.Debugger;
using Pango;

namespace MonoDevelop.AnalysisCore.Gui
{
	internal class ResultMarker : UnderlineTextSegmentMarker
	{
		private readonly Result result;

		public Result Result => result;

		public int Line => result.Region.BeginLine;

		public int ColStart
		{
			get
			{
				if (!IsOneLine(result))
				{
					return 0;
				}
				return result.Region.BeginColumn;
			}
		}

		public int ColEnd
		{
			get
			{
				if (!IsOneLine(result))
				{
					return 0;
				}
				return result.Region.EndColumn;
			}
		}

		public string Message => result.Message;

		public ResultMarker(Result result, TextSegment segment)
			: base("", segment)
		{
			this.result = result;
		}

		private static bool IsOneLine(Result result)
		{
			return result.Region.BeginLine == result.Region.EndLine;
		}

		private static Cairo.Color GetColor(TextEditor editor, Result result)
		{
			switch (result.Level)
			{
			case Severity.None:
				return editor.ColorStyle.PlainText.Background;
			case Severity.Error:
				return editor.ColorStyle.UnderlineError.Color;
			case Severity.Warning:
				return editor.ColorStyle.UnderlineWarning.Color;
			case Severity.Suggestion:
				return editor.ColorStyle.UnderlineSuggestion.Color;
			case Severity.Hint:
				return editor.ColorStyle.UnderlineHint.Color;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public override void Draw(TextEditor editor, Cairo.Context cr, Layout layout, bool selected, int startOffset, int endOffset, double y, double startXPos, double endXPos)
		{
			if (DebuggingService.IsDebugging)
			{
				return;
			}
			int num = base.Segment.Offset;
			int num2 = base.Segment.EndOffset;
			if (num2 < startOffset || num > endOffset)
			{
				return;
			}
			bool flag = result.InspectionMark == IssueMarker.GrayOut;
			if (flag && editor.IsSomethingSelected)
			{
				TextSegment selectionRange = editor.SelectionRange;
				if (selectionRange.Contains(num) && selectionRange.Contains(num2))
				{
					return;
				}
				if (selectionRange.Contains(num2))
				{
					num2 = selectionRange.Offset;
				}
				if (selectionRange.Contains(num))
				{
					num = selectionRange.EndOffset;
				}
				if (num2 <= num)
				{
					return;
				}
			}
			double val2;
			double val;
			if (num < startOffset && endOffset < num2)
			{
				val = endXPos;
				DocumentLine lineByOffset = editor.GetLineByOffset(startOffset);
				int length = lineByOffset.GetIndentation(editor.Document).Length;
				val2 = startXPos + (double)layout.IndexToPos(length).X / Scale.PangoScale;
			}
			else
			{
				int num3;
				if (startOffset < num)
				{
					num3 = num;
				}
				else
				{
					DocumentLine lineByOffset2 = editor.GetLineByOffset(startOffset);
					int length2 = lineByOffset2.GetIndentation(editor.Document).Length;
					num3 = startOffset + length2;
				}
				int num4 = ((endOffset < num2) ? endOffset : num2);
				int x = layout.IndexToPos(num3 - startOffset).X;
				val2 = startXPos + (double)(int)((double)x / Scale.PangoScale);
				x = layout.IndexToPos(num4 - startOffset).X;
				val = startXPos + (double)(int)((double)x / Scale.PangoScale);
			}
			val2 = Math.Max(val2, editor.TextViewMargin.XOffset);
			val = Math.Max(val, editor.TextViewMargin.XOffset);
			if (!(val2 >= val))
			{
				double num5 = editor.LineHeight / 5.0;
				cr.SetSourceColor(GetColor(editor, Result));
				if (flag)
				{
					cr.Rectangle(val2, y, val - val2, editor.LineHeight);
					Cairo.Color background = editor.ColorStyle.PlainText.Background;
					background.A = 0.6;
					cr.SetSourceColor(background);
					cr.Fill();
				}
				else if (result.InspectionMark == IssueMarker.WavedLine)
				{
					CairoHelper.ShowErrorUnderline(cr, val2, y + editor.LineHeight - num5, val - val2, num5);
				}
				else if (result.InspectionMark == IssueMarker.DottedLine)
				{
					cr.Save();
					cr.LineWidth = 1.0;
					cr.MoveTo(val2 + 1.0, y + editor.LineHeight - 1.0 + 0.5);
					cr.RelLineTo(Math.Min(val - val2, 12.0), 0.0);
					cr.SetDash(new double[2] { 2.0, 2.0 }, 0.0);
					cr.Stroke();
					cr.Restore();
				}
				else
				{
					cr.MoveTo(val2, y + editor.LineHeight - 1.0);
					cr.LineTo(val, y + editor.LineHeight - 1.0);
					cr.Stroke();
				}
			}
		}
	}
}
