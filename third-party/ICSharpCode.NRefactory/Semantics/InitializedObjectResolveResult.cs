using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Refers to the object that is currently being initialized.
	/// Used within <see cref="F:ICSharpCode.NRefactory.Semantics.InvocationResolveResult.InitializerStatements" />.
	/// </summary>
	public class InitializedObjectResolveResult : ResolveResult
	{
		public InitializedObjectResolveResult(IType type) : base(type)
		{
		}
	}
}
