using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Type parameter of a generic class/method.
	/// </summary>
	// Token: 0x020000CB RID: 203
	public interface IUnresolvedTypeParameter : INamedElement
	{
		/// <summary>
		/// Get the type of this type parameter's owner.
		/// </summary>
		/// <returns>SymbolKind.TypeDefinition or SymbolKind.Method</returns>
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000770 RID: 1904
		SymbolKind OwnerType { get; }

		/// <summary>
		/// Gets the index of the type parameter in the type parameter list of the owning method/class.
		/// </summary>
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000771 RID: 1905
		int Index { get; }

		/// <summary>
		/// Gets the list of attributes declared on this type parameter.
		/// </summary>
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000772 RID: 1906
		IList<IUnresolvedAttribute> Attributes { get; }

		/// <summary>
		/// Gets the variance of this type parameter.
		/// </summary>
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000773 RID: 1907
		VarianceModifier Variance { get; }

		/// <summary>
		/// Gets the region where the type parameter is defined.
		/// </summary>
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000774 RID: 1908
		DomRegion Region { get; }

		// Token: 0x06000775 RID: 1909
		ITypeParameter CreateResolvedTypeParameter(ITypeResolveContext context);
	}
}
