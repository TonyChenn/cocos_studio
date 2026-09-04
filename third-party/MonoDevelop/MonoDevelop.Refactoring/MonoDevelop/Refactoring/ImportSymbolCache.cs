using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Refactoring
{
	internal class ImportSymbolCache
	{
		private Dictionary<string, GenerateNamespaceImport> cache = new Dictionary<string, GenerateNamespaceImport>();

		public GenerateNamespaceImport GetResult(IUnresolvedFile unit, IType type, Document doc)
		{
			if (cache.TryGetValue(type.Namespace, out var value))
			{
				return value;
			}
			value = new GenerateNamespaceImport();
			cache[type.Namespace] = value;
			TextEditorData editor = doc.Editor;
			value.InsertNamespace = false;
			TextLocation loc = new TextLocation(editor.Caret.Line, editor.Caret.Column);
			foreach (string usedNamespace in RefactoringOptions.GetUsedNamespaces(doc, loc))
			{
				if (type.Namespace == usedNamespace)
				{
					value.GenerateUsing = false;
					return value;
				}
			}
			value.GenerateUsing = true;
			string name = type.Name;
			foreach (string usedNamespace2 in RefactoringOptions.GetUsedNamespaces(doc, loc))
			{
				if (doc.Compilation.MainAssembly.GetTypeDefinition(usedNamespace2, name, type.TypeParameterCount) != null)
				{
					value.GenerateUsing = false;
					value.InsertNamespace = true;
					return value;
				}
			}
			return value;
		}
	}
}
