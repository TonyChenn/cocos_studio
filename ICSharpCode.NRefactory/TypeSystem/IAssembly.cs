using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an assembly.
	/// </summary>
	// Token: 0x0200008F RID: 143
	public interface IAssembly : ICompilationProvider
	{
		/// <summary>
		/// Gets the original unresolved assembly.
		/// </summary>
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000490 RID: 1168
		IUnresolvedAssembly UnresolvedAssembly { get; }

		/// <summary>
		/// Gets whether this assembly is the main assembly of the compilation.
		/// </summary>
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000491 RID: 1169
		bool IsMainAssembly { get; }

		/// <summary>
		/// Gets the assembly name (short name).
		/// </summary>
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000492 RID: 1170
		string AssemblyName { get; }

		/// <summary>
		/// Gets the full assembly name (including public key token etc.)
		/// </summary>
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000493 RID: 1171
		string FullAssemblyName { get; }

		/// <summary>
		/// Gets the list of all assembly attributes in the project.
		/// </summary>
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000494 RID: 1172
		IList<IAttribute> AssemblyAttributes { get; }

		/// <summary>
		/// Gets the list of all module attributes in the project.
		/// </summary>
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000495 RID: 1173
		IList<IAttribute> ModuleAttributes { get; }

		/// <summary>
		/// Gets whether the internals of this assembly are visible in the specified assembly.
		/// </summary>
		// Token: 0x06000496 RID: 1174
		bool InternalsVisibleTo(IAssembly assembly);

		/// <summary>
		/// Gets the root namespace for this assembly.
		/// </summary>
		/// <remarks>
		/// This always is the namespace without a name - it's unrelated to the 'root namespace' project setting.
		/// </remarks>
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000497 RID: 1175
		INamespace RootNamespace { get; }

		/// <summary>
		/// Gets the type definition for a top-level type.
		/// </summary>
		/// <remarks>This method uses ordinal name comparison, not the compilation's name comparer.</remarks>
		// Token: 0x06000498 RID: 1176
		ITypeDefinition GetTypeDefinition(TopLevelTypeName topLevelTypeName);

		/// <summary>
		/// Gets all non-nested types in the assembly.
		/// </summary>
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000499 RID: 1177
		IEnumerable<ITypeDefinition> TopLevelTypeDefinitions { get; }
	}
}
