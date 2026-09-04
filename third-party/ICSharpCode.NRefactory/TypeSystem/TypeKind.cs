using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// .
	/// </summary>
	// Token: 0x020000FC RID: 252
	public enum TypeKind : byte
	{
		/// <summary>Language-specific type that is not part of NRefactory.TypeSystem itself.</summary>
		// Token: 0x040002F8 RID: 760
		Other,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a class.</summary>
		// Token: 0x040002F9 RID: 761
		Class,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is an interface.</summary>
		// Token: 0x040002FA RID: 762
		Interface,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a struct.</summary>
		// Token: 0x040002FB RID: 763
		Struct,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> or <see cref="T:ICSharpCode.NRefactory.TypeSystem.ParameterizedType" /> that is a delegate.</summary>
		/// <remarks><c>System.Delegate</c> itself is TypeKind.Class</remarks>
		// Token: 0x040002FC RID: 764
		Delegate,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> that is an enum.</summary>
		/// <remarks><c>System.Enum</c> itself is TypeKind.Class</remarks>
		// Token: 0x040002FD RID: 765
		Enum,
		/// <summary>A <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" /> that is a module (VB).</summary>
		// Token: 0x040002FE RID: 766
		Module,
		/// <summary>The <c>System.Void</c> type.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.KnownTypeReference.Void" />
		// Token: 0x040002FF RID: 767
		Void,
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />
		// Token: 0x04000300 RID: 768
		Unknown,
		/// <summary>The type of the null literal.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.NullType" />
		// Token: 0x04000301 RID: 769
		Null,
		/// <summary>Type representing the C# 'dynamic' type.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.Dynamic" />
		// Token: 0x04000302 RID: 770
		Dynamic,
		/// <summary>Represents missing type arguments in partially parameterized types.</summary>
		/// <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnboundTypeArgument" />
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IType.GetNestedTypes(System.Predicate{ICSharpCode.NRefactory.TypeSystem.ITypeDefinition},ICSharpCode.NRefactory.TypeSystem.GetMemberOptions)" />
		// Token: 0x04000303 RID: 771
		UnboundTypeArgument,
		/// <summary>The type is a type parameter.</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeParameter" />
		// Token: 0x04000304 RID: 772
		TypeParameter,
		/// <summary>An array type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ArrayType" />
		// Token: 0x04000305 RID: 773
		Array,
		/// <summary>A pointer type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.PointerType" />
		// Token: 0x04000306 RID: 774
		Pointer,
		/// <summary>A managed reference type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.ByReferenceType" />
		// Token: 0x04000307 RID: 775
		ByReference,
		/// <summary>An anonymous type</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.AnonymousType" />
		// Token: 0x04000308 RID: 776
		Anonymous,
		/// <summary>Intersection of several types</summary>
		/// <see cref="T:ICSharpCode.NRefactory.TypeSystem.IntersectionType" />
		// Token: 0x04000309 RID: 777
		Intersection
	}
}
