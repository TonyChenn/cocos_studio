using System.Linq;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.SourceEditor
{
	public static class EditActions
	{
		private const string open = "'\"([{<";

		private const string closing = "'\")]}>";

		public static void AdvancedBackspace(TextEditorData data)
		{
			RemoveCharBeforCaret(data);
		}

		private static int GetNextNonWsCharOffset(TextEditorData data, int offset)
		{
			int num = offset;
			if (num >= data.Document.TextLength)
			{
				return -1;
			}
			while (char.IsWhiteSpace(data.Document.GetCharAt(num)))
			{
				num++;
				if (num >= data.Document.TextLength)
				{
					return -1;
				}
			}
			return num;
		}

		private static void RemoveCharBeforCaret(TextEditorData data)
		{
			if (!data.IsSomethingSelected && ((ISourceEditorOptions)data.Options).AutoInsertMatchingBracket && data.Caret.Offset > 0)
			{
				DocumentLine line = data.GetLine(data.Caret.Line);
				CloneableStack<Span> source = line.StartSpan.Clone();
				if (source.Any((Span s) => s.Color == "string.other"))
				{
					DeleteActions.Backspace(data);
					return;
				}
				source = line.StartSpan.Clone();
				if (source.Any((Span s) => s.Color == "string.other"))
				{
					DeleteActions.Backspace(data);
					return;
				}
				char charAt = data.Document.GetCharAt(data.Caret.Offset - 1);
				int num = "'\"([{<".IndexOf(charAt);
				if (num >= 0)
				{
					int nextNonWsCharOffset = GetNextNonWsCharOffset(data, data.Caret.Offset);
					if (nextNonWsCharOffset >= 0 && "'\")]}>"[num] == data.Document.GetCharAt(nextNonWsCharOffset))
					{
						data.Remove(data.Caret.Offset, nextNonWsCharOffset - data.Caret.Offset + 1);
					}
				}
			}
			DeleteActions.Backspace(data);
		}
	}
}
