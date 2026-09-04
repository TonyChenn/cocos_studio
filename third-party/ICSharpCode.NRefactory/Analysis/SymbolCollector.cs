using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// The symbol collector collects related symbols that form a group of symbols that should be renamed
	/// when a name of one symbol changes. For example if a type definition name should be changed
	/// the constructors and destructor names should change as well.
	/// </summary>
	// Token: 0x0200013C RID: 316
	public class SymbolCollector
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ICSharpCode.NRefactory.Analysis.SymbolCollector" /> should include overloads.
		/// </summary>
		/// <value><c>true</c> if overloads should be included; otherwise, <c>false</c>.</value>
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x000207EF File Offset: 0x0001F7EF
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x000207F7 File Offset: 0x0001F7F7
		public bool IncludeOverloads { get; set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00020800 File Offset: 0x0001F800
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00020808 File Offset: 0x0001F808
		public bool GroupForRenaming { get; set; }

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00020A10 File Offset: 0x0001FA10
		private static IEnumerable<ISymbol> CollectTypeRelatedMembers(ITypeDefinition type)
		{
			yield return type;
			foreach (IMember c in type.GetDefinition().GetMembers((IUnresolvedMember m) => !m.IsSynthetic && (m.SymbolKind == SymbolKind.Constructor || m.SymbolKind == SymbolKind.Destructor), GetMemberOptions.IgnoreInheritedMembers))
			{
				yield return c;
			}
			yield break;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00020A5C File Offset: 0x0001FA5C
		private static IEnumerable<ISymbol> CollectOverloads(IMethod method)
		{
			return from m in method.DeclaringType.GetMethods((IUnresolvedMethod m) => m.Name == method.Name, GetMemberOptions.None)
			where m != method
			select m;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00020AA4 File Offset: 0x0001FAA4
		private static IMember SearchMember(ITypeDefinition derivedType, IMember method)
		{
			foreach (IMember member in derivedType.Members)
			{
				if (member.ImplementedInterfaceMembers.Contains(method))
				{
					return member;
				}
			}
			return null;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00020CC0 File Offset: 0x0001FCC0
		private static IEnumerable<ISymbol> MakeUnique(List<ISymbol> symbols)
		{
			HashSet<ISymbol> taken = new HashSet<ISymbol>();
			foreach (ISymbol sym in symbols)
			{
				if (!taken.Contains(sym))
				{
					taken.Add(sym);
					yield return sym;
				}
			}
			yield break;
		}

		/// <summary>
		/// Gets the related symbols.
		/// </summary>
		/// <returns>The related symbols.</returns>
		/// <param name="g">The type graph.</param>
		/// <param name="m">The symbol to search</param>
		// Token: 0x06000AEA RID: 2794 RVA: 0x00020CE8 File Offset: 0x0001FCE8
		public IEnumerable<ISymbol> GetRelatedSymbols(Lazy<TypeGraph> g, ISymbol m)
		{
			switch (m.SymbolKind)
			{
			case SymbolKind.TypeDefinition:
				return SymbolCollector.CollectTypeRelatedMembers((ITypeDefinition)m);
			case SymbolKind.Field:
			case SymbolKind.Operator:
			case SymbolKind.Variable:
			case SymbolKind.Parameter:
			case SymbolKind.TypeParameter:
				return new ISymbol[]
				{
					m
				};
			case SymbolKind.Property:
			case SymbolKind.Indexer:
			case SymbolKind.Event:
			case SymbolKind.Method:
			{
				IMember member = (IMember)m;
				List<ISymbol> list = new List<ISymbol>();
				if (!member.IsExplicitInterfaceImplementation)
				{
					list.Add(member);
				}
				if (this.GroupForRenaming)
				{
					using (IEnumerator<IMember> enumerator = member.ImplementedInterfaceMembers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IMember m2 = enumerator.Current;
							list.AddRange(this.GetRelatedSymbols(g, m2));
						}
						goto IL_168;
					}
				}
				list.AddRange(member.ImplementedInterfaceMembers);
				IL_168:
				if (member.DeclaringType.Kind == TypeKind.Interface)
				{
					TypeGraphNode node = g.Value.GetNode(member.DeclaringTypeDefinition);
					if (node != null)
					{
						foreach (TypeGraphNode typeGraphNode in node.DerivedTypes)
						{
							IMember member2 = SymbolCollector.SearchMember(typeGraphNode.TypeDefinition, member);
							if (member2 != null)
							{
								list.Add(member2);
							}
						}
					}
				}
				if (this.IncludeOverloads)
				{
					this.IncludeOverloads = false;
					if (member is IMethod)
					{
						using (IEnumerator<ISymbol> enumerator3 = SymbolCollector.CollectOverloads((IMethod)member).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								ISymbol m3 = enumerator3.Current;
								list.AddRange(this.GetRelatedSymbols(g, m3));
							}
							goto IL_26D;
						}
					}
					if (member.SymbolKind == SymbolKind.Indexer)
					{
						list.AddRange(member.DeclaringTypeDefinition.GetProperties((IUnresolvedProperty p) => p.IsIndexer, GetMemberOptions.None));
					}
				}
				IL_26D:
				return SymbolCollector.MakeUnique(list);
			}
			case SymbolKind.Constructor:
			{
				if (this.GroupForRenaming)
				{
					return this.GetRelatedSymbols(g, ((IMethod)m).DeclaringTypeDefinition);
				}
				List<ISymbol> list2 = new List<ISymbol>();
				if (this.IncludeOverloads)
				{
					foreach (ISymbol item in SymbolCollector.CollectOverloads((IMethod)m))
					{
						list2.Add(item);
					}
				}
				return list2;
			}
			case SymbolKind.Destructor:
				if (this.GroupForRenaming)
				{
					return this.GetRelatedSymbols(g, ((IMethod)m).DeclaringTypeDefinition);
				}
				return new ISymbol[]
				{
					m
				};
			case SymbolKind.Namespace:
				return new ISymbol[]
				{
					m
				};
			}
			throw new ArgumentOutOfRangeException("symbol:" + m.SymbolKind);
		}
	}
}
