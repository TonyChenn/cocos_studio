using System;
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;
using Mono.TextEditor;

namespace MonoDevelop.SourceEditor.JSon
{
	internal class JSonIndentEngine : IStateMachineIndentEngine, IDocumentIndentEngine, ICloneable
	{
		private TextEditorData data;

		private int offset;

		private int line;

		private int column;

		internal Indent thisLineIndent;

		internal Indent nextLineIndent;

		private StringBuilder currentIndent;

		private char previousNewline;

		private char previousChar;

		private bool isLineStart;

		private bool isInString;

		public bool IsInsidePreprocessorDirective => false;

		public bool IsInsidePreprocessorComment => false;

		public bool IsInsideStringLiteral => false;

		public bool IsInsideVerbatimString => false;

		public bool IsInsideCharacter => false;

		public bool IsInsideString => isInString;

		public bool IsInsideLineComment => false;

		public bool IsInsideMultiLineComment => false;

		public bool IsInsideDocLineComment => false;

		public bool IsInsideComment => false;

		public bool IsInsideOrdinaryComment => false;

		public bool IsInsideOrdinaryCommentOrString => false;

		public bool LineBeganInsideVerbatimString => false;

		public bool LineBeganInsideMultiLineComment => false;

		public IDocument Document => data.Document;

		public string ThisLineIndent => thisLineIndent.IndentString;

		public string NextLineIndent => nextLineIndent.IndentString;

		public string CurrentIndent => currentIndent.ToString();

		public bool NeedsReindent => ThisLineIndent != CurrentIndent;

		public int Offset => offset;

		public TextLocation Location => new TextLocation(line, column);

		public bool EnableCustomIndentLevels { get; set; }

		public JSonIndentEngine(TextEditorData data)
		{
			this.data = data;
			Reset();
		}

		public IStateMachineIndentEngine Clone()
		{
			return (IStateMachineIndentEngine)MemberwiseClone();
		}

		public static ICSharpCode.NRefactory.CSharp.TextEditorOptions CreateNRefactoryTextEditorOptions(TextEditorData doc)
		{
			ICSharpCode.NRefactory.CSharp.TextEditorOptions textEditorOptions = new ICSharpCode.NRefactory.CSharp.TextEditorOptions();
			textEditorOptions.TabsToSpaces = doc.TabsToSpaces;
			textEditorOptions.TabSize = doc.Options.TabSize;
			textEditorOptions.IndentSize = doc.Options.IndentationSize;
			textEditorOptions.ContinuationIndent = doc.Options.IndentationSize;
			textEditorOptions.LabelIndent = -doc.Options.IndentationSize;
			textEditorOptions.EolMarker = doc.EolMarker;
			textEditorOptions.IndentBlankLines = doc.Options.IndentStyle != IndentStyle.Virtual;
			textEditorOptions.WrapLineLength = doc.Options.RulerColumn;
			return textEditorOptions;
		}

		public void Push(char ch)
		{
			bool flag = NewLine.IsNewLine(ch);
			if (!flag)
			{
				if (ch == '"')
				{
					isInString = !IsInsideString;
				}
				switch (ch)
				{
				case '[':
				case '{':
					nextLineIndent.Push(IndentType.Block);
					break;
				case ']':
				case '}':
					if (thisLineIndent.Count > 0)
					{
						thisLineIndent.Pop();
					}
					if (nextLineIndent.Count > 0)
					{
						nextLineIndent.Pop();
					}
					break;
				}
			}
			else if (ch == '\n' && previousChar == '\r')
			{
				offset++;
				previousChar = ch;
				return;
			}
			offset++;
			if (!flag)
			{
				previousNewline = '\0';
				isLineStart &= char.IsWhiteSpace(ch);
				if (isLineStart)
				{
					currentIndent.Append(ch);
				}
				if (ch == '\t')
				{
					int num = (column - 1 + data.Options.IndentationSize) / data.Options.IndentationSize;
					column = 1 + num * data.Options.IndentationSize;
				}
				else
				{
					column++;
				}
			}
			else
			{
				previousNewline = ch;
				currentIndent.Length = 0;
				isLineStart = true;
				column = 1;
				line++;
				thisLineIndent = nextLineIndent.Clone();
			}
			previousChar = ch;
		}

		public void Reset()
		{
			offset = 0;
			line = (column = 1);
			thisLineIndent = new Indent(CreateNRefactoryTextEditorOptions(data));
			nextLineIndent = new Indent(CreateNRefactoryTextEditorOptions(data));
			currentIndent = new StringBuilder();
			previousNewline = '\0';
			previousChar = '\0';
			isLineStart = true;
			isInString = false;
		}

		public void Update(int offset)
		{
			if (Offset > offset)
			{
				Reset();
			}
			while (Offset < offset)
			{
				Push(Document.GetCharAt(Offset));
			}
		}

		IDocumentIndentEngine IDocumentIndentEngine.Clone()
		{
			return Clone();
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
