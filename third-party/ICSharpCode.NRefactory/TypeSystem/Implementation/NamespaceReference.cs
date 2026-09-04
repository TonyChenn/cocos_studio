using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000BD RID: 189
	public sealed class NamespaceReference : ISymbolReference
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x00011416 File Offset: 0x00010416
		public NamespaceReference(IAssemblyReference assemblyReference, string fullName)
		{
			if (assemblyReference == null)
			{
				throw new ArgumentNullException("assemblyReference");
			}
			this.assemblyReference = assemblyReference;
			this.fullName = fullName;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001143C File Offset: 0x0001043C
		public ISymbol Resolve(ITypeResolveContext context)
		{
			IAssembly assembly = this.assemblyReference.Resolve(context);
			INamespace @namespace = assembly.RootNamespace;
			string[] array = this.fullName.Split(new char[]
			{
				'.'
			});
			int num = 0;
			while (num < array.Length && @namespace != null)
			{
				@namespace = @namespace.GetChildNamespace(array[num]);
				num++;
			}
			return @namespace;
		}

		// Token: 0x040001EE RID: 494
		private IAssemblyReference assemblyReference;

		// Token: 0x040001EF RID: 495
		private string fullName;
	}
}
