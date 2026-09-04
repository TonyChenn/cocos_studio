using System;
using ICSharpCode.NRefactory.CSharp;
using Mono.TextEditor;
using MonoDevelop.Core;

namespace MonoDevelop.SourceEditor.JSon
{
	internal class JSonIndentationTracker : IIndentationTracker
	{
		private readonly TextEditorData data;

		private readonly CacheIndentEngine stateTracker;

		public JSonIndentationTracker(TextEditorData data, CacheIndentEngine stateTracker)
		{
			this.data = data;
			this.stateTracker = stateTracker;
		}

		private string GetIndentationString(DocumentLocation loc)
		{
			DocumentLine line = data.Document.GetLine(loc.Line);
			if (line == null)
			{
				return "";
			}
			int offset = line.Offset;
			string indentation = line.GetIndentation(data.Document);
			try
			{
				stateTracker.Update(Math.Min(data.Length, offset + Math.Min(line.Length, loc.Column - 1)));
				int length = indentation.Length;
				if (!stateTracker.LineBeganInsideMultiLineComment || (length < line.LengthIncludingDelimiter && data.Document.GetCharAt(offset + length) == '*'))
				{
					return stateTracker.ThisLineIndent;
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error while indenting at " + loc, ex);
			}
			return indentation;
		}

		public string GetIndentationString(int offset)
		{
			return GetIndentationString(data.OffsetToLocation(offset));
		}

		public string GetIndentationString(int lineNumber, int column)
		{
			return GetIndentationString(new DocumentLocation(lineNumber, column));
		}

		public int GetVirtualIndentationColumn(int offset)
		{
			return 1 + GetIndentationString(offset).Length;
		}

		public int GetVirtualIndentationColumn(int lineNumber, int column)
		{
			return 1 + GetIndentationString(lineNumber, column).Length;
		}
	}
}
