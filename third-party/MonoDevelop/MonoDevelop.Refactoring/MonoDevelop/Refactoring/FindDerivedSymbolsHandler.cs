using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.Refactoring
{
	internal class FindDerivedSymbolsHandler
	{
		private readonly IMember member;

		public bool IsValid
		{
			get
			{
				if (IdeApp.ProjectOperations.CurrentSelectedSolution == null)
				{
					return false;
				}
				if (TypeSystemService.GetProject(member) == null)
				{
					return false;
				}
				if (!member.IsVirtual && !member.IsAbstract)
				{
					return member.DeclaringType.Kind == TypeKind.Interface;
				}
				return true;
			}
		}

		public FindDerivedSymbolsHandler(IMember member)
		{
			this.member = member;
		}

		public void Run()
		{
			FindDerivedClassesHandler.FindDerivedMembers(member);
		}
	}
}
