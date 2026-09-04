using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation for IType interface.
	/// </summary>
	// Token: 0x02000058 RID: 88
	[Serializable]
	public abstract class AbstractType : IType, INamedElement, IEquatable<IType>
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000286 RID: 646 RVA: 0x00006E2C File Offset: 0x00005E2C
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000287 RID: 647
		public abstract string Name { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00006E5D File Offset: 0x00005E5D
		public virtual string Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00006E64 File Offset: 0x00005E64
		public virtual string ReflectionName
		{
			get
			{
				return this.FullName;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600028A RID: 650
		public abstract bool? IsReferenceType { get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600028B RID: 651
		public abstract TypeKind Kind { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00006E6C File Offset: 0x00005E6C
		public virtual int TypeParameterCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00006E6F File Offset: 0x00005E6F
		public virtual IList<IType> TypeArguments
		{
			get
			{
				return AbstractType.emptyTypeArguments;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00006E76 File Offset: 0x00005E76
		public virtual IType DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00006E79 File Offset: 0x00005E79
		public virtual bool IsParameterized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00006E7C File Offset: 0x00005E7C
		public virtual ITypeDefinition GetDefinition()
		{
			return null;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00006E7F File Offset: 0x00005E7F
		public virtual IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				return EmptyList<IType>.Instance;
			}
		}

		// Token: 0x06000292 RID: 658
		public abstract ITypeReference ToTypeReference();

		// Token: 0x06000293 RID: 659 RVA: 0x00006E86 File Offset: 0x00005E86
		public virtual IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IType>.Instance;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00006E8D File Offset: 0x00005E8D
		public virtual IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IType>.Instance;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00006E94 File Offset: 0x00005E94
		public virtual IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00006E9B File Offset: 0x00005E9B
		public virtual IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00006EA2 File Offset: 0x00005EA2
		public virtual IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00006EA9 File Offset: 0x00005EA9
		public virtual IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IProperty>.Instance;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00006EB0 File Offset: 0x00005EB0
		public virtual IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IField>.Instance;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00006EB7 File Offset: 0x00005EB7
		public virtual IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IEvent>.Instance;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00006EC0 File Offset: 0x00005EC0
		public virtual IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			IEnumerable<IMember> methods = this.GetMethods(filter, options);
			return methods.Concat(this.GetProperties(filter, options)).Concat(this.GetFields(filter, options)).Concat(this.GetEvents(filter, options));
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00006EFE File Offset: 0x00005EFE
		public virtual IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00006F05 File Offset: 0x00005F05
		public TypeParameterSubstitution GetSubstitution()
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00006F0C File Offset: 0x00005F0C
		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return TypeParameterSubstitution.Identity;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00006F13 File Offset: 0x00005F13
		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as IType);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00006F21 File Offset: 0x00005F21
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00006F29 File Offset: 0x00005F29
		public virtual bool Equals(IType other)
		{
			return this == other;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00006F2F File Offset: 0x00005F2F
		public override string ToString()
		{
			return this.ReflectionName;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00006F37 File Offset: 0x00005F37
		public virtual IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitOtherType(this);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00006F40 File Offset: 0x00005F40
		public virtual IType VisitChildren(TypeVisitor visitor)
		{
			return this;
		}

		// Token: 0x040000BC RID: 188
		private static readonly IList<IType> emptyTypeArguments = new IType[0];
	}
}
