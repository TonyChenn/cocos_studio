using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a resolved namespace.
	/// </summary>
	// Token: 0x020000BB RID: 187
	public interface INamespace : ISymbol, ICompilationProvider
	{
		/// <summary>
		/// Gets the extern alias for this namespace.
		/// Returns an empty string for normal namespaces.
		/// </summary>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600067E RID: 1662
		string ExternAlias { get; }

		/// <summary>
		/// Gets the full name of this namespace. (e.g. "System.Collections")
		/// </summary>
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600067F RID: 1663
		string FullName { get; }

		/// <summary>
		/// Gets the short name of this namespace (e.g. "Collections").
		/// </summary>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000680 RID: 1664
		string Name { get; }

		/// <summary>
		/// Gets the parent namespace.
		/// Returns null if this is the root namespace.
		/// </summary>
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000681 RID: 1665
		INamespace ParentNamespace { get; }

		/// <summary>
		/// Gets the child namespaces in this namespace.
		/// </summary>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000682 RID: 1666
		IEnumerable<INamespace> ChildNamespaces { get; }

		/// <summary>
		/// Gets the types in this namespace.
		/// </summary>
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000683 RID: 1667
		IEnumerable<ITypeDefinition> Types { get; }

		/// <summary>
		/// Gets the assemblies that contribute types to this namespace (or to child namespaces).
		/// </summary>
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000684 RID: 1668
		IEnumerable<IAssembly> ContributingAssemblies { get; }

		/// <summary>
		/// Gets a direct child namespace by its short name.
		/// Returns null when the namespace cannot be found.
		/// </summary>
		/// <remarks>
		/// This method uses the compilation's current string comparer.
		/// </remarks>
		// Token: 0x06000685 RID: 1669
		INamespace GetChildNamespace(string name);

		/// <summary>
		/// Gets the type with the specified short name and type parameter count.
		/// Returns null if the type cannot be found.
		/// </summary>
		/// <remarks>
		/// This method uses the compilation's current string comparer.
		/// </remarks>
		// Token: 0x06000686 RID: 1670
		ITypeDefinition GetTypeDefinition(string name, int typeParameterCount);
	}
}
