using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.AnalysisCore.Rules
{
	public static class Adapters
	{
		public static IUnresolvedFile GetCompilationUnit(Document input)
		{
			return input.ParsedDocument.ParsedFile;
		}
	}
}
