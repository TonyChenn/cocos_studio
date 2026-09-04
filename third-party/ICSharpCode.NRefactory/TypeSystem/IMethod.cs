using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a method, constructor, destructor or operator.
	/// </summary>
	// Token: 0x02000064 RID: 100
	public interface IMethod : IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		/// <summary>
		/// Gets the unresolved method parts.
		/// For partial methods, this returns all parts.
		/// Otherwise, this returns an array with a single element (new[] { UnresolvedMember }).
		/// NOTE: The type will change to IReadOnlyList&lt;IUnresolvedMethod&gt; in future versions.
		/// </summary>
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600031A RID: 794
		IList<IUnresolvedMethod> Parts { get; }

		/// <summary>
		/// Gets the attributes associated with the return type. (e.g. [return: MarshalAs(...)])
		/// NOTE: The type will change to IReadOnlyList&lt;IAttribute&gt; in future versions.
		/// </summary>
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600031B RID: 795
		IList<IAttribute> ReturnTypeAttributes { get; }

		/// <summary>
		/// Gets the type parameters of this method; or an empty list if the method is not generic.
		/// NOTE: The type will change to IReadOnlyList&lt;ITypeParameter&gt; in future versions.
		/// </summary>
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600031C RID: 796
		IList<ITypeParameter> TypeParameters { get; }

		/// <summary>
		/// Gets whether this is a generic method that has been parameterized.
		/// </summary>
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600031D RID: 797
		bool IsParameterized { get; }

		/// <summary>
		/// Gets the type arguments passed to this method.
		/// If the method is generic but not parameterized yet, this property returns the type parameters,
		/// as if the method was parameterized with its own type arguments (<c>void M&lt;T&gt;() { M&lt;T&gt;(); }</c>).
		///
		/// NOTE: The type will change to IReadOnlyList&lt;IType&gt; in future versions.
		/// </summary>
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600031E RID: 798
		IList<IType> TypeArguments { get; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600031F RID: 799
		bool IsExtensionMethod { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000320 RID: 800
		bool IsConstructor { get; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000321 RID: 801
		bool IsDestructor { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000322 RID: 802
		bool IsOperator { get; }

		/// <summary>
		/// Gets whether the method is a C#-style partial method.
		/// A call to such a method is ignored by the compiler if the partial method has no body.
		/// </summary>
		/// <seealso cref="P:ICSharpCode.NRefactory.TypeSystem.IMethod.HasBody" />
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000323 RID: 803
		bool IsPartial { get; }

		/// <summary>
		/// Gets whether the method is a C#-style async method.
		/// </summary>
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000324 RID: 804
		bool IsAsync { get; }

		/// <summary>
		/// Gets whether the method has a body.
		/// This property returns <c>false</c> for <c>abstract</c> or <c>extern</c> methods,
		/// or for <c>partial</c> methods without implementation.
		/// </summary>
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000325 RID: 805
		bool HasBody { get; }

		/// <summary>
		/// Gets whether the method is a property/event accessor.
		/// </summary>
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000326 RID: 806
		bool IsAccessor { get; }

		/// <summary>
		/// If this method is an accessor, returns the corresponding property/event.
		/// Otherwise, returns null.
		/// </summary>
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000327 RID: 807
		IMember AccessorOwner { get; }

		/// <summary>
		/// If this method is reduced from an extension method return the original method, <c>null</c> otherwise.
		/// A reduced method doesn't contain the extension method parameter. That means that has one parameter less than it's definition.
		/// </summary>
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000328 RID: 808
		IMethod ReducedFrom { get; }

		/// <summary>
		/// Specializes this method with the given substitution.
		/// If this method is already specialized, the new substitution is composed with the existing substition.
		/// </summary>
		// Token: 0x06000329 RID: 809
		IMethod Specialize(TypeParameterSubstitution substitution);
	}
}
