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
	[Serializable]
	public sealed class ExplicitInterfaceImplementationMemberReference : IMemberReference, ISymbolReference
	{
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

		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

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

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		private ITypeReference typeReference;

		private IMemberReference interfaceMemberReference;
	}
}
