using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Refers to the object that is currently being initialized.
	/// Used within <see cref="F:ICSharpCode.NRefactory.Semantics.InvocationResolveResult.InitializerStatements" />.
	/// </summary>
	// Token: 0x02000046 RID: 70
	public class InitializedObjectResolveResult : ResolveResult
	{
		// Token: 0x06000222 RID: 546 RVA: 0x000066FE File Offset: 0x000056FE
		public InitializedObjectResolveResult(IType type) : base(type)
		{
		}
	}
}
