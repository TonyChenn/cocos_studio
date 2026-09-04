using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// This interface represents a resolved type in the type system.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A type is potentially
	/// - a type definition (<see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" />, i.e. a class, struct, interface, delegate, or built-in primitive type)
	/// - a parameterized type (<see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" />, e.g. List&lt;int&gt;)
	/// - a type parameter (<see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeParameter" />, e.g. T)
	/// - an array (<see cref="T:ICSharpCode.NRefactory.TypeSystem.ArrayType" />)
	/// - a pointer (<see cref="T:ICSharpCode.NRefactory.TypeSystem.PointerType" />)
	/// - a managed reference (<see cref="T:ICSharpCode.NRefactory.TypeSystem.ByReferenceType" />)
	/// - one of the special types (<see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />, <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.NullType" />,
	///      <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.Dynamic" />, <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnboundTypeArgument" />)
	///
	/// The <see cref="P:ICSharpCode.NRefactory.TypeSystem.IType.Kind" /> property can be used to switch on the kind of a type.
	/// </para>
	/// <para>
	/// IType uses the null object pattern: <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" /> serves as the null object.
	/// Methods or properties returning IType never return null unless documented otherwise.
	/// </para>
	/// <para>
	/// Types should be compared for equality using the <see cref="M:System.IEquatable`1.Equals(`0)" /> method.
	/// Identical types do not necessarily use the same object reference.
	/// </para>
	/// </remarks>
	// Token: 0x02000057 RID: 87
	public interface IType : INamedElement, IEquatable<IType>
	{
		/// <summary>
		/// Gets the type kind.
		/// </summary>
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026F RID: 623
		TypeKind Kind { get; }

		/// <summary>
		/// Gets whether the type is a reference type or value type.
		/// </summary>
		/// <returns>
		/// true, if the type is a reference type.
		/// false, if the type is a value type.
		/// null, if the type is not known (e.g. unconstrained generic type parameter or type not found)
		/// </returns>
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000270 RID: 624
		bool? IsReferenceType { get; }

		/// <summary>
		/// Gets the underlying type definition.
		/// Can return null for types which do not have a type definition (for example arrays, pointers, type parameters).
		/// </summary>
		// Token: 0x06000271 RID: 625
		ITypeDefinition GetDefinition();

		/// <summary>
		/// Gets the parent type, if this is a nested type.
		/// Returns null for top-level types.
		/// </summary>
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000272 RID: 626
		IType DeclaringType { get; }

		/// <summary>
		/// Gets the number of type parameters.
		/// </summary>
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000273 RID: 627
		int TypeParameterCount { get; }

		/// <summary>
		/// Gets the type arguments passed to this type.
		/// If this type is a generic type definition that is not parameterized, this property returns the type parameters,
		/// as if the type was parameterized with its own type arguments (<c>class C&lt;T&gt; { C&lt;T&gt; field; }</c>).
		///
		/// NOTE: The type will change to IReadOnlyList&lt;IType&gt; in future versions.
		/// </summary>
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000274 RID: 628
		IList<IType> TypeArguments { get; }

		/// <summary>
		/// If true the type represents an instance of a generic type.
		/// </summary>
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000275 RID: 629
		bool IsParameterized { get; }

		/// <summary>
		/// Calls ITypeVisitor.Visit for this type.
		/// </summary>
		/// <returns>The return value of the ITypeVisitor.Visit call</returns>
		// Token: 0x06000276 RID: 630
		IType AcceptVisitor(TypeVisitor visitor);

		/// <summary>
		/// Calls ITypeVisitor.Visit for all children of this type, and reconstructs this type with the children based
		/// on the return values of the visit calls.
		/// </summary>
		/// <returns>A copy of this type, with all children replaced by the return value of the corresponding visitor call.
		/// If the visitor returned the original types for all children (or if there are no children), returns <c>this</c>.
		/// </returns>
		// Token: 0x06000277 RID: 631
		IType VisitChildren(TypeVisitor visitor);

		/// <summary>
		/// Gets the direct base types.
		/// </summary>
		/// <returns>Returns the direct base types including interfaces</returns>
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000278 RID: 632
		IEnumerable<IType> DirectBaseTypes { get; }

		/// <summary>
		/// Creates a type reference that can be used to look up a type equivalent to this type in another compilation.
		/// </summary>
		/// <remarks>
		/// If this type contains open generics, the resulting type reference will need to be looked up in an appropriate generic context.
		/// Otherwise, the main resolve context of a compilation is sufficient.
		/// </remarks>
		// Token: 0x06000279 RID: 633
		ITypeReference ToTypeReference();

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type.
		/// Returns TypeParameterSubstitution.Identity if the type is not parametrized.
		/// </summary>
		// Token: 0x0600027A RID: 634
		TypeParameterSubstitution GetSubstitution();

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type,
		/// and also substitutes method type parameters with the specified method type arguments.
		/// Returns TypeParameterSubstitution.Identity if the type is not parametrized.
		/// </summary>
		// Token: 0x0600027B RID: 635
		TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments);

		/// <summary>
		/// Gets inner classes (including inherited inner classes).
		/// </summary>
		/// <param name="filter">The filter used to select which types to return.
		/// The filter is tested on the original type definitions (before parameterization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// <para>
		/// If the nested type is generic, this method will return a parameterized type,
		/// where the additional type parameters are set to <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnboundTypeArgument" />.
		/// </para>
		/// <para>
		/// Type parameters belonging to the outer class will have the value copied from the outer type
		/// if it is a parameterized type. Otherwise, those existing type parameters will be self-parameterized,
		/// and thus 'leaked' to the caller in the same way the GetMembers() method does not specialize members
		/// from an <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> and 'leaks' type parameters in member signatures.
		/// </para>
		/// </remarks>
		/// <example>
		/// <code>
		/// class Base&lt;T&gt; {
		/// 	class Nested&lt;X&gt; {}
		/// }
		/// class Derived&lt;A, B&gt; : Base&lt;B&gt; {}
		///
		/// Derived[string,int].GetNestedTypes() = { Base`1+Nested`1[int, unbound] }
		/// Derived.GetNestedTypes() = { Base`1+Nested`1[`1, unbound] }
		/// Base[`1].GetNestedTypes() = { Base`1+Nested`1[`1, unbound] }
		/// Base.GetNestedTypes() = { Base`1+Nested`1[`0, unbound] }
		/// </code>
		/// </example>
		// Token: 0x0600027C RID: 636
		IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets inner classes (including inherited inner classes)
		/// that have <c>typeArguments.Count</c> additional type parameters.
		/// </summary>
		/// <param name="typeArguments">The type arguments passed to the inner class</param>
		/// <param name="filter">The filter used to select which types to return.
		/// The filter is tested on the original type definitions (before parameterization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// Type parameters belonging to the outer class will have the value copied from the outer type
		/// if it is a parameterized type. Otherwise, those existing type parameters will be self-parameterized,
		/// and thus 'leaked' to the caller in the same way the GetMembers() method does not specialize members
		/// from an <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> and 'leaks' type parameters in member signatures.
		/// </remarks>
		// Token: 0x0600027D RID: 637
		IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all instance constructors for this type.
		/// </summary>
		/// <param name="filter">The filter used to select which constructors to return.
		/// The filter is tested on the original method definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// <para>The result does not include static constructors.
		/// Constructors in base classes are not returned by default, as GetMemberOptions.IgnoreInheritedMembers is the default value.</para>
		/// <para>
		/// For methods on parameterized types, type substitution will be performed on the method signature,
		/// and the appropriate <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedMethod" /> will be returned.
		/// </para>
		/// </remarks>
		// Token: 0x0600027E RID: 638
		IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers);

		/// <summary>
		/// Gets all methods that can be called on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which methods to return.
		/// The filter is tested on the original method definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// <para>
		/// The result does not include constructors or accessors.
		/// </para>
		/// <para>
		/// For methods on parameterized types, type substitution will be performed on the method signature,
		/// and the appropriate <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedMethod" /> will be returned.
		/// </para>
		/// <para>
		/// If the method being returned is generic, and this type is a parameterized type where the type
		/// arguments involve another method's type parameters, the resulting specialized signature
		/// will be ambiguous as to which method a type parameter belongs to.
		/// For example, "List[[``0]].GetMethods()" will return "ConvertAll(Converter`2[[``0, ``0]])".
		///
		/// If possible, use the other GetMethods() overload to supply type arguments to the method,
		/// so that both class and method type parameter can be substituted at the same time, so that
		/// the ambiguity can be avoided.
		/// </para>
		/// </remarks>
		// Token: 0x0600027F RID: 639
		IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all generic methods that can be called on this type with the specified type arguments.
		/// </summary>
		/// <param name="typeArguments">The type arguments used for the method call.</param>
		/// <param name="filter">The filter used to select which methods to return.
		/// The filter is tested on the original method definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// <para>The result does not include constructors or accessors.</para>
		/// <para>
		/// Type substitution will be performed on the method signature, creating a <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedMethod" />
		/// with the specified type arguments.
		/// </para>
		/// <para>
		/// When the list of type arguments is empty, this method acts like the GetMethods() overload without
		/// the type arguments parameter - that is, it also returns generic methods,
		/// and the other overload's remarks about ambiguous signatures apply here as well.
		/// </para>
		/// </remarks>
		// Token: 0x06000280 RID: 640
		IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all properties that can be called on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which properties to return.
		/// The filter is tested on the original property definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// For properties on parameterized types, type substitution will be performed on the property signature,
		/// and the appropriate <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedProperty" /> will be returned.
		/// </remarks>
		// Token: 0x06000281 RID: 641
		IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all fields that can be accessed on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which constructors to return.
		/// The filter is tested on the original field definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// For fields on parameterized types, type substitution will be performed on the field's return type,
		/// and the appropriate <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedField" /> will be returned.
		/// </remarks>
		// Token: 0x06000282 RID: 642
		IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all events that can be accessed on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which events to return.
		/// The filter is tested on the original event definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// For fields on parameterized types, type substitution will be performed on the event's return type,
		/// and the appropriate <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.SpecializedEvent" /> will be returned.
		/// </remarks>
		// Token: 0x06000283 RID: 643
		IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all members that can be called on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which members to return.
		/// The filter is tested on the original member definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// <para>
		/// The resulting list is the union of GetFields(), GetProperties(), GetMethods() and GetEvents().
		/// It does not include constructors.
		/// For parameterized types, type substitution will be performed.
		/// </para>
		/// <para>
		/// For generic methods, the remarks about ambiguous signatures from the
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IType.GetMethods(System.Predicate{ICSharpCode.NRefactory.TypeSystem.IUnresolvedMethod},ICSharpCode.NRefactory.TypeSystem.GetMemberOptions)" /> method apply here as well.
		/// </para>
		/// </remarks>
		// Token: 0x06000284 RID: 644
		IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None);

		/// <summary>
		/// Gets all accessors belonging to properties or events on this type.
		/// </summary>
		/// <param name="filter">The filter used to select which members to return.
		/// The filter is tested on the original member definitions (before specialization).</param>
		/// <param name="options">Specified additional options for the GetMembers() operation.</param>
		/// <remarks>
		/// Accessors are not returned by GetMembers() or GetMethods().
		/// </remarks>
		// Token: 0x06000285 RID: 645
		IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None);
	}
}
