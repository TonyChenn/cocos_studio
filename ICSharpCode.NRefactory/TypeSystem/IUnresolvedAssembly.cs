using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an unresolved assembly.
	/// </summary>
	// Token: 0x0200008E RID: 142
	public interface IUnresolvedAssembly : IAssemblyReference
	{
		/// <summary>
		/// Gets the assembly name (short name).
		/// </summary>
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600048A RID: 1162
		string AssemblyName { get; }

		/// <summary>
		/// Gets the full assembly name (including public key token etc.)
		/// </summary>
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600048B RID: 1163
		string FullAssemblyName { get; }

		/// <summary>
		/// Gets the path to the assembly location. 
		/// For projects it is the same as the output path.
		/// </summary>
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600048C RID: 1164
		string Location { get; }

		/// <summary>
		/// Gets the list of all assembly attributes in the project.
		/// </summary>
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600048D RID: 1165
		IEnumerable<IUnresolvedAttribute> AssemblyAttributes { get; }

		/// <summary>
		/// Gets the list of all module attributes in the project.
		/// </summary>
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600048E RID: 1166
		IEnumerable<IUnresolvedAttribute> ModuleAttributes { get; }

		/// <summary>
		/// Gets all non-nested types in the assembly.
		/// </summary>
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600048F RID: 1167
		IEnumerable<IUnresolvedTypeDefinition> TopLevelTypeDefinitions { get; }
	}
}
