using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x02000081 RID: 129
	[Serializable]
	internal sealed class UnresolvedSecurityAttribute : IUnresolvedAttribute, ISupportsInterning
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x0000A1AA File Offset: 0x000091AA
		public UnresolvedSecurityAttribute(UnresolvedSecurityDeclarationBlob secDecl, int index)
		{
			this.secDecl = secDecl;
			this.index = index;
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000A1C0 File Offset: 0x000091C0
		DomRegion IUnresolvedAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000A1C7 File Offset: 0x000091C7
		IAttribute IUnresolvedAttribute.CreateResolvedAttribute(ITypeResolveContext context)
		{
			return this.secDecl.Resolve(context.CurrentAssembly)[this.index];
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000A1E5 File Offset: 0x000091E5
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.index ^ this.secDecl.GetHashCode();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000A1FC File Offset: 0x000091FC
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			UnresolvedSecurityAttribute unresolvedSecurityAttribute = other as UnresolvedSecurityAttribute;
			return unresolvedSecurityAttribute != null && this.index == unresolvedSecurityAttribute.index && this.secDecl == unresolvedSecurityAttribute.secDecl;
		}

		// Token: 0x04000113 RID: 275
		private readonly UnresolvedSecurityDeclarationBlob secDecl;

		// Token: 0x04000114 RID: 276
		private readonly int index;
	}
}
