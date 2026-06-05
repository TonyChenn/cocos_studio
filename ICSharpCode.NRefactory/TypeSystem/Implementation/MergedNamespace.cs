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
	// Token: 0x020000D5 RID: 213
	public sealed class MergedNamespace : INamespace, ISymbol, ICompilationProvider
	{
		/// <summary>
		/// Creates a new merged root namespace.
		/// </summary>
		/// <param name="compilation">The main compilation.</param>
		/// <param name="namespaces">The individual namespaces being merged.</param>
		/// <param name="externAlias">The extern alias for this namespace.</param>
		// Token: 0x060007D9 RID: 2009 RVA: 0x00014CBC File Offset: 0x00013CBC
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
		// Token: 0x060007DA RID: 2010 RVA: 0x00014CF8 File Offset: 0x00013CF8
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00014D4D File Offset: 0x00013D4D
		public string ExternAlias
		{
			get
			{
				return this.externAlias;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00014D55 File Offset: 0x00013D55
		public string FullName
		{
			get
			{
				return this.namespaces[0].FullName;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00014D64 File Offset: 0x00013D64
		public string Name
		{
			get
			{
				return this.namespaces[0].Name;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00014D73 File Offset: 0x00013D73
		public INamespace ParentNamespace
		{
			get
			{
				return this.parentNamespace;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00014D83 File Offset: 0x00013D83
		public IEnumerable<ITypeDefinition> Types
		{
			get
			{
				return this.namespaces.SelectMany((INamespace ns) => ns.Types);
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00014DAD File Offset: 0x00013DAD
		public SymbolKind SymbolKind
		{
			get
			{
				return SymbolKind.Namespace;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00014DB1 File Offset: 0x00013DB1
		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00014DC1 File Offset: 0x00013DC1
		public IEnumerable<IAssembly> ContributingAssemblies
		{
			get
			{
				return this.namespaces.SelectMany((INamespace ns) => ns.ContributingAssemblies);
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00014DEB File Offset: 0x00013DEB
		public IEnumerable<INamespace> ChildNamespaces
		{
			get
			{
				return this.GetChildNamespaces().Values;
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00014DF8 File Offset: 0x00013DF8
		public INamespace GetChildNamespace(string name)
		{
			INamespace result;
			if (this.GetChildNamespaces().TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00014E28 File Offset: 0x00013E28
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

		// Token: 0x060007E6 RID: 2022 RVA: 0x00014F04 File Offset: 0x00013F04
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

		// Token: 0x060007E7 RID: 2023 RVA: 0x00014F54 File Offset: 0x00013F54
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[MergedNamespace {0}{1} (from {2} assemblies)]", new object[]
			{
				(this.externAlias != null) ? (this.externAlias + "::") : null,
				this.FullName,
				this.namespaces.Length
			});
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00014FAF File Offset: 0x00013FAF
		public ISymbolReference ToReference()
		{
			return new MergedNamespaceReference(this.externAlias, this.FullName);
		}

		// Token: 0x0400023E RID: 574
		private readonly string externAlias;

		// Token: 0x0400023F RID: 575
		private readonly ICompilation compilation;

		// Token: 0x04000240 RID: 576
		private readonly INamespace parentNamespace;

		// Token: 0x04000241 RID: 577
		private readonly INamespace[] namespaces;

		// Token: 0x04000242 RID: 578
		private Dictionary<string, INamespace> childNamespaces;
	}
}
