using System;
using System.Collections.Generic;
using System.Text;
using Mono.TextEditor;
using Mono.TextEditor.Utils;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Refactoring
{
	public class TextReplaceChange : Change
	{
		private int removedChars;

		private static List<TextEditorData> textEditorDatas = new List<TextEditorData>();

		private static List<IDisposable> undoGroups = new List<IDisposable>();

		public string FileName { get; set; }

		public int Offset { get; set; }

		public bool MoveCaretToReplace { get; set; }

		public int RemovedChars
		{
			get
			{
				return removedChars;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("RemovedChars", "needs to be >= 0");
				}
				removedChars = value;
			}
		}

		public string InsertedText { get; set; }

		protected virtual TextEditorData TextEditorData => GetTextEditorData(FileName);

		public static void FinishRefactoringOperation()
		{
			foreach (TextEditorData textEditorData in textEditorDatas)
			{
				textEditorData.Document.CommitUpdateAll();
			}
			textEditorDatas.Clear();
			undoGroups.ForEach(delegate(IDisposable grp)
			{
				grp.Dispose();
			});
			undoGroups.Clear();
		}

		internal static TextEditorData GetTextEditorData(string fileName)
		{
			if (IdeApp.Workbench == null)
			{
				return null;
			}
			foreach (Document document in IdeApp.Workbench.Documents)
			{
				if (document.FileName == (FilePath)fileName)
				{
					TextEditorData editor = document.Editor;
					if (editor != null)
					{
						textEditorDatas.Add(editor);
						undoGroups.Add(editor.OpenUndoGroup());
						return editor;
					}
				}
			}
			return null;
		}

		public override void PerformChange(IProgressMonitor monitor, RefactoringOptions rctx)
		{
			if (rctx == null)
			{
				throw new InvalidOperationException("Refactory context not available.");
			}
			TextEditorData textEditorData = TextEditorData;
			bool flag = false;
			bool hadBom = true;
			Encoding encoding = Encoding.UTF8;
			if (textEditorData == null)
			{
				textEditorData = TextFileProvider.Instance.GetTextEditorData(FileName, out hadBom, out encoding, out var _);
				flag = true;
			}
			int offset = textEditorData.Caret.Offset;
			int num = textEditorData.Replace(Offset, RemovedChars, InsertedText);
			if (MoveCaretToReplace)
			{
				textEditorData.Caret.Offset = Offset + num;
			}
			else if (Offset < offset)
			{
				int num2 = RemovedChars;
				if (Offset + num2 > offset)
				{
					num2 = offset - Offset;
				}
				textEditorData.Caret.Offset = offset - num2 + num;
			}
			if (flag)
			{
				TextFileUtility.WriteText(FileName, textEditorData.Text, encoding, hadBom);
			}
		}

		public override string ToString()
		{
			return $"[TextReplaceChange: FileName={FileName}, Offset={Offset}, RemovedChars={RemovedChars}, InsertedText={InsertedText}]";
		}
	}
}
