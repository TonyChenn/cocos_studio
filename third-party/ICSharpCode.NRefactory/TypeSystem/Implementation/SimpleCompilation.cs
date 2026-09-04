using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Simple compilation implementation.
	/// </summary>
	// Token: 0x020000D8 RID: 216
	public class SimpleCompilation : ICompilation
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x000152E0 File Offset: 0x000142E0
		public SimpleCompilation(IUnresolvedAssembly mainAssembly, params IAssemblyReference[] assemblyReferences) : this(new DefaultSolutionSnapshot(), mainAssembly, (IEnumerable<IAssemblyReference>)assemblyReferences)
		{
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000152F4 File Offset: 0x000142F4
		public SimpleCompilation(IUnresolvedAssembly mainAssembly, IEnumerable<IAssemblyReference> assemblyReferences) : this(new DefaultSolutionSnapshot(), mainAssembly, assemblyReferences)
		{
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00015303 File Offset: 0x00014303
		public SimpleCompilation(ISolutionSnapshot solutionSnapshot, IUnresolvedAssembly mainAssembly, params IAssemblyReference[] assemblyReferences) : this(solutionSnapshot, mainAssembly, (IEnumerable<IAssemblyReference>)assemblyReferences)
		{
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00015314 File Offset: 0x00014314
		public SimpleCompilation(ISolutionSnapshot solutionSnapshot, IUnresolvedAssembly mainAssembly, IEnumerable<IAssemblyReference> assemblyReferences)
		{
			if (solutionSnapshot == null)
			{
				throw new ArgumentNullException("solutionSnapshot");
			}
			if (mainAssembly == null)
			{
				throw new ArgumentNullException("mainAssembly");
			}
			if (assemblyReferences == null)
			{
				throw new ArgumentNullException("assemblyReferences");
			}
			this.solutionSnapshot = solutionSnapshot;
			this.context = new SimpleTypeResolveContext(this);
			this.mainAssembly = mainAssembly.Resolve(this.context);
			List<IAssembly> list = new List<IAssembly>();
			list.Add(this.mainAssembly);
			List<IAssembly> list2 = new List<IAssembly>();
			foreach (IAssemblyReference assemblyReference in assemblyReferences)
			{
				IAssembly assembly;
				try
				{
					assembly = assemblyReference.Resolve(this.context);
				}
				catch (InvalidOperationException)
				{
					throw new InvalidOperationException("Tried to initialize compilation with an invalid assembly reference. (Forgot to load the assembly reference ? - see CecilLoader)");
				}
				if (assembly != null && !list.Contains(assembly))
				{
					list.Add(assembly);
				}
				if (assembly != null && !list2.Contains(assembly))
				{
					list2.Add(assembly);
				}
			}
			this.assemblies = list.AsReadOnly();
			this.referencedAssemblies = list2.AsReadOnly();
			this.knownTypeCache = new KnownTypeCache(this);
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00015444 File Offset: 0x00014444
		public IAssembly MainAssembly
		{
			get
			{
				if (this.mainAssembly == null)
				{
					throw new InvalidOperationException("Compilation isn't initialized yet");
				}
				return this.mainAssembly;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001545F File Offset: 0x0001445F
		public IList<IAssembly> Assemblies
		{
			get
			{
				if (this.assemblies == null)
				{
					throw new InvalidOperationException("Compilation isn't initialized yet");
				}
				return this.assemblies;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0001547A File Offset: 0x0001447A
		public IList<IAssembly> ReferencedAssemblies
		{
			get
			{
				if (this.referencedAssemblies == null)
				{
					throw new InvalidOperationException("Compilation isn't initialized yet");
				}
				return this.referencedAssemblies;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00015495 File Offset: 0x00014495
		public ITypeResolveContext TypeResolveContext
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x000154A0 File Offset: 0x000144A0
		public INamespace RootNamespace
		{
			get
			{
				INamespace @namespace = LazyInit.VolatileRead<INamespace>(ref this.rootNamespace);
				if (@namespace != null)
				{
					return @namespace;
				}
				if (this.referencedAssemblies == null)
				{
					throw new InvalidOperationException("Compilation isn't initialized yet");
				}
				return LazyInit.GetOrSet<INamespace>(ref this.rootNamespace, this.CreateRootNamespace());
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000154E4 File Offset: 0x000144E4
		protected virtual INamespace CreateRootNamespace()
		{
			INamespace[] array = new INamespace[this.referencedAssemblies.Count + 1];
			array[0] = this.mainAssembly.RootNamespace;
			for (int i = 0; i < this.referencedAssemblies.Count; i++)
			{
				array[i + 1] = this.referencedAssemblies[i].RootNamespace;
			}
			return new MergedNamespace(this, array, null);
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00015546 File Offset: 0x00014546
		public CacheManager CacheManager
		{
			get
			{
				return this.cacheManager;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001554E File Offset: 0x0001454E
		public virtual INamespace GetNamespaceForExternAlias(string alias)
		{
			if (string.IsNullOrEmpty(alias))
			{
				return this.RootNamespace;
			}
			return null;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00015560 File Offset: 0x00014560
		public IType FindType(KnownTypeCode typeCode)
		{
			return this.knownTypeCache.FindType(typeCode);
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0001556E File Offset: 0x0001456E
		public StringComparer NameComparer
		{
			get
			{
				return StringComparer.Ordinal;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00015575 File Offset: 0x00014575
		public ISolutionSnapshot SolutionSnapshot
		{
			get
			{
				return this.solutionSnapshot;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001557D File Offset: 0x0001457D
		public override string ToString()
		{
			return "[SimpleCompilation " + this.mainAssembly.AssemblyName + "]";
		}

		// Token: 0x0400024C RID: 588
		private readonly ISolutionSnapshot solutionSnapshot;

		// Token: 0x0400024D RID: 589
		private readonly ITypeResolveContext context;

		// Token: 0x0400024E RID: 590
		private readonly CacheManager cacheManager = new CacheManager();

		// Token: 0x0400024F RID: 591
		private readonly KnownTypeCache knownTypeCache;

		// Token: 0x04000250 RID: 592
		private readonly IAssembly mainAssembly;

		// Token: 0x04000251 RID: 593
		private readonly IList<IAssembly> assemblies;

		// Token: 0x04000252 RID: 594
		private readonly IList<IAssembly> referencedAssemblies;

		// Token: 0x04000253 RID: 595
		private INamespace rootNamespace;
	}
}
