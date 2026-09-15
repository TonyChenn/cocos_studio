using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Type reference used to reference nested types.
	/// </summary>
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

		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.declaringTypeRef;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int AdditionalTypeParameterCount
		{
			get
			{
				return this.additionalTypeParameterCount;
			}
		}

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

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			IType type = this.Resolve(context);
			if (type is ITypeDefinition)
			{
				return (ISymbol)type;
			}
			return null;
		}

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

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.declaringTypeRef.GetHashCode() ^ this.name.GetHashCode() ^ this.additionalTypeParameterCount;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			NestedTypeReference nestedTypeReference = other as NestedTypeReference;
			return nestedTypeReference != null && this.declaringTypeRef == nestedTypeReference.declaringTypeRef && this.name == nestedTypeReference.name && this.additionalTypeParameterCount == nestedTypeReference.additionalTypeParameterCount;
		}

		private readonly ITypeReference declaringTypeRef;

		private readonly string name;

		private readonly int additionalTypeParameterCount;
	}
}
