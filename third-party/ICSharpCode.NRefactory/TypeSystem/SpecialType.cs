using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Contains static implementations of special types.
	/// </summary>
	[Serializable]
	public sealed class SpecialType : AbstractType, ITypeReference
	{
		private SpecialType(TypeKind kind, string name, bool? isReferenceType)
		{
			this.kind = kind;
			this.name = name;
			this.isReferenceType = isReferenceType;
		}

		public override ITypeReference ToTypeReference()
		{
			return this;
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public override TypeKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		public override bool? IsReferenceType
		{
			get
			{
				return this.isReferenceType;
			}
		}

		IType ITypeReference.Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return this;
		}

		[Obsolete("Please compare special types using the kind property instead.")]
		public override bool Equals(IType other)
		{
			return other is SpecialType && other.Kind == this.kind;
		}

		public override int GetHashCode()
		{
			return 81625621 ^ (int)this.kind;
		}

		/// <summary>
		/// Gets the type representing resolve errors.
		/// </summary>
		public static readonly SpecialType UnknownType = new SpecialType(TypeKind.Unknown, "?", null);

		/// <summary>
		/// The null type is used as type of the null literal. It is a reference type without any members; and it is a subtype of all reference types.
		/// </summary>
		public static readonly SpecialType NullType = new SpecialType(TypeKind.Null, "null", new bool?(true));

		/// <summary>
		/// Type representing the C# 'dynamic' type.
		/// </summary>
		public static readonly SpecialType Dynamic = new SpecialType(TypeKind.Dynamic, "dynamic", new bool?(true));

		/// <summary>
		/// A type used for unbound type arguments in partially parameterized types.
		/// </summary>
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.IType.GetNestedTypes(System.Predicate{ICSharpCode.NRefactory.TypeSystem.ITypeDefinition},ICSharpCode.NRefactory.TypeSystem.GetMemberOptions)" />
		public static readonly SpecialType UnboundTypeArgument = new SpecialType(TypeKind.UnboundTypeArgument, "", null);

		private readonly TypeKind kind;

		private readonly string name;

		private readonly bool? isReferenceType;
	}
}
