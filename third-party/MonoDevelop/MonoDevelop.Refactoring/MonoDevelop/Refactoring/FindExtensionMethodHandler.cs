using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	internal class FindExtensionMethodHandler
	{
		private ITypeDefinition entity;

		public FindExtensionMethodHandler(Document doc, ITypeDefinition entity)
		{
			this.entity = entity;
		}

		public void Run()
		{
			using (ISearchProgressMonitor searchProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(bringToFront: true, focusPad: true))
			{
				foreach (Project allProject in IdeApp.ProjectOperations.CurrentSelectedSolution.GetAllProjects())
				{
					ICompilation compilation = TypeSystemService.GetCompilation(allProject);
					foreach (ITypeDefinition allTypeDefinition in compilation.MainAssembly.GetAllTypeDefinitions())
					{
						if (!allTypeDefinition.IsStatic)
						{
							continue;
						}
						foreach (IMethod method in allTypeDefinition.GetMethods((IUnresolvedMethod m) => m.IsStatic))
						{
							if (!method.IsExtensionMethod)
							{
								continue;
							}
							ITypeDefinition typeDefinition = compilation.Import(entity);
							if (typeDefinition != null && CSharpResolver.IsEligibleExtensionMethod(typeDefinition, method, useTypeInference: true, out var _))
							{
								TextEditorData readOnlyTextEditorData = TextFileProvider.Instance.GetReadOnlyTextEditorData(method.Region.FileName);
								int num = readOnlyTextEditorData.LocationToOffset(method.Region.Begin);
								readOnlyTextEditorData.SearchRequest.SearchPattern = method.Name;
								Mono.TextEditor.SearchResult searchResult = readOnlyTextEditorData.SearchForward(num);
								if (searchResult != null)
								{
									num = searchResult.Offset;
								}
								searchProgressMonitor.ReportResult(new MemberReference(method, method.Region, num, method.Name.Length));
							}
						}
					}
				}
			}
		}
	}
}
