using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a SpecializedMember (a member on which type substitution has been performed).
	/// </summary>
	// Token: 0x020000DD RID: 221
	public abstract class SpecializedMember : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x0600081B RID: 2075 RVA: 0x00015868 File Offset: 0x00014868
		protected SpecializedMember(IMember memberDefinition)
		{
			if (memberDefinition == null)
			{
				throw new ArgumentNullException("memberDefinition");
			}
			if (memberDefinition is SpecializedMember)
			{
				throw new ArgumentException("Member definition cannot be specialized. Please use IMember.Specialize() instead of directly constructing SpecializedMember instances.");
			}
			this.baseMember = memberDefinition;
			this.substitution = TypeParameterSubstitution.Identity;
		}

		/// <summary>
		/// Performs a substitution. This method may only be called by constructors in derived classes.
		/// </summary>
		// Token: 0x0600081C RID: 2076 RVA: 0x000158A3 File Offset: 0x000148A3
		protected void AddSubstitution(TypeParameterSubstitution newSubstitution)
		{
			this.substitution = TypeParameterSubstitution.Compose(newSubstitution, this.substitution);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000158B7 File Offset: 0x000148B7
		[Obsolete("Use IMember.Specialize() instead")]
		public static IMember Create(IMember memberDefinition, TypeParameterSubstitution substitution)
		{
			if (memberDefinition == null)
			{
				return null;
			}
			return memberDefinition.Specialize(substitution);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x000158C5 File Offset: 0x000148C5
		public virtual IMemberReference ToMemberReference()
		{
			return this.ToReference();
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000158CD File Offset: 0x000148CD
		public virtual IMemberReference ToReference()
		{
			return new SpecializingMemberReference(this.baseMember.ToReference(), SpecializedMember.ToTypeReference(this.substitution.ClassTypeArguments), null);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x000158F0 File Offset: 0x000148F0
		ISymbolReference ISymbol.ToReference()
		{
			return this.ToReference();
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00015900 File Offset: 0x00014900
		internal static IList<ITypeReference> ToTypeReference(IList<IType> typeArguments)
		{
			if (typeArguments == null)
			{
				return null;
			}
			return (from t in typeArguments
			select t.ToTypeReference()).ToArray<ITypeReference>();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00015930 File Offset: 0x00014930
		internal IMethod WrapAccessor(ref IMethod cachingField, IMethod accessorDefinition)
		{
			if (accessorDefinition == null)
			{
				return null;
			}
			IMethod method = LazyInit.VolatileRead<IMethod>(ref cachingField);
			if (method != null)
			{
				return method;
			}
			IMethod newValue = accessorDefinition.Specialize(this.substitution);
			return LazyInit.GetOrSet<IMethod>(ref cachingField, newValue);
		}

		/// <summary>
		/// Gets the substitution belonging to this specialized member.
		/// </summary>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00015962 File Offset: 0x00014962
		public TypeParameterSubstitution Substitution
		{
			get
			{
				return this.substitution;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001596C File Offset: 0x0001496C
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00015A15 File Offset: 0x00014A15
		public IType DeclaringType
		{
			get
			{
				IType type = LazyInit.VolatileRead<IType>(ref this.declaringType);
				if (type != null)
				{
					return type;
				}
				IType type2 = this.baseMember.DeclaringType;
				ITypeDefinition typeDefinition = type2 as ITypeDefinition;
				if (typeDefinition != null && type2.TypeParameterCount > 0)
				{
					if (this.substitution.ClassTypeArguments != null && this.substitution.ClassTypeArguments.Count == type2.TypeParameterCount)
					{
						type = new ParameterizedType(typeDefinition, this.substitution.ClassTypeArguments);
					}
					else
					{
						type = new ParameterizedType(typeDefinition, typeDefinition.TypeParameters).AcceptVisitor(this.substitution);
					}
				}
				else
				{
					type = type2.AcceptVisitor(this.substitution);
				}
				return LazyInit.GetOrSet<IType>(ref this.declaringType, type);
			}
			internal set
			{
				this.declaringType = value;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00015A1E File Offset: 0x00014A1E
		public IMember MemberDefinition
		{
			get
			{
				return this.baseMember.MemberDefinition;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00015A2B File Offset: 0x00014A2B
		public IUnresolvedMember UnresolvedMember
		{
			get
			{
				return this.baseMember.UnresolvedMember;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00015A38 File Offset: 0x00014A38
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00015A77 File Offset: 0x00014A77
		public IType ReturnType
		{
			get
			{
				IType type = LazyInit.VolatileRead<IType>(ref this.returnType);
				if (type != null)
				{
					return type;
				}
				return LazyInit.GetOrSet<IType>(ref this.returnType, this.baseMember.ReturnType.AcceptVisitor(this.substitution));
			}
			protected set
			{
				this.returnType = value;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00015A80 File Offset: 0x00014A80
		public bool IsVirtual
		{
			get
			{
				return this.baseMember.IsVirtual;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00015A8D File Offset: 0x00014A8D
		public bool IsOverride
		{
			get
			{
				return this.baseMember.IsOverride;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00015A9A File Offset: 0x00014A9A
		public bool IsOverridable
		{
			get
			{
				return this.baseMember.IsOverridable;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00015AA7 File Offset: 0x00014AA7
		public SymbolKind SymbolKind
		{
			get
			{
				return this.baseMember.SymbolKind;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00015AB4 File Offset: 0x00014AB4
		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return this.baseMember.EntityType;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00015AC1 File Offset: 0x00014AC1
		public DomRegion Region
		{
			get
			{
				return this.baseMember.Region;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00015ACE File Offset: 0x00014ACE
		public DomRegion BodyRegion
		{
			get
			{
				return this.baseMember.BodyRegion;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00015ADB File Offset: 0x00014ADB
		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.baseMember.DeclaringTypeDefinition;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00015AE8 File Offset: 0x00014AE8
		public IList<IAttribute> Attributes
		{
			get
			{
				return this.baseMember.Attributes;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00015AF5 File Offset: 0x00014AF5
		public IList<IMember> ImplementedInterfaceMembers
		{
			get
			{
				return LazyInitializer.EnsureInitialized<IList<IMember>>(ref this.implementedInterfaceMembers, new Func<IList<IMember>>(this.FindImplementedInterfaceMembers));
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00015B10 File Offset: 0x00014B10
		private IList<IMember> FindImplementedInterfaceMembers()
		{
			IList<IMember> list = this.baseMember.ImplementedInterfaceMembers;
			IMember[] array = new IMember[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i].Specialize(this.substitution);
			}
			return array;
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00015B59 File Offset: 0x00014B59
		public bool IsExplicitInterfaceImplementation
		{
			get
			{
				return this.baseMember.IsExplicitInterfaceImplementation;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00015B66 File Offset: 0x00014B66
		public DocumentationComment Documentation
		{
			get
			{
				return this.baseMember.Documentation;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00015B73 File Offset: 0x00014B73
		public Accessibility Accessibility
		{
			get
			{
				return this.baseMember.Accessibility;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00015B80 File Offset: 0x00014B80
		public bool IsStatic
		{
			get
			{
				return this.baseMember.IsStatic;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00015B8D File Offset: 0x00014B8D
		public bool IsAbstract
		{
			get
			{
				return this.baseMember.IsAbstract;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00015B9A File Offset: 0x00014B9A
		public bool IsSealed
		{
			get
			{
				return this.baseMember.IsSealed;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00015BA7 File Offset: 0x00014BA7
		public bool IsShadowing
		{
			get
			{
				return this.baseMember.IsShadowing;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00015BB4 File Offset: 0x00014BB4
		public bool IsSynthetic
		{
			get
			{
				return this.baseMember.IsSynthetic;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00015BC1 File Offset: 0x00014BC1
		public bool IsPrivate
		{
			get
			{
				return this.baseMember.IsPrivate;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00015BCE File Offset: 0x00014BCE
		public bool IsPublic
		{
			get
			{
				return this.baseMember.IsPublic;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00015BDB File Offset: 0x00014BDB
		public bool IsProtected
		{
			get
			{
				return this.baseMember.IsProtected;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00015BE8 File Offset: 0x00014BE8
		public bool IsInternal
		{
			get
			{
				return this.baseMember.IsInternal;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00015BF5 File Offset: 0x00014BF5
		public bool IsProtectedOrInternal
		{
			get
			{
				return this.baseMember.IsProtectedOrInternal;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00015C02 File Offset: 0x00014C02
		public bool IsProtectedAndInternal
		{
			get
			{
				return this.baseMember.IsProtectedAndInternal;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00015C0F File Offset: 0x00014C0F
		public string FullName
		{
			get
			{
				return this.baseMember.FullName;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00015C1C File Offset: 0x00014C1C
		public string Name
		{
			get
			{
				return this.baseMember.Name;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x00015C29 File Offset: 0x00014C29
		public string Namespace
		{
			get
			{
				return this.baseMember.Namespace;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x00015C36 File Offset: 0x00014C36
		public string ReflectionName
		{
			get
			{
				return this.baseMember.ReflectionName;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00015C43 File Offset: 0x00014C43
		public ICompilation Compilation
		{
			get
			{
				return this.baseMember.Compilation;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x00015C50 File Offset: 0x00014C50
		public IAssembly ParentAssembly
		{
			get
			{
				return this.baseMember.ParentAssembly;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00015C5D File Offset: 0x00014C5D
		public virtual IMember Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.baseMember.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitution));
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00015C78 File Offset: 0x00014C78
		public override bool Equals(object obj)
		{
			SpecializedMember specializedMember = obj as SpecializedMember;
			return specializedMember != null && this.baseMember.Equals(specializedMember.baseMember) && this.substitution.Equals(specializedMember.substitution);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00015CB7 File Offset: 0x00014CB7
		public override int GetHashCode()
		{
			return 1000000007 * this.baseMember.GetHashCode() + 1000000009 * this.substitution.GetHashCode();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00015CDC File Offset: 0x00014CDC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.DeclaringType.ToString());
			stringBuilder.Append('.');
			stringBuilder.Append(this.Name);
			stringBuilder.Append(':');
			stringBuilder.Append(this.ReturnType.ToString());
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x04000259 RID: 601
		protected readonly IMember baseMember;

		// Token: 0x0400025A RID: 602
		private TypeParameterSubstitution substitution;

		// Token: 0x0400025B RID: 603
		private IType declaringType;

		// Token: 0x0400025C RID: 604
		private IType returnType;

		// Token: 0x0400025D RID: 605
		private IList<IMember> implementedInterfaceMembers;
	}
}
