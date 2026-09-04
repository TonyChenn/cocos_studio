using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public sealed class ArrayTypeReference : ITypeReference, ISupportsInterning
	{
		// Token: 0x0600037B RID: 891 RVA: 0x000087DF File Offset: 0x000077DF
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000881D File Offset: 0x0000781D
		public ITypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00008825 File Offset: 0x00007825
		public int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000882D File Offset: 0x0000782D
		public IType Resolve(ITypeResolveContext context)
		{
			return new ArrayType(context.Compilation, this.elementType.Resolve(context), this.dimensions);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000884C File Offset: 0x0000784C
		public override string ToString()
		{
			return this.elementType.ToString() + "[" + new string(',', this.dimensions - 1) + "]";
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00008877 File Offset: 0x00007877
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.elementType.GetHashCode() ^ this.dimensions;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000888C File Offset: 0x0000788C
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			ArrayTypeReference arrayTypeReference = other as ArrayTypeReference;
			return arrayTypeReference != null && this.elementType == arrayTypeReference.elementType && this.dimensions == arrayTypeReference.dimensions;
		}

		// Token: 0x040000DB RID: 219
		private readonly ITypeReference elementType;

		// Token: 0x040000DC RID: 220
		private readonly int dimensions;
	}
}
