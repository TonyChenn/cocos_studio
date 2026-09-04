using Mono.TextEditor;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	public interface IDebuggerExpressionResolver
	{
		string ResolveExpression(TextEditorData editor, Document doc, int offset, out int startOffset);
	}
}
