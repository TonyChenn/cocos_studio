using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// References a member that is an explicit interface implementation.
	/// </summary>
	/// <remarks>
	/// Resolving an ExplicitInterfaceImplementationMemberReference requires a context
	/// that provides enough information for resolving the declaring type reference
	/// and the interface member reference.
	/// Note that the interface member reference is resolved in '<c>context.WithCurrentTypeDefinition(declaringType.GetDefinition())</c>'
	/// - this is done to ensure that open generics in the interface member reference resolve to the type parameters of the
	/// declaring type.
	/// </remarks>
	// Token: 0x020000D0 RID: 208
	[Serializable]
	public sealed class ExplicitInterfaceImplementationMemberReference : IMemberReference, ISymbolReference
	{
		// Token: 0x060007AF RID: 1967 RVA: 0x00013573 File Offset: 0x00012573
		public ExplicitInterfaceImplementationMemberReference(ITypeReference typeReference, IMemberReference interfaceMemberReference)
		{
			if (typeReference == null)
			{
				throw new ArgumentNullException("typeReference");
			}
			if (interfaceMemberReference == null)
			{
				throw new ArgumentNullException("interfaceMemberReference");
			}
			this.typeReference = typeReference;
			this.interfaceMemberReference = interfaceMemberReference;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000135A5 File Offset: 0x000125A5
		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00013604 File Offset: 0x00012604
		public IMember Resolve(ITypeResolveContext context)
		{
			IType type = this.typeReference.Resolve(context);
			IMember interfaceMember = this.interfaceMemberReference.Resolve(context.WithCurrentTypeDefinition(type.GetDefinition()));
			if (interfaceMember == null)
			{
				return null;
			}
			IEnumerable<IMember> source;
			if (interfaceMember.SymbolKind == SymbolKind.Accessor)
			{
				source = type.GetAccessors((IUnresolvedMethod m) => m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
			}
			else
			{
				source = type.GetMembers((IUnresolvedMember m) => m.SymbolKind == interfaceMember.SymbolKind && m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
			}
			return source.FirstOrDefault((IMember m) => m.ImplementedInterfaceMembers.Count == 1 && interfaceMember.Equals(m.ImplementedInterfaceMembers[0]));
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000136AE File Offset: 0x000126AE
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		// Token: 0x04000236 RID: 566
		private ITypeReference typeReference;

		// Token: 0x04000237 RID: 567
		private IMemberReference interfaceMemberReference;
	}
}
