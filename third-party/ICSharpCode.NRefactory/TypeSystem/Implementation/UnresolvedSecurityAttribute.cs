using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	[Serializable]
	internal sealed class UnresolvedSecurityAttribute : IUnresolvedAttribute, ISupportsInterning
	{
		public UnresolvedSecurityAttribute(UnresolvedSecurityDeclarationBlob secDecl, int index)
		{
			this.secDecl = secDecl;
			this.index = index;
		}

		DomRegion IUnresolvedAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		IAttribute IUnresolvedAttribute.CreateResolvedAttribute(ITypeResolveContext context)
		{
			return this.secDecl.Resolve(context.CurrentAssembly)[this.index];
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.index ^ this.secDecl.GetHashCode();
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			UnresolvedSecurityAttribute unresolvedSecurityAttribute = other as UnresolvedSecurityAttribute;
			return unresolvedSecurityAttribute != null && this.index == unresolvedSecurityAttribute.index && this.secDecl == unresolvedSecurityAttribute.secDecl;
		}

		private readonly UnresolvedSecurityDeclarationBlob secDecl;

		private readonly int index;
	}
}
