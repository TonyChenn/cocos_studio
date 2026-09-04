using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	public sealed class PointerTypeReference : ITypeReference, ISupportsInterning
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x00018DC5 File Offset: 0x00017DC5
		public PointerTypeReference(ITypeReference elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			this.elementType = elementType;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00018DE2 File Offset: 0x00017DE2
		public ITypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00018DEA File Offset: 0x00017DEA
		public IType Resolve(ITypeResolveContext context)
		{
			return new PointerType(this.elementType.Resolve(context));
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00018DFD File Offset: 0x00017DFD
		public override string ToString()
		{
			return this.elementType.ToString() + "*";
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00018E14 File Offset: 0x00017E14
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ 91725812;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00018E28 File Offset: 0x00017E28
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			PointerTypeReference pointerTypeReference = other as PointerTypeReference;
			return pointerTypeReference != null && this.elementType == pointerTypeReference.elementType;
		}

		// Token: 0x0400030A RID: 778
		private readonly ITypeReference elementType;
	}
}
