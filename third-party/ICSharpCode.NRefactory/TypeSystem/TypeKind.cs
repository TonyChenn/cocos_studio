using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// .
	/// </summary>
	public enum TypeKind : byte
	{
		/// <summary>Language-specific type that is not part of NRefactory.TypeSystem itself.</summary>
		Other,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a class.</summary>
		Class,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is an interface.</summary>
		Interface,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a struct.</summary>
		Struct,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a delegate.</summary>
		/// <remarks><c>System.Delegate</c> itself is TypeKind.Class</remarks>
		Delegate,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> that is an enum.</summary>
		/// <remarks><c>System.Enum</c> itself is TypeKind.Class</remarks>
		Enum,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> that is a module (VB).</summary>
		Module,
		/// <summary>The <c>System.Void</c> type.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.KnownTypeReference.Void" />
		Void,
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />
		Unknown,
		/// <summary>The type of the null literal.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.NullType" />
		Null,
		/// <summary>Type representing the C# 'dynamic' type.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.Dynamic" />
		Dynamic,
		/// <summary>Represents missing type arguments in partially parameterized types.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnboundTypeArgument" />
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IType.GetNestedTypes(System.Predicate{ICSharpCode.NRefactory.TypeSystem.ITypeDefinition},ICSharpCode.NRefactory.TypeSystem.GetMemberOptions)" />
		UnboundTypeArgument,
		/// <summary>The type is a type parameter.</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeParameter" />
		TypeParameter,
		/// <summary>An array type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ArrayType" />
		Array,
		/// <summary>A pointer type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.PointerType" />
		Pointer,
		/// <summary>A managed reference type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ByReferenceType" />
		ByReference,
		/// <summary>An anonymous type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.AnonymousType" />
		Anonymous,
		/// <summary>Intersection of several types</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.IntersectionType" />
		Intersection
	}
}
