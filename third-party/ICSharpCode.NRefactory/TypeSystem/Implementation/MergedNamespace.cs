using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// A merged namespace.
	/// </summary>
	public sealed class MergedNamespace : INamespace, ISymbol, ICompilationProvider
	{
		/// <summary>
		/// Creates a new merged root namespace.
		/// </summary>
		/// <param name="compilation">The main compilation.</param>
		/// <param name="namespaces">The individual namespaces being merged.</param>
		/// <param name="externAlias">The extern alias for this namespace.</param>
		public MergedNamespace(ICompilation compilation, INamespace[] namespaces, string externAlias = null)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (namespaces == null)
			{
				throw new ArgumentNullException("namespaces");
			}
			this.compilation = compilation;
			this.namespaces = namespaces;
			this.externAlias = externAlias;
		}

		/// <summary>
		/// Creates a new merged child namespace.
		/// </summary>
		/// <param name="parentNamespace">The parent merged namespace.</param>
		/// <param name="namespaces">The individual namespaces being merged.</param>
		public MergedNamespace(INamespace parentNamespace, INamespace[] namespaces)
		{
			if (parentNamespace == null)
			{
				throw new ArgumentNullException("parentNamespace");
			}
			if (namespaces == null)
			{
				throw new ArgumentNullException("namespaces");
			}
			this.parentNamespace = parentNamespace;
			this.namespaces = namespaces;
			this.compilation = parentNamespace.Compilation;
			this.externAlias = parentNamespace.ExternAlias;
		}

		public string ExternAlias
		{
			get
			{
				return this.externAlias;
			}
		}

		public string FullName
		{
			get
			{
				return this.namespaces[0].FullName;
			}
		}

		public string Name
		{
			get
			{
				return this.namespaces[0].Name;
			}
		}

		public INamespace ParentNamespace
		{
			get
			{
				return this.parentNamespace;
			}
		}

		public IEnumerable<ITypeDefinition> Types
		{
			get
			{
				return this.namespaces.SelectMany((INamespace ns) => ns.Types);
			}
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return SymbolKind.Namespace;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		public IEnumerable<IAssembly> ContributingAssemblies
		{
			get
			{
				return this.namespaces.SelectMany((INamespace ns) => ns.ContributingAssemblies);
			}
		}

		public IEnumerable<INamespace> ChildNamespaces
		{
			get
			{
				return this.GetChildNamespaces().Values;
			}
		}

		public INamespace GetChildNamespace(string name)
		{
			INamespace result;
			if (this.GetChildNamespaces().TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		private Dictionary<string, INamespace> GetChildNamespaces()
		{
			Dictionary<string, INamespace> dictionary = LazyInit.VolatileRead<Dictionary<string, INamespace>>(ref this.childNamespaces);
			if (dictionary != null)
			{
				return dictionary;
			}
			dictionary = new Dictionary<string, INamespace>(this.compilation.NameComparer);
			foreach (IGrouping<string, INamespace> grouping in this.namespaces.SelectMany((INamespace ns) => ns.ChildNamespaces).GroupBy((INamespace ns) => ns.Name, this.compilation.NameComparer))
			{
				dictionary.Add(grouping.Key, new MergedNamespace(this, grouping.ToArray<INamespace>()));
			}
			return LazyInit.GetOrSet<Dictionary<string, INamespace>>(ref this.childNamespaces, dictionary);
		}

		public ITypeDefinition GetTypeDefinition(string name, int typeParameterCount)
		{
			ITypeDefinition result = null;
			foreach (INamespace @namespace in this.namespaces)
			{
				ITypeDefinition typeDefinition = @namespace.GetTypeDefinition(name, typeParameterCount);
				if (typeDefinition != null)
				{
					if (typeDefinition.IsPublic)
					{
						return typeDefinition;
					}
					result = typeDefinition;
				}
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[MergedNamespace {0}{1} (from {2} assemblies)]", new object[]
			{
				(this.externAlias != null) ? (this.externAlias + "::") : null,
				this.FullName,
				this.namespaces.Length
			});
		}

		public ISymbolReference ToReference()
		{
			return new MergedNamespaceReference(this.externAlias, this.FullName);
		}

		private readonly string externAlias;

		private readonly ICompilation compilation;

		private readonly INamespace parentNamespace;

		private readonly INamespace[] namespaces;

		private Dictionary<string, INamespace> childNamespaces;
	}
}
