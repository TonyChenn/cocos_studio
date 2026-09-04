using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	// Token: 0x02000130 RID: 304
	[Serializable]
	internal class IdStringMemberReference : IMemberReference, ISymbolReference
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x0001FA7B File Offset: 0x0001EA7B
		public IdStringMemberReference(ITypeReference declaringTypeReference, char memberType, string memberIdString)
		{
			this.declaringTypeReference = declaringTypeReference;
			this.memberType = memberType;
			this.memberIdString = memberIdString;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001FA98 File Offset: 0x0001EA98
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

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0001FB17 File Offset: 0x0001EB17
		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.declaringTypeReference;
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001FB20 File Offset: 0x0001EB20
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

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001FB9C File Offset: 0x0001EB9C
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		// Token: 0x04000394 RID: 916
		private readonly ITypeReference declaringTypeReference;

		// Token: 0x04000395 RID: 917
		private readonly char memberType;

		// Token: 0x04000396 RID: 918
		private readonly string memberIdString;
	}
}
