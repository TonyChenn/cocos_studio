using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents the intersection of several types.
	/// </summary>
	// Token: 0x020000E9 RID: 233
	public class IntersectionType : AbstractType
	{
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0001720C File Offset: 0x0001620C
		public ReadOnlyCollection<IType> Types
		{
			get
			{
				return this.types;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00017214 File Offset: 0x00016214
		private IntersectionType(IType[] types)
		{
			this.types = Array.AsReadOnly<IType>(types);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00017228 File Offset: 0x00016228
		public static IType Create(IEnumerable<IType> types)
		{
			IType[] array = types.Distinct<IType>().ToArray<IType>();
			IType[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i] == null)
				{
					throw new ArgumentNullException();
				}
			}
			if (array.Length == 0)
			{
				return SpecialType.UnknownType;
			}
			if (array.Length == 1)
			{
				return array[0];
			}
			return new IntersectionType(array);
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00017279 File Offset: 0x00016279
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Intersection;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00017280 File Offset: 0x00016280
		public override string Name
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IType type in this.types)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" & ");
					}
					stringBuilder.Append(type.Name);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000172F4 File Offset: 0x000162F4
		public override string ReflectionName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IType type in this.types)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" & ");
					}
					stringBuilder.Append(type.ReflectionName);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00017368 File Offset: 0x00016368
		public override bool? IsReferenceType
		{
			get
			{
				foreach (IType type in this.types)
				{
					bool? isReferenceType = type.IsReferenceType;
					if (isReferenceType != null)
					{
						return new bool?(isReferenceType.Value);
					}
				}
				return null;
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000173DC File Offset: 0x000163DC
		public override int GetHashCode()
		{
			int num = 0;
			foreach (IType type in this.types)
			{
				num *= 7137517;
				num += type.GetHashCode();
			}
			return num;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00017438 File Offset: 0x00016438
		public override bool Equals(IType other)
		{
			IntersectionType intersectionType = other as IntersectionType;
			if (intersectionType != null && this.types.Count == intersectionType.types.Count)
			{
				for (int i = 0; i < this.types.Count; i++)
				{
					if (!this.types[i].Equals(intersectionType.types[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000174A1 File Offset: 0x000164A1
		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				return this.types;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000174A9 File Offset: 0x000164A9
		public override ITypeReference ToTypeReference()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000174B0 File Offset: 0x000164B0
		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMethods(this, IntersectionType.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000174BF File Offset: 0x000164BF
		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000174CA File Offset: 0x000164CA
		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetProperties(this, IntersectionType.FilterNonStatic<IUnresolvedProperty>(filter), options);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000174D9 File Offset: 0x000164D9
		public override IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetFields(this, IntersectionType.FilterNonStatic<IUnresolvedField>(filter), options);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000174E8 File Offset: 0x000164E8
		public override IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetEvents(this, IntersectionType.FilterNonStatic<IUnresolvedEvent>(filter), options);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000174F7 File Offset: 0x000164F7
		public override IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMembers(this, IntersectionType.FilterNonStatic<IUnresolvedMember>(filter), options);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00017506 File Offset: 0x00016506
		public override IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetAccessors(this, IntersectionType.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00017550 File Offset: 0x00016550
		private static Predicate<T> FilterNonStatic<T>(Predicate<T> filter) where T : class, IUnresolvedMember
		{
			if (filter == null)
			{
				return (T member) => !member.IsStatic;
			}
			return (T member) => !member.IsStatic && filter(member);
		}

		// Token: 0x04000278 RID: 632
		private readonly ReadOnlyCollection<IType> types;
	}
}
