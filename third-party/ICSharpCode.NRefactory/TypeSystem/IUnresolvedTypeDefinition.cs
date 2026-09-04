using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an unresolved class, enum, interface, struct, delegate or VB module.
	/// For partial classes, an unresolved type definition represents only a single part.
	/// </summary>
	// Token: 0x020000C9 RID: 201
	public interface IUnresolvedTypeDefinition : ITypeReference, IUnresolvedEntity, INamedElement, IHasAccessibility
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000745 RID: 1861
		TypeKind Kind { get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000746 RID: 1862
		FullTypeName FullTypeName { get; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000747 RID: 1863
		IList<ITypeReference> BaseTypes { get; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000748 RID: 1864
		IList<IUnresolvedTypeParameter> TypeParameters { get; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000749 RID: 1865
		IList<IUnresolvedTypeDefinition> NestedTypes { get; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600074A RID: 1866
		IList<IUnresolvedMember> Members { get; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600074B RID: 1867
		IEnumerable<IUnresolvedMethod> Methods { get; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600074C RID: 1868
		IEnumerable<IUnresolvedProperty> Properties { get; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600074D RID: 1869
		IEnumerable<IUnresolvedField> Fields { get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600074E RID: 1870
		IEnumerable<IUnresolvedEvent> Events { get; }

		/// <summary>
		/// Gets whether the type definition contains extension methods.
		/// Returns null when the type definition needs to be resolved in order to determine whether
		/// methods are extension methods.
		/// </summary>
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600074F RID: 1871
		bool? HasExtensionMethods { get; }

		/// <summary>
		/// Gets whether the partial modifier is set on this part of the type definition.
		/// </summary>
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000750 RID: 1872
		bool IsPartial { get; }

		/// <summary>
		/// Gets whether this unresolved type definition causes the addition of a default constructor
		/// if no other constructor is present.
		/// </summary>
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000751 RID: 1873
		bool AddDefaultConstructorIfRequired { get; }

		/// <summary>
		/// Looks up the resolved type definition from the <paramref name="context" /> corresponding to this unresolved
		/// type definition.
		/// </summary>
		/// <param name="context">
		/// Context for looking up the type. The context must specify the current assembly.
		/// A <see cref="T:ICSharpCode.NRefactory.TypeSystem.SimpleTypeResolveContext" /> that specifies the current assembly is sufficient.
		/// </param>
		/// <returns>
		/// Returns the resolved type definition.
		/// In case of an error, returns an <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.UnknownType" /> instance.
		/// Never returns null.
		/// </returns>
		// Token: 0x06000752 RID: 1874
		IType Resolve(ITypeResolveContext context);

		/// <summary>
		/// This method is used to add language-specific elements like the C# UsingScope
		/// to the type resolve context.
		/// </summary>
		/// <param name="parentContext">The parent context (e.g. the parent assembly),
		/// including the parent type definition for inner classes.</param>
		/// <returns>
		/// The parent context, modified to include language-specific elements (e.g. using scope)
		/// associated with this type definition.
		/// </returns>
		/// <remarks>
		/// Use <c>unresolvedTypeDef.CreateResolveContext(parentContext).WithTypeDefinition(typeDef)</c> to
		/// create the context for use within the type definition.
		/// </remarks>
		// Token: 0x06000753 RID: 1875
		ITypeResolveContext CreateResolveContext(ITypeResolveContext parentContext);
	}
}
