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
	// Token: 0x020000B7 RID: 183
	[Serializable]
	public class DefaultUnresolvedAssembly : AbstractFreezable, IUnresolvedAssembly, IAssemblyReference
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x00010894 File Offset: 0x0000F894
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
		// Token: 0x0600064F RID: 1615 RVA: 0x00010914 File Offset: 0x0000F914
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
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00010996 File Offset: 0x0000F996
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001099E File Offset: 0x0000F99E
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
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x000109BB File Offset: 0x0000F9BB
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x000109C3 File Offset: 0x0000F9C3
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

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000109E0 File Offset: 0x0000F9E0
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x000109E8 File Offset: 0x0000F9E8
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

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x000109F7 File Offset: 0x0000F9F7
		public IList<IUnresolvedAttribute> AssemblyAttributes
		{
			get
			{
				return this.assemblyAttributes;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x000109FF File Offset: 0x0000F9FF
		IEnumerable<IUnresolvedAttribute> IUnresolvedAssembly.AssemblyAttributes
		{
			get
			{
				return this.assemblyAttributes;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00010A07 File Offset: 0x0000FA07
		public IList<IUnresolvedAttribute> ModuleAttributes
		{
			get
			{
				return this.moduleAttributes;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00010A0F File Offset: 0x0000FA0F
		IEnumerable<IUnresolvedAttribute> IUnresolvedAssembly.ModuleAttributes
		{
			get
			{
				return this.moduleAttributes;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00010A17 File Offset: 0x0000FA17
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
		// Token: 0x0600065B RID: 1627 RVA: 0x00010A24 File Offset: 0x0000FA24
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
		// Token: 0x0600065C RID: 1628 RVA: 0x00010A84 File Offset: 0x0000FA84
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

		// Token: 0x0600065D RID: 1629 RVA: 0x00010AEC File Offset: 0x0000FAEC
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

		// Token: 0x0600065E RID: 1630 RVA: 0x00010B20 File Offset: 0x0000FB20
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

		// Token: 0x0600065F RID: 1631 RVA: 0x00010B7C File Offset: 0x0000FB7C
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

		// Token: 0x06000660 RID: 1632 RVA: 0x00010BC5 File Offset: 0x0000FBC5
		private Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> GetTypeDictionary(StringComparer nameComparer)
		{
			if (nameComparer == StringComparer.Ordinal)
			{
				return this.typeDefinitions;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00010BDC File Offset: 0x0000FBDC
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

		// Token: 0x06000662 RID: 1634 RVA: 0x00010D04 File Offset: 0x0000FD04
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

		// Token: 0x040001D0 RID: 464
		private string assemblyName;

		// Token: 0x040001D1 RID: 465
		private string fullAssemblyName;

		// Token: 0x040001D2 RID: 466
		private IList<IUnresolvedAttribute> assemblyAttributes;

		// Token: 0x040001D3 RID: 467
		private IList<IUnresolvedAttribute> moduleAttributes;

		// Token: 0x040001D4 RID: 468
		private Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> typeDefinitions = new Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition>(TopLevelTypeNameComparer.Ordinal);

		// Token: 0x040001D5 RID: 469
		private Dictionary<TopLevelTypeName, ITypeReference> typeForwarders = new Dictionary<TopLevelTypeName, ITypeReference>(TopLevelTypeNameComparer.Ordinal);

		// Token: 0x040001D6 RID: 470
		private string location;

		// Token: 0x040001D7 RID: 471
		private static readonly ITypeReference typeForwardedToAttributeTypeRef = typeof(TypeForwardedToAttribute).ToTypeReference();

		// Token: 0x040001D8 RID: 472
		[NonSerialized]
		private List<KeyValuePair<StringComparer, DefaultUnresolvedAssembly.UnresolvedNamespace>> unresolvedNamespacesPerNameComparer;

		// Token: 0x020000B8 RID: 184
		[Serializable]
		private sealed class TypeOfConstantValue : IConstantValue
		{
			// Token: 0x06000664 RID: 1636 RVA: 0x00010D87 File Offset: 0x0000FD87
			public TypeOfConstantValue(ITypeReference typeRef)
			{
				this.typeRef = typeRef;
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x00010D96 File Offset: 0x0000FD96
			public ResolveResult Resolve(ITypeResolveContext context)
			{
				return new TypeOfResolveResult(context.Compilation.FindType(KnownTypeCode.Type), this.typeRef.Resolve(context));
			}

			// Token: 0x040001D9 RID: 473
			private readonly ITypeReference typeRef;
		}

		// Token: 0x020000B9 RID: 185
		private sealed class UnresolvedNamespace
		{
			// Token: 0x06000666 RID: 1638 RVA: 0x00010DB6 File Offset: 0x0000FDB6
			public UnresolvedNamespace(string fullName, string name)
			{
				this.FullName = fullName;
				this.Name = name;
			}

			// Token: 0x040001DA RID: 474
			internal readonly string FullName;

			// Token: 0x040001DB RID: 475
			internal readonly string Name;

			// Token: 0x040001DC RID: 476
			internal readonly List<DefaultUnresolvedAssembly.UnresolvedNamespace> Children = new List<DefaultUnresolvedAssembly.UnresolvedNamespace>();
		}

		// Token: 0x020000BA RID: 186
		private sealed class DefaultResolvedAssembly : IAssembly, ICompilationProvider
		{
			// Token: 0x06000667 RID: 1639 RVA: 0x00010DD8 File Offset: 0x0000FDD8
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

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000668 RID: 1640 RVA: 0x00010E69 File Offset: 0x0000FE69
			public IUnresolvedAssembly UnresolvedAssembly
			{
				get
				{
					return this.unresolvedAssembly;
				}
			}

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000669 RID: 1641 RVA: 0x00010E71 File Offset: 0x0000FE71
			public bool IsMainAssembly
			{
				get
				{
					return this.Compilation.MainAssembly == this;
				}
			}

			// Token: 0x17000296 RID: 662
			// (get) Token: 0x0600066A RID: 1642 RVA: 0x00010E81 File Offset: 0x0000FE81
			public string AssemblyName
			{
				get
				{
					return this.unresolvedAssembly.AssemblyName;
				}
			}

			// Token: 0x17000297 RID: 663
			// (get) Token: 0x0600066B RID: 1643 RVA: 0x00010E8E File Offset: 0x0000FE8E
			public string FullAssemblyName
			{
				get
				{
					return this.unresolvedAssembly.FullAssemblyName;
				}
			}

			// Token: 0x17000298 RID: 664
			// (get) Token: 0x0600066C RID: 1644 RVA: 0x00010E9B File Offset: 0x0000FE9B
			// (set) Token: 0x0600066D RID: 1645 RVA: 0x00010EA3 File Offset: 0x0000FEA3
			public IList<IAttribute> AssemblyAttributes { get; private set; }

			// Token: 0x17000299 RID: 665
			// (get) Token: 0x0600066E RID: 1646 RVA: 0x00010EAC File Offset: 0x0000FEAC
			// (set) Token: 0x0600066F RID: 1647 RVA: 0x00010EB4 File Offset: 0x0000FEB4
			public IList<IAttribute> ModuleAttributes { get; private set; }

			// Token: 0x1700029A RID: 666
			// (get) Token: 0x06000670 RID: 1648 RVA: 0x00010EBD File Offset: 0x0000FEBD
			public INamespace RootNamespace
			{
				get
				{
					return this.rootNamespace;
				}
			}

			// Token: 0x1700029B RID: 667
			// (get) Token: 0x06000671 RID: 1649 RVA: 0x00010EC5 File Offset: 0x0000FEC5
			public ICompilation Compilation
			{
				get
				{
					return this.compilation;
				}
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x00010ED0 File Offset: 0x0000FED0
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

			// Token: 0x06000673 RID: 1651 RVA: 0x00010F70 File Offset: 0x0000FF70
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

			// Token: 0x06000674 RID: 1652 RVA: 0x00011028 File Offset: 0x00010028
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

			// Token: 0x06000675 RID: 1653 RVA: 0x00011054 File Offset: 0x00010054
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

			// Token: 0x06000676 RID: 1654 RVA: 0x000110E9 File Offset: 0x000100E9
			private ITypeDefinition GetTypeDefinition(IUnresolvedTypeDefinition unresolved)
			{
				return this.typeDict.GetOrAdd(unresolved, (IUnresolvedTypeDefinition t) => this.CreateTypeDefinition(t));
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x00011104 File Offset: 0x00010104
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

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000678 RID: 1656 RVA: 0x000111AD File Offset: 0x000101AD
			public IEnumerable<ITypeDefinition> TopLevelTypeDefinitions
			{
				get
				{
					return from t in this.unresolvedAssembly.TopLevelTypeDefinitions
					select this.GetTypeDefinition(t);
				}
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x000111CB File Offset: 0x000101CB
			public override string ToString()
			{
				return "[DefaultResolvedAssembly " + this.AssemblyName + "]";
			}

			// Token: 0x040001DD RID: 477
			private readonly DefaultUnresolvedAssembly unresolvedAssembly;

			// Token: 0x040001DE RID: 478
			private readonly ICompilation compilation;

			// Token: 0x040001DF RID: 479
			private readonly ITypeResolveContext context;

			// Token: 0x040001E0 RID: 480
			private readonly Dictionary<TopLevelTypeName, IUnresolvedTypeDefinition> unresolvedTypeDict;

			// Token: 0x040001E1 RID: 481
			private readonly ConcurrentDictionary<IUnresolvedTypeDefinition, ITypeDefinition> typeDict = new ConcurrentDictionary<IUnresolvedTypeDefinition, ITypeDefinition>();

			// Token: 0x040001E2 RID: 482
			private readonly INamespace rootNamespace;

			// Token: 0x040001E3 RID: 483
			private volatile string[] internalsVisibleTo;

			// Token: 0x020000BC RID: 188
			private sealed class NS : INamespace, ISymbol, ICompilationProvider
			{
				// Token: 0x06000687 RID: 1671 RVA: 0x000111F4 File Offset: 0x000101F4
				public NS(DefaultUnresolvedAssembly.DefaultResolvedAssembly assembly, DefaultUnresolvedAssembly.UnresolvedNamespace ns, INamespace parentNamespace)
				{
					this.assembly = assembly;
					this.ns = ns;
					this.parentNamespace = parentNamespace;
					this.childNamespaces = new ProjectedList<DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS, DefaultUnresolvedAssembly.UnresolvedNamespace, DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS>(this, ns.Children, (DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS self, DefaultUnresolvedAssembly.UnresolvedNamespace c) => new DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS(self.assembly, c, self));
				}

				// Token: 0x170002A4 RID: 676
				// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001124B File Offset: 0x0001024B
				string INamespace.ExternAlias
				{
					get
					{
						return null;
					}
				}

				// Token: 0x170002A5 RID: 677
				// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001124E File Offset: 0x0001024E
				string INamespace.FullName
				{
					get
					{
						return this.ns.FullName;
					}
				}

				// Token: 0x170002A6 RID: 678
				// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001125B File Offset: 0x0001025B
				SymbolKind ISymbol.SymbolKind
				{
					get
					{
						return SymbolKind.Namespace;
					}
				}

				// Token: 0x170002A7 RID: 679
				// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001125F File Offset: 0x0001025F
				public string Name
				{
					get
					{
						return this.ns.Name;
					}
				}

				// Token: 0x170002A8 RID: 680
				// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001126C File Offset: 0x0001026C
				INamespace INamespace.ParentNamespace
				{
					get
					{
						return this.parentNamespace;
					}
				}

				// Token: 0x170002A9 RID: 681
				// (get) Token: 0x0600068D RID: 1677 RVA: 0x00011274 File Offset: 0x00010274
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

				// Token: 0x170002AA RID: 682
				// (get) Token: 0x0600068E RID: 1678 RVA: 0x00011292 File Offset: 0x00010292
				IEnumerable<INamespace> INamespace.ChildNamespaces
				{
					get
					{
						return this.childNamespaces;
					}
				}

				// Token: 0x0600068F RID: 1679 RVA: 0x0001129C File Offset: 0x0001029C
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

				// Token: 0x170002AB RID: 683
				// (get) Token: 0x06000690 RID: 1680 RVA: 0x000112FD File Offset: 0x000102FD
				ICompilation ICompilationProvider.Compilation
				{
					get
					{
						return this.assembly.compilation;
					}
				}

				// Token: 0x170002AC RID: 684
				// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001130C File Offset: 0x0001030C
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

				// Token: 0x06000692 RID: 1682 RVA: 0x000113B0 File Offset: 0x000103B0
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

				// Token: 0x06000693 RID: 1683 RVA: 0x000113F4 File Offset: 0x000103F4
				public ISymbolReference ToReference()
				{
					return new NamespaceReference(new DefaultAssemblyReference(this.assembly.AssemblyName), this.ns.FullName);
				}

				// Token: 0x040001E8 RID: 488
				private readonly DefaultUnresolvedAssembly.DefaultResolvedAssembly assembly;

				// Token: 0x040001E9 RID: 489
				private readonly DefaultUnresolvedAssembly.UnresolvedNamespace ns;

				// Token: 0x040001EA RID: 490
				private readonly INamespace parentNamespace;

				// Token: 0x040001EB RID: 491
				private readonly IList<DefaultUnresolvedAssembly.DefaultResolvedAssembly.NS> childNamespaces;

				// Token: 0x040001EC RID: 492
				private IEnumerable<ITypeDefinition> types;
			}
		}
	}
}
