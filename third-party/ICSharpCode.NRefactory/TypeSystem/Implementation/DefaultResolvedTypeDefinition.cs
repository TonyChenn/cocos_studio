using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" />.
	/// </summary>
	public class DefaultResolvedTypeDefinition : ITypeDefinition, IType, IEquatable<IType>, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public DefaultResolvedTypeDefinition(ITypeResolveContext parentContext, params IUnresolvedTypeDefinition[] parts)
		{
			if (parentContext == null || parentContext.CurrentAssembly == null)
			{
				throw new ArgumentException("Parent context does not specify any assembly", "parentContext");
			}
			if (parts == null || parts.Length == 0)
			{
				throw new ArgumentException("No parts were specified", "parts");
			}
			this.parentContext = parentContext;
			this.parts = parts;
			foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in parts)
			{
				this.isAbstract |= unresolvedTypeDefinition.IsAbstract;
				this.isSealed |= unresolvedTypeDefinition.IsSealed;
				this.isShadowing |= unresolvedTypeDefinition.IsShadowing;
				this.isSynthetic &= unresolvedTypeDefinition.IsSynthetic;
				if (this.accessibility == Accessibility.Internal)
				{
					this.accessibility = unresolvedTypeDefinition.Accessibility;
				}
			}
		}

		public IList<ITypeParameter> TypeParameters
		{
			get
			{
				IList<ITypeParameter> list = LazyInit.VolatileRead<IList<ITypeParameter>>(ref this.typeParameters);
				if (list != null)
				{
					return list;
				}
				ITypeResolveContext typeResolveContext = this.parts[0].CreateResolveContext(this.parentContext);
				typeResolveContext = typeResolveContext.WithCurrentTypeDefinition(this);
				if (this.parentContext.CurrentTypeDefinition == null || this.parentContext.CurrentTypeDefinition.TypeParameterCount == 0)
				{
					list = this.parts[0].TypeParameters.CreateResolvedTypeParameters(typeResolveContext);
				}
				else
				{
					ITypeDefinition currentTypeDefinition = this.parentContext.CurrentTypeDefinition;
					ITypeParameter[] array = new ITypeParameter[this.parts[0].TypeParameters.Count];
					for (int i = 0; i < array.Length; i++)
					{
						IUnresolvedTypeParameter unresolvedTypeParameter = this.parts[0].TypeParameters[i];
						if (i < currentTypeDefinition.TypeParameterCount && currentTypeDefinition.TypeParameters[i].Name == unresolvedTypeParameter.Name)
						{
							array[i] = currentTypeDefinition.TypeParameters[i];
						}
						else
						{
							array[i] = unresolvedTypeParameter.CreateResolvedTypeParameter(typeResolveContext);
						}
					}
					list = Array.AsReadOnly<ITypeParameter>(array);
				}
				return LazyInit.GetOrSet<IList<ITypeParameter>>(ref this.typeParameters, list);
			}
		}

		public IList<IAttribute> Attributes
		{
			get
			{
				IList<IAttribute> list = LazyInit.VolatileRead<IList<IAttribute>>(ref this.attributes);
				if (list != null)
				{
					return list;
				}
				list = new List<IAttribute>();
				ITypeResolveContext typeResolveContext = this.parentContext.WithCurrentTypeDefinition(this);
				foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
				{
					ITypeResolveContext context = unresolvedTypeDefinition.CreateResolveContext(typeResolveContext);
					foreach (IUnresolvedAttribute unresolvedAttribute in unresolvedTypeDefinition.Attributes)
					{
						list.Add(unresolvedAttribute.CreateResolvedAttribute(context));
					}
				}
				if (list.Count == 0)
				{
					list = EmptyList<IAttribute>.Instance;
				}
				return LazyInit.GetOrSet<IList<IAttribute>>(ref this.attributes, list);
			}
		}

		public IList<IUnresolvedTypeDefinition> Parts
		{
			get
			{
				return this.parts;
			}
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return this.parts[0].SymbolKind;
			}
		}

		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return (EntityType)this.parts[0].SymbolKind;
			}
		}

		public virtual TypeKind Kind
		{
			get
			{
				return this.parts[0].Kind;
			}
		}

		public IList<ITypeDefinition> NestedTypes
		{
			get
			{
				IList<ITypeDefinition> list = LazyInit.VolatileRead<IList<ITypeDefinition>>(ref this.nestedTypes);
				if (list != null)
				{
					return list;
				}
				list = (from part in this.parts
				from nestedTypeRef in part.NestedTypes
				group nestedTypeRef by new
				{
					nestedTypeRef.Name,
					nestedTypeRef.TypeParameters.Count
				} into g
				select new DefaultResolvedTypeDefinition(new SimpleTypeResolveContext(this), g.ToArray<IUnresolvedTypeDefinition>())).ToList<ITypeDefinition>().AsReadOnly();
				return LazyInit.GetOrSet<IList<ITypeDefinition>>(ref this.nestedTypes, list);
			}
		}

		private DefaultResolvedTypeDefinition.MemberList GetMemberList()
		{
			DefaultResolvedTypeDefinition.MemberList memberList = LazyInit.VolatileRead<DefaultResolvedTypeDefinition.MemberList>(ref this.memberList);
			if (memberList != null)
			{
				return memberList;
			}
			List<IUnresolvedMember> list = new List<IUnresolvedMember>();
			List<ITypeResolveContext> list2 = new List<ITypeResolveContext>();
			List<DefaultResolvedTypeDefinition.PartialMethodInfo> list3 = null;
			bool flag = false;
			foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
			{
				ITypeResolveContext typeResolveContext = unresolvedTypeDefinition.CreateResolveContext(this.parentContext);
				ITypeResolveContext typeResolveContext2 = typeResolveContext.WithCurrentTypeDefinition(this);
				foreach (IUnresolvedMember unresolvedMember in unresolvedTypeDefinition.Members)
				{
					IUnresolvedMethod unresolvedMethod = unresolvedMember as IUnresolvedMethod;
					if (unresolvedMethod != null && unresolvedMethod.IsPartial)
					{
						if (list3 == null)
						{
							list3 = new List<DefaultResolvedTypeDefinition.PartialMethodInfo>();
						}
						DefaultResolvedTypeDefinition.PartialMethodInfo partialMethodInfo = new DefaultResolvedTypeDefinition.PartialMethodInfo(unresolvedMethod, typeResolveContext2);
						DefaultResolvedTypeDefinition.PartialMethodInfo partialMethodInfo2 = null;
						foreach (DefaultResolvedTypeDefinition.PartialMethodInfo partialMethodInfo3 in list3)
						{
							if (partialMethodInfo.IsSameSignature(partialMethodInfo3, this.Compilation.NameComparer))
							{
								partialMethodInfo2 = partialMethodInfo3;
								break;
							}
						}
						if (partialMethodInfo2 != null)
						{
							partialMethodInfo2.AddPart(unresolvedMethod, typeResolveContext2);
						}
						else
						{
							list3.Add(partialMethodInfo);
						}
					}
					else
					{
						list.Add(unresolvedMember);
						list2.Add(typeResolveContext2);
					}
				}
				flag |= unresolvedTypeDefinition.AddDefaultConstructorIfRequired;
			}
			if (flag)
			{
				TypeKind kind = this.Kind;
				if (kind == TypeKind.Class && !this.IsStatic)
				{
					if (!list.Any((IUnresolvedMember m) => m.SymbolKind == SymbolKind.Constructor && !m.IsStatic))
					{
						goto IL_190;
					}
				}
				if (kind != TypeKind.Enum && kind != TypeKind.Struct)
				{
					goto IL_1C2;
				}
				IL_190:
				list2.Add(this.parts[0].CreateResolveContext(this.parentContext).WithCurrentTypeDefinition(this));
				list.Add(DefaultUnresolvedMethod.CreateDefaultConstructor(this.parts[0]));
			}
			IL_1C2:
			memberList = new DefaultResolvedTypeDefinition.MemberList(list2, list, list3);
			return LazyInit.GetOrSet<DefaultResolvedTypeDefinition.MemberList>(ref this.memberList, memberList);
		}

		public IList<IMember> Members
		{
			get
			{
				return this.GetMemberList();
			}
		}

		public IEnumerable<IField> Fields
		{
			get
			{
				DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
				for (int i = 0; i < members.unresolvedMembers.Length; i++)
				{
					if (members.unresolvedMembers[i].SymbolKind == SymbolKind.Field)
					{
						yield return (IField)members[i];
					}
				}
				yield break;
			}
		}

		public IEnumerable<IMethod> Methods
		{
			get
			{
				DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
				for (int i = 0; i < members.unresolvedMembers.Length; i++)
				{
					if (members.unresolvedMembers[i] is IUnresolvedMethod)
					{
						yield return (IMethod)members[i];
					}
				}
				for (int j = members.unresolvedMembers.Length; j < members.Count; j++)
				{
					yield return (IMethod)members[j];
				}
				yield break;
			}
		}

		public IEnumerable<IProperty> Properties
		{
			get
			{
				DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
				for (int i = 0; i < members.unresolvedMembers.Length; i++)
				{
					switch (members.unresolvedMembers[i].SymbolKind)
					{
					case SymbolKind.Property:
					case SymbolKind.Indexer:
						yield return (IProperty)members[i];
						break;
					}
				}
				yield break;
			}
		}

		public IEnumerable<IEvent> Events
		{
			get
			{
				DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
				for (int i = 0; i < members.unresolvedMembers.Length; i++)
				{
					if (members.unresolvedMembers[i].SymbolKind == SymbolKind.Event)
					{
						yield return (IEvent)members[i];
					}
				}
				yield break;
			}
		}

		public KnownTypeCode KnownTypeCode
		{
			get
			{
				KnownTypeCode knownTypeCode = this.knownTypeCode;
				if (knownTypeCode == (KnownTypeCode)(-1))
				{
					knownTypeCode = KnownTypeCode.None;
					ICompilation compilation = this.Compilation;
					for (int i = 0; i < 46; i++)
					{
						if (compilation.FindType((KnownTypeCode)i) == this)
						{
							knownTypeCode = (KnownTypeCode)i;
							break;
						}
					}
					this.knownTypeCode = knownTypeCode;
				}
				return knownTypeCode;
			}
		}

		public IType EnumUnderlyingType
		{
			get
			{
				IType type = this.enumUnderlyingType;
				if (type == null)
				{
					if (this.Kind == TypeKind.Enum)
					{
						type = this.CalculateEnumUnderlyingType();
					}
					else
					{
						type = SpecialType.UnknownType;
					}
					this.enumUnderlyingType = type;
				}
				return type;
			}
		}

		private IType CalculateEnumUnderlyingType()
		{
			foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
			{
				ITypeResolveContext context = unresolvedTypeDefinition.CreateResolveContext(this.parentContext).WithCurrentTypeDefinition(this);
				foreach (ITypeReference typeReference in unresolvedTypeDefinition.BaseTypes)
				{
					IType type = typeReference.Resolve(context);
					if (type.Kind != TypeKind.Unknown)
					{
						return type;
					}
				}
			}
			return this.Compilation.FindType(KnownTypeCode.Int32);
		}

		public bool HasExtensionMethods
		{
			get
			{
				byte b = this.hasExtensionMethods;
				if (b == 0)
				{
					if (this.CalculateHasExtensionMethods())
					{
						b = 1;
					}
					else
					{
						b = 2;
					}
					this.hasExtensionMethods = b;
				}
				return b == 1;
			}
		}

		private bool CalculateHasExtensionMethods()
		{
			bool flag = true;
			foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
			{
				if (unresolvedTypeDefinition.HasExtensionMethods == true)
				{
					return true;
				}
				if (unresolvedTypeDefinition.HasExtensionMethods == null)
				{
					flag = false;
				}
			}
			if (flag)
			{
				return false;
			}
			return this.Methods.Any((IMethod m) => m.IsExtensionMethod);
		}

		public bool IsPartial
		{
			get
			{
				return this.parts.Length > 1 || this.parts[0].IsPartial;
			}
		}

		public bool? IsReferenceType
		{
			get
			{
				switch (this.Kind)
				{
				case TypeKind.Class:
				case TypeKind.Interface:
				case TypeKind.Delegate:
				case TypeKind.Module:
					return new bool?(true);
				case TypeKind.Struct:
				case TypeKind.Enum:
				case TypeKind.Void:
					return new bool?(false);
				default:
					throw new InvalidOperationException("Invalid value for TypeKind");
				}
			}
		}

		public int TypeParameterCount
		{
			get
			{
				return this.parts[0].TypeParameters.Count;
			}
		}

		public IList<IType> TypeArguments
		{
			get
			{
				return this.TypeParameters.ToList<IType>();
			}
		}

		public bool IsParameterized
		{
			get
			{
				return false;
			}
		}

		public IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				IList<IType> list = LazyInit.VolatileRead<IList<IType>>(ref this.directBaseTypes);
				if (list != null)
				{
					return list;
				}
				IEnumerable<IType> result;
				using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
				{
					if (busyLock.Success)
					{
						list = this.CalculateDirectBaseTypes();
						result = LazyInit.GetOrSet<IList<IType>>(ref this.directBaseTypes, list);
					}
					else
					{
						result = EmptyList<IType>.Instance;
					}
				}
				return result;
			}
		}

		private IList<IType> CalculateDirectBaseTypes()
		{
			List<IType> list = new List<IType>();
			bool flag = false;
			if (this.Kind != TypeKind.Enum)
			{
				foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
				{
					ITypeResolveContext context = unresolvedTypeDefinition.CreateResolveContext(this.parentContext).WithCurrentTypeDefinition(this);
					foreach (ITypeReference typeReference in unresolvedTypeDefinition.BaseTypes)
					{
						IType type = typeReference.Resolve(context);
						if (type.Kind != TypeKind.Unknown && !list.Contains(type))
						{
							list.Add(type);
							if (type.Kind != TypeKind.Interface)
							{
								flag = true;
							}
						}
					}
				}
			}
			if (!flag && (!(this.Name == "Object") || !(this.Namespace == "System") || this.TypeParameterCount != 0))
			{
				KnownTypeCode typeCode;
				switch (this.Kind)
				{
				case TypeKind.Struct:
				case TypeKind.Void:
					typeCode = KnownTypeCode.ValueType;
					goto IL_11D;
				case TypeKind.Delegate:
					typeCode = KnownTypeCode.Delegate;
					goto IL_11D;
				case TypeKind.Enum:
					typeCode = KnownTypeCode.Enum;
					goto IL_11D;
				}
				typeCode = KnownTypeCode.Object;
				IL_11D:
				IType type2 = this.parentContext.Compilation.FindType(typeCode);
				if (type2.Kind != TypeKind.Unknown)
				{
					list.Add(type2);
				}
			}
			return list;
		}

		public string FullName
		{
			get
			{
				return this.parts[0].FullName;
			}
		}

		public string Name
		{
			get
			{
				return this.parts[0].Name;
			}
		}

		public string ReflectionName
		{
			get
			{
				return this.parts[0].ReflectionName;
			}
		}

		public string Namespace
		{
			get
			{
				return this.parts[0].Namespace;
			}
		}

		public FullTypeName FullTypeName
		{
			get
			{
				return this.parts[0].FullTypeName;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.parts[0].Region;
			}
		}

		public DomRegion BodyRegion
		{
			get
			{
				return this.parts[0].BodyRegion;
			}
		}

		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		public IType DeclaringType
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		public IAssembly ParentAssembly
		{
			get
			{
				return this.parentContext.CurrentAssembly;
			}
		}

		public virtual DocumentationComment Documentation
		{
			get
			{
				foreach (IUnresolvedTypeDefinition unresolvedTypeDefinition in this.parts)
				{
					IUnresolvedDocumentationProvider unresolvedDocumentationProvider = unresolvedTypeDefinition.UnresolvedFile as IUnresolvedDocumentationProvider;
					if (unresolvedDocumentationProvider != null)
					{
						DocumentationComment documentation = unresolvedDocumentationProvider.GetDocumentation(unresolvedTypeDefinition, this);
						if (documentation != null)
						{
							return documentation;
						}
					}
				}
				IDocumentationProvider documentationProvider = AbstractResolvedEntity.FindDocumentation(this.parentContext);
				if (documentationProvider != null)
				{
					return documentationProvider.GetDocumentation(this);
				}
				return null;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.parentContext.Compilation;
			}
		}

		public bool IsStatic
		{
			get
			{
				return this.isAbstract && this.isSealed;
			}
		}

		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
		}

		public bool IsSealed
		{
			get
			{
				return this.isSealed;
			}
		}

		public bool IsShadowing
		{
			get
			{
				return this.isShadowing;
			}
		}

		public bool IsSynthetic
		{
			get
			{
				return this.isSynthetic;
			}
		}

		public Accessibility Accessibility
		{
			get
			{
				return this.accessibility;
			}
		}

		bool IHasAccessibility.IsPrivate
		{
			get
			{
				return this.accessibility == Accessibility.Private;
			}
		}

		bool IHasAccessibility.IsPublic
		{
			get
			{
				return this.accessibility == Accessibility.Public;
			}
		}

		bool IHasAccessibility.IsProtected
		{
			get
			{
				return this.accessibility == Accessibility.Protected;
			}
		}

		bool IHasAccessibility.IsInternal
		{
			get
			{
				return this.accessibility == Accessibility.Internal;
			}
		}

		bool IHasAccessibility.IsProtectedOrInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedOrInternal;
			}
		}

		bool IHasAccessibility.IsProtectedAndInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedAndInternal;
			}
		}

		ITypeDefinition IType.GetDefinition()
		{
			return this;
		}

		IType IType.AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeDefinition(this);
		}

		IType IType.VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		public ITypeReference ToTypeReference()
		{
			ITypeDefinition declaringTypeDefinition = this.DeclaringTypeDefinition;
			if (declaringTypeDefinition != null)
			{
				return new NestedTypeReference(declaringTypeDefinition.ToTypeReference(), this.Name, this.TypeParameterCount - declaringTypeDefinition.TypeParameterCount);
			}
			IAssembly parentAssembly = this.ParentAssembly;
			IAssemblyReference assembly;
			if (parentAssembly != null)
			{
				assembly = new DefaultAssemblyReference(parentAssembly.AssemblyName);
			}
			else
			{
				assembly = null;
			}
			return new GetClassTypeReference(assembly, this.Namespace, this.Name, this.TypeParameterCount);
		}

		ISymbolReference ISymbol.ToReference()
		{
			return (ISymbolReference)this.ToTypeReference();
		}

		public IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers)) != (GetMemberOptions.ReturnMemberDefinitions | GetMemberOptions.IgnoreInheritedMembers))
			{
				return GetMembersHelper.GetNestedTypes(this, filter, options);
			}
			if (filter == null)
			{
				return this.NestedTypes;
			}
			return this.GetNestedTypesImpl(filter);
		}

		private IEnumerable<IType> GetNestedTypesImpl(Predicate<ITypeDefinition> filter)
		{
			foreach (ITypeDefinition nestedType in this.NestedTypes)
			{
				if (filter(nestedType))
				{
					yield return nestedType;
				}
			}
			yield break;
		}

		public IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return GetMembersHelper.GetNestedTypes(this, typeArguments, filter, options);
		}

		private IEnumerable<IMember> GetFilteredMembers(Predicate<IUnresolvedMember> filter)
		{
			DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
			for (int i = 0; i < members.unresolvedMembers.Length; i++)
			{
				if (filter == null || filter(members.unresolvedMembers[i]))
				{
					yield return members[i];
				}
			}
			for (int j = members.unresolvedMembers.Length; j < members.Count; j++)
			{
				IMethod method = (IMethod)members[j];
				bool ok = false;
				foreach (IUnresolvedMethod obj in method.Parts)
				{
					if (filter == null || filter(obj))
					{
						ok = true;
						break;
					}
				}
				if (ok)
				{
					yield return method;
				}
			}
			yield break;
		}

		private IEnumerable<IMethod> GetFilteredMethods(Predicate<IUnresolvedMethod> filter)
		{
			DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
			for (int i = 0; i < members.unresolvedMembers.Length; i++)
			{
				IUnresolvedMethod unresolved = members.unresolvedMembers[i] as IUnresolvedMethod;
				if (unresolved != null && (filter == null || filter(unresolved)))
				{
					yield return (IMethod)members[i];
				}
			}
			for (int j = members.unresolvedMembers.Length; j < members.Count; j++)
			{
				IMethod method = (IMethod)members[j];
				bool ok = false;
				foreach (IUnresolvedMethod obj in method.Parts)
				{
					if (filter == null || filter(obj))
					{
						ok = true;
						break;
					}
				}
				if (ok)
				{
					yield return method;
				}
			}
			yield break;
		}

		private IEnumerable<TResolved> GetFilteredNonMethods<TUnresolved, TResolved>(Predicate<TUnresolved> filter) where TUnresolved : class, IUnresolvedMember where TResolved : class, IMember
		{
			DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
			for (int i = 0; i < members.unresolvedMembers.Length; i++)
			{
				TUnresolved unresolved = members.unresolvedMembers[i] as TUnresolved;
				if (unresolved != null && (filter == null || filter(unresolved)))
				{
					yield return (TResolved)((object)members[i]);
				}
			}
			yield break;
		}

		public virtual IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				Predicate<IUnresolvedMethod> predicate = delegate(IUnresolvedMethod m)
				{
					return !m.IsConstructor;
				};
				return this.GetFilteredMethods(predicate.And(filter));
			}
			return GetMembersHelper.GetMethods(this, filter, options);
		}

		public virtual IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		public virtual IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			if (ComHelper.IsComImport(this))
			{
				IType coClass = ComHelper.GetCoClass(this);
				using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
				{
					if (busyLock.Success)
					{
						return from m in coClass.GetConstructors(filter, options)
						select new SpecializedMethod(m, m.Substitution)
						{
							DeclaringType = this
						};
					}
				}
				return EmptyList<IMethod>.Instance;
			}
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				Predicate<IUnresolvedMethod> predicate = delegate(IUnresolvedMethod m)
				{
					return m.IsConstructor && !m.IsStatic;
				};
				return this.GetFilteredMethods(predicate.And(filter));
			}
			return GetMembersHelper.GetConstructors(this, filter, options);
		}

		public virtual IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedProperty, IProperty>(filter);
			}
			return GetMembersHelper.GetProperties(this, filter, options);
		}

		public virtual IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedField, IField>(filter);
			}
			return GetMembersHelper.GetFields(this, filter, options);
		}

		public virtual IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedEvent, IEvent>(filter);
			}
			return GetMembersHelper.GetEvents(this, filter, options);
		}

		public virtual IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredMembers(filter);
			}
			return GetMembersHelper.GetMembers(this, filter, options);
		}

		public virtual IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredAccessors(filter);
			}
			return GetMembersHelper.GetAccessors(this, filter, options);
		}

		private IEnumerable<IMethod> GetFilteredAccessors(Predicate<IUnresolvedMethod> filter)
		{
			DefaultResolvedTypeDefinition.MemberList members = this.GetMemberList();
			for (int i = 0; i < members.unresolvedMembers.Length; i++)
			{
				IUnresolvedMember unresolved = members.unresolvedMembers[i];
				IUnresolvedProperty unresolvedProperty = unresolved as IUnresolvedProperty;
				IUnresolvedEvent unresolvedEvent = unresolved as IUnresolvedEvent;
				if (unresolvedProperty != null)
				{
					if (unresolvedProperty.CanGet && (filter == null || filter(unresolvedProperty.Getter)))
					{
						yield return ((IProperty)members[i]).Getter;
					}
					if (unresolvedProperty.CanSet && (filter == null || filter(unresolvedProperty.Setter)))
					{
						yield return ((IProperty)members[i]).Setter;
					}
				}
				else if (unresolvedEvent != null)
				{
					if (unresolvedEvent.CanAdd && (filter == null || filter(unresolvedEvent.AddAccessor)))
					{
						yield return ((IEvent)members[i]).AddAccessor;
					}
					if (unresolvedEvent.CanRemove && (filter == null || filter(unresolvedEvent.RemoveAccessor)))
					{
						yield return ((IEvent)members[i]).RemoveAccessor;
					}
					if (unresolvedEvent.CanInvoke && (filter == null || filter(unresolvedEvent.InvokeAccessor)))
					{
						yield return ((IEvent)members[i]).InvokeAccessor;
					}
				}
			}
			yield break;
		}

		public IMember GetInterfaceImplementation(IMember interfaceMember)
		{
			return this.GetInterfaceImplementation(new IMember[]
			{
				interfaceMember
			})[0];
		}

		public IList<IMember> GetInterfaceImplementation(IList<IMember> interfaceMembers)
		{
			interfaceMembers = interfaceMembers.ToList<IMember>();
			IMember[] array = new IMember[interfaceMembers.Count];
			MultiDictionary<IMember, int> multiDictionary = new MultiDictionary<IMember, int>(SignatureComparer.Ordinal);
			for (int i = 0; i < interfaceMembers.Count; i++)
			{
				multiDictionary.Add(interfaceMembers[i], i);
			}
			foreach (IMember member in this.GetMembers((IUnresolvedMember m) => !m.IsExplicitInterfaceImplementation, GetMemberOptions.None))
			{
				foreach (int num in multiDictionary[member])
				{
					array[num] = member;
				}
			}
			foreach (IMember member2 in this.GetMembers((IUnresolvedMember m) => m.IsExplicitInterfaceImplementation, GetMemberOptions.None))
			{
				foreach (IMember member3 in member2.ImplementedInterfaceMembers)
				{
					foreach (int num2 in multiDictionary[member3])
					{
						if (member3.Equals(interfaceMembers[num2]))
						{
							array[num2] = member2;
						}
					}
				}
			}
			return array;
		}

		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		public bool Equals(IType other)
		{
			return this == other;
		}

		public override string ToString()
		{
			return this.ReflectionName;
		}

		private readonly ITypeResolveContext parentContext;

		private readonly IUnresolvedTypeDefinition[] parts;

		private Accessibility accessibility = Accessibility.Internal;

		private bool isAbstract;

		private bool isSealed;

		private bool isShadowing;

		private bool isSynthetic = true;

		private IList<ITypeParameter> typeParameters;

		private IList<IAttribute> attributes;

		private IList<ITypeDefinition> nestedTypes;

		private DefaultResolvedTypeDefinition.MemberList memberList;

		private volatile KnownTypeCode knownTypeCode = (KnownTypeCode)(-1);

		private volatile IType enumUnderlyingType;

		private volatile byte hasExtensionMethods;

		private IList<IType> directBaseTypes;

		private sealed class MemberList : IList<IMember>, ICollection<IMember>, IEnumerable<IMember>, IEnumerable
		{
			public MemberList(List<ITypeResolveContext> contextPerMember, List<IUnresolvedMember> unresolvedNonPartialMembers, List<DefaultResolvedTypeDefinition.PartialMethodInfo> partialMethodInfos)
			{
				this.NonPartialMemberCount = unresolvedNonPartialMembers.Count;
				this.contextPerMember = contextPerMember.ToArray();
				this.unresolvedMembers = unresolvedNonPartialMembers.ToArray();
				if (partialMethodInfos == null)
				{
					this.resolvedMembers = new IMember[unresolvedNonPartialMembers.Count];
					return;
				}
				this.resolvedMembers = new IMember[unresolvedNonPartialMembers.Count + partialMethodInfos.Count];
				for (int i = 0; i < partialMethodInfos.Count; i++)
				{
					DefaultResolvedTypeDefinition.PartialMethodInfo partialMethodInfo = partialMethodInfos[i];
					int num = this.NonPartialMemberCount + i;
					this.resolvedMembers[num] = DefaultResolvedMethod.CreateFromMultipleParts(partialMethodInfo.Parts.ToArray(), partialMethodInfo.Contexts.ToArray(), false);
				}
			}

			public IMember this[int index]
			{
				get
				{
					IMember member = LazyInit.VolatileRead<IMember>(ref this.resolvedMembers[index]);
					if (member != null)
					{
						return member;
					}
					return LazyInit.GetOrSet<IMember>(ref this.resolvedMembers[index], this.unresolvedMembers[index].CreateResolved(this.contextPerMember[index]));
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			public int Count
			{
				get
				{
					return this.resolvedMembers.Length;
				}
			}

			bool ICollection<IMember>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public int IndexOf(IMember item)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i].Equals(item))
					{
						return i;
					}
				}
				return -1;
			}

			void IList<IMember>.Insert(int index, IMember item)
			{
				throw new NotSupportedException();
			}

			void IList<IMember>.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			void ICollection<IMember>.Add(IMember item)
			{
				throw new NotSupportedException();
			}

			void ICollection<IMember>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<IMember>.Contains(IMember item)
			{
				return this.IndexOf(item) >= 0;
			}

			void ICollection<IMember>.CopyTo(IMember[] array, int arrayIndex)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array[arrayIndex + i] = this[i];
				}
			}

			bool ICollection<IMember>.Remove(IMember item)
			{
				throw new NotSupportedException();
			}

			public IEnumerator<IMember> GetEnumerator()
			{
				for (int i = 0; i < this.Count; i++)
				{
					yield return this[i];
				}
				yield break;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			internal readonly ITypeResolveContext[] contextPerMember;

			internal readonly IUnresolvedMember[] unresolvedMembers;

			internal readonly IMember[] resolvedMembers;

			internal readonly int NonPartialMemberCount;
		}

		private sealed class PartialMethodInfo
		{
			public PartialMethodInfo(IUnresolvedMethod method, ITypeResolveContext context)
			{
				this.Name = method.Name;
				this.TypeParameterCount = method.TypeParameters.Count;
				this.Parameters = method.Parameters.CreateResolvedParameters(context);
				this.Parts.Add(method);
				this.Contexts.Add(context);
			}

			public void AddPart(IUnresolvedMethod method, ITypeResolveContext context)
			{
				if (method.HasBody)
				{
					this.Parts.Insert(0, method);
					this.Contexts.Insert(0, context);
					return;
				}
				this.Parts.Add(method);
				this.Contexts.Add(context);
			}

			public bool IsSameSignature(DefaultResolvedTypeDefinition.PartialMethodInfo other, StringComparer nameComparer)
			{
				return nameComparer.Equals(this.Name, other.Name) && this.TypeParameterCount == other.TypeParameterCount && ParameterListComparer.Instance.Equals(this.Parameters, other.Parameters);
			}

			public readonly string Name;

			public readonly int TypeParameterCount;

			public readonly IList<IParameter> Parameters;

			public readonly List<IUnresolvedMethod> Parts = new List<IUnresolvedMethod>();

			public readonly List<ITypeResolveContext> Contexts = new List<ITypeResolveContext>();
		}
	}
}
