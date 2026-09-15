using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Provides helper methods for inheritance.
	/// </summary>
	public static class InheritanceHelper
	{
		/// <summary>
		/// Gets the base member that has the same signature.
		/// </summary>
		public static IMember GetBaseMember(IMember member)
		{
			return InheritanceHelper.GetBaseMembers(member, false).FirstOrDefault<IMember>();
		}

		/// <summary>
		/// Gets all base members that have the same signature.
		/// </summary>
		/// <returns>
		/// List of base members with the same signature. The member from the derived-most base class is returned first.
		/// </returns>
		public static IEnumerable<IMember> GetBaseMembers(IMember member, bool includeImplementedInterfaces)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (member.IsExplicitInterfaceImplementation && member.ImplementedInterfaceMembers.Count == 1)
			{
				member = member.ImplementedInterfaceMembers[0];
				yield return member;
			}
			TypeParameterSubstitution substitution = member.Substitution;
			member = member.MemberDefinition;
			if (member.DeclaringTypeDefinition != null)
			{
				IEnumerable<IType> allBaseTypes;
				if (includeImplementedInterfaces)
				{
					allBaseTypes = member.DeclaringTypeDefinition.GetAllBaseTypes();
				}
				else
				{
					allBaseTypes = member.DeclaringTypeDefinition.GetNonInterfaceBaseTypes();
				}
				foreach (IType baseType in allBaseTypes.Reverse<IType>())
				{
					if (baseType != member.DeclaringTypeDefinition)
					{
						IEnumerable<IMember> baseMembers;
						if (member.SymbolKind == SymbolKind.Accessor)
						{
							baseMembers = baseType.GetAccessors((IUnresolvedMethod m) => m.Name == member.Name && !m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
						}
						else
						{
							baseMembers = baseType.GetMembers((IUnresolvedMember m) => m.Name == member.Name && !m.IsExplicitInterfaceImplementation, GetMemberOptions.IgnoreInheritedMembers);
						}
						foreach (IMember baseMember in baseMembers)
						{
							if (SignatureComparer.Ordinal.Equals(member, baseMember))
							{
								yield return baseMember.Specialize(substitution);
							}
						}
					}
				}
			}
			yield break;
		}

		/// <summary>
		/// Finds the member declared in 'derivedType' that has the same signature (could override) 'baseMember'.
		/// </summary>
		public static IMember GetDerivedMember(IMember baseMember, ITypeDefinition derivedType)
		{
			if (baseMember == null)
			{
				throw new ArgumentNullException("baseMember");
			}
			if (derivedType == null)
			{
				throw new ArgumentNullException("derivedType");
			}
			if (baseMember.Compilation != derivedType.Compilation)
			{
				throw new ArgumentException("baseMember and derivedType must be from the same compilation");
			}
			baseMember = baseMember.MemberDefinition;
			bool includeImplementedInterfaces = baseMember.DeclaringTypeDefinition.Kind == TypeKind.Interface;
			IMethod method = baseMember as IMethod;
			if (method != null)
			{
				foreach (IMethod method2 in derivedType.Methods)
				{
					if (method2.Name == method.Name && method2.Parameters.Count == method.Parameters.Count && method2.TypeParameters.Count == method.TypeParameters.Count)
					{
						if (InheritanceHelper.GetBaseMembers(method2, includeImplementedInterfaces).Any((IMember m) => m.MemberDefinition == baseMember))
						{
							return method2;
						}
					}
				}
			}
			IProperty property = baseMember as IProperty;
			if (property != null)
			{
				foreach (IProperty property2 in derivedType.Properties)
				{
					if (property2.Name == property.Name && property2.Parameters.Count == property.Parameters.Count)
					{
						if (InheritanceHelper.GetBaseMembers(property2, includeImplementedInterfaces).Any((IMember m) => m.MemberDefinition == baseMember))
						{
							return property2;
						}
					}
				}
			}
			if (baseMember is IEvent)
			{
				foreach (IEvent @event in derivedType.Events)
				{
					if (@event.Name == baseMember.Name)
					{
						return @event;
					}
				}
			}
			if (baseMember is IField)
			{
				foreach (IField field in derivedType.Fields)
				{
					if (field.Name == baseMember.Name)
					{
						return field;
					}
				}
			}
			return null;
		}
	}
}
