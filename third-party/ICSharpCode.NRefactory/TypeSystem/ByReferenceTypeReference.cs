using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public sealed class ByReferenceTypeReference : ITypeReference, ISupportsInterning
	{
		// Token: 0x06000393 RID: 915 RVA: 0x00008A95 File Offset: 0x00007A95
		public ByReferenceTypeReference(ITypeReference elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			this.elementType = elementType;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00008AB2 File Offset: 0x00007AB2
		public ITypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00008ABA File Offset: 0x00007ABA
		public IType Resolve(ITypeResolveContext context)
		{
			return new ByReferenceType(this.elementType.Resolve(context));
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00008ACD File Offset: 0x00007ACD
		public override string ToString()
		{
			return this.elementType.ToString() + "&";
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00008AE4 File Offset: 0x00007AE4
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ 91725814;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00008AF8 File Offset: 0x00007AF8
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			ByReferenceTypeReference byReferenceTypeReference = other as ByReferenceTypeReference;
			return byReferenceTypeReference != null && this.elementType == byReferenceTypeReference.elementType;
		}

		// Token: 0x040000DF RID: 223
		private readonly ITypeReference elementType;
	}
}
