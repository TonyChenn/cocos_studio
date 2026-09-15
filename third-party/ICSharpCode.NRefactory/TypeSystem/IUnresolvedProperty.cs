using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a property or indexer.
	/// </summary>
	public interface IUnresolvedProperty : IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		bool CanGet { get; }

		bool CanSet { get; }

		IUnresolvedMethod Getter { get; }

		IUnresolvedMethod Setter { get; }

		bool IsIndexer { get; }

		/// <summary>
		/// Resolves the member.
		/// </summary>
		/// <param name="context">
		/// Context for looking up the member. The context must specify the current assembly.
		/// A <see cref="T:ICSharpCode.NRefactory.TypeSystem.SimpleTypeResolveContext" /> that specifies the current assembly is sufficient.
		/// </param>
		/// <returns>
		/// Returns the resolved member, or <c>null</c> if the member could not be found.
		/// </returns>
		IProperty Resolve(ITypeResolveContext context);
	}
}
