using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// References an entity by its type and name.
	/// This class can be used to refer to all members except for constructors and explicit interface implementations.
	/// </summary>
	/// <remarks>
	/// Resolving a DefaultMemberReference requires a context that provides enough information for resolving the declaring type reference
	/// and the parameter types references.
	/// </remarks>
	[Serializable]
	public sealed class DefaultMemberReference : IMemberReference, ISymbolReference, ISupportsInterning
	{
		public DefaultMemberReference(SymbolKind symbolKind, ITypeReference typeReference, string name, int typeParameterCount = 0, IList<ITypeReference> parameterTypes = null)
		{
			if (typeReference == null)
			{
				throw new ArgumentNullException("typeReference");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (typeParameterCount != 0 && symbolKind != SymbolKind.Method)
			{
				throw new ArgumentException("Type parameter count > 0 is only supported for methods.");
			}
			this.symbolKind = symbolKind;
			this.typeReference = typeReference;
			this.name = name;
			this.typeParameterCount = typeParameterCount;
			this.parameterTypes = (parameterTypes ?? EmptyList<ITypeReference>.Instance);
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
			IEnumerable<IMember> enumerable;
			if (this.symbolKind == SymbolKind.Accessor)
			{
				enumerable = type.GetAccessors((IUnresolvedMethod m) => m.Name == this.name && !m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
			}
			else if (this.symbolKind == SymbolKind.Method)
			{
				enumerable = type.GetMethods((IUnresolvedMethod m) => m.Name == this.name && m.SymbolKind == SymbolKind.Method && m.TypeParameters.Count == this.typeParameterCount && !m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
			}
			else
			{
				enumerable = type.GetMembers((IUnresolvedMember m) => m.Name == this.name && m.SymbolKind == this.symbolKind && !m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
			}
			IList<IType> list = this.parameterTypes.Resolve(context);
			foreach (IMember member in enumerable)
			{
				IParameterizedMember parameterizedMember = member as IParameterizedMember;
				if (parameterizedMember == null)
				{
					if (this.parameterTypes.Count == 0)
					{
						return member;
					}
				}
				else if (this.parameterTypes.Count == parameterizedMember.Parameters.Count)
				{
					bool flag = true;
					for (int i = 0; i < this.parameterTypes.Count; i++)
					{
						IType type2 = DummyTypeParameter.NormalizeAllTypeParameters(list[i]);
						IType other = DummyTypeParameter.NormalizeAllTypeParameters(parameterizedMember.Parameters[i].Type);
						if (!type2.Equals(other))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return member;
					}
				}
			}
			return null;
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return ((IMemberReference)this).Resolve(context);
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return (int)this.symbolKind ^ this.typeReference.GetHashCode() ^ this.name.GetHashCode() ^ this.parameterTypes.GetHashCode();
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultMemberReference defaultMemberReference = other as DefaultMemberReference;
			return defaultMemberReference != null && this.symbolKind == defaultMemberReference.symbolKind && this.typeReference == defaultMemberReference.typeReference && this.name == defaultMemberReference.name && this.parameterTypes == defaultMemberReference.parameterTypes;
		}

		private readonly SymbolKind symbolKind;

		private readonly ITypeReference typeReference;

		private readonly string name;

		private readonly int typeParameterCount;

		private readonly IList<ITypeReference> parameterTypes;
	}
}
