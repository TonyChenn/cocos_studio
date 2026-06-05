using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Contains static implementations of special types.
	/// </summary>
	// Token: 0x02000103 RID: 259
	[Serializable]
	public sealed class SpecialType : AbstractType, ITypeReference
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x00019633 File Offset: 0x00018633
		private SpecialType(TypeKind kind, string name, bool? isReferenceType)
		{
			this.kind = kind;
			this.name = name;
			this.isReferenceType = isReferenceType;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00019650 File Offset: 0x00018650
		public override ITypeReference ToTypeReference()
		{
			return this;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00019653 File Offset: 0x00018653
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0001965B File Offset: 0x0001865B
		public override TypeKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00019663 File Offset: 0x00018663
		public override bool? IsReferenceType
		{
			get
			{
				return this.isReferenceType;
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001966B File Offset: 0x0001866B
		IType ITypeReference.Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return this;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001967C File Offset: 0x0001867C
		[Obsolete("Please compare special types using the kind property instead.")]
		public override bool Equals(IType other)
		{
			return other is SpecialType && other.Kind == this.kind;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00019696 File Offset: 0x00018696
		public override int GetHashCode()
		{
			return 81625621 ^ (int)this.kind;
		}

		/// <summary>
		/// Gets the type representing resolve errors.
		/// </summary>
		// Token: 0x0400030B RID: 779
		public static readonly SpecialType UnknownType = new SpecialType(TypeKind.Unknown, "?", null);

		/// <summary>
		/// The null type is used as type of the null literal. It is a reference type without any members; and it is a subtype of all reference types.
		/// </summary>
		// Token: 0x0400030C RID: 780
		public static readonly SpecialType NullType = new SpecialType(TypeKind.Null, "null", new bool?(true));

		/// <summary>
		/// Type representing the C# 'dynamic' type.
		/// </summary>
		// Token: 0x0400030D RID: 781
		public static readonly SpecialType Dynamic = new SpecialType(TypeKind.Dynamic, "dynamic", new bool?(true));

		/// <summary>
		/// A type used for unbound type arguments in partially parameterized types.
		/// </summary>
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IType.GetNestedTypes(System.Predicate{ICSharpCode.NRefactory.TypeSystem.ITypeDefinition},ICSharpCode.NRefactory.TypeSystem.GetMemberOptions)" />
		// Token: 0x0400030E RID: 782
		public static readonly SpecialType UnboundTypeArgument = new SpecialType(TypeKind.UnboundTypeArgument, "", null);

		// Token: 0x0400030F RID: 783
		private readonly TypeKind kind;

		// Token: 0x04000310 RID: 784
		private readonly string name;

		// Token: 0x04000311 RID: 785
		private readonly bool? isReferenceType;
	}
}
