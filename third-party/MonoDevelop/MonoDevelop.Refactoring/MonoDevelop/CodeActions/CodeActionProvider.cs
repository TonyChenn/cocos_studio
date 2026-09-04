using System.Collections.Generic;
using System.Threading;
using ICSharpCode.NRefactory;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.CodeActions
{
	public abstract class CodeActionProvider
	{
		public string MimeType { get; set; }

		public string Category { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public virtual string IdString => GetType().FullName;

		public abstract IEnumerable<CodeAction> GetActions(Document document, object refactoringContext, TextLocation loc, CancellationToken cancellationToken);
	}
}
