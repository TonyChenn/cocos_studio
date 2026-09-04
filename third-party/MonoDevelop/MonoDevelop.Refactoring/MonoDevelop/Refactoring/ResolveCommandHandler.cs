using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Completion;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public class ResolveCommandHandler : CommandHandler
	{
		public class PossibleNamespace
		{
			public string Namespace { get; private set; }

			public bool IsAccessibleWithGlobalUsing { get; private set; }

			public bool OnlyAddReference
			{
				get
				{
					if (!IsAccessibleWithGlobalUsing)
					{
						return Reference != null;
					}
					return false;
				}
			}

			public MonoDevelop.Projects.ProjectReference Reference { get; private set; }

			public PossibleNamespace(string @namespace, bool isAccessibleWithGlobalUsing, MonoDevelop.Projects.ProjectReference reference = null)
			{
				Namespace = @namespace;
				IsAccessibleWithGlobalUsing = isAccessibleWithGlobalUsing;
				Reference = reference;
			}

			private string GetLibraryName()
			{
				string reference = Reference.Reference;
				int num = reference.IndexOf(',');
				if (num >= 0)
				{
					return reference.Substring(0, num);
				}
				return reference;
			}

			public string GetImportText()
			{
				if (OnlyAddReference)
				{
					return GettextCatalog.GetString("Reference '{0}'", GetLibraryName().Replace("_", "__"));
				}
				if (Reference != null)
				{
					return GettextCatalog.GetString("Reference '{0}' and use '{1}'", GetLibraryName(), string.Format("using {0};", Namespace.Replace("_", "__")));
				}
				return string.Format("using {0};", Namespace.Replace("_", "__"));
			}

			public string GetInsertNamespaceText(string member)
			{
				if (Reference != null)
				{
					return GettextCatalog.GetString("Reference '{0}' and use '{1}'", GetLibraryName().Replace("_", "__"), (Namespace + "." + member).Replace("_", "__"));
				}
				return (Namespace + "." + member).Replace("_", "__");
			}
		}

		internal class AddImport
		{
			private readonly Document doc;

			private readonly ResolveResult resolveResult;

			private readonly string ns;

			private readonly bool addUsing;

			private readonly AstNode node;

			private readonly MonoDevelop.Projects.ProjectReference reference;

			public AddImport(Document doc, ResolveResult resolveResult, string ns, MonoDevelop.Projects.ProjectReference reference, bool addUsing, AstNode node)
			{
				this.doc = doc;
				this.resolveResult = resolveResult;
				this.ns = ns;
				this.reference = reference;
				this.addUsing = addUsing;
				this.node = node;
			}

			public void Run()
			{
				DocumentLocation location = doc.Editor.Caret.Location;
				if (reference != null)
				{
					Project project = doc.Project;
					project.Items.Add(reference);
					IdeApp.ProjectOperations.Save(project);
				}
				if (string.IsNullOrEmpty(ns))
				{
					return;
				}
				if (!addUsing)
				{
					int offset = doc.Editor.LocationToOffset(node.StartLocation);
					doc.Editor.Insert(offset, ns + ".");
					doc.Editor.Document.CommitLineUpdate(location.Line);
					return;
				}
				CodeGenerator codeGenerator = doc.CreateCodeGenerator();
				if (resolveResult is NamespaceResolveResult)
				{
					codeGenerator.AddLocalNamespaceImport(doc, ns, location);
				}
				else
				{
					codeGenerator.AddGlobalNamespaceImport(doc, ns);
				}
			}
		}

		public static bool ResolveAt(Document doc, out ResolveResult resolveResult, out AstNode node, CancellationToken token = default(CancellationToken))
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			TextEditorData editor = doc.Editor;
			if (editor == null || editor.MimeType != "text/x-csharp")
			{
				node = null;
				resolveResult = null;
				return false;
			}
			if (!InternalResolveAt(doc, out resolveResult, out node))
			{
				DocumentLocation correctResolveLocation = RefactoringService.GetCorrectResolveLocation(doc, editor.Caret.Location);
				resolveResult = GetHeuristicResult(doc, correctResolveLocation, ref node);
				if (resolveResult == null)
				{
					return false;
				}
			}
			if (node is ObjectCreateExpression objectCreateExpression)
			{
				node = objectCreateExpression.Type;
			}
			return true;
		}

		private static bool InternalResolveAt(Document doc, out ResolveResult resolveResult, out AstNode node, CancellationToken token = default(CancellationToken))
		{
			ParsedDocument parsedDocument = doc.ParsedDocument;
			resolveResult = null;
			node = null;
			if (parsedDocument == null)
			{
				return false;
			}
			SyntaxTree ast = parsedDocument.GetAst<SyntaxTree>();
			CSharpUnresolvedFile cSharpUnresolvedFile = parsedDocument.ParsedFile as CSharpUnresolvedFile;
			if (ast == null || cSharpUnresolvedFile == null)
			{
				return false;
			}
			try
			{
				DocumentLocation correctResolveLocation = RefactoringService.GetCorrectResolveLocation(doc, doc.Editor.Caret.Location);
				resolveResult = ResolveAtLocation.Resolve(doc.Compilation, cSharpUnresolvedFile, ast, correctResolveLocation, out node, token);
				if (resolveResult == null || node is Statement)
				{
					return false;
				}
			}
			catch (OperationCanceledException)
			{
				return false;
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Got resolver exception:" + ex2);
				return false;
			}
			return true;
		}

		protected override void Update(CommandArrayInfo ainfo)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.FileName == FilePath.Null || activeDocument.ParsedDocument == null || !ResolveAt(activeDocument, out var resolveResult, out var node))
			{
				return;
			}
			CommandInfoSet commandInfoSet = new CommandInfoSet();
			commandInfoSet.Text = GettextCatalog.GetString("Resolve");
			List<PossibleNamespace> possibleNamespaces = GetPossibleNamespaces(activeDocument, node, ref resolveResult);
			foreach (PossibleNamespace item in possibleNamespaces.Where((PossibleNamespace tp) => tp.OnlyAddReference))
			{
				MonoDevelop.Projects.ProjectReference reference = item.Reference;
				CommandInfo commandInfo = commandInfoSet.CommandInfos.Add(item.GetImportText(), new Action(new AddImport(activeDocument, resolveResult, null, reference, addUsing: true, node).Run));
				commandInfo.Icon = MonoDevelop.Ide.Gui.Stock.AddNamespace;
			}
			if (!(resolveResult is AmbiguousTypeResolveResult))
			{
				foreach (PossibleNamespace item2 in possibleNamespaces.Where((PossibleNamespace tp) => tp.IsAccessibleWithGlobalUsing))
				{
					string ns = item2.Namespace;
					MonoDevelop.Projects.ProjectReference reference2 = item2.Reference;
					CommandInfo commandInfo2 = commandInfoSet.CommandInfos.Add(item2.GetImportText(), new Action(new AddImport(activeDocument, resolveResult, ns, reference2, addUsing: true, node).Run));
					commandInfo2.Icon = MonoDevelop.Ide.Gui.Stock.AddNamespace;
				}
			}
			if (!(resolveResult is UnknownMemberResolveResult))
			{
				if (commandInfoSet.CommandInfos.Count > 0)
				{
					commandInfoSet.CommandInfos.AddSeparator();
				}
				foreach (PossibleNamespace item3 in possibleNamespaces)
				{
					string ns2 = item3.Namespace;
					MonoDevelop.Projects.ProjectReference reference3 = item3.Reference;
					commandInfoSet.CommandInfos.Add(item3.GetInsertNamespaceText(activeDocument.Editor.GetTextBetween(node.StartLocation, node.EndLocation)), new Action(new AddImport(activeDocument, resolveResult, ns2, reference3, addUsing: false, node).Run));
				}
			}
			if (commandInfoSet.CommandInfos.Count > 0)
			{
				ainfo.Insert(0, commandInfoSet);
			}
		}

		private static string CreateStub(Document doc, int offset)
		{
			if (offset <= 0)
			{
				return "";
			}
			string textAt = doc.Editor.GetTextAt(0, Math.Min(doc.Editor.Length, offset));
			StringBuilder stringBuilder = new StringBuilder(textAt);
			CSharpCompletionEngineBase.AppendMissingClosingBrackets(stringBuilder, appendSemicolon: false);
			return stringBuilder.ToString();
		}

		private static ResolveResult GetHeuristicResult(Document doc, DocumentLocation location, ref AstNode node)
		{
			TextEditorData editor = doc.Editor;
			if (editor == null || editor.Caret == null)
			{
				return null;
			}
			int num = editor.Caret.Offset;
			bool flag = false;
			bool flag2 = false;
			while (num < editor.Length)
			{
				char charAt = editor.GetCharAt(num);
				bool flag3 = char.IsLetterOrDigit(charAt) || charAt == '_';
				bool flag4 = char.IsWhiteSpace(charAt);
				bool flag5 = charAt == '.' || charAt == '<' || charAt == '>';
				if ((!flag || !flag2) && (flag3 || flag4 || flag5))
				{
					if (flag5)
					{
						flag2 = false;
						flag = false;
					}
					num++;
					flag |= flag3;
					if (flag)
					{
						flag2 |= flag4;
					}
					continue;
				}
				for (num--; num > 1; num--)
				{
					charAt = editor.GetCharAt(num - 1);
					if (charAt != '.' && !char.IsWhiteSpace(charAt))
					{
						break;
					}
				}
				break;
			}
			SyntaxTree syntaxTree = SyntaxTree.Parse(CreateStub(doc, num), doc.FileName);
			ParsedDocument parsedDocument = doc.ParsedDocument;
			if (parsedDocument == null)
			{
				return null;
			}
			return ResolveAtLocation.Resolve(doc.Compilation, parsedDocument.ParsedFile as CSharpUnresolvedFile, syntaxTree, location, out node);
		}

		public static List<PossibleNamespace> GetPossibleNamespaces(Document doc, AstNode node, ref ResolveResult resolveResult)
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			DocumentLocation correctResolveLocation = RefactoringService.GetCorrectResolveLocation(doc, doc.Editor.Caret.Location);
			if (resolveResult == null || resolveResult.Type.FullName == "System.Void")
			{
				resolveResult = GetHeuristicResult(doc, correctResolveLocation, ref node) ?? resolveResult;
			}
			IEnumerable<PossibleNamespace> enumerable = GetPossibleNamespaces(doc, node, resolveResult, correctResolveLocation);
			if (!(resolveResult is AmbiguousTypeResolveResult))
			{
				List<string> usedNamespaces = RefactoringOptions.GetUsedNamespaces(doc, correctResolveLocation);
				enumerable = enumerable.Where((PossibleNamespace n) => !usedNamespaces.Contains(n.Namespace));
			}
			List<PossibleNamespace> list = new List<PossibleNamespace>();
			foreach (PossibleNamespace ns in enumerable)
			{
				Func<PossibleNamespace, bool> predicate = (PossibleNamespace n) => n.Namespace == ns.Namespace;
				if (!list.Any(predicate))
				{
					list.Add(ns);
				}
			}
			return list;
		}

		private static int GetTypeParameterCount(AstNode node)
		{
			if (node is ObjectCreateExpression)
			{
				node = ((ObjectCreateExpression)node).Type;
			}
			if (node is SimpleType)
			{
				return ((SimpleType)node).TypeArguments.Count;
			}
			if (node is MemberType)
			{
				return ((MemberType)node).TypeArguments.Count;
			}
			if (node is IdentifierExpression)
			{
				return ((IdentifierExpression)node).TypeArguments.Count;
			}
			return 0;
		}

		internal static bool CanBeReferenced(Project project, SystemAssembly systemAssembly)
		{
			if (!(project is DotNetProject dotNetProject))
			{
				return false;
			}
			string assemblyNameForVersion = dotNetProject.TargetRuntime.AssemblyContext.GetAssemblyNameForVersion(systemAssembly.FullName, dotNetProject.TargetFramework);
			return !string.IsNullOrEmpty(assemblyNameForVersion);
		}

		private static bool CanReference(Document doc, MonoDevelop.Projects.ProjectReference projectReference)
		{
			if (!(doc.Project is DotNetProject dotNetProject) || projectReference == null || dotNetProject.ParentSolution == null)
			{
				return true;
			}
			ReferenceType referenceType = projectReference.ReferenceType;
			if (referenceType == ReferenceType.Project)
			{
				if (!(projectReference.ResolveProject(dotNetProject.ParentSolution) is DotNetProject targetProject))
				{
					return true;
				}
				string reason;
				return dotNetProject.CanReferenceProject(targetProject, out reason);
			}
			return true;
		}

		private static IEnumerable<PossibleNamespace> GetPossibleNamespaces(Document doc, AstNode node, ResolveResult resolveResult, DocumentLocation location)
		{
			SyntaxTree unit = doc.ParsedDocument.GetAst<SyntaxTree>();
			if (unit == null)
			{
				yield break;
			}
			Project project = doc.Project;
			if (project == null)
			{
				yield break;
			}
			int tc = GetTypeParameterCount(node);
			bool isInsideAttributeType = unit.GetNodeAt<ICSharpCode.NRefactory.CSharp.Attribute>(location)?.Type.Contains(location) ?? false;
			List<Tuple<ICompilation, MonoDevelop.Projects.ProjectReference>> compilations = new List<Tuple<ICompilation, MonoDevelop.Projects.ProjectReference>> { Tuple.Create<ICompilation, MonoDevelop.Projects.ProjectReference>(doc.Compilation, null) };
			IEnumerable<SolutionItem> referencedItems = ((IdeApp.Workspace != null) ? project.GetReferencedItems(IdeApp.Workspace.ActiveConfiguration).ToList() : ((IEnumerable<SolutionItem>)new SolutionItem[0]));
			Solution solution = project?.ParentSolution;
			if (solution != null)
			{
				foreach (Project allProject in solution.GetAllProjects())
				{
					if (allProject == project || referencedItems.Contains(allProject))
					{
						continue;
					}
					IEnumerable<SolutionItem> source = ((IdeApp.Workspace != null) ? allProject.GetReferencedItems(IdeApp.Workspace.ActiveConfiguration).ToList() : ((IEnumerable<SolutionItem>)new SolutionItem[0]));
					if (!source.Contains(project))
					{
						ICompilation compilation = TypeSystemService.GetCompilation(allProject);
						if (compilation != null)
						{
							compilations.Add(Tuple.Create(compilation, new MonoDevelop.Projects.ProjectReference(allProject)));
						}
					}
				}
			}
			if (!(project is DotNetProject netProject))
			{
				yield break;
			}
			if (!TypeSystemService.TryGetFrameworkLookup(netProject, out var frameworkLookup))
			{
				frameworkLookup = null;
			}
			if (frameworkLookup != null && resolveResult is UnknownMemberResolveResult)
			{
				UnknownMemberResolveResult unknownMemberResolveResult = (UnknownMemberResolveResult)resolveResult;
				try
				{
					foreach (FrameworkLookup.AssemblyLookup extensionMethodLookup in frameworkLookup.GetExtensionMethodLookups(unknownMemberResolveResult))
					{
						SystemAssembly assemblyFromFullName = netProject.AssemblyContext.GetAssemblyFromFullName(extensionMethodLookup.FullName, extensionMethodLookup.Package, netProject.TargetFramework);
						if (assemblyFromFullName != null && CanBeReferenced(doc.Project, assemblyFromFullName))
						{
							compilations.Add(Tuple.Create(TypeSystemService.GetCompilation(assemblyFromFullName, doc.Compilation), new MonoDevelop.Projects.ProjectReference(assemblyFromFullName)));
						}
					}
				}
				catch (Exception ex)
				{
					if (!TypeSystemService.RecreateFrameworkLookup(netProject))
					{
						LoggingService.LogError($"Error while looking up extension method {unknownMemberResolveResult.MemberName}", ex);
					}
				}
			}
			bool foundIdentifier = false;
			MemberLookup lookup = new MemberLookup(null, doc.Compilation.MainAssembly);
			foreach (Tuple<ICompilation, MonoDevelop.Projects.ProjectReference> comp in compilations)
			{
				ICompilation compilation2 = comp.Item1;
				MonoDevelop.Projects.ProjectReference requiredReference = comp.Item2;
				if (resolveResult is AmbiguousTypeResolveResult)
				{
					if (compilation2 != doc.Compilation)
					{
						continue;
					}
					AmbiguousTypeResolveResult aResult = resolveResult as AmbiguousTypeResolveResult;
					CSharpUnresolvedFile file = doc.ParsedDocument.ParsedFile as CSharpUnresolvedFile;
					for (ResolvedUsingScope scope = file.GetUsingScope(location).Resolve(compilation2); scope != null; scope = scope.Parent)
					{
						foreach (INamespace u in scope.Usings)
						{
							foreach (ITypeDefinition typeDefinition in u.Types)
							{
								if (typeDefinition.Name == aResult.Type.Name && typeDefinition.TypeParameterCount == tc && lookup.IsAccessible(typeDefinition, allowProtectedAccess: false) && CanReference(doc, requiredReference))
								{
									yield return new PossibleNamespace(typeDefinition.Namespace, isAccessibleWithGlobalUsing: true, requiredReference);
								}
							}
						}
					}
				}
				IEnumerable<ITypeDefinition> allTypes = ((compilation2 == doc.Compilation) ? compilation2.GetAllTypeDefinitions() : compilation2.MainAssembly.GetAllTypeDefinitions());
				if (resolveResult is UnknownIdentifierResolveResult)
				{
					UnknownIdentifierResolveResult uiResult = resolveResult as UnknownIdentifierResolveResult;
					string possibleAttributeName = (isInsideAttributeType ? (uiResult.Identifier + "Attribute") : uiResult.Identifier);
					foreach (ITypeDefinition typeDefinition2 in allTypes)
					{
						if ((typeDefinition2.Name == possibleAttributeName || typeDefinition2.Name == uiResult.Identifier) && typeDefinition2.TypeParameterCount == tc && lookup.IsAccessible(typeDefinition2, allowProtectedAccess: false) && CanReference(doc, requiredReference))
						{
							if (typeDefinition2.DeclaringTypeDefinition != null)
							{
								TypeSystemAstBuilder builder = new TypeSystemAstBuilder(new CSharpResolver(doc.Compilation));
								foundIdentifier = true;
								yield return new PossibleNamespace(builder.ConvertType(typeDefinition2.DeclaringTypeDefinition).ToString(), isAccessibleWithGlobalUsing: false, requiredReference);
							}
							else
							{
								foundIdentifier = true;
								yield return new PossibleNamespace(typeDefinition2.Namespace, isAccessibleWithGlobalUsing: true, requiredReference);
							}
						}
					}
				}
				if (resolveResult is UnknownMemberResolveResult)
				{
					UnknownMemberResolveResult umResult = (UnknownMemberResolveResult)resolveResult;
					string possibleAttributeName2 = (isInsideAttributeType ? (umResult.MemberName + "Attribute") : umResult.MemberName);
					foreach (ITypeDefinition typeDefinition3 in allTypes.Where((ITypeDefinition t) => t.HasExtensionMethods))
					{
						if (!lookup.IsAccessible(typeDefinition3, allowProtectedAccess: false))
						{
							continue;
						}
						foreach (IMethod method in typeDefinition3.Methods.Where((IMethod m) => m.IsExtensionMethod && (m.Name == possibleAttributeName2 || m.Name == umResult.MemberName)))
						{
							if (lookup.IsAccessible(method, allowProtectedAccess: false) && CSharpResolver.IsEligibleExtensionMethod(compilation2.Import(umResult.TargetType), method, useTypeInference: true, out var _))
							{
								if (CanReference(doc, requiredReference))
								{
									yield return new PossibleNamespace(typeDefinition3.Namespace, isAccessibleWithGlobalUsing: true, requiredReference);
								}
								break;
							}
						}
					}
				}
				if (!(resolveResult is ErrorResolveResult))
				{
					continue;
				}
				Identifier identifier = unit?.GetNodeAt<Identifier>(location);
				if (identifier == null || !(resolveResult is UnknownIdentifierResolveResult uiResult2))
				{
					continue;
				}
				string possibleAttributeName3 = (isInsideAttributeType ? (uiResult2.Identifier + "Attribute") : uiResult2.Identifier);
				foreach (ITypeDefinition typeDefinition4 in allTypes)
				{
					if ((identifier.Name == possibleAttributeName3 || identifier.Name == uiResult2.Identifier) && typeDefinition4.TypeParameterCount == tc && lookup.IsAccessible(typeDefinition4, allowProtectedAccess: false) && CanReference(doc, requiredReference))
					{
						yield return new PossibleNamespace(typeDefinition4.Namespace, isAccessibleWithGlobalUsing: true, requiredReference);
					}
				}
			}
			if (!foundIdentifier && frameworkLookup != null && resolveResult is UnknownIdentifierResolveResult && node is AstType && resolveResult is UnknownIdentifierResolveResult uiResult3)
			{
				List<Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly>> lookups = new List<Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly>>();
				try
				{
					foreach (FrameworkLookup.AssemblyLookup lookup2 in frameworkLookup.GetLookups(uiResult3, tc, isInsideAttributeType))
					{
						SystemAssembly assemblyFromFullName2 = netProject.AssemblyContext.GetAssemblyFromFullName(lookup2.FullName, lookup2.Package, netProject.TargetFramework);
						if (assemblyFromFullName2 != null && CanBeReferenced(doc.Project, assemblyFromFullName2))
						{
							lookups.Add(Tuple.Create(lookup2, assemblyFromFullName2));
						}
					}
				}
				catch (Exception ex2)
				{
					if (!TypeSystemService.RecreateFrameworkLookup(netProject))
					{
						LoggingService.LogError($"Error while looking up identifier {uiResult3.Identifier}", ex2);
					}
				}
				foreach (Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly> kv in lookups)
				{
					if (CanReference(doc, new MonoDevelop.Projects.ProjectReference(kv.Item2)))
					{
						yield return new PossibleNamespace(kv.Item1.Namespace, isAccessibleWithGlobalUsing: true, new MonoDevelop.Projects.ProjectReference(kv.Item2));
					}
				}
			}
			if (foundIdentifier || frameworkLookup == null || !(resolveResult is UnknownMemberResolveResult) || !(resolveResult is UnknownMemberResolveResult uiResult4))
			{
				yield break;
			}
			List<Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly>> lookups2 = new List<Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly>>();
			try
			{
				foreach (FrameworkLookup.AssemblyLookup lookup3 in frameworkLookup.GetLookups(uiResult4, node.ToString(), tc, isInsideAttributeType))
				{
					SystemAssembly assemblyFromFullName3 = netProject.AssemblyContext.GetAssemblyFromFullName(lookup3.FullName, lookup3.Package, netProject.TargetFramework);
					if (assemblyFromFullName3 != null && CanBeReferenced(doc.Project, assemblyFromFullName3))
					{
						lookups2.Add(Tuple.Create(lookup3, assemblyFromFullName3));
					}
				}
			}
			catch (Exception ex3)
			{
				if (!TypeSystemService.RecreateFrameworkLookup(netProject))
				{
					LoggingService.LogError($"Error while looking up member resolve result {node}", ex3);
				}
			}
			foreach (Tuple<FrameworkLookup.AssemblyLookup, SystemAssembly> kv2 in lookups2)
			{
				if (CanReference(doc, new MonoDevelop.Projects.ProjectReference(kv2.Item2)))
				{
					yield return new PossibleNamespace(kv2.Item1.Namespace, isAccessibleWithGlobalUsing: true, new MonoDevelop.Projects.ProjectReference(kv2.Item2));
				}
			}
		}

		protected override void Run(object data)
		{
			((Action)data)?.Invoke();
		}
	}
}
