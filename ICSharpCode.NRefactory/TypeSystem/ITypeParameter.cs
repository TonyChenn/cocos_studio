using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Type parameter of a generic class/method.
	/// </summary>
	// Token: 0x020000A0 RID: 160
	public interface ITypeParameter : IType, INamedElement, IEquatable<IType>, ISymbol
	{
		/// <summary>
		/// Get the type of this type parameter's owner.
		/// </summary>
		/// <returns>SymbolKind.TypeDefinition or SymbolKind.Method</returns>
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060004F8 RID: 1272
		SymbolKind OwnerType { get; }

		/// <summary>
		/// Gets the owning method/class.
		/// This property may return null (for example for the dummy type parameters used by <see cref="M:ICSharpCode.NRefactory.TypeSystem.ParameterListComparer.NormalizeMethodTypeParameters(ICSharpCode.NRefactory.TypeSystem.IType)" />).
		/// </summary>
		/// <remarks>
		/// For "class Outer&lt;T&gt; { class Inner {} }",
		/// inner.TypeParameters[0].Owner will be the outer class, because the same
		/// ITypeParameter instance is used both on Outer`1 and Outer`1+Inner.
		/// </remarks>
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060004F9 RID: 1273
		IEntity Owner { get; }

		/// <summary>
		/// Gets the index of the type parameter in the type parameter list of the owning method/class.
		/// </summary>
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060004FA RID: 1274
		int Index { get; }

		/// <summary>
		/// Gets the name of the type parameter.
		/// </summary>
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060004FB RID: 1275
		string Name { get; }

		/// <summary>
		/// Gets the list of attributes declared on this type parameter.
		/// </summary>
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060004FC RID: 1276
		IList<IAttribute> Attributes { get; }

		/// <summary>
		/// Gets the variance of this type parameter.
		/// </summary>
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060004FD RID: 1277
		VarianceModifier Variance { get; }

		/// <summary>
		/// Gets the region where the type parameter is defined.
		/// </summary>
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060004FE RID: 1278
		DomRegion Region { get; }

		/// <summary>
		/// Gets the effective base class of this type parameter.
		/// </summary>
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060004FF RID: 1279
		IType EffectiveBaseClass { get; }

		/// <summary>
		/// Gets the effective interface set of this type parameter.
		/// </summary>
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000500 RID: 1280
		ICollection<IType> EffectiveInterfaceSet { get; }

		/// <summary>
		/// Gets if the type parameter has the 'new()' constraint.
		/// </summary>
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000501 RID: 1281
		bool HasDefaultConstructorConstraint { get; }

		/// <summary>
		/// Gets if the type parameter has the 'class' constraint.
		/// </summary>
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000502 RID: 1282
		bool HasReferenceTypeConstraint { get; }

		/// <summary>
		/// Gets if the type parameter has the 'struct' constraint.
		/// </summary>
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000503 RID: 1283
		bool HasValueTypeConstraint { get; }
	}
}
