using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// References an existing assembly by name.
	/// </summary>
	// Token: 0x020000A8 RID: 168
	[Serializable]
	public sealed class DefaultAssemblyReference : IAssemblyReference, ISupportsInterning
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x0000D29C File Offset: 0x0000C29C
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

		// Token: 0x06000588 RID: 1416 RVA: 0x0000D2D8 File Offset: 0x0000C2D8
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

		// Token: 0x06000589 RID: 1417 RVA: 0x0000D360 File Offset: 0x0000C360
		public override string ToString()
		{
			return this.shortName;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000D368 File Offset: 0x0000C368
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.shortName.GetHashCode();
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000D378 File Offset: 0x0000C378
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultAssemblyReference defaultAssemblyReference = other as DefaultAssemblyReference;
			return defaultAssemblyReference != null && this.shortName == defaultAssemblyReference.shortName;
		}

		// Token: 0x04000182 RID: 386
		public static readonly IAssemblyReference CurrentAssembly = new DefaultAssemblyReference.CurrentAssemblyReference();

		// Token: 0x04000183 RID: 387
		[Obsolete("The corlib is not always called 'mscorlib' (as returned by this property), but might be 'System.Runtime'.")]
		public static readonly IAssemblyReference Corlib = new DefaultAssemblyReference("mscorlib");

		// Token: 0x04000184 RID: 388
		private readonly string shortName;

		// Token: 0x020000A9 RID: 169
		[Serializable]
		private sealed class CurrentAssemblyReference : IAssemblyReference
		{
			// Token: 0x0600058D RID: 1421 RVA: 0x0000D3C0 File Offset: 0x0000C3C0
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
