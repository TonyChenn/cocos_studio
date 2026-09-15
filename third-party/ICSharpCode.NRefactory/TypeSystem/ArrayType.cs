using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an array type.
	/// </summary>
	public sealed class ArrayType : TypeWithElementType, ICompilationProvider
	{
		public ArrayType(ICompilation compilation, IType elementType, int dimensions = 1) : base(elementType)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (dimensions <= 0)
			{
				throw new ArgumentOutOfRangeException("dimensions", dimensions, "dimensions must be positive");
			}
			this.compilation = compilation;
			this.dimensions = dimensions;
			ICompilationProvider compilationProvider = elementType as ICompilationProvider;
			if (compilationProvider != null && compilationProvider.Compilation != compilation)
			{
				throw new InvalidOperationException("Cannot create an array type using a different compilation from the element type.");
			}
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Array;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		public int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		public override string NameSuffix
		{
			get
			{
				return "[" + new string(',', this.dimensions - 1) + "]";
			}
		}

		public override bool? IsReferenceType
		{
			get
			{
				return new bool?(true);
			}
		}

		public override int GetHashCode()
		{
			return this.elementType.GetHashCode() * 71681 + this.dimensions;
		}

		public override bool Equals(IType other)
		{
			ArrayType arrayType = other as ArrayType;
			return arrayType != null && this.elementType.Equals(arrayType.elementType) && arrayType.dimensions == this.dimensions;
		}

		public override ITypeReference ToTypeReference()
		{
			return new ArrayTypeReference(this.elementType.ToTypeReference(), this.dimensions);
		}

		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				List<IType> list = new List<IType>();
				IType type = this.compilation.FindType(KnownTypeCode.Array);
				if (type.Kind != TypeKind.Unknown)
				{
					list.Add(type);
				}
				if (this.dimensions == 1 && this.elementType.Kind != TypeKind.Pointer)
				{
					ITypeDefinition typeDefinition = this.compilation.FindType(KnownTypeCode.IListOfT) as ITypeDefinition;
					if (typeDefinition != null)
					{
						list.Add(new ParameterizedType(typeDefinition, new IType[]
						{
							this.elementType
						}));
					}
					typeDefinition = (this.compilation.FindType(KnownTypeCode.IReadOnlyListOfT) as ITypeDefinition);
					if (typeDefinition != null)
					{
						list.Add(new ParameterizedType(typeDefinition, new IType[]
						{
							this.elementType
						}));
					}
				}
				return list;
			}
		}

		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetMethods(filter, options);
		}

		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetMethods(typeArguments, filter, options);
		}

		public override IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetAccessors(filter, options);
		}

		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IProperty>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetProperties(filter, options);
		}

		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitArrayType(this);
		}

		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new ArrayType(this.compilation, type, this.dimensions);
		}

		private readonly int dimensions;

		private readonly ICompilation compilation;
	}
}
