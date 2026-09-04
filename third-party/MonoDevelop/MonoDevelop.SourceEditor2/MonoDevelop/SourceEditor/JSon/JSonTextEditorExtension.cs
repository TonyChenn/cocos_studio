using System;
using Gdk;
using ICSharpCode.NRefactory.CSharp;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.SourceEditor.JSon
{
	internal class JSonTextEditorExtension : TextEditorExtension
	{
		private CacheIndentEngine stateTracker;

		private TextEditorData textEditorData => document.Editor;

		public override void Initialize()
		{
			base.Initialize();
			IStateMachineIndentEngine decoratedEngine = new JSonIndentEngine(document.Editor);
			stateTracker = new CacheIndentEngine(decoratedEngine);
			document.Editor.IndentationTracker = new JSonIndentationTracker(document.Editor, stateTracker);
		}

		public override bool KeyPress(Key key, char keyChar, ModifierType modifier)
		{
			bool result = base.KeyPress(key, keyChar, modifier);
			if (key == Key.Return)
			{
				if (textEditorData.Options.IndentStyle == IndentStyle.Virtual)
				{
					if (textEditorData.GetLine(textEditorData.Caret.Line).Length == 0)
					{
						textEditorData.Caret.Column = textEditorData.IndentationTracker.GetVirtualIndentationColumn(textEditorData.Caret.Location);
					}
				}
				else
				{
					DoReSmartIndent();
				}
			}
			return result;
		}

		private void DoReSmartIndent()
		{
			DoReSmartIndent(textEditorData.Caret.Offset);
		}

		private void DoReSmartIndent(int cursor)
		{
			SafeUpdateIndentEngine(cursor);
			if (stateTracker.LineBeganInsideVerbatimString || stateTracker.LineBeganInsideMultiLineComment)
			{
				return;
			}
			DocumentLine lineByOffset = textEditorData.Document.GetLineByOffset(cursor);
			IStateMachineIndentEngine stateMachineIndentEngine = stateTracker.Clone();
			try
			{
				for (int i = cursor; i < lineByOffset.EndOffset; i++)
				{
					stateMachineIndentEngine.Push(textEditorData.Document.GetCharAt(i));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Exception during indentation", ex);
			}
			int offset = lineByOffset.Offset;
			string indentation = lineByOffset.GetIndentation(textEditorData.Document);
			int length = indentation.Length;
			int num = ((cursor > offset + length) ? (cursor - (offset + length)) : 0);
			if (!stateTracker.LineBeganInsideMultiLineComment || (length < lineByOffset.LengthIncludingDelimiter && textEditorData.Document.GetCharAt(lineByOffset.Offset + length) == '*'))
			{
				string thisLineIndent = stateMachineIndentEngine.ThisLineIndent;
				int num2 = thisLineIndent.Length;
				if (thisLineIndent != indentation)
				{
					if (CompletionWindowManager.IsVisible && offset < CompletionWindowManager.CodeCompletionContext.TriggerOffset)
					{
						CompletionWindowManager.CodeCompletionContext.TriggerOffset -= length;
					}
					num2 = textEditorData.Replace(offset, length, thisLineIndent);
					textEditorData.Document.CommitLineUpdate(textEditorData.Caret.Line);
					CompletionWindowManager.HideWindow();
				}
				offset += num2;
			}
			else
			{
				offset += indentation.Length;
			}
			offset += num;
			textEditorData.FixVirtualIndentation();
		}

		internal void SafeUpdateIndentEngine(int offset)
		{
			try
			{
				stateTracker.Update(offset);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error while updating the indentation engine", ex);
			}
		}
	}
}
