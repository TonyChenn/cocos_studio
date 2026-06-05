using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000BE RID: 190
	public sealed class MergedNamespaceReference : ISymbolReference
	{
		// Token: 0x06000697 RID: 1687 RVA: 0x00011495 File Offset: 0x00010495
		public MergedNamespaceReference(string externAlias, string fullName)
		{
			this.externAlias = externAlias;
			this.fullName = fullName;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000114AC File Offset: 0x000104AC
		public ISymbol Resolve(ITypeResolveContext context)
		{
			string[] array = this.fullName.Split(new char[]
			{
				'.'
			});
			INamespace @namespace = context.Compilation.GetNamespaceForExternAlias(this.externAlias);
			int num = 0;
			while (num < array.Length && @namespace != null)
			{
				@namespace = @namespace.GetChildNamespace(array[num]);
				num++;
			}
			return @namespace;
		}

		// Token: 0x040001F0 RID: 496
		private string externAlias;

		// Token: 0x040001F1 RID: 497
		private string fullName;
	}
}
