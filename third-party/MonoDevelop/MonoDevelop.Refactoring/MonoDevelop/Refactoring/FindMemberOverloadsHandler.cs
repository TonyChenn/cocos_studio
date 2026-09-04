using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Refactoring
{
	internal class FindMemberOverloadsHandler
	{
		private IMember entity;

		public bool IsValid
		{
			get
			{
				foreach (IMember member in entity.DeclaringType.GetMembers((IUnresolvedMember m) => m.Name == entity.Name && m.SymbolKind == entity.SymbolKind))
				{
					string fileName = member.Region.FileName;
					if (!string.IsNullOrEmpty(fileName))
					{
						return true;
					}
				}
				return false;
			}
		}

		public FindMemberOverloadsHandler(Document doc, IMember entity)
		{
			this.entity = entity;
		}

		public void Run()
		{
			using (ISearchProgressMonitor searchProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(bringToFront: true, focusPad: true))
			{
				foreach (IMember member in entity.DeclaringType.GetMembers((IUnresolvedMember m) => m.Name == entity.Name && m.SymbolKind == entity.SymbolKind))
				{
					string fileName = member.Region.FileName;
					if (!string.IsNullOrEmpty(fileName))
					{
						TextEditorData readOnlyTextEditorData = TextFileProvider.Instance.GetReadOnlyTextEditorData(fileName);
						int num = readOnlyTextEditorData.LocationToOffset(member.Region.Begin);
						readOnlyTextEditorData.SearchRequest.SearchPattern = member.Name;
						Mono.TextEditor.SearchResult searchResult = readOnlyTextEditorData.SearchForward(num);
						if (searchResult != null)
						{
							num = searchResult.Offset;
						}
						searchProgressMonitor.ReportResult(new MemberReference(member, member.Region, num, member.Name.Length));
					}
				}
			}
		}
	}
}
