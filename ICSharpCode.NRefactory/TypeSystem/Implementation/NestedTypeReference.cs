using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Type reference used to reference nested types.
	/// </summary>
	// Token: 0x020000D7 RID: 215
	[Serializable]
	public sealed class NestedTypeReference : ITypeReference, ISymbolReference, ISupportsInterning
	{
		/// <summary>
		/// Creates a new NestedTypeReference.
		/// </summary>
		/// <param name="declaringTypeRef">Reference to the declaring type.</param>
		/// <param name="name">Name of the nested class</param>
		/// <param name="additionalTypeParameterCount">Number of type parameters on the inner class (without type parameters on baseTypeRef)</param>
		/// <remarks>
		/// <paramref name="declaringTypeRef" /> must be exactly the (unbound) declaring type, not a derived type, not a parameterized type.
		/// NestedTypeReference thus always resolves to a type definition, never to (partially) parameterized types.
		/// </remarks>
		// Token: 0x060007F2 RID: 2034 RVA: 0x000150EF File Offset: 0x000140EF
		public NestedTypeReference(ITypeReference declaringTypeRef, string name, int additionalTypeParameterCount)
		{
			if (declaringTypeRef == null)
			{
				throw new ArgumentNullException("declaringTypeRef");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.declaringTypeRef = declaringTypeRef;
			this.name = name;
			this.additionalTypeParameterCount = additionalTypeParameterCount;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00015128 File Offset: 0x00014128
		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.declaringTypeRef;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00015130 File Offset: 0x00014130
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00015138 File Offset: 0x00014138
		public int AdditionalTypeParameterCount
		{
			get
			{
				return this.additionalTypeParameterCount;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00015140 File Offset: 0x00014140
		public IType Resolve(ITypeResolveContext context)
		{
			ITypeDefinition typeDefinition = this.declaringTypeRef.Resolve(context) as ITypeDefinition;
			if (typeDefinition != null)
			{
				int typeParameterCount = typeDefinition.TypeParameterCount;
				foreach (IType type in typeDefinition.NestedTypes)
				{
					if (type.Name == this.name && type.TypeParameterCount == typeParameterCount + this.additionalTypeParameterCount)
					{
						return type;
					}
				}
			}
			return new UnknownType(null, this.name, this.additionalTypeParameterCount);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000151E4 File Offset: 0x000141E4
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			IType type = this.Resolve(context);
			if (type is ITypeDefinition)
			{
				return (ISymbol)type;
			}
			return null;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001520C File Offset: 0x0001420C
		public override string ToString()
		{
			if (this.additionalTypeParameterCount == 0)
			{
				return this.declaringTypeRef + "+" + this.name;
			}
			return string.Concat(new object[]
			{
				this.declaringTypeRef,
				"+",
				this.name,
				"`",
				this.additionalTypeParameterCount
			});
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00015275 File Offset: 0x00014275
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.declaringTypeRef.GetHashCode() ^ this.name.GetHashCode() ^ this.additionalTypeParameterCount;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00015298 File Offset: 0x00014298
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			NestedTypeReference nestedTypeReference = other as NestedTypeReference;
			return nestedTypeReference != null && this.declaringTypeRef == nestedTypeReference.declaringTypeRef && this.name == nestedTypeReference.name && this.additionalTypeParameterCount == nestedTypeReference.additionalTypeParameterCount;
		}

		// Token: 0x04000249 RID: 585
		private readonly ITypeReference declaringTypeRef;

		// Token: 0x0400024A RID: 586
		private readonly string name;

		// Token: 0x0400024B RID: 587
		private readonly int additionalTypeParameterCount;
	}
}
