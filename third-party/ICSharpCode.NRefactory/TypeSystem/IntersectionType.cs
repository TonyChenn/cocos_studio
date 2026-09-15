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
	public class IntersectionType : AbstractType
	{
		public ReadOnlyCollection<IType> Types
		{
			get
			{
				return this.types;
			}
		}

		private IntersectionType(IType[] types)
		{
			this.types = Array.AsReadOnly<IType>(types);
		}

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

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Intersection;
			}
		}

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

		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				return this.types;
			}
		}

		public override ITypeReference ToTypeReference()
		{
			throw new NotSupportedException();
		}

		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMethods(this, IntersectionType.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetProperties(this, IntersectionType.FilterNonStatic<IUnresolvedProperty>(filter), options);
		}

		public override IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetFields(this, IntersectionType.FilterNonStatic<IUnresolvedField>(filter), options);
		}

		public override IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetEvents(this, IntersectionType.FilterNonStatic<IUnresolvedEvent>(filter), options);
		}

		public override IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetMembers(this, IntersectionType.FilterNonStatic<IUnresolvedMember>(filter), options);
		}

		public override IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return GetMembersHelper.GetAccessors(this, IntersectionType.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		private static Predicate<T> FilterNonStatic<T>(Predicate<T> filter) where T : class, IUnresolvedMember
		{
			if (filter == null)
			{
				return (T member) => !member.IsStatic;
			}
			return (T member) => !member.IsStatic && filter(member);
		}

		private readonly ReadOnlyCollection<IType> types;
	}
}
