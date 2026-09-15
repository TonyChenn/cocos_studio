using System;
using System.Collections.Generic;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public abstract class AbstractTypeParameter : ITypeParameter, IType, INamedElement, IEquatable<IType>, ISymbol, ICompilationProvider
	{
		protected AbstractTypeParameter(IEntity owner, int index, string name, VarianceModifier variance, IList<IAttribute> attributes, DomRegion region)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
			this.compilation = owner.Compilation;
			this.ownerType = owner.SymbolKind;
			this.index = index;
			this.name = (name ?? (((this.OwnerType == SymbolKind.Method) ? "!!" : "!") + index.ToString(CultureInfo.InvariantCulture)));
			this.attributes = (attributes ?? EmptyList<IAttribute>.Instance);
			this.region = region;
			this.variance = variance;
		}

		protected AbstractTypeParameter(ICompilation compilation, SymbolKind ownerType, int index, string name, VarianceModifier variance, IList<IAttribute> attributes, DomRegion region)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			this.compilation = compilation;
			this.ownerType = ownerType;
			this.index = index;
			this.name = (name ?? (((this.OwnerType == SymbolKind.Method) ? "!!" : "!") + index.ToString(CultureInfo.InvariantCulture)));
			this.attributes = (attributes ?? EmptyList<IAttribute>.Instance);
			this.region = region;
			this.variance = variance;
		}

		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.TypeParameter;
			}
		}

		public SymbolKind OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		public IEntity Owner
		{
			get
			{
				return this.owner;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public IList<IAttribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		public VarianceModifier Variance
		{
			get
			{
				return this.variance;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		public IType EffectiveBaseClass
		{
			get
			{
				if (this.effectiveBaseClass == null)
				{
					using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
					{
						if (!busyLock.Success)
						{
							return SpecialType.UnknownType;
						}
						this.effectiveBaseClass = this.CalculateEffectiveBaseClass();
					}
				}
				return this.effectiveBaseClass;
			}
		}

		private IType CalculateEffectiveBaseClass()
		{
			if (this.HasValueTypeConstraint)
			{
				return this.Compilation.FindType(KnownTypeCode.ValueType);
			}
			List<IType> list = new List<IType>();
			foreach (IType type in this.DirectBaseTypes)
			{
				if (type.Kind == TypeKind.Class)
				{
					list.Add(type);
				}
				else if (type.Kind == TypeKind.TypeParameter)
				{
					IType type2 = ((ITypeParameter)type).EffectiveBaseClass;
					if (type2.Kind == TypeKind.Class)
					{
						list.Add(type2);
					}
				}
			}
			if (list.Count == 0)
			{
				return this.Compilation.FindType(KnownTypeCode.Object);
			}
			IType type3 = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].GetDefinition().IsDerivedFrom(type3.GetDefinition()))
				{
					type3 = list[i];
				}
			}
			return type3;
		}

		public ICollection<IType> EffectiveInterfaceSet
		{
			get
			{
				ICollection<IType> collection = LazyInit.VolatileRead<ICollection<IType>>(ref this.effectiveInterfaceSet);
				if (collection != null)
				{
					return collection;
				}
				ICollection<IType> result;
				using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
				{
					if (!busyLock.Success)
					{
						result = EmptyList<IType>.Instance;
					}
					else
					{
						result = LazyInit.GetOrSet<ICollection<IType>>(ref this.effectiveInterfaceSet, this.CalculateEffectiveInterfaceSet());
					}
				}
				return result;
			}
		}

		private ICollection<IType> CalculateEffectiveInterfaceSet()
		{
			HashSet<IType> hashSet = new HashSet<IType>();
			foreach (IType type in this.DirectBaseTypes)
			{
				if (type.Kind == TypeKind.Interface)
				{
					hashSet.Add(type);
				}
				else if (type.Kind == TypeKind.TypeParameter)
				{
					hashSet.UnionWith(((ITypeParameter)type).EffectiveInterfaceSet);
				}
			}
			return hashSet;
		}

		public abstract bool HasDefaultConstructorConstraint { get; }

		public abstract bool HasReferenceTypeConstraint { get; }

		public abstract bool HasValueTypeConstraint { get; }

		public TypeKind Kind
		{
			get
			{
				return TypeKind.TypeParameter;
			}
		}

		public bool? IsReferenceType
		{
			get
			{
				if (this.HasValueTypeConstraint)
				{
					return new bool?(false);
				}
				if (this.HasReferenceTypeConstraint)
				{
					return new bool?(true);
				}
				IType type = this.EffectiveBaseClass;
				if (type.Kind == TypeKind.Class || type.Kind == TypeKind.Delegate)
				{
					ITypeDefinition definition = type.GetDefinition();
					if (definition != null)
					{
						KnownTypeCode knownTypeCode = definition.KnownTypeCode;
						if (knownTypeCode != KnownTypeCode.Object)
						{
							switch (knownTypeCode)
							{
							case KnownTypeCode.ValueType:
							case KnownTypeCode.Enum:
								break;
							default:
								goto IL_69;
							}
						}
						return null;
					}
					IL_69:
					return new bool?(true);
				}
				if (type.Kind == TypeKind.Struct || type.Kind == TypeKind.Enum)
				{
					return new bool?(false);
				}
				return null;
			}
		}

		IType IType.DeclaringType
		{
			get
			{
				return null;
			}
		}

		int IType.TypeParameterCount
		{
			get
			{
				return 0;
			}
		}

		bool IType.IsParameterized
		{
			get
			{
				return false;
			}
		}

		IList<IType> IType.TypeArguments
		{
			get
			{
				return AbstractTypeParameter.emptyTypeArguments;
			}
		}

		public abstract IEnumerable<IType> DirectBaseTypes { get; }

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		string INamedElement.Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		string INamedElement.FullName
		{
			get
			{
				return this.name;
			}
		}

		public string ReflectionName
		{
			get
			{
				return ((this.OwnerType == SymbolKind.Method) ? "``" : "`") + this.index.ToString(CultureInfo.InvariantCulture);
			}
		}

		ITypeDefinition IType.GetDefinition()
		{
			return null;
		}

		public IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeParameter(this);
		}

		public IType VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		public ITypeReference ToTypeReference()
		{
			return TypeParameterReference.Create(this.OwnerType, this.Index);
		}

		IEnumerable<IType> IType.GetNestedTypes(Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			return EmptyList<IType>.Instance;
		}

		IEnumerable<IType> IType.GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			return EmptyList<IType>.Instance;
		}

		public IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) != GetMemberOptions.IgnoreInheritedMembers)
			{
				return GetMembersHelper.GetConstructors(this, filter, options);
			}
			if ((this.HasDefaultConstructorConstraint || this.HasValueTypeConstraint) && (filter == null || filter(DefaultUnresolvedMethod.DummyConstructor)))
			{
				return new IMethod[]
				{
					DefaultResolvedMethod.GetDummyConstructor(this.compilation, this)
				};
			}
			return EmptyList<IMethod>.Instance;
		}

		public IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetMethods(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		public IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetMethods(this, typeArguments, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		public IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IProperty>.Instance;
			}
			return GetMembersHelper.GetProperties(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedProperty>(filter), options);
		}

		public IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IField>.Instance;
			}
			return GetMembersHelper.GetFields(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedField>(filter), options);
		}

		public IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IEvent>.Instance;
			}
			return GetMembersHelper.GetEvents(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedEvent>(filter), options);
		}

		public IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMember>.Instance;
			}
			return GetMembersHelper.GetMembers(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMember>(filter), options);
		}

		public IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetAccessors(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		private static Predicate<T> FilterNonStatic<T>(Predicate<T> filter) where T : class, IUnresolvedMember
		{
			if (filter == null)
			{
				return (T member) => !member.IsStatic;
			}
			return (T member) => !member.IsStatic && filter(member);
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

		public virtual ISymbolReference ToReference()
		{
			if (this.owner == null)
			{
				return TypeParameterReference.Create(this.ownerType, this.index);
			}
			return new OwnedTypeParameterReference(this.owner.ToReference(), this.index);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.ReflectionName,
				" (owner=",
				this.owner,
				")"
			});
		}

		private readonly ICompilation compilation;

		private readonly SymbolKind ownerType;

		private readonly IEntity owner;

		private readonly int index;

		private readonly string name;

		private readonly IList<IAttribute> attributes;

		private readonly DomRegion region;

		private readonly VarianceModifier variance;

		private volatile IType effectiveBaseClass;

		private ICollection<IType> effectiveInterfaceSet;

		private static readonly IList<IType> emptyTypeArguments = new IType[0];
	}
}
