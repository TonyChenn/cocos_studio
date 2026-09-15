using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public sealed class MergedNamespaceReference : ISymbolReference
	{
		public MergedNamespaceReference(string externAlias, string fullName)
		{
			this.externAlias = externAlias;
			this.fullName = fullName;
		}

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

		private string externAlias;

		private string fullName;
	}
}
