using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public sealed class NamespaceReference : ISymbolReference
	{
		public NamespaceReference(IAssemblyReference assemblyReference, string fullName)
		{
			if (assemblyReference == null)
			{
				throw new ArgumentNullException("assemblyReference");
			}
			this.assemblyReference = assemblyReference;
			this.fullName = fullName;
		}

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

		private IAssemblyReference assemblyReference;

		private string fullName;
	}
}
