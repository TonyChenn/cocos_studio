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
	public class SymbolCollector
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ICSharpCode.NRefactory.Analysis.SymbolCollector" /> should include overloads.
		/// </summary>
		/// <value><c>true</c> if overloads should be included; otherwise, <c>false</c>.</value>
		public bool IncludeOverloads { get; set; }

		public bool GroupForRenaming { get; set; }

		private static IEnumerable<ISymbol> CollectTypeRelatedMembers(ITypeDefinition type)
		{
			yield return type;
			foreach (IMember c in type.GetDefinition().GetMembers((IUnresolvedMember m) => !m.IsSynthetic && (m.SymbolKind == SymbolKind.Constructor || m.SymbolKind == SymbolKind.Destructor), GetMemberOptions.IgnoreInheritedMembers))
			{
				yield return c;
			}
			yield break;
		}

		private static IEnumerable<ISymbol> CollectOverloads(IMethod method)
		{
			return from m in method.DeclaringType.GetMethods((IUnresolvedMethod m) => m.Name == method.Name, GetMemberOptions.None)
			where m != method
			select m;
		}

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
