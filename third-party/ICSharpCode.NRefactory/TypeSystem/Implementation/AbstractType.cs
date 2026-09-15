using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation for IType interface.
	/// </summary>
	[Serializable]
	public abstract class AbstractType : IType, INamedElement, IEquatable<IType>
	{
		public virtual string FullName
		{
			get
			{
				string @namespace = this.Namespace;
				string name = this.Name;
				if (string.IsNullOrEmpty(@namespace))
				{
					return name;
				}
				return @namespace + "." + name;
			}
		}

		public abstract string Name { get; }

		public virtual string Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual string ReflectionName
		{
			get
			{
				return this.FullName;
			}
		}

		public abstract bool? IsReferenceType { get; }

		public abstract TypeKind Kind { get; }

		public virtual int TypeParameterCount
		{
			get
			{
				return 0;
			}
		}

		public virtual IList<IType> TypeArguments
		{
			get
			{
				return AbstractType.emptyTypeArguments;
			}
		}

		public virtual IType DeclaringType
		{
			get
			{
				return null;
			}
		}

		public virtual bool IsParameterized
		{
			get
			{
				return false;
			}
		}

		public virtual ITypeDefinition GetDefinition()
		{
			return null;
		}

		public virtual IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				return EmptyList<IType>.Instance;
			}
		}

		public abstract ITypeReference ToTypeReference();

		public virtual IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IType>.Instance;
		}

		public virtual IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IType>.Instance;
		}

		public virtual IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		public virtual IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		public virtual IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			return EmptyList<IMethod>.Instance;
		}

		public virtual IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IProperty>.Instance;
		}

		public virtual IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IField>.Instance;
		}

		public virtual IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IEvent>.Instance;
		}

		public virtual IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			IEnumerable<IMember> methods = this.GetMethods(filter, options);
			return methods.Concat(this.GetProperties(filter, options)).Concat(this.GetFields(filter, options)).Concat(this.GetEvents(filter, options));
		}

		public virtual IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as IType);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public virtual bool Equals(IType other)
		{
			return this == other;
		}

		public override string ToString()
		{
			return this.ReflectionName;
		}

		public virtual IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitOtherType(this);
		}

		public virtual IType VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		private static readonly IList<IType> emptyTypeArguments = new IType[0];
	}
}
