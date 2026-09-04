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
	// Token: 0x020000B3 RID: 179
	public class DefaultResolvedTypeDefinition : ITypeDefinition, IType, IEquatable<IType>, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0000DDFC File Offset: 0x0000CDFC
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

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0000DEDC File Offset: 0x0000CEDC
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0000DFF8 File Offset: 0x0000CFF8
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

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0000E0BC File Offset: 0x0000D0BC
		public IList<IUnresolvedTypeDefinition> Parts
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0000E0C4 File Offset: 0x0000D0C4
		public SymbolKind SymbolKind
		{
			get
			{
				return this.parts[0].SymbolKind;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0000E0D3 File Offset: 0x0000D0D3
		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return (EntityType)this.parts[0].SymbolKind;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0000E0E2 File Offset: 0x0000D0E2
		public virtual TypeKind Kind
		{
			get
			{
				return this.parts[0].Kind;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0000E370 File Offset: 0x0000D370
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

		// Token: 0x060005E6 RID: 1510 RVA: 0x0000E458 File Offset: 0x0000D458
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

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0000E658 File Offset: 0x0000D658
		public IList<IMember> Members
		{
			get
			{
				return this.GetMemberList();
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0000E79C File Offset: 0x0000D79C
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0000E95C File Offset: 0x0000D95C
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

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0000EAC8 File Offset: 0x0000DAC8
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

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0000EC24 File Offset: 0x0000DC24
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

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0000EC44 File Offset: 0x0000DC44
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

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0000EC8C File Offset: 0x0000DC8C
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

		// Token: 0x060005EE RID: 1518 RVA: 0x0000ECC8 File Offset: 0x0000DCC8
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

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0000ED70 File Offset: 0x0000DD70
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

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000EDAC File Offset: 0x0000DDAC
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0000EE3C File Offset: 0x0000DE3C
		public bool IsPartial
		{
			get
			{
				return this.parts.Length > 1 || this.parts[0].IsPartial;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0000EE58 File Offset: 0x0000DE58
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0000EEAA File Offset: 0x0000DEAA
		public int TypeParameterCount
		{
			get
			{
				return this.parts[0].TypeParameters.Count;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000EEBE File Offset: 0x0000DEBE
		public IList<IType> TypeArguments
		{
			get
			{
				return this.TypeParameters.ToList<IType>();
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0000EECB File Offset: 0x0000DECB
		public bool IsParameterized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0000EED0 File Offset: 0x0000DED0
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

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000EF3C File Offset: 0x0000DF3C
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

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000F0A0 File Offset: 0x0000E0A0
		public string FullName
		{
			get
			{
				return this.parts[0].FullName;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0000F0AF File Offset: 0x0000E0AF
		public string Name
		{
			get
			{
				return this.parts[0].Name;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000F0BE File Offset: 0x0000E0BE
		public string ReflectionName
		{
			get
			{
				return this.parts[0].ReflectionName;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0000F0CD File Offset: 0x0000E0CD
		public string Namespace
		{
			get
			{
				return this.parts[0].Namespace;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0000F0DC File Offset: 0x0000E0DC
		public FullTypeName FullTypeName
		{
			get
			{
				return this.parts[0].FullTypeName;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0000F0EB File Offset: 0x0000E0EB
		public DomRegion Region
		{
			get
			{
				return this.parts[0].Region;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0000F0FA File Offset: 0x0000E0FA
		public DomRegion BodyRegion
		{
			get
			{
				return this.parts[0].BodyRegion;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0000F109 File Offset: 0x0000E109
		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0000F116 File Offset: 0x0000E116
		public IType DeclaringType
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0000F123 File Offset: 0x0000E123
		public IAssembly ParentAssembly
		{
			get
			{
				return this.parentContext.CurrentAssembly;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0000F130 File Offset: 0x0000E130
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

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0000F199 File Offset: 0x0000E199
		public ICompilation Compilation
		{
			get
			{
				return this.parentContext.Compilation;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0000F1A6 File Offset: 0x0000E1A6
		public bool IsStatic
		{
			get
			{
				return this.isAbstract && this.isSealed;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0000F1B8 File Offset: 0x0000E1B8
		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0000F1C0 File Offset: 0x0000E1C0
		public bool IsSealed
		{
			get
			{
				return this.isSealed;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0000F1C8 File Offset: 0x0000E1C8
		public bool IsShadowing
		{
			get
			{
				return this.isShadowing;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0000F1D0 File Offset: 0x0000E1D0
		public bool IsSynthetic
		{
			get
			{
				return this.isSynthetic;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0000F1D8 File Offset: 0x0000E1D8
		public Accessibility Accessibility
		{
			get
			{
				return this.accessibility;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0000F1E0 File Offset: 0x0000E1E0
		bool IHasAccessibility.IsPrivate
		{
			get
			{
				return this.accessibility == Accessibility.Private;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0000F1EB File Offset: 0x0000E1EB
		bool IHasAccessibility.IsPublic
		{
			get
			{
				return this.accessibility == Accessibility.Public;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0000F1F6 File Offset: 0x0000E1F6
		bool IHasAccessibility.IsProtected
		{
			get
			{
				return this.accessibility == Accessibility.Protected;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0000F201 File Offset: 0x0000E201
		bool IHasAccessibility.IsInternal
		{
			get
			{
				return this.accessibility == Accessibility.Internal;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0000F20C File Offset: 0x0000E20C
		bool IHasAccessibility.IsProtectedOrInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedOrInternal;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0000F217 File Offset: 0x0000E217
		bool IHasAccessibility.IsProtectedAndInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedAndInternal;
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0000F222 File Offset: 0x0000E222
		ITypeDefinition IType.GetDefinition()
		{
			return this;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0000F225 File Offset: 0x0000E225
		IType IType.AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeDefinition(this);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000F22E File Offset: 0x0000E22E
		IType IType.VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000F234 File Offset: 0x0000E234
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

		// Token: 0x06000614 RID: 1556 RVA: 0x0000F29C File Offset: 0x0000E29C
		ISymbolReference ISymbol.ToReference()
		{
			return (ISymbolReference)this.ToTypeReference();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0000F2A9 File Offset: 0x0000E2A9
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

		// Token: 0x06000616 RID: 1558 RVA: 0x0000F47C File Offset: 0x0000E47C
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

		// Token: 0x06000617 RID: 1559 RVA: 0x0000F4A0 File Offset: 0x0000E4A0
		public IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return GetMembersHelper.GetNestedTypes(this, typeArguments, filter, options);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0000F6F0 File Offset: 0x0000E6F0
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

		// Token: 0x06000619 RID: 1561 RVA: 0x0000F978 File Offset: 0x0000E978
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

		// Token: 0x0600061A RID: 1562 RVA: 0x0000FB20 File Offset: 0x0000EB20
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

		// Token: 0x0600061B RID: 1563 RVA: 0x0000FB4F File Offset: 0x0000EB4F
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

		// Token: 0x0600061C RID: 1564 RVA: 0x0000FB89 File Offset: 0x0000EB89
		public virtual IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000FBCC File Offset: 0x0000EBCC
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

		// Token: 0x0600061E RID: 1566 RVA: 0x0000FC7C File Offset: 0x0000EC7C
		public virtual IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedProperty, IProperty>(filter);
			}
			return GetMembersHelper.GetProperties(this, filter, options);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0000FC94 File Offset: 0x0000EC94
		public virtual IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedField, IField>(filter);
			}
			return GetMembersHelper.GetFields(this, filter, options);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000FCAC File Offset: 0x0000ECAC
		public virtual IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredNonMethods<IUnresolvedEvent, IEvent>(filter);
			}
			return GetMembersHelper.GetEvents(this, filter, options);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0000FCC4 File Offset: 0x0000ECC4
		public virtual IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredMembers(filter);
			}
			return GetMembersHelper.GetMembers(this, filter, options);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0000FCDC File Offset: 0x0000ECDC
		public virtual IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return this.GetFilteredAccessors(filter);
			}
			return GetMembersHelper.GetAccessors(this, filter, options);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001003C File Offset: 0x0000F03C
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

		// Token: 0x06000624 RID: 1572 RVA: 0x00010060 File Offset: 0x0000F060
		public IMember GetInterfaceImplementation(IMember interfaceMember)
		{
			return this.GetInterfaceImplementation(new IMember[]
			{
				interfaceMember
			})[0];
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00010098 File Offset: 0x0000F098
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

		// Token: 0x06000626 RID: 1574 RVA: 0x00010270 File Offset: 0x0000F270
		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00010277 File Offset: 0x0000F277
		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001027E File Offset: 0x0000F27E
		public bool Equals(IType other)
		{
			return this == other;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00010284 File Offset: 0x0000F284
		public override string ToString()
		{
			return this.ReflectionName;
		}

		// Token: 0x040001AA RID: 426
		private readonly ITypeResolveContext parentContext;

		// Token: 0x040001AB RID: 427
		private readonly IUnresolvedTypeDefinition[] parts;

		// Token: 0x040001AC RID: 428
		private Accessibility accessibility = Accessibility.Internal;

		// Token: 0x040001AD RID: 429
		private bool isAbstract;

		// Token: 0x040001AE RID: 430
		private bool isSealed;

		// Token: 0x040001AF RID: 431
		private bool isShadowing;

		// Token: 0x040001B0 RID: 432
		private bool isSynthetic = true;

		// Token: 0x040001B1 RID: 433
		private IList<ITypeParameter> typeParameters;

		// Token: 0x040001B2 RID: 434
		private IList<IAttribute> attributes;

		// Token: 0x040001B3 RID: 435
		private IList<ITypeDefinition> nestedTypes;

		// Token: 0x040001B4 RID: 436
		private DefaultResolvedTypeDefinition.MemberList memberList;

		// Token: 0x040001B5 RID: 437
		private volatile KnownTypeCode knownTypeCode = (KnownTypeCode)(-1);

		// Token: 0x040001B6 RID: 438
		private volatile IType enumUnderlyingType;

		// Token: 0x040001B7 RID: 439
		private volatile byte hasExtensionMethods;

		// Token: 0x040001B8 RID: 440
		private IList<IType> directBaseTypes;

		// Token: 0x020000B4 RID: 180
		private sealed class MemberList : IList<IMember>, ICollection<IMember>, IEnumerable<IMember>, IEnumerable
		{
			// Token: 0x06000636 RID: 1590 RVA: 0x0001028C File Offset: 0x0000F28C
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

			// Token: 0x17000285 RID: 645
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

			// Token: 0x17000286 RID: 646
			// (get) Token: 0x06000639 RID: 1593 RVA: 0x00010389 File Offset: 0x0000F389
			public int Count
			{
				get
				{
					return this.resolvedMembers.Length;
				}
			}

			// Token: 0x17000287 RID: 647
			// (get) Token: 0x0600063A RID: 1594 RVA: 0x00010393 File Offset: 0x0000F393
			bool ICollection<IMember>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x00010398 File Offset: 0x0000F398
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

			// Token: 0x0600063C RID: 1596 RVA: 0x000103C8 File Offset: 0x0000F3C8
			void IList<IMember>.Insert(int index, IMember item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x000103CF File Offset: 0x0000F3CF
			void IList<IMember>.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x000103D6 File Offset: 0x0000F3D6
			void ICollection<IMember>.Add(IMember item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x000103DD File Offset: 0x0000F3DD
			void ICollection<IMember>.Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x000103E4 File Offset: 0x0000F3E4
			bool ICollection<IMember>.Contains(IMember item)
			{
				return this.IndexOf(item) >= 0;
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x000103F4 File Offset: 0x0000F3F4
			void ICollection<IMember>.CopyTo(IMember[] array, int arrayIndex)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array[arrayIndex + i] = this[i];
				}
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x0001041E File Offset: 0x0000F41E
			bool ICollection<IMember>.Remove(IMember item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x000104D0 File Offset: 0x0000F4D0
			public IEnumerator<IMember> GetEnumerator()
			{
				for (int i = 0; i < this.Count; i++)
				{
					yield return this[i];
				}
				yield break;
			}

			// Token: 0x06000644 RID: 1604 RVA: 0x000104EC File Offset: 0x0000F4EC
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040001C3 RID: 451
			internal readonly ITypeResolveContext[] contextPerMember;

			// Token: 0x040001C4 RID: 452
			internal readonly IUnresolvedMember[] unresolvedMembers;

			// Token: 0x040001C5 RID: 453
			internal readonly IMember[] resolvedMembers;

			// Token: 0x040001C6 RID: 454
			internal readonly int NonPartialMemberCount;
		}

		// Token: 0x020000B5 RID: 181
		private sealed class PartialMethodInfo
		{
			// Token: 0x06000645 RID: 1605 RVA: 0x000104F4 File Offset: 0x0000F4F4
			public PartialMethodInfo(IUnresolvedMethod method, ITypeResolveContext context)
			{
				this.Name = method.Name;
				this.TypeParameterCount = method.TypeParameters.Count;
				this.Parameters = method.Parameters.CreateResolvedParameters(context);
				this.Parts.Add(method);
				this.Contexts.Add(context);
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x00010564 File Offset: 0x0000F564
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

			// Token: 0x06000647 RID: 1607 RVA: 0x000105A1 File Offset: 0x0000F5A1
			public bool IsSameSignature(DefaultResolvedTypeDefinition.PartialMethodInfo other, StringComparer nameComparer)
			{
				return nameComparer.Equals(this.Name, other.Name) && this.TypeParameterCount == other.TypeParameterCount && ParameterListComparer.Instance.Equals(this.Parameters, other.Parameters);
			}

			// Token: 0x040001C7 RID: 455
			public readonly string Name;

			// Token: 0x040001C8 RID: 456
			public readonly int TypeParameterCount;

			// Token: 0x040001C9 RID: 457
			public readonly IList<IParameter> Parameters;

			// Token: 0x040001CA RID: 458
			public readonly List<IUnresolvedMethod> Parts = new List<IUnresolvedMethod>();

			// Token: 0x040001CB RID: 459
			public readonly List<ITypeResolveContext> Contexts = new List<ITypeResolveContext>();
		}
	}
}
