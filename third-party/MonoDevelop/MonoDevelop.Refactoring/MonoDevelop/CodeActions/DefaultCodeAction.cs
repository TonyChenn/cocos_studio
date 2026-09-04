using System;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.CodeActions
{
	public class DefaultCodeAction : CodeAction
	{
		public Action<RefactoringContext, Script> act;

		public DefaultCodeAction(string title, Action<RefactoringContext, Script> act)
		{
			base.Title = title;
			this.act = act;
		}

		public override void Run(IRefactoringContext context, object script)
		{
			act((RefactoringContext)context, (Script)script);
		}
	}
}
