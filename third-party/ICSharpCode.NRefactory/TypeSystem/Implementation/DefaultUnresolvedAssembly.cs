using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation for <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedAssembly" />.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedAssembly : AbstractFreezable, IUnresolvedAssembly, IAssemblyReference
	{
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.assemblyAttributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.assemblyAttributes);
			this.moduleAttributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.moduleAttributes);
			foreach (IUnresolvedTypeDefinition item in this.typeDefinitions.Values)
			{
				FreezableHelper.Freeze(item);
			}
		}

		/// <summary>
		/// Creates a new unresolved assembly.
		/// </summary>
		/// <param name="assemblyName">Full assembly name</param>
		public DefaultUnresolvedAssembly(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			this.fullAssemblyName = assemblyName;
			int num = (assemblyName != null) ? assemblyName.IndexOf(',') : -1;
			this.assemblyName = ((num < 0) ? assemblyName : assemblyName.Substring(0, num));
			this.assemblyAttributes = new List<IUnresolvedAttribute>();
			this.moduleAttributes = new List<IUnresolvedAttribute>();
		}

		/// <summary>
		/// Gets/Sets the short assembly name.
		/// </summary>
		/// <remarks>
		/// This class handles the short and the full name independently;
		/// if you change the short name, you should also change the full name.
		/// </remarks>
		public string AssemblyName
		{
			get
			{
				return this.assemblyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.assemblyName = value;
			}
		}

		/// <summary>
		/// Gets/Sets the full assembly name.
		/// </summary>
		/// <remarks>
		/// This class handles the short and the full name independently;
		/// if you change the full name, you should also change the short name.
		/// </remarks>
		public string FullAssemblyName
		{
			get
			{
				return this.fullAssemblyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.fullAssemblyName = value;
			}
		}

		public string Location
		{
			get
			{
				return this.location;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.location = value;
			}
		}

		public IList<IUnresolvedAttribute> AssemblyAttributes
		{
			get
			{
				return this.assemblyAttributes;
			}
		}

		IEnumerable<IUnresolvedAttribute> IUnresolvedAssembly.AssemblyAttributes
		{
			get
			{
				return this.assemblyAttributes;
			}
		}

		public IList<IUnresolvedAttribute> ModuleAttributes
		{
			get
			{
				return this.moduleAttributes;
			}
		}

		IEnumerable<IUnresolvedAttribute> IUnresolvedAssembly.ModuleAttributes
		{
			get
			{
				return this.moduleAttributes;
			}
		}

		public IEnumerable<IUnresolvedTypeDefinition> TopLevelTypeDefinitions
		{
			get
			{
				return this.typeDefinitions.Values;
			}
		}

		/// <summary>
		/// Adds a new top-level type definition to this assembly.
		/// </summary>
		/// <remarks>DefaultUnresolvedAssembly does not support partial classes.
		/// Adding more than one part of a type will cause an ArgumentException.</remarks>
		public void AddTypeDefinition(IUnresolvedTypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException("typeDefinition");
			}
			if (typeDefinition.DeclaringTypeDefinition != null)
			{
				throw new ArgumentException("Cannot add nested types.");
			}
			FreezableHelper.ThrowIfFrozen(this);
			TopLevelTypeName key = new TopLevelTypeName(typeDefinition.Namespace, typeDefinition.Name, typeDefinition.TypeParameters.Count);
			this.typeDefinitions.Add(key, typeDefinition);
		}

		/// <summary>
		/// Adds a type forwarder.
		/// This adds both an assembly attribute and an internal forwarder entry, which will be used
		/// by the resolved assembly to provide the forwarded types.
		/// </summary>
		/// <param name="typeName">The name of the type.</param>
		/// <param name="referencedType">The reference used to look up the type in the target assembly.</param>
		public void AddTypeForwarder(TopLevelTypeName typeName, ITypeReference referencedType)
		{
			if (referencedType == null)
			{
				throw new ArgumentNullException("referencedType");
			}
			FreezableHelper.ThrowIfFrozen(this);
			DefaultUnresolvedAttribute defaultUnresolvedAttribute = new DefaultUnresolvedAttribute(DefaultUnresolvedAssembly.typeForwardedToAttributeTypeRef, new KnownTypeReference[]
			{
				KnownTypeReference.Type
			});
			defaultUnresolvedAttribute.PositionalArguments.Add(new DefaultUnresolvedAssembly.TypeOfConstantValue(referencedType));
			this.assemblyAttributes.Add(defaultUnresolvedAttribute);
			this.typeForwarders[typeName] = referencedType;
		}

		public IUnresolvedTypeDefinition GetTypeDefinition(string ns, string name, int typeParameterCount)
		{
			TopLevelTypeName key = new TopLevelTypeName(ns ?? string.Empty, name, typeParameterCount);
			IUnresolvedTypeDefinition result;
			if (this.typeDefinitions.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		public IAssembly Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			base.Freeze();
			CacheManager cacheManager = context.Compilation.CacheManager;
			IAssembly assembly = (IAssembly)cacheManager.GetShared(this);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = new DefaultUnresolvedAssembly.DefaultResolvedAssembly(context.Compilation, this);
			return (IAssembly)cacheManager.GetOrAddShared(this, assembly);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				base.GetType().Name,
				" ",
				this.assemblyName,
				"]"
			});
		}

		private Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> GetTypeDictionary(StringComparer nameComparer)
		{
			if (nameComparer == StringComparer.Ordinal)
			{
				return this.typeDefinitions;
			}
			throw new NotImplementedException();
		}

		private DefaultUnresolvedAssembly.UnresolvedNamespace GetUnresolvedRootNamespace(StringComparer nameComparer)
		{
			LazyInitializer.EnsureInitialized<List<KeyValuePair<StringComparer, DefaultUnresolvedAssembly.UnresolvedNamespace>>>(ref this.unresolvedNamespacesPerNameComparer);
			DefaultUnresolvedAssembly.UnresolvedNamespace result;
			lock (this.unresolvedNamespacesPerNameComparer)
			{
				foreach (KeyValuePair<StringComparer, DefaultUnresolvedAssembly.UnresolvedNamespace> keyValuePair in this.unresolvedNamespacesPerNameComparer)
				{
					if (keyValuePair.Key == nameComparer)
					{
						return keyValuePair.Value;
					}
				}
				DefaultUnresolvedAssembly.UnresolvedNamespace unresolvedNamespace = new DefaultUnresolvedAssembly.UnresolvedNamespace(string.Empty, string.Empty);
				Dictionary<string, DefaultUnresolvedAssembly.UnresolvedNamespace> dictionary = new Dictionary<string, DefaultUnresolvedAssembly.UnresolvedNamespace>(nameComparer);
				dictionary.Add(unresolvedNamespace.FullName, unresolvedNamespace);
				foreach (TopLevelTypeName topLevelTypeName in this.typeDefinitions.Keys)
				{
					DefaultUnresolvedAssembly.GetOrAddNamespace(dictionary, topLevelTypeName.Namespace);
				}
				this.unresolvedNamespacesPerNameComparer.Add(new KeyValuePair<StringComparer, DefaultUnresolvedAssembly.UnresolvedNamespace>(nameComparer, unresolvedNamespace));
				result = unresolvedNamespace;
			}
			return result;
		}

		private static DefaultUnresolvedAssembly.UnresolvedNamespace GetOrAddNamespace(Dictionary<string, DefaultUnresolvedAssembly.UnresolvedNamespace> dict, string fullName)
		{
			DefaultUnresolvedAssembly.UnresolvedNamespace unresolvedNamespace;
			if (dict.TryGetValue(fullName, out unresolvedNamespace))
			{
				return unresolvedNamespace;
			}
			int num = fullName.LastIndexOf('.');
			DefaultUnresolvedAssembly.UnresolvedNamespace unresolvedNamespace2;
			string name;
			if (num < 0)
			{
				unresolvedNamespace2 = dict[string.Empty];
				name = fullName;
			}
			else
			{
				unresolvedNamespace2 = DefaultUnresolvedAssembly.GetOrAddNamespace(dict, fullName.Substring(0, num));
				name = fullName.Substring(num + 1);
			}
			unresolvedNamespace = new DefaultUnresolvedAssembly.UnresolvedNamespace(fullName, name);
			unresolvedNamespace2.Children.Add(unresolvedNamespace);
			dict.Add(fullName, unresolvedNamespace);
			return unresolvedNamespace;
		}

		private string assemblyName;

		private string fullAssemblyName;

		private IList<IUnresolvedAttribute> assemblyAttributes;

		private IList<IUnresolvedAttribute> moduleAttributes;

		private Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> typeDefinitions = new Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition>(TopLevelTypeNameComparer.Ordinal);

		private Dictionary<TopLevelTypeName, ITypeReference> typeForwarders = new Dictionary<TopLevelTypeName, ITypeReference>(TopLevelTypeNameComparer.Ordinal);

		private string location;

		private static readonly ITypeReference typeForwardedToAttributeTypeRef = typeof(TypeForwardedToAttribute).ToTypeReference();

		[NonSerialized]
		private List<KeyValuePair<StringComparer, DefaultUnresolvedAssembly.UnresolvedNamespace>> unresolvedNamespacesPerNameComparer;

		[Serializable]
		private sealed class TypeOfConstantValue : IConstantValue
		{
			public TypeOfConstantValue(ITypeReference typeRef)
			{
				this.typeRef = typeRef;
			}

			public ResolveResult Resolve(ITypeResolveContext context)
			{
				return new TypeOfResolveResult(context.Compilation.FindType(KnownTypeCode.Type), this.typeRef.Resolve(context));
			}

			private readonly ITypeReference typeRef;
		}

		private sealed class UnresolvedNamespace
		{
			public UnresolvedNamespace(string fullName, string name)
			{
				this.FullName = fullName;
				this.Name = name;
			}

			internal readonly string FullName;

			internal readonly string Name;

			internal readonly List<DefaultUnresolvedAssembly.UnresolvedNamespace> Children = new List<DefaultUnresolvedAssembly.UnresolvedNamespace>();
		}

		private sealed class DefaultResolvedAssembly : IAssembly, ICompilationProvider
		{
			public DefaultResolvedAssembly(ICompilation compilation, DefaultUnresolvedAssembly unresolved)
			{
				this.compilation = compilation;
				this.unresolvedAssembly = unresolved;
				this.unresolvedTypeDict = unresolved.GetTypeDictionary(compilation.NameComparer);
				this.rootNamespace = new DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS(this, unresolved.GetUnresolvedRootNamespace(compilation.NameComparer), null);
				this.context = new SimpleTypeResolveContext(this);
				this.AssemblyAttributes = unresolved.AssemblyAttributes.CreateResolvedAttributes(this.context);
				this.ModuleAttributes = unresolved.ModuleAttributes.CreateResolvedAttributes(this.context);
			}

			public IUnresolvedAssembly UnresolvedAssembly
			{
				get
				{
					return this.unresolvedAssembly;
				}
			}

			public bool IsMainAssembly
			{
				get
				{
					return this.Compilation.MainAssembly == this;
				}
			}

			public string AssemblyName
			{
				get
				{
					return this.unresolvedAssembly.AssemblyName;
				}
			}

			public string FullAssemblyName
			{
				get
				{
					return this.unresolvedAssembly.FullAssemblyName;
				}
			}

			public IList<IAttribute> AssemblyAttributes { get; private set; }

			public IList<IAttribute> ModuleAttributes { get; private set; }

			public INamespace RootNamespace
			{
				get
				{
					return this.rootNamespace;
				}
			}

			public ICompilation Compilation
			{
				get
				{
					return this.compilation;
				}
			}

			public bool InternalsVisibleTo(IAssembly assembly)
			{
				if (this == assembly)
				{
					return true;
				}
				foreach (string b in this.GetInternalsVisibleTo())
				{
					if (assembly.AssemblyName == b)
					{
						return true;
					}
				}
				return false;
			}

			private string[] GetInternalsVisibleTo()
			{
				string[] array = this.internalsVisibleTo;
				if (array != null)
				{
					return array;
				}
				using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
				{
					if (!busyLock.Success)
					{
						return new string[0];
					}
					this.internalsVisibleTo = (from attr in this.AssemblyAttributes
					where attr.AttributeType.Name == "InternalsVisibleToAttribute" && attr.AttributeType.Namespace == "System.Runtime.CompilerServices" && attr.PositionalArguments.Count == 1
					select DefaultUnresolvedAssembly.DefaultResolvedAssembly.GetShortName(attr.PositionalArguments.Single<ResolveResult>().ConstantValue as string)).ToArray<string>();
				}
				return this.internalsVisibleTo;
			}

			private static string GetShortName(string fullAssemblyName)
			{
				if (fullAssemblyName == null)
				{
					return null;
				}
				int num = fullAssemblyName.IndexOf(',');
				if (num < 0)
				{
					return fullAssemblyName;
				}
				return fullAssemblyName.Substring(0, num);
			}

			public ITypeDefinition GetTypeDefinition(TopLevelTypeName topLevelTypeName)
			{
				IUnresolvedTypeDefinition unresolved;
				if (this.unresolvedAssembly.typeDefinitions.TryGetValue(topLevelTypeName, out unresolved))
				{
					return this.GetTypeDefinition(unresolved);
				}
				ITypeReference typeReference;
				if (this.unresolvedAssembly.typeForwarders.TryGetValue(topLevelTypeName, out typeReference))
				{
					using (BusyManager.BusyLock busyLock = BusyManager.Enter(typeReference))
					{
						if (busyLock.Success)
						{
							return typeReference.Resolve(this.compilation.TypeResolveContext).GetDefinition();
						}
					}
				}
				return null;
			}

			private ITypeDefinition GetTypeDefinition(IUnresolvedTypeDefinition unresolved)
			{
				return this.typeDict.GetOrAdd(unresolved, (IUnresolvedTypeDefinition t) => this.CreateTypeDefinition(t));
			}

			private ITypeDefinition CreateTypeDefinition(IUnresolvedTypeDefinition unresolved)
			{
				if (unresolved.DeclaringTypeDefinition != null)
				{
					ITypeDefinition typeDefinition = this.GetTypeDefinition(unresolved.DeclaringTypeDefinition);
					return new DefaultResolvedTypeDefinition(this.context.WithCurrentTypeDefinition(typeDefinition), new IUnresolvedTypeDefinition[]
					{
						unresolved
					});
				}
				if (unresolved.Name == "Void" && unresolved.Namespace == "System" && unresolved.TypeParameters.Count == 0)
				{
					return new VoidTypeDefinition(this.context, new IUnresolvedTypeDefinition[]
					{
						unresolved
					});
				}
				return new DefaultResolvedTypeDefinition(this.context, new IUnresolvedTypeDefinition[]
				{
					unresolved
				});
			}

			public IEnumerable<ITypeDefinition> TopLevelTypeDefinitions
			{
				get
				{
					return from t in this.unresolvedAssembly.TopLevelTypeDefinitions
					select this.GetTypeDefinition(t);
				}
			}

			public override string ToString()
			{
				return "[DefaultResolvedAssembly " + this.AssemblyName + "]";
			}

			private readonly DefaultUnresolvedAssembly unresolvedAssembly;

			private readonly ICompilation compilation;

			private readonly ITypeResolveContext context;

			private readonly Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> unresolvedTypeDict;

			private readonly ConcurrentDictionary<IUnresolvedTypeDefinition, ITypeDefinition> typeDict = new ConcurrentDictionary<IUnresolvedTypeDefinition, ITypeDefinition>();

			private readonly INamespace rootNamespace;

			private volatile string[] internalsVisibleTo;

			private sealed class NS : INamespace, ISymbol, ICompilationProvider
			{
				public NS(DefaultUnresolvedAssembly.DefaultResolvedAssembly assembly, DefaultUnresolvedAssembly.UnresolvedNamespace ns, INamespace parentNamespace)
				{
					this.assembly = assembly;
					this.ns = ns;
					this.parentNamespace = parentNamespace;
					this.childNamespaces = new ProjectedList<DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS, DefaultUnresolvedAssembly.UnresolvedNamespace, DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS>(this, ns.Children, (DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS self, DefaultUnresolvedAssembly.UnresolvedNamespace c) => new DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS(self.assembly, c, self));
				}

				string INamespace.ExternAlias
				{
					get
					{
						return null;
					}
				}

				string INamespace.FullName
				{
					get
					{
						return this.ns.FullName;
					}
				}

				SymbolKind ISymbol.SymbolKind
				{
					get
					{
						return SymbolKind.Namespace;
					}
				}

				public string Name
				{
					get
					{
						return this.ns.Name;
					}
				}

				INamespace INamespace.ParentNamespace
				{
					get
					{
						return this.parentNamespace;
					}
				}

				IEnumerable<IAssembly> INamespace.ContributingAssemblies
				{
					get
					{
						return new DefaultUnresolvedAssembly.DefaultResolvedAssembly[]
						{
							this.assembly
						};
					}
				}

				IEnumerable<INamespace> INamespace.ChildNamespaces
				{
					get
					{
						return this.childNamespaces;
					}
				}

				INamespace INamespace.GetChildNamespace(string name)
				{
					StringComparer nameComparer = this.assembly.compilation.NameComparer;
					for (int i = 0; i < this.childNamespaces.Count; i++)
					{
						if (nameComparer.Equals(name, this.ns.Children[i].Name))
						{
							return this.childNamespaces[i];
						}
					}
					return null;
				}

				ICompilation ICompilationProvider.Compilation
				{
					get
					{
						return this.assembly.compilation;
					}
				}

				IEnumerable<ITypeDefinition> INamespace.Types
				{
					get
					{
						IEnumerable<ITypeDefinition> enumerable = LazyInit.VolatileRead<IEnumerable<ITypeDefinition>>(ref this.types);
						if (enumerable != null)
						{
							return enumerable;
						}
						HashSet<ITypeDefinition> hashSet = new HashSet<ITypeDefinition>();
						foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.assembly.UnresolvedAssembly.TopLevelTypeDefinitions)
						{
							if (unresolvedTypeDefinition.Namespace == this.ns.FullName)
							{
								hashSet.Add(this.assembly.GetTypeDefinition(unresolvedTypeDefinition));
							}
						}
						return LazyInit.GetOrSet<IEnumerable<ITypeDefinition>>(ref this.types, hashSet.ToArray<ITypeDefinition>());
					}
				}

				ITypeDefinition INamespace.GetTypeDefinition(string name, int typeParameterCount)
				{
					TopLevelTypeName key = new TopLevelTypeName(this.ns.FullName, name, typeParameterCount);
					IUnresolvedTypeDefinition unresolved;
					if (this.assembly.unresolvedTypeDict.TryGetValue(key, out unresolved))
					{
						return this.assembly.GetTypeDefinition(unresolved);
					}
					return null;
				}

				public ISymbolReference ToReference()
				{
					return new NamespaceReference(new DefaultAssemblyReference(this.assembly.AssemblyName), this.ns.FullName);
				}

				private readonly DefaultUnresolvedAssembly.DefaultResolvedAssembly assembly;

				private readonly DefaultUnresolvedAssembly.UnresolvedNamespace ns;

				private readonly INamespace parentNamespace;

				private readonly IList<DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS> childNamespaces;

				private IEnumerable<ITypeDefinition> types;
			}
		}
	}
}
