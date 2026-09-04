using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.CodeActions
{
	public abstract class CodeAction
	{
		public string Title { get; set; }

		public string IdString { get; set; }

		public DocumentRegion DocumentRegion { get; set; }

		public Type InspectorType { get; set; }

		public object SiblingKey { get; set; }

		public Severity Severity { get; set; }

		public virtual bool SupportsBatchRunning => false;

		protected CodeAction()
		{
			IdString = GetType().FullName;
		}

		public abstract void Run(IRefactoringContext context, object script);

		public virtual void BatchRun(Document document, TextLocation loc)
		{
			if (!SupportsBatchRunning)
			{
				throw new InvalidOperationException("Batch running is not supported.");
			}
		}
	}
}
