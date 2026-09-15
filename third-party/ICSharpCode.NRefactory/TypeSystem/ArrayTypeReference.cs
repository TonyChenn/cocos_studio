using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Serializable]
	public sealed class ArrayTypeReference : ITypeReference, ISupportsInterning
	{
		public ArrayTypeReference(ITypeReference elementType, int dimensions = 1)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (dimensions <= 0)
			{
				throw new ArgumentOutOfRangeException("dimensions", dimensions, "dimensions must be positive");
			}
			this.elementType = elementType;
			this.dimensions = dimensions;
		}

		public ITypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		public int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		public IType Resolve(ITypeResolveContext context)
		{
			return new ArrayType(context.Compilation, this.elementType.Resolve(context), this.dimensions);
		}

		public override string ToString()
		{
			return this.elementType.ToString() + "[" + new string(',', this.dimensions - 1) + "]";
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ this.dimensions;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			ArrayTypeReference arrayTypeReference = other as ArrayTypeReference;
			return arrayTypeReference != null && this.elementType == arrayTypeReference.elementType && this.dimensions == arrayTypeReference.dimensions;
		}

		private readonly ITypeReference elementType;

		private readonly int dimensions;
	}
}
