using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	[Serializable]
	internal class IdStringMemberReference : IMemberReference, ISymbolReference
	{
		public IdStringMemberReference(ITypeReference declaringTypeReference, char memberType, string memberIdString)
		{
			this.declaringTypeReference = declaringTypeReference;
			this.memberType = memberType;
			this.memberIdString = memberIdString;
		}

		private bool CanMatch(IUnresolvedMember member)
		{
			switch (member.SymbolKind)
			{
			case SymbolKind.Field:
				return this.memberType == 'F';
			case SymbolKind.Property:
			case SymbolKind.Indexer:
				return this.memberType == 'P';
			case SymbolKind.Event:
				return this.memberType == 'E';
			case SymbolKind.Method:
			case SymbolKind.Operator:
			case SymbolKind.Constructor:
			case SymbolKind.Destructor:
				return this.memberType == 'M';
			default:
				throw new NotSupportedException(member.SymbolKind.ToString());
			}
		}

		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.declaringTypeReference;
			}
		}

		public IMember Resolve(ITypeResolveContext context)
		{
			IType type = this.declaringTypeReference.Resolve(context);
			foreach (IMember member in type.GetMembers(new Predicate<IUnresolvedMember>(this.CanMatch), GetMemberOptions.IgnoreInheritedMembers))
			{
				if (member.GetIdString() == this.memberIdString)
				{
					return member;
				}
			}
			return null;
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		private readonly ITypeReference declaringTypeReference;

		private readonly char memberType;

		private readonly string memberIdString;
	}
}
