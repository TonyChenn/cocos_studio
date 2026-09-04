using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000090 RID: 144
	public interface ICompilation
	{
		/// <summary>
		/// Gets the current assembly.
		/// </summary>
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600049A RID: 1178
		IAssembly MainAssembly { get; }

		/// <summary>
		/// Gets the type resolve context that specifies this compilation and no current assembly or entity.
		/// </summary>
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600049B RID: 1179
		ITypeResolveContext TypeResolveContext { get; }

		/// <summary>
		/// Gets the list of all assemblies in the compilation.
		/// </summary>
		/// <remarks>
		/// This main assembly is the first entry in the list.
		/// </remarks>
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600049C RID: 1180
		IList<IAssembly> Assemblies { get; }

		/// <summary>
		/// Gets the referenced assemblies.
		/// This list does not include the main assembly.
		/// </summary>
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600049D RID: 1181
		IList<IAssembly> ReferencedAssemblies { get; }

		/// <summary>
		/// Gets the root namespace of this compilation.
		/// This is a merged version of the root namespaces of all assemblies.
		/// </summary>
		/// <remarks>
		/// This always is the namespace without a name - it's unrelated to the 'root namespace' project setting.
		/// </remarks>
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600049E RID: 1182
		INamespace RootNamespace { get; }

		/// <summary>
		/// Gets the root namespace for a given extern alias.
		/// </summary>
		/// <remarks>
		/// If <paramref name="alias" /> is <c>null</c> or an empty string, this method
		/// returns the global root namespace.
		/// If no alias with the specified name exists, this method returns null.
		/// </remarks>
		// Token: 0x0600049F RID: 1183
		INamespace GetNamespaceForExternAlias(string alias);

		// Token: 0x060004A0 RID: 1184
		IType FindType(KnownTypeCode typeCode);

		/// <summary>
		/// Gets the name comparer for the language being compiled.
		/// This is the string comparer used for the INamespace.GetTypeDefinition method.
		/// </summary>
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060004A1 RID: 1185
		StringComparer NameComparer { get; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060004A2 RID: 1186
		ISolutionSnapshot SolutionSnapshot { get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060004A3 RID: 1187
		CacheManager CacheManager { get; }
	}
}
