using System;
using System.Collections.Generic;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000A1 RID: 161
	public abstract class AbstractTypeParameter : ITypeParameter, IType, INamedElement, IEquatable<IType>, ISymbol, ICompilationProvider
	{
		// Token: 0x06000504 RID: 1284 RVA: 0x0000C008 File Offset: 0x0000B008
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

		// Token: 0x06000505 RID: 1285 RVA: 0x0000C0A4 File Offset: 0x0000B0A4
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0000C12D File Offset: 0x0000B12D
		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.TypeParameter;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0000C131 File Offset: 0x0000B131
		public SymbolKind OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000C139 File Offset: 0x0000B139
		public IEntity Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0000C141 File Offset: 0x0000B141
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000C149 File Offset: 0x0000B149
		public IList<IAttribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0000C151 File Offset: 0x0000B151
		public VarianceModifier Variance
		{
			get
			{
				return this.variance;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0000C159 File Offset: 0x0000B159
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0000C161 File Offset: 0x0000B161
		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0000C16C File Offset: 0x0000B16C
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

		// Token: 0x0600050F RID: 1295 RVA: 0x0000C1D4 File Offset: 0x0000B1D4
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

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0000C2CC File Offset: 0x0000B2CC
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

		// Token: 0x06000511 RID: 1297 RVA: 0x0000C338 File Offset: 0x0000B338
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

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000512 RID: 1298
		public abstract bool HasDefaultConstructorConstraint { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000513 RID: 1299
		public abstract bool HasReferenceTypeConstraint { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000514 RID: 1300
		public abstract bool HasValueTypeConstraint { get; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0000C3B4 File Offset: 0x0000B3B4
		public TypeKind Kind
		{
			get
			{
				return TypeKind.TypeParameter;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000C3B8 File Offset: 0x0000B3B8
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0000C458 File Offset: 0x0000B458
		IType IType.DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000C45B File Offset: 0x0000B45B
		int IType.TypeParameterCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0000C45E File Offset: 0x0000B45E
		bool IType.IsParameterized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000C461 File Offset: 0x0000B461
		IList<IType> IType.TypeArguments
		{
			get
			{
				return AbstractTypeParameter.emptyTypeArguments;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600051B RID: 1307
		public abstract IEnumerable<IType> DirectBaseTypes { get; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000C468 File Offset: 0x0000B468
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0000C470 File Offset: 0x0000B470
		string INamedElement.Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0000C477 File Offset: 0x0000B477
		string INamedElement.FullName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0000C480 File Offset: 0x0000B480
		public string ReflectionName
		{
			get
			{
				return ((this.OwnerType == SymbolKind.Method) ? "``" : "`") + this.index.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000C4BA File Offset: 0x0000B4BA
		ITypeDefinition IType.GetDefinition()
		{
			return null;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000C4BD File Offset: 0x0000B4BD
		public IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeParameter(this);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000C4C6 File Offset: 0x0000B4C6
		public IType VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000C4C9 File Offset: 0x0000B4C9
		public ITypeReference ToTypeReference()
		{
			return TypeParameterReference.Create(this.OwnerType, this.Index);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000C4DC File Offset: 0x0000B4DC
		IEnumerable<IType> IType.GetNestedTypes(Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			return EmptyList<IType>.Instance;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000C4E3 File Offset: 0x0000B4E3
		IEnumerable<IType> IType.GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter, GetMemberOptions options)
		{
			return EmptyList<IType>.Instance;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000C4EC File Offset: 0x0000B4EC
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

		// Token: 0x06000527 RID: 1319 RVA: 0x0000C545 File Offset: 0x0000B545
		public IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetMethods(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000C560 File Offset: 0x0000B560
		public IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetMethods(this, typeArguments, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000C57C File Offset: 0x0000B57C
		public IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IProperty>.Instance;
			}
			return GetMembersHelper.GetProperties(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedProperty>(filter), options);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000C597 File Offset: 0x0000B597
		public IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IField>.Instance;
			}
			return GetMembersHelper.GetFields(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedField>(filter), options);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000C5B2 File Offset: 0x0000B5B2
		public IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IEvent>.Instance;
			}
			return GetMembersHelper.GetEvents(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedEvent>(filter), options);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000C5CD File Offset: 0x0000B5CD
		public IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMember>.Instance;
			}
			return GetMembersHelper.GetMembers(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMember>(filter), options);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000C5E8 File Offset: 0x0000B5E8
		public IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return GetMembersHelper.GetAccessors(this, AbstractTypeParameter.FilterNonStatic<IUnresolvedMethod>(filter), options);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000C603 File Offset: 0x0000B603
		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000C60A File Offset: 0x0000B60A
		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000C64C File Offset: 0x0000B64C
		private static Predicate<T> FilterNonStatic<T>(Predicate<T> filter) where T : class, IUnresolvedMember
		{
			if (filter == null)
			{
				return (T member) => !member.IsStatic;
			}
			return (T member) => !member.IsStatic && filter(member);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000C687 File Offset: 0x0000B687
		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as IType);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000C695 File Offset: 0x0000B695
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000C69D File Offset: 0x0000B69D
		public virtual bool Equals(IType other)
		{
			return this == other;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000C6A3 File Offset: 0x0000B6A3
		public virtual ISymbolReference ToReference()
		{
			if (this.owner == null)
			{
				return TypeParameterReference.Create(this.ownerType, this.index);
			}
			return new OwnedTypeParameterReference(this.owner.ToReference(), this.index);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000C6D8 File Offset: 0x0000B6D8
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

		// Token: 0x04000151 RID: 337
		private readonly ICompilation compilation;

		// Token: 0x04000152 RID: 338
		private readonly SymbolKind ownerType;

		// Token: 0x04000153 RID: 339
		private readonly IEntity owner;

		// Token: 0x04000154 RID: 340
		private readonly int index;

		// Token: 0x04000155 RID: 341
		private readonly string name;

		// Token: 0x04000156 RID: 342
		private readonly IList<IAttribute> attributes;

		// Token: 0x04000157 RID: 343
		private readonly DomRegion region;

		// Token: 0x04000158 RID: 344
		private readonly VarianceModifier variance;

		// Token: 0x04000159 RID: 345
		private volatile IType effectiveBaseClass;

		// Token: 0x0400015A RID: 346
		private ICollection<IType> effectiveInterfaceSet;

		// Token: 0x0400015B RID: 347
		private static readonly IList<IType> emptyTypeArguments = new IType[0];
	}
}
