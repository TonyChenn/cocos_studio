using System.Collections.Generic;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.SourceEditor
{
	public class DynamicAbbrevHandler : CommandHandler
	{
		private enum AbbrevState
		{
			SearchBackward,
			SearchForward,
			SearchOtherBuffers,
			CycleThroughFoundWords
		}

		private static SourceEditorView lastView = null;

		private static string lastAbbrev = null;

		private static int lastTriggerOffset = 0;

		private static int lastInsertPos = 0;

		private static List<string> foundWords = new List<string>();

		private static int lastStartOffset = 0;

		private static AbbrevState curState;

		protected override void Run(object data)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null)
			{
				return;
			}
			SourceEditorView content = IdeApp.Workbench.ActiveDocument.GetContent<SourceEditorView>();
			if (content == null)
			{
				return;
			}
			string text;
			int num;
			if (lastView == content && content.TextEditor.Caret.Offset == lastTriggerOffset)
			{
				text = lastAbbrev;
				num = lastStartOffset;
			}
			else
			{
				text = (lastAbbrev = GetWordBeforeCaret(content.TextEditor));
				num = content.TextEditor.Caret.Offset - text.Length - 1;
				lastInsertPos = (lastTriggerOffset = num + 1);
				foundWords.Clear();
				foundWords.Add(text);
				curState = AbbrevState.SearchBackward;
			}
			lastView = content;
			switch (curState)
			{
			default:
				return;
			case AbbrevState.SearchBackward:
				while (num > 0)
				{
					if (IsMatchAt(content, num, text))
					{
						int endOffset = SearchEndPos(num, content);
						string textBetween = content.TextEditor.Document.GetTextBetween(num, endOffset);
						if (!foundWords.Contains(textBetween))
						{
							foundWords.Add(textBetween);
							ReplaceWord(content, textBetween);
							lastStartOffset = num - 1;
							return;
						}
						num--;
					}
					else
					{
						num--;
					}
				}
				num = content.TextEditor.Caret.Offset;
				curState = AbbrevState.SearchForward;
				goto case AbbrevState.SearchForward;
			case AbbrevState.SearchForward:
				while (num < content.TextEditor.Document.TextLength)
				{
					if (IsMatchAt(content, num, text))
					{
						int endOffset3 = SearchEndPos(num, content);
						string textBetween3 = content.TextEditor.Document.GetTextBetween(num, endOffset3);
						if (!foundWords.Contains(textBetween3))
						{
							foundWords.Add(textBetween3);
							ReplaceWord(content, textBetween3);
							lastStartOffset = num + 1;
							return;
						}
						num++;
					}
					else
					{
						num++;
					}
				}
				curState = AbbrevState.SearchOtherBuffers;
				goto case AbbrevState.SearchOtherBuffers;
			case AbbrevState.SearchOtherBuffers:
				foreach (Document document in IdeApp.Workbench.Documents)
				{
					SourceEditorView content2 = document.GetContent<SourceEditorView>();
					if (document == activeDocument || content2 == null || content2.Document == null)
					{
						continue;
					}
					for (int i = 0; i < content2.Document.TextLength; i++)
					{
						if (IsMatchAt(content2, i, text))
						{
							int endOffset2 = SearchEndPos(i, content2);
							string textBetween2 = content2.TextEditor.Document.GetTextBetween(i, endOffset2);
							if (!foundWords.Contains(textBetween2))
							{
								foundWords.Add(textBetween2);
							}
						}
					}
				}
				curState = AbbrevState.CycleThroughFoundWords;
				break;
			case AbbrevState.CycleThroughFoundWords:
				break;
			}
			int num2 = foundWords.IndexOf(content.TextEditor.Document.GetTextAt(lastInsertPos, content.TextEditor.Caret.Offset - lastInsertPos));
			if (num2 >= 0)
			{
				int num3 = num;
				num = num3 + foundWords[num2].Length;
				num2 = (num2 + foundWords.Count + 1) % foundWords.Count;
				ReplaceWord(content, foundWords[num2]);
			}
		}

		public static bool IsIdentifierPart(char ch)
		{
			if (!char.IsLetterOrDigit(ch))
			{
				return ch == '_';
			}
			return true;
		}

		private static string GetWordBeforeCaret(ExtensibleTextEditor editor)
		{
			int offset = editor.Caret.Offset;
			int num;
			for (num = offset - 1; num > 0; num--)
			{
				char charAt = editor.Document.GetCharAt(num);
				if (!IsIdentifierPart(charAt))
				{
					num++;
					break;
				}
			}
			if (num >= offset)
			{
				return "";
			}
			return editor.Document.GetTextBetween(num, offset);
		}

		private static void ReplaceWord(SourceEditorView view, string curWord)
		{
			view.TextEditor.Replace(lastInsertPos, view.TextEditor.Caret.Offset - lastInsertPos, curWord);
			view.TextEditor.Document.CommitLineUpdate(view.TextEditor.Caret.Line);
			lastTriggerOffset = view.TextEditor.Caret.Offset;
		}

		private static int SearchEndPos(int offset, SourceEditorView view)
		{
			while (offset < view.TextEditor.Document.TextLength && IsIdentifierPart(view.TextEditor.Document.GetCharAt(offset)))
			{
				offset++;
			}
			return offset;
		}

		private static bool IsMatchAt(SourceEditorView view, int offset, string abbrevWord)
		{
			if (offset + abbrevWord.Length >= view.TextEditor.Document.TextLength)
			{
				return false;
			}
			if (offset > 0 && IsIdentifierPart(view.TextEditor.Document.GetCharAt(offset - 1)))
			{
				return false;
			}
			if (offset + abbrevWord.Length < view.TextEditor.Document.TextLength && !IsIdentifierPart(view.TextEditor.Document.GetCharAt(offset + abbrevWord.Length)))
			{
				return false;
			}
			return view.TextEditor.Document.GetTextAt(offset, abbrevWord.Length) == abbrevWord;
		}
	}
}
