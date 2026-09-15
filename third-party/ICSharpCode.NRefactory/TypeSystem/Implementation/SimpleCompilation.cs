using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Simple compilation implementation.
	/// </summary>
	public class SimpleCompilation : ICompilation
	{
		public SimpleCompilation(IUnresolvedAssembly mainAssembly, params IAssemblyReference[] assemblyReferences) : this(new DefaultSolutionSnapshot(), mainAssembly, (IEnumerable<IAssemblyReference>)assemblyReferences)
		{
		}

		public SimpleCompilation(IUnresolvedAssembly mainAssembly, IEnumerable<IAssemblyReference> assemblyReferences) : this(new DefaultSolutionSnapshot(), mainAssembly, assemblyReferences)
		{
		}

		public SimpleCompilation(ISolutionSnapshot solutionSnapshot, IUnresolvedAssembly mainAssembly, params IAssemblyReference[] assemblyReferences) : this(solutionSnapshot, mainAssembly, (IEnumerable<IAssemblyReference>)assemblyReferences)
		{
		}

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

		public ITypeResolveContext TypeResolveContext
		{
			get
			{
				return this.context;
			}
		}

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

		public CacheManager CacheManager
		{
			get
			{
				return this.cacheManager;
			}
		}

		public virtual INamespace GetNamespaceForExternAlias(string alias)
		{
			if (string.IsNullOrEmpty(alias))
			{
				return this.RootNamespace;
			}
			return null;
		}

		public IType FindType(KnownTypeCode typeCode)
		{
			return this.knownTypeCache.FindType(typeCode);
		}

		public StringComparer NameComparer
		{
			get
			{
				return StringComparer.Ordinal;
			}
		}

		public ISolutionSnapshot SolutionSnapshot
		{
			get
			{
				return this.solutionSnapshot;
			}
		}

		public override string ToString()
		{
			return "[SimpleCompilation " + this.mainAssembly.AssemblyName + "]";
		}

		private readonly ISolutionSnapshot solutionSnapshot;

		private readonly ITypeResolveContext context;

		private readonly CacheManager cacheManager = new CacheManager();

		private readonly KnownTypeCache knownTypeCache;

		private readonly IAssembly mainAssembly;

		private readonly IList<IAssembly> assemblies;

		private readonly IList<IAssembly> referencedAssemblies;

		private INamespace rootNamespace;
	}
}
