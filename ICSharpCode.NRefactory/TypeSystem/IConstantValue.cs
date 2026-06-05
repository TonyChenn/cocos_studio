using System;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an unresolved constant value.
	/// </summary>
	// Token: 0x02000091 RID: 145
	public interface IConstantValue
	{
		/// <summary>
		/// Resolves the value of this constant.
		/// </summary>
		/// <param name="context">Context where the constant value will be used.</param>
		/// <returns>Resolve result representing the constant value.
		/// This method never returns null; in case of errors, an ErrorResolveResult will be returned.</returns>
		// Token: 0x060004A4 RID: 1188
		ResolveResult Resolve(ITypeResolveContext context);
	}
}
