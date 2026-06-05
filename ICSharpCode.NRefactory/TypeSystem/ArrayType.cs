using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an array type.
	/// </summary>
	// Token: 0x0200006A RID: 106
	public sealed class ArrayType : TypeWithElementType, ICompilationProvider
	{
		// Token: 0x06000369 RID: 873 RVA: 0x0000854C File Offset: 0x0000754C
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000085B4 File Offset: 0x000075B4
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Array;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000085B8 File Offset: 0x000075B8
		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000085C0 File Offset: 0x000075C0
		public int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600036D RID: 877 RVA: 0x000085C8 File Offset: 0x000075C8
		public override string NameSuffix
		{
			get
			{
				return "[" + new string(',', this.dimensions - 1) + "]";
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000085E8 File Offset: 0x000075E8
		public override bool? IsReferenceType
		{
			get
			{
				return new bool?(true);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000085F0 File Offset: 0x000075F0
		public override int GetHashCode()
		{
			return this.elementType.GetHashCode() * 71681 + this.dimensions;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000860C File Offset: 0x0000760C
		public override bool Equals(IType other)
		{
			ArrayType arrayType = other as ArrayType;
			return arrayType != null && this.elementType.Equals(arrayType.elementType) && arrayType.dimensions == this.dimensions;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00008646 File Offset: 0x00007646
		public override ITypeReference ToTypeReference()
		{
			return new ArrayTypeReference(this.elementType.ToTypeReference(), this.dimensions);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00008660 File Offset: 0x00007660
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

		// Token: 0x06000373 RID: 883 RVA: 0x00008713 File Offset: 0x00007713
		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetMethods(filter, options);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00008735 File Offset: 0x00007735
		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetMethods(typeArguments, filter, options);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00008758 File Offset: 0x00007758
		public override IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetAccessors(filter, options);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000877A File Offset: 0x0000777A
		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IProperty>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Array).GetProperties(filter, options);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000879C File Offset: 0x0000779C
		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitArrayType(this);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000087A8 File Offset: 0x000077A8
		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new ArrayType(this.compilation, type, this.dimensions);
		}

		// Token: 0x040000D9 RID: 217
		private readonly int dimensions;

		// Token: 0x040000DA RID: 218
		private readonly ICompilation compilation;
	}
}
