using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public class FindDerivedClassesHandler : CommandHandler
	{
		public static void FindDerivedClasses(ITypeDefinition cls)
		{
			FindDerivedSymbols(cls, null);
		}

		public static void FindDerivedMembers(IMember member)
		{
			ITypeDefinition declaringTypeDefinition = member.DeclaringTypeDefinition;
			if (declaringTypeDefinition != null)
			{
				FindDerivedSymbols(declaringTypeDefinition, member);
			}
		}

		private static void FindDerivedSymbols(ITypeDefinition cls, IMember member)
		{
			Solution currentSelectedSolution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution == null)
			{
				return;
			}
			Project project = TypeSystemService.GetProject(cls);
			if (project == null)
			{
				return;
			}
			List<ICompilation> list = (from c in ReferenceFinder.GetAllReferencingProjects(currentSelectedSolution, project).Select(TypeSystemService.GetCompilation)
				where c != null
				select c).ToList();
			ISearchProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(bringToFront: true, focusPad: true);
			try
			{
				string name = ((member == null) ? GettextCatalog.GetString("Searching for derived classes in solution...") : GettextCatalog.GetString("Searching for derived members in solution..."));
				monitor.BeginTask(name, list.Count);
				Parallel.ForEach(list, delegate(ICompilation comp)
				{
					try
					{
						SearchCompilation(monitor, comp, cls, member);
					}
					catch (Exception ex)
					{
						LoggingService.LogInternalError(ex);
						monitor.ReportError("Unhandled error while searching", ex);
					}
					monitor.Step(1);
				});
				monitor.EndTask();
			}
			finally
			{
				if (monitor != null)
				{
					monitor.Dispose();
				}
			}
		}

		private static void SearchCompilation(ISearchProgressMonitor monitor, ICompilation comp, ITypeDefinition cls, IMember member)
		{
			ITypeDefinition typeDefinition = comp.Import(cls);
			if (typeDefinition == null)
			{
				return;
			}
			IMember member2 = null;
			if (member != null)
			{
				member2 = comp.Import(member);
				if (member2 == null)
				{
					return;
				}
			}
			foreach (ITypeDefinition allTypeDefinition in comp.MainAssembly.GetAllTypeDefinitions())
			{
				if (!allTypeDefinition.IsDerivedFrom(typeDefinition))
				{
					continue;
				}
				IEntity entity;
				if (member != null)
				{
					entity = FindDerivedMember(member2, allTypeDefinition);
					if (entity == null)
					{
						continue;
					}
				}
				else
				{
					entity = allTypeDefinition;
				}
				ReportResult(monitor, entity);
			}
		}

		private static IMember FindDerivedMember(IMember importedMember, ITypeDefinition derivedType)
		{
			if (importedMember.DeclaringTypeDefinition.Kind == TypeKind.Interface)
			{
				return derivedType.GetMembers(null, GetMemberOptions.IgnoreInheritedMembers).FirstOrDefault((IMember m) => m.ImplementedInterfaceMembers.Any((IMember im) => im.Region == importedMember.Region));
			}
			return InheritanceHelper.GetDerivedMember(importedMember, derivedType);
		}

		private static void ReportResult(ISearchProgressMonitor monitor, IEntity result)
		{
			string fileName = result.Region.FileName;
			if (!string.IsNullOrEmpty(fileName))
			{
				TextEditorData textEditorData = TextFileProvider.Instance.GetTextEditorData(fileName);
				int num = textEditorData.LocationToOffset(result.Region.Begin);
				textEditorData.SearchRequest.SearchPattern = result.Name;
				Mono.TextEditor.SearchResult searchResult = textEditorData.SearchForward(num);
				if (searchResult != null)
				{
					num = searchResult.Offset;
				}
				if (textEditorData.Parent == null)
				{
					textEditorData.Dispose();
				}
				monitor.ReportResult(new MemberReference(result, result.Region, num, result.Name.Length));
			}
		}

		protected override void Run(object data)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.FileName == FilePath.Null)
			{
				return;
			}
			object item = CurrentRefactoryOperationsHandler.GetItem(activeDocument, out var _);
			if (item is ITypeDefinition typeDefinition && ((typeDefinition.Kind == TypeKind.Class && !typeDefinition.IsSealed) || typeDefinition.Kind == TypeKind.Interface))
			{
				FindDerivedClasses(typeDefinition);
				return;
			}
			IMember member = item as IMember;
			FindDerivedSymbolsHandler findDerivedSymbolsHandler = new FindDerivedSymbolsHandler(member);
			if (findDerivedSymbolsHandler.IsValid)
			{
				findDerivedSymbolsHandler.Run();
			}
		}
	}
}
