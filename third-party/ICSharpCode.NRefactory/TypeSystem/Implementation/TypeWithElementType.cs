using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x02000069 RID: 105
	public abstract class TypeWithElementType : AbstractType
	{
		// Token: 0x06000361 RID: 865 RVA: 0x000084D0 File Offset: 0x000074D0
		protected TypeWithElementType(IType elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			this.elementType = elementType;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000362 RID: 866 RVA: 0x000084ED File Offset: 0x000074ED
		public override string Name
		{
			get
			{
				return this.elementType.Name + this.NameSuffix;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00008505 File Offset: 0x00007505
		public override string Namespace
		{
			get
			{
				return this.elementType.Namespace;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00008512 File Offset: 0x00007512
		public override string FullName
		{
			get
			{
				return this.elementType.FullName + this.NameSuffix;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000852A File Offset: 0x0000752A
		public override string ReflectionName
		{
			get
			{
				return this.elementType.ReflectionName + this.NameSuffix;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000366 RID: 870
		public abstract string NameSuffix { get; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00008542 File Offset: 0x00007542
		public IType ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x06000368 RID: 872
		public abstract override IType VisitChildren(TypeVisitor visitor);

		// Token: 0x040000D8 RID: 216
		[CLSCompliant(false)]
		protected IType elementType;
	}
}
