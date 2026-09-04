using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.Refactoring
{
	public class ImportSymbolHandler : CommandHandler
	{
		protected override void Run()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.FileName == FilePath.Null || activeDocument.ParsedDocument == null)
			{
				return;
			}
			ITextEditorExtension textEditorExtension = activeDocument.EditorExtension;
			while (textEditorExtension != null && !(textEditorExtension is CompletionTextEditorExtension))
			{
				textEditorExtension = textEditorExtension.Next;
			}
			if (textEditorExtension == null)
			{
				return;
			}
			ICompilation compilation = activeDocument.Compilation;
			ImportSymbolCache cache = new ImportSymbolCache();
			MemberLookup memberLookup = new MemberLookup(null, activeDocument.Compilation.MainAssembly);
			List<ImportSymbolCompletionData> list = new List<ImportSymbolCompletionData>();
			foreach (ITypeDefinition topLevelTypeDefiniton in compilation.GetTopLevelTypeDefinitons())
			{
				if (memberLookup.IsAccessible(topLevelTypeDefiniton, allowProtectedAccess: false))
				{
					list.Add(new ImportSymbolCompletionData(activeDocument, cache, topLevelTypeDefiniton));
				}
			}
			list.Sort((ImportSymbolCompletionData left, ImportSymbolCompletionData right) => left.Type.Name.CompareTo(right.Type.Name));
			CompletionDataList completionList = new CompletionDataList();
			completionList.IsSorted = true;
			list.ForEach(delegate(ImportSymbolCompletionData cd)
			{
				completionList.Add(cd);
			});
			((CompletionTextEditorExtension)textEditorExtension).ShowCompletion(completionList);
		}
	}
}
