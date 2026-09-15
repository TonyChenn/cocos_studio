using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Serializable]
	public sealed class PointerTypeReference : ITypeReference, ISupportsInterning
	{
		public PointerTypeReference(ITypeReference elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			this.elementType = elementType;
		}

		public ITypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		public IType Resolve(ITypeResolveContext context)
		{
			return new PointerType(this.elementType.Resolve(context));
		}

		public override string ToString()
		{
			return this.elementType.ToString() + "*";
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ 91725812;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			PointerTypeReference pointerTypeReference = other as PointerTypeReference;
			return pointerTypeReference != null && this.elementType == pointerTypeReference.elementType;
		}

		private readonly ITypeReference elementType;
	}
}
