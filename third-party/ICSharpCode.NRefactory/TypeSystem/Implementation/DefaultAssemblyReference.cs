using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// References an existing assembly by name.
	/// </summary>
	[Serializable]
	public sealed class DefaultAssemblyReference : IAssemblyReference, ISupportsInterning
	{
		public DefaultAssemblyReference(string assemblyName)
		{
			int num = (assemblyName != null) ? assemblyName.IndexOf(',') : -1;
			if (num >= 0)
			{
				this.shortName = assemblyName.Substring(0, num);
				return;
			}
			this.shortName = assemblyName;
		}

		public IAssembly Resolve(ITypeResolveContext context)
		{
			IAssembly currentAssembly = context.CurrentAssembly;
			if (currentAssembly != null && string.Equals(this.shortName, currentAssembly.AssemblyName, StringComparison.OrdinalIgnoreCase))
			{
				return currentAssembly;
			}
			foreach (IAssembly assembly in context.Compilation.Assemblies)
			{
				if (string.Equals(this.shortName, assembly.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		public override string ToString()
		{
			return this.shortName;
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.shortName.GetHashCode();
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultAssemblyReference defaultAssemblyReference = other as DefaultAssemblyReference;
			return defaultAssemblyReference != null && this.shortName == defaultAssemblyReference.shortName;
		}

		public static readonly IAssemblyReference CurrentAssembly = new DefaultAssemblyReference.CurrentAssemblyReference();

		[Obsolete("The corlib is not always called 'mscorlib' (as returned by this property), but might be 'System.Runtime'.")]
		public static readonly IAssemblyReference Corlib = new DefaultAssemblyReference("mscorlib");

		private readonly string shortName;

		[Serializable]
		private sealed class CurrentAssemblyReference : IAssemblyReference
		{
			public IAssembly Resolve(ITypeResolveContext context)
			{
				IAssembly currentAssembly = context.CurrentAssembly;
				if (currentAssembly == null)
				{
					throw new ArgumentException("A reference to the current assembly cannot be resolved in the compilation's global type resolve context.");
				}
				return currentAssembly;
			}
		}
	}
}
