using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Serializable]
	public sealed class ByReferenceTypeReference : ITypeReference, ISupportsInterning
	{
		public ByReferenceTypeReference(ITypeReference elementType)
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
			return new ByReferenceType(this.elementType.Resolve(context));
		}

		public override string ToString()
		{
			return this.elementType.ToString() + "&";
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ 91725814;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			ByReferenceTypeReference byReferenceTypeReference = other as ByReferenceTypeReference;
			return byReferenceTypeReference != null && this.elementType == byReferenceTypeReference.elementType;
		}

		private readonly ITypeReference elementType;
	}
}
