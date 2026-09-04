using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Provides helper methods for implementing GetMembers() on IType-implementations.
	/// Note: GetMembersHelper will recursively call back into IType.GetMembers(), but only with
	/// both GetMemberOptions.IgnoreInheritedMembers and GetMemberOptions.ReturnMemberDefinitions set,
	/// and only the 'simple' overloads (not taking type arguments).
	///
	/// Ensure that your IType implementation does not use the GetMembersHelper if both flags are set,
	/// otherwise you'll get a StackOverflowException!
	/// </summary>
	// Token: 0x020000D3 RID: 211
	internal static class GetMembersHelper
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00013940 File Offset: 0x00012940
		public static IEnumerable<IType> GetNestedTypes(IType type, Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetNestedTypes(type, null, filter, options);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00013970 File Offset: 0x00012970
		public static IEnumerable<IType> GetNestedTypes(IType type, IList<IType> nestedTypeArguments, Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetNestedTypesImpl(type, nestedTypeArguments, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetNestedTypesImpl(t, nestedTypeArguments, filter, options));
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00013D1C File Offset: 0x00012D1C
		private static IEnumerable<IType> GetNestedTypesImpl(IType outerType, IList<IType> nestedTypeArguments, Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			ITypeDefinition outerTypeDef = outerType.GetDefinition();
			if (outerTypeDef != null)
			{
				int outerTypeParameterCount = outerTypeDef.TypeParameterCount;
				ParameterizedType pt = outerType as ParameterizedType;
				foreach (ITypeDefinition nestedType in outerTypeDef.NestedTypes)
				{
					int totalTypeParameterCount = nestedType.TypeParameterCount;
					if ((nestedTypeArguments == null || totalTypeParameterCount - outerTypeParameterCount == nestedTypeArguments.Count) && (filter == null || filter(nestedType)))
					{
						if (totalTypeParameterCount == 0 || (options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
						{
							yield return nestedType;
						}
						else
						{
							IType[] newTypeArguments = new IType[totalTypeParameterCount];
							for (int i = 0; i < outerTypeParameterCount; i++)
							{
								newTypeArguments[i] = ((pt != null) ? pt.GetTypeArgument(i) : outerTypeDef.TypeParameters[i]);
							}
							for (int j = outerTypeParameterCount; j < totalTypeParameterCount; j++)
							{
								if (nestedTypeArguments != null)
								{
									newTypeArguments[j] = nestedTypeArguments[j - outerTypeParameterCount];
								}
								else
								{
									newTypeArguments[j] = SpecialType.UnboundTypeArgument;
								}
							}
							yield return new ParameterizedType(nestedType, newTypeArguments);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00013D4E File Offset: 0x00012D4E
		public static IEnumerable<IMethod> GetMethods(IType type, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMethods(type, null, filter, options);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00013D7C File Offset: 0x00012D7C
		public static IEnumerable<IMethod> GetMethods(IType type, IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			if (typeArguments != null && typeArguments.Count > 0)
			{
				filter = GetMembersHelper.FilterTypeParameterCount(typeArguments.Count).And(filter);
			}
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetMethodsImpl(type, typeArguments, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetMethodsImpl(t, typeArguments, filter, options));
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00013E3C File Offset: 0x00012E3C
		private static Predicate<IUnresolvedMethod> FilterTypeParameterCount(int expectedTypeParameterCount)
		{
			return (IUnresolvedMethod m) => m.TypeParameters.Count == expectedTypeParameterCount;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000141BC File Offset: 0x000131BC
		private static IEnumerable<IMethod> GetMethodsImpl(IType baseType, IList<IType> methodTypeArguments, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			IEnumerable<IMethod> declaredMethods = baseType.GetMethods(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers));
			ParameterizedType pt = baseType as ParameterizedType;
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.None && (pt != null || (methodTypeArguments != null && methodTypeArguments.Count > 0)))
			{
				TypeParameterSubstitution substitution = null;
				foreach (IMethod i in declaredMethods)
				{
					if (methodTypeArguments == null || methodTypeArguments.Count <= 0 || i.TypeParameters.Count == methodTypeArguments.Count)
					{
						if (substitution == null)
						{
							if (pt != null)
							{
								substitution = pt.GetSubstitution(methodTypeArguments);
							}
							else
							{
								substitution = new TypeParameterSubstitution(null, methodTypeArguments);
							}
						}
						yield return new SpecializedMethod(i, substitution);
					}
				}
			}
			else
			{
				foreach (IMethod j in declaredMethods)
				{
					yield return j;
				}
			}
			yield break;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001420C File Offset: 0x0001320C
		public static IEnumerable<IMethod> GetAccessors(IType type, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetAccessorsImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetAccessorsImpl(t, filter, options));
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00014269 File Offset: 0x00013269
		private static IEnumerable<IMethod> GetAccessorsImpl(IType baseType, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetConstructorsOrAccessorsImpl(baseType, baseType.GetAccessors(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers)), filter, options);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001429C File Offset: 0x0001329C
		public static IEnumerable<IMethod> GetConstructors(IType type, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetConstructorsImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetConstructorsImpl(t, filter, options));
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000142F9 File Offset: 0x000132F9
		private static IEnumerable<IMethod> GetConstructorsImpl(IType baseType, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetConstructorsOrAccessorsImpl(baseType, baseType.GetConstructors(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers)), filter, options);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001434C File Offset: 0x0001334C
		private static IEnumerable<IMethod> GetConstructorsOrAccessorsImpl(IType baseType, IEnumerable<IMethod> declaredMembers, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return declaredMembers;
			}
			ParameterizedType pt = baseType as ParameterizedType;
			if (pt != null)
			{
				TypeParameterSubstitution substitution = pt.GetSubstitution();
				return from m in declaredMembers
				select new SpecializedMethod(m, substitution)
				{
					DeclaringType = pt
				};
			}
			return declaredMembers;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000143CC File Offset: 0x000133CC
		public static IEnumerable<IProperty> GetProperties(IType type, Predicate<IUnresolvedProperty> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetPropertiesImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetPropertiesImpl(t, filter, options));
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00014468 File Offset: 0x00013468
		private static IEnumerable<IProperty> GetPropertiesImpl(IType baseType, Predicate<IUnresolvedProperty> filter, GetMemberOptions options)
		{
			IEnumerable<IProperty> properties = baseType.GetProperties(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers));
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return properties;
			}
			ParameterizedType pt = baseType as ParameterizedType;
			if (pt != null)
			{
				TypeParameterSubstitution substitution = pt.GetSubstitution();
				return from m in properties
				select new SpecializedProperty(m, substitution)
				{
					DeclaringType = pt
				};
			}
			return properties;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000144F0 File Offset: 0x000134F0
		public static IEnumerable<IField> GetFields(IType type, Predicate<IUnresolvedField> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetFieldsImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetFieldsImpl(t, filter, options));
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001458C File Offset: 0x0001358C
		private static IEnumerable<IField> GetFieldsImpl(IType baseType, Predicate<IUnresolvedField> filter, GetMemberOptions options)
		{
			IEnumerable<IField> fields = baseType.GetFields(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers));
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return fields;
			}
			ParameterizedType pt = baseType as ParameterizedType;
			if (pt != null)
			{
				TypeParameterSubstitution substitution = pt.GetSubstitution();
				return from m in fields
				select new SpecializedField(m, substitution)
				{
					DeclaringType = pt
				};
			}
			return fields;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00014614 File Offset: 0x00013614
		public static IEnumerable<IEvent> GetEvents(IType type, Predicate<IUnresolvedEvent> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetEventsImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetEventsImpl(t, filter, options));
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000146B0 File Offset: 0x000136B0
		private static IEnumerable<IEvent> GetEventsImpl(IType baseType, Predicate<IUnresolvedEvent> filter, GetMemberOptions options)
		{
			IEnumerable<IEvent> events = baseType.GetEvents(filter, options | (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers));
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return events;
			}
			ParameterizedType pt = baseType as ParameterizedType;
			if (pt != null)
			{
				TypeParameterSubstitution substitution = pt.GetSubstitution();
				return from m in events
				select new SpecializedEvent(m, substitution)
				{
					DeclaringType = pt
				};
			}
			return events;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00014738 File Offset: 0x00013738
		public static IEnumerable<IMember> GetMembers(IType type, Predicate<IUnresolvedMember> filter, GetMemberOptions options)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetMembersImpl(type, filter, options);
			}
			return type.GetNonInterfaceBaseTypes().SelectMany((IType t) => GetMembersHelper.GetMembersImpl(t, filter, options));
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00014BA0 File Offset: 0x00013BA0
		private static IEnumerable<IMember> GetMembersImpl(IType baseType, Predicate<IUnresolvedMember> filter, GetMemberOptions options)
		{
			foreach (IMethod i in GetMembersHelper.GetMethodsImpl(baseType, null, filter, options))
			{
				yield return i;
			}
			foreach (IProperty j in GetMembersHelper.GetPropertiesImpl(baseType, filter, options))
			{
				yield return j;
			}
			foreach (IField k in GetMembersHelper.GetFieldsImpl(baseType, filter, options))
			{
				yield return k;
			}
			foreach (IEvent l in GetMembersHelper.GetEventsImpl(baseType, filter, options))
			{
				yield return l;
			}
			yield break;
		}

		// Token: 0x0400023B RID: 571
		private const GetMemberOptions declaredMembers = GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers;
	}
}
