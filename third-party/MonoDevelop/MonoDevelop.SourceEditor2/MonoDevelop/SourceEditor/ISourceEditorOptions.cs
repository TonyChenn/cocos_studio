using System;
using Mono.TextEditor;

namespace MonoDevelop.SourceEditor
{
	public interface ISourceEditorOptions : ITextEditorOptions, IDisposable
	{
		bool EnableAutoCodeCompletion { get; }

		bool DefaultRegionsFolding { get; }

		bool DefaultCommentFolding { get; }

		bool EnableSemanticHighlighting { get; }

		bool TabIsReindent { get; }

		bool AutoInsertMatchingBracket { get; }

		bool UnderlineErrors { get; }

		EditorFontType EditorFontType { get; }

		bool UseViModes { get; }
	}
}
