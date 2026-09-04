using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a single file that was parsed.
	/// </summary>
	// Token: 0x020000EB RID: 235
	public interface IUnresolvedFile
	{
		/// <summary>
		/// Returns the full path of the file.
		/// </summary>
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060008BF RID: 2239
		string FileName { get; }

		/// <summary>
		/// Gets the time when the file was last written.
		/// </summary>
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060008C0 RID: 2240
		// (set) Token: 0x060008C1 RID: 2241
		DateTime? LastWriteTime { get; set; }

		/// <summary>
		/// Gets all top-level type definitions.
		/// </summary>
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060008C2 RID: 2242
		IList<IUnresolvedTypeDefinition> TopLevelTypeDefinitions { get; }

		/// <summary>
		/// Gets all assembly attributes that are defined in this file.
		/// </summary>
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060008C3 RID: 2243
		IList<IUnresolvedAttribute> AssemblyAttributes { get; }

		/// <summary>
		/// Gets all module attributes that are defined in this file.
		/// </summary>
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060008C4 RID: 2244
		IList<IUnresolvedAttribute> ModuleAttributes { get; }

		/// <summary>
		/// Gets the top-level type defined at the specified location.
		/// Returns null if no type is defined at that location.
		/// </summary>
		// Token: 0x060008C5 RID: 2245
		IUnresolvedTypeDefinition GetTopLevelTypeDefinition(TextLocation location);

		/// <summary>
		/// Gets the type (potentially a nested type) defined at the specified location.
		/// Returns null if no type is defined at that location.
		/// </summary>
		// Token: 0x060008C6 RID: 2246
		IUnresolvedTypeDefinition GetInnermostTypeDefinition(TextLocation location);

		/// <summary>
		/// Gets the member defined at the specified location.
		/// Returns null if no member is defined at that location.
		/// </summary>
		// Token: 0x060008C7 RID: 2247
		IUnresolvedMember GetMember(TextLocation location);

		/// <summary>
		/// Gets the parser errors.
		/// </summary>
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060008C8 RID: 2248
		IList<Error> Errors { get; }
	}
}
