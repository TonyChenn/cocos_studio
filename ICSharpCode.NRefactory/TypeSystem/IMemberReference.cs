using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000093 RID: 147
	public interface IMemberReference : ISymbolReference
	{
		/// <summary>
		/// Gets the declaring type reference for the member.
		/// </summary>
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060004B0 RID: 1200
		ITypeReference DeclaringTypeReference { get; }

		/// <summary>
		/// Resolves the member.
		/// </summary>
		/// <param name="context">
		/// Context to use for resolving this member reference.
		/// Which kind of context is required depends on the which kind of member reference this is;
		/// please consult the documentation of the method that was used to create this member reference,
		/// or that of the class implementing this method.
		/// </param>
		/// <returns>
		/// Returns the resolved member, or <c>null</c> if the member could not be found.
		/// </returns>
		// Token: 0x060004B1 RID: 1201
		IMember Resolve(ITypeResolveContext context);
	}
}
