using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a property or indexer.
	/// </summary>
	// Token: 0x020000C7 RID: 199
	public interface IUnresolvedProperty : IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600072E RID: 1838
		bool CanGet { get; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600072F RID: 1839
		bool CanSet { get; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000730 RID: 1840
		IUnresolvedMethod Getter { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000731 RID: 1841
		IUnresolvedMethod Setter { get; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000732 RID: 1842
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
		// Token: 0x06000733 RID: 1843
		IProperty Resolve(ITypeResolveContext context);
	}
}
