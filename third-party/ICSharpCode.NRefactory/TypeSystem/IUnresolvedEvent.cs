using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public interface IUnresolvedEvent : IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		bool CanAdd { get; }

		bool CanRemove { get; }

		bool CanInvoke { get; }

		IUnresolvedMethod AddAccessor { get; }

		IUnresolvedMethod RemoveAccessor { get; }

		IUnresolvedMethod InvokeAccessor { get; }

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
		IEvent Resolve(ITypeResolveContext context);
	}
}
