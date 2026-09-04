using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects.Text;

namespace MonoDevelop.Refactoring
{
	public class RefactoringOptions
	{
		private readonly Task<CSharpAstResolver> resolver;

		public Document Document { get; private set; }

		public object SelectedItem { get; set; }

		public ResolveResult ResolveResult { get; set; }

		public ITextFileProvider TestFileProvider { get; set; }

		public string MimeType => DesktopService.GetMimeTypeForUri(Document.FileName);

		public TextLocation Location => new TextLocation(Document.Editor.Caret.Line, Document.Editor.Caret.Column);

		public RefactoringOptions()
		{
		}

		public RefactoringOptions(Document doc)
		{
			Document = doc;
			if (doc != null && doc.ParsedDocument != null)
			{
				Task<CSharpAstResolver> sharedResolver = doc.GetSharedResolver();
				if (sharedResolver != null)
				{
					resolver = sharedResolver;
				}
			}
		}

		public TextEditorData GetTextEditorData()
		{
			return Document.Editor;
		}

		public static string GetWhitespaces(Document document, int insertionOffset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = insertionOffset; i < document.Editor.Length; i++)
			{
				char charAt = document.Editor.GetCharAt(i);
				if (charAt != ' ' && charAt != '\t')
				{
					break;
				}
				stringBuilder.Append(charAt);
			}
			return stringBuilder.ToString();
		}

		public string OutputNode(AstNode node)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				TextWriterTokenWriter writer = new TextWriterTokenWriter(stringWriter);
				stringWriter.NewLine = Document.Editor.EolMarker;
				CSharpOutputVisitor visitor = new CSharpOutputVisitor(writer, FormattingOptionsFactory.CreateMono());
				node.AcceptVisitor(visitor);
				return stringWriter.ToString();
			}
		}

		public CodeGenerator CreateCodeGenerator()
		{
			CodeGenerator codeGenerator = CodeGenerator.CreateGenerator(Document);
			if (codeGenerator == null)
			{
				LoggingService.LogError("Generator can't be generated for : " + Document.Editor.MimeType);
			}
			return codeGenerator;
		}

		public static string GetIndent(Document document, IEntity member)
		{
			return GetWhitespaces(document, document.Editor.Document.LocationToOffset(member.Region.BeginLine, 1));
		}

		public string GetWhitespaces(int insertionOffset)
		{
			return GetWhitespaces(Document, insertionOffset);
		}

		public string GetIndent(IEntity member)
		{
			return GetIndent(Document, member);
		}

		public List<string> GetUsedNamespaces()
		{
			return GetUsedNamespaces(Document, Location);
		}

		public static List<string> GetUsedNamespaces(Document doc, TextLocation loc)
		{
			List<string> list = new List<string>();
			if (!(doc.ParsedDocument.ParsedFile is CSharpUnresolvedFile cSharpUnresolvedFile))
			{
				return list;
			}
			UsingScope usingScope = cSharpUnresolvedFile.GetUsingScope(loc);
			if (usingScope == null)
			{
				return list;
			}
			CSharpResolver resolver = cSharpUnresolvedFile.GetResolver(doc.Compilation, loc);
			for (UsingScope usingScope2 = usingScope; usingScope2 != null; usingScope2 = usingScope2.Parent)
			{
				list.Add(usingScope2.NamespaceName);
				list.AddRange(from u in usingScope2.Usings
					select u.ResolveNamespace(resolver) into nr
					where nr != null
					select nr.FullName);
			}
			return list;
		}

		public ResolveResult Resolve(AstNode node)
		{
			if (!resolver.IsCompleted)
			{
				resolver.Wait(2000);
			}
			if (!resolver.IsCompleted)
			{
				return null;
			}
			return resolver.Result.Resolve(node);
		}

		public AstType CreateShortType(IType fullType)
		{
			CSharpUnresolvedFile cSharpUnresolvedFile = Document.ParsedDocument.ParsedFile as CSharpUnresolvedFile;
			CSharpResolver cSharpResolver = cSharpUnresolvedFile.GetResolver(Document.Compilation, Document.Editor.Caret.Location);
			TypeSystemAstBuilder typeSystemAstBuilder = new TypeSystemAstBuilder(cSharpResolver);
			return typeSystemAstBuilder.ConvertType(fullType);
		}
	}
}
