using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.CodeActions;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring.Rename;

namespace MonoDevelop.Refactoring
{
	public class CurrentRefactoryOperationsHandler : CommandHandler
	{
		private class JumpTo
		{
			private object el;

			public JumpTo(object el)
			{
				this.el = el;
			}

			public void Run()
			{
				if (el is IUnresolvedEntity)
				{
					IUnresolvedEntity unresolvedEntity = (IUnresolvedEntity)el;
					IdeApp.Workbench.OpenDocument(unresolvedEntity.Region.FileName, unresolvedEntity.Region.BeginLine, unresolvedEntity.Region.BeginColumn);
					return;
				}
				if (el is IVariable)
				{
					IdeApp.ProjectOperations.JumpToDeclaration((IVariable)el);
				}
				if (el is INamedElement)
				{
					IdeApp.ProjectOperations.JumpToDeclaration((INamedElement)el);
				}
			}
		}

		private class GotoBase
		{
			private IEntity item;

			public GotoBase(IEntity item)
			{
				this.item = item;
			}

			public void Run()
			{
				if (item is ITypeDefinition typeDefinition && typeDefinition.DirectBaseTypes != null)
				{
					foreach (IType directBaseType in typeDefinition.DirectBaseTypes)
					{
						ITypeDefinition definition = directBaseType.GetDefinition();
						if (definition != null && definition.Kind != TypeKind.Interface)
						{
							IdeApp.ProjectOperations.JumpToDeclaration(definition);
							return;
						}
					}
				}
				if (item is IMember member)
				{
					IMember baseMember = InheritanceHelper.GetBaseMember(member);
					if (baseMember != null)
					{
						IdeApp.ProjectOperations.JumpToDeclaration(baseMember);
					}
				}
			}
		}

		private class FindRefs
		{
			private object obj;

			private bool allOverloads;

			public FindRefs(object obj, bool all)
			{
				this.obj = obj;
				allOverloads = all;
			}

			public void Run()
			{
				if (allOverloads)
				{
					FindAllReferencesHandler.FindRefs(obj);
				}
				else
				{
					FindReferencesHandler.FindRefs(obj);
				}
			}
		}

		private class FindDerivedClasses
		{
			private ITypeDefinition type;

			public FindDerivedClasses(ITypeDefinition type)
			{
				this.type = type;
			}

			public void Run()
			{
				FindDerivedClassesHandler.FindDerivedClasses(type);
			}
		}

		private class RefactoringDocumentInfo
		{
			public IEnumerable<CodeAction> validActions;

			public ParsedDocument lastDocument;

			public override string ToString()
			{
				return string.Format("[RefactoringDocumentInfo: #validActions={0}, lastDocument={1}]", (validActions != null) ? validActions.Count().ToString() : "null", lastDocument);
			}
		}

		private class RefactoringOperationWrapper
		{
			private RefactoringOperation refactoring;

			private RefactoringOptions options;

			public RefactoringOperationWrapper(RefactoringOperation refactoring, RefactoringOptions options)
			{
				this.refactoring = refactoring;
				this.options = options;
			}

			public void Operation()
			{
				refactoring.Run(options);
			}
		}

		private DocumentLocation lastLocation;

		protected override void Run(object data)
		{
			((Action)data)?.Invoke();
		}

		public static ResolveResult GetResolveResult(Document doc)
		{
			return doc.GetContent<ITextEditorResolver>()?.GetLanguageItem(doc.Editor.IsSomethingSelected ? doc.Editor.SelectionRange.Offset : doc.Editor.Caret.Offset);
		}

		public static object GetItem(Document doc, out ResolveResult resolveResult)
		{
			resolveResult = GetResolveResult(doc);
			if (resolveResult is LocalResolveResult)
			{
				return ((LocalResolveResult)resolveResult).Variable;
			}
			if (resolveResult is MemberResolveResult)
			{
				return ((MemberResolveResult)resolveResult).Member;
			}
			if (resolveResult is MethodGroupResolveResult)
			{
				MethodGroupResolveResult methodGroupResolveResult = (MethodGroupResolveResult)resolveResult;
				IMethod method = methodGroupResolveResult.Methods.FirstOrDefault();
				if (method == null && methodGroupResolveResult.GetExtensionMethods().Any())
				{
					method = methodGroupResolveResult.GetExtensionMethods().First().FirstOrDefault();
				}
				return method;
			}
			if (resolveResult is TypeResolveResult)
			{
				return resolveResult.Type;
			}
			if (resolveResult is NamespaceResolveResult)
			{
				return ((NamespaceResolveResult)resolveResult).Namespace;
			}
			if (resolveResult is OperatorResolveResult)
			{
				return ((OperatorResolveResult)resolveResult).UserDefinedOperatorMethod;
			}
			return null;
		}

		private static bool HasOverloads(Solution solution, object item)
		{
			if (item is IMember member && member.ImplementedInterfaceMembers.Any())
			{
				return true;
			}
			IMethod method = item as IMethod;
			if (method == null)
			{
				return false;
			}
			return method.DeclaringType.GetMethods((IUnresolvedMethod m) => m.Name == method.Name).Count() > 1;
		}

		protected override void Update(CommandArrayInfo ainfo)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.FileName == FilePath.Null)
			{
				return;
			}
			ParsedDocument parsedDocument = activeDocument.ParsedDocument;
			if (parsedDocument == null || parsedDocument.IsInvalid)
			{
				return;
			}
			object item = GetItem(activeDocument, out var resolveResult);
			bool flag = false;
			RefactoringOptions refactoringOptions = new RefactoringOptions(activeDocument);
			refactoringOptions.ResolveResult = resolveResult;
			refactoringOptions.SelectedItem = item;
			RefactoringOptions options = refactoringOptions;
			CommandInfoSet commandInfoSet = new CommandInfoSet();
			commandInfoSet.Text = GettextCatalog.GetString("Refactor");
			if (item is IVariable || item is IParameter || ((item is ITypeDefinition) ? (!((ITypeDefinition)item).Region.IsEmpty) : ((item is IType) ? (((IType)item).Kind == TypeKind.TypeParameter) : ((item is IMember) ? (!((IMember)item).Region.IsEmpty) : ((item is INamespace) ? true : false)))))
			{
				commandInfoSet.CommandInfos.Add(IdeApp.CommandService.GetCommandInfo(EditCommands.Rename), (Action)delegate
				{
					new RenameHandler().Start(null);
				});
				flag = true;
			}
			foreach (RefactoringOperation refactoring in RefactoringService.Refactorings)
			{
				if (refactoring.IsValid(options))
				{
					CommandInfo commandInfo = new CommandInfo(refactoring.GetMenuDescription(options));
					commandInfo.AccelKey = refactoring.AccelKey;
					commandInfoSet.CommandInfos.Add(commandInfo, new Action(new RefactoringOperationWrapper(refactoring, options).Operation));
				}
			}
			RefactoringDocumentInfo refactoringDocumentInfo = activeDocument.Annotation<RefactoringDocumentInfo>();
			if (refactoringDocumentInfo == null)
			{
				refactoringDocumentInfo = new RefactoringDocumentInfo();
				activeDocument.AddAnnotation(refactoringDocumentInfo);
			}
			DocumentLocation location = activeDocument.Editor.Caret.Location;
			bool flag2 = true;
			if (refactoringDocumentInfo.lastDocument != activeDocument.ParsedDocument || location != lastLocation)
			{
				try
				{
					refactoringDocumentInfo.validActions = RefactoringService.GetValidActions(activeDocument, location, new CancellationTokenSource(500).Token);
				}
				catch (TaskCanceledException)
				{
				}
				catch (AggregateException ex2)
				{
					ex2.Flatten().Handle((Exception x) => x is TaskCanceledException);
				}
				lastLocation = location;
				refactoringDocumentInfo.lastDocument = activeDocument.ParsedDocument;
			}
			if (refactoringDocumentInfo.validActions != null && refactoringDocumentInfo.lastDocument != null && refactoringDocumentInfo.lastDocument.CreateRefactoringContext != null)
			{
				IRefactoringContext context = refactoringDocumentInfo.lastDocument.CreateRefactoringContext(activeDocument, CancellationToken.None);
				foreach (CodeAction item2 in refactoringDocumentInfo.validActions.OrderByDescending((CodeAction i) => Tuple.Create(CodeActionEditorExtension.IsAnalysisOrErrorFix(i), (int)i.Severity, CodeActionEditorExtension.GetUsage(i.IdString))))
				{
					if (CodeActionEditorExtension.IsAnalysisOrErrorFix(item2))
					{
						continue;
					}
					CodeAction fix = item2;
					if (flag2)
					{
						flag2 = false;
						if (commandInfoSet.CommandInfos.Count > 0)
						{
							commandInfoSet.CommandInfos.AddSeparator();
						}
					}
					commandInfoSet.CommandInfos.Add(fix.Title, (Action)delegate
					{
						RefactoringService.ApplyFix(fix, context);
					});
				}
			}
			if (commandInfoSet.CommandInfos.Count > 0)
			{
				ainfo.Add(commandInfoSet, null);
				flag = true;
			}
			if (IdeApp.ProjectOperations.CanJumpToDeclaration(item))
			{
				if (item is IType type && type.GetDefinition().Parts.Count > 1)
				{
					CommandInfoSet commandInfoSet2 = new CommandInfoSet();
					commandInfoSet2.Text = GettextCatalog.GetString("_Go to Declaration");
					ITypeDefinition definition = type.GetDefinition();
					foreach (IUnresolvedTypeDefinition part in definition.Parts)
					{
						commandInfoSet2.CommandInfos.Add(string.Format(GettextCatalog.GetString("{0}, Line {1}"), FormatFileName(part.Region.FileName), part.Region.BeginLine), new Action(new JumpTo(part).Run));
					}
					ainfo.Add(commandInfoSet2);
				}
				else
				{
					ainfo.Add(IdeApp.CommandService.GetCommandInfo(RefactoryCommands.GotoDeclaration), new Action(new JumpTo(item).Run));
				}
				flag = true;
			}
			if (item is IMember)
			{
				IMember member = (IMember)item;
				if (member.IsOverride || member.ImplementedInterfaceMembers.Any())
				{
					ainfo.Add(GettextCatalog.GetString("Go to _Base Symbol"), new Action(new GotoBase(member).Run));
					flag = true;
				}
			}
			if ((!(item is IMethod) || ((IMethod)item).SymbolKind != SymbolKind.Operator) && (item is IEntity || item is ITypeParameter || item is IVariable || item is INamespace))
			{
				ainfo.Add(IdeApp.CommandService.GetCommandInfo(RefactoryCommands.FindReferences), new Action(new FindRefs(item, all: false).Run));
				if (activeDocument.HasProject && HasOverloads(activeDocument.Project.ParentSolution, item))
				{
					ainfo.Add(IdeApp.CommandService.GetCommandInfo(RefactoryCommands.FindAllReferences), new Action(new FindRefs(item, all: true).Run));
				}
				flag = true;
			}
			if (item is IMember)
			{
				IMember member2 = (IMember)item;
				FindDerivedSymbolsHandler findDerivedSymbolsHandler = new FindDerivedSymbolsHandler(member2);
				if (findDerivedSymbolsHandler.IsValid)
				{
					CommandInfo commandInfo2 = ainfo.Add(GettextCatalog.GetString("Find Derived Symbols"), new Action(findDerivedSymbolsHandler.Run));
					commandInfo2.AccelKey = IdeApp.CommandService.GetCommandInfo(RefactoryCommands.FindDerivedClasses).AccelKey;
					flag = true;
				}
			}
			if (item is IMember)
			{
				IMember member3 = (IMember)item;
				if (member3.SymbolKind == SymbolKind.Method || member3.SymbolKind == SymbolKind.Indexer)
				{
					FindMemberOverloadsHandler findMemberOverloadsHandler = new FindMemberOverloadsHandler(activeDocument, member3);
					if (findMemberOverloadsHandler.IsValid)
					{
						ainfo.Add(GettextCatalog.GetString("Find Member Overloads"), new Action(findMemberOverloadsHandler.Run));
						flag = true;
					}
				}
			}
			if (item is ITypeDefinition)
			{
				ITypeDefinition typeDefinition = (ITypeDefinition)item;
				foreach (IType directBaseType in typeDefinition.DirectBaseTypes)
				{
					if (directBaseType != null && directBaseType.GetDefinition() != null && directBaseType.GetDefinition().Kind != TypeKind.Interface)
					{
						ainfo.Add(GettextCatalog.GetString("Go to _Base"), new Action(new GotoBase((ITypeDefinition)item).Run));
						break;
					}
				}
				if ((typeDefinition.Kind == TypeKind.Class && !typeDefinition.IsSealed) || typeDefinition.Kind == TypeKind.Interface)
				{
					string text = ((typeDefinition.Kind != TypeKind.Interface) ? GettextCatalog.GetString("Find _derived classes") : GettextCatalog.GetString("Find _implementor classes"));
					CommandInfo commandInfo3 = ainfo.Add(text, new Action(new FindDerivedClasses(typeDefinition).Run));
					commandInfo3.AccelKey = IdeApp.CommandService.GetCommandInfo(RefactoryCommands.FindDerivedClasses).AccelKey;
				}
				ainfo.Add(GettextCatalog.GetString("Find Extension Methods"), new Action(new FindExtensionMethodHandler(activeDocument, typeDefinition).Run));
				flag = true;
			}
			if (flag)
			{
				ainfo.AddSeparator();
			}
		}

		private bool IsModifiable(object member)
		{
			if (member is IType type)
			{
				return (FilePath)type.GetDefinition().Region.FileName == IdeApp.Workbench.ActiveDocument.FileName;
			}
			if (member is IMember)
			{
				return (FilePath)((IMember)member).DeclaringTypeDefinition.Region.FileName == IdeApp.Workbench.ActiveDocument.FileName;
			}
			return false;
		}

		private static string FormatFileName(string fileName)
		{
			if (fileName == null)
			{
				return null;
			}
			char[] anyOf = new char[2]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			};
			int num = fileName.LastIndexOfAny(anyOf);
			if (num > 0)
			{
				num = fileName.LastIndexOfAny(anyOf, num - 1);
			}
			if (num > 0)
			{
				return "..." + fileName.Substring(num);
			}
			return fileName;
		}

		public static bool ContainsAbstractMembers(IType cls)
		{
			return cls?.GetMembers().Any((IMember m) => m.IsAbstract) ?? false;
		}
	}
}
