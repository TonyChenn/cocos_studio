using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Method/field/property/event.
	/// </summary>
	// Token: 0x02000094 RID: 148
	public interface IUnresolvedMember : IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		/// <summary>
		/// Gets the return type of this member.
		/// This property never returns null.
		/// </summary>
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060004B2 RID: 1202
		ITypeReference ReturnType { get; }

		/// <summary>
		/// Gets whether this member is explicitly implementing an interface.
		/// If this property is true, the member can only be called through the interfaces it implements.
		/// </summary>
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060004B3 RID: 1203
		bool IsExplicitInterfaceImplementation { get; }

		/// <summary>
		/// Gets the interfaces that are explicitly implemented by this member.
		/// </summary>
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060004B4 RID: 1204
		IList<IMemberReference> ExplicitInterfaceImplementations { get; }

		/// <summary>
		/// Gets if the member is virtual. Is true only if the "virtual" modifier was used, but non-virtual
		/// members can be overridden, too; if they are abstract or overriding a method.
		/// </summary>
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060004B5 RID: 1205
		bool IsVirtual { get; }

		/// <summary>
		/// Gets whether this member is overriding another member.
		/// </summary>
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060004B6 RID: 1206
		bool IsOverride { get; }

		/// <summary>
		/// Gets if the member can be overridden. Returns true when the member is "abstract", "virtual" or "override" but not "sealed".
		/// </summary>
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060004B7 RID: 1207
		bool IsOverridable { get; }

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
		// Token: 0x060004B8 RID: 1208
		IMember Resolve(ITypeResolveContext context);

		/// <summary>
		/// Creates the resolved member.
		/// </summary>
		/// <param name="context">
		/// The language-specific context that includes the parent type definition.
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IUnresolvedTypeDefinition.CreateResolveContext(ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext)" />
		/// </param>
		// Token: 0x060004B9 RID: 1209
		IMember CreateResolved(ITypeResolveContext context);
	}
}
