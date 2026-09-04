using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IMember" /> that resolves an unresolved member.
	/// </summary>
	// Token: 0x0200005F RID: 95
	public abstract class AbstractResolvedMember : AbstractResolvedEntity, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x000077EE File Offset: 0x000067EE
		protected AbstractResolvedMember(IUnresolvedMember unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
			this.context = parentContext.WithCurrentMember(this);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000780C File Offset: 0x0000680C
		IMember IMember.MemberDefinition
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00007810 File Offset: 0x00006810
		public IType ReturnType
		{
			get
			{
				IType result;
				if ((result = this.returnType) == null)
				{
					result = (this.returnType = this.unresolved.ReturnType.Resolve(this.context));
				}
				return result;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000784A File Offset: 0x0000684A
		public IUnresolvedMember UnresolvedMember
		{
			get
			{
				return this.unresolved;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00007854 File Offset: 0x00006854
		public IList<IMember> ImplementedInterfaceMembers
		{
			get
			{
				IList<IMember> list = LazyInit.VolatileRead<IList<IMember>>(ref this.implementedInterfaceMembers);
				if (list != null)
				{
					return list;
				}
				return LazyInit.GetOrSet<IList<IMember>>(ref this.implementedInterfaceMembers, this.FindImplementedInterfaceMembers());
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00007908 File Offset: 0x00006908
		private IList<IMember> FindImplementedInterfaceMembers()
		{
			if (this.unresolved.IsExplicitInterfaceImplementation)
			{
				List<IMember> list = new List<IMember>();
				foreach (IMemberReference memberReference in this.unresolved.ExplicitInterfaceImplementations)
				{
					IMember member = memberReference.Resolve(this.context);
					if (member != null)
					{
						list.Add(member);
					}
				}
				return list.ToArray();
			}
			if (this.unresolved.IsStatic || !this.unresolved.IsPublic || base.DeclaringTypeDefinition == null || base.DeclaringTypeDefinition.Kind == TypeKind.Interface)
			{
				return EmptyList<IMember>.Instance;
			}
			IMember[] source = (from m in InheritanceHelper.GetBaseMembers(this, true)
			where m.DeclaringTypeDefinition != null && m.DeclaringTypeDefinition.Kind == TypeKind.Interface
			select m).ToArray<IMember>();
			IEnumerable<IMember> otherMembers = base.DeclaringTypeDefinition.Members;
			if (base.SymbolKind == SymbolKind.Accessor)
			{
				otherMembers = base.DeclaringTypeDefinition.GetAccessors(null, GetMemberOptions.IgnoreInheritedMembers);
			}
			return (from item in source
			where !otherMembers.Any((IMember m) => m.IsExplicitInterfaceImplementation && m.ImplementedInterfaceMembers.Contains(item))
			select item).ToArray<IMember>();
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00007A40 File Offset: 0x00006A40
		public override DocumentationComment Documentation
		{
			get
			{
				IUnresolvedDocumentationProvider unresolvedDocumentationProvider = this.unresolved.UnresolvedFile as IUnresolvedDocumentationProvider;
				if (unresolvedDocumentationProvider != null)
				{
					DocumentationComment documentation = unresolvedDocumentationProvider.GetDocumentation(this.unresolved, this);
					if (documentation != null)
					{
						return documentation;
					}
				}
				return base.Documentation;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00007A7A File Offset: 0x00006A7A
		public bool IsExplicitInterfaceImplementation
		{
			get
			{
				return this.unresolved.IsExplicitInterfaceImplementation;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00007A87 File Offset: 0x00006A87
		public bool IsVirtual
		{
			get
			{
				return this.unresolved.IsVirtual;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00007A94 File Offset: 0x00006A94
		public bool IsOverride
		{
			get
			{
				return this.unresolved.IsOverride;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00007AA1 File Offset: 0x00006AA1
		public bool IsOverridable
		{
			get
			{
				return this.unresolved.IsOverridable;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00007AAE File Offset: 0x00006AAE
		public TypeParameterSubstitution Substitution
		{
			get
			{
				return TypeParameterSubstitution.Identity;
			}
		}

		// Token: 0x060002FE RID: 766
		public abstract IMember Specialize(TypeParameterSubstitution substitution);

		// Token: 0x060002FF RID: 767 RVA: 0x00007AB5 File Offset: 0x00006AB5
		IMemberReference IMember.ToReference()
		{
			return (IMemberReference)this.ToReference();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00007AC4 File Offset: 0x00006AC4
		public override ISymbolReference ToReference()
		{
			ITypeReference typeReference = this.DeclaringType.ToTypeReference();
			if (this.IsExplicitInterfaceImplementation && this.ImplementedInterfaceMembers.Count == 1)
			{
				return new ExplicitInterfaceImplementationMemberReference(typeReference, this.ImplementedInterfaceMembers[0].ToReference());
			}
			return new DefaultMemberReference(base.SymbolKind, typeReference, base.Name, 0, null);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00007B1F File Offset: 0x00006B1F
		public virtual IMemberReference ToMemberReference()
		{
			return (IMemberReference)this.ToReference();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00007B2C File Offset: 0x00006B2C
		internal IMethod GetAccessor(ref IMethod accessorField, IUnresolvedMethod unresolvedAccessor)
		{
			if (unresolvedAccessor == null)
			{
				return null;
			}
			IMethod method = LazyInit.VolatileRead<IMethod>(ref accessorField);
			if (method != null)
			{
				return method;
			}
			return LazyInit.GetOrSet<IMethod>(ref accessorField, this.CreateResolvedAccessor(unresolvedAccessor));
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00007B57 File Offset: 0x00006B57
		protected virtual IMethod CreateResolvedAccessor(IUnresolvedMethod unresolvedAccessor)
		{
			return (IMethod)unresolvedAccessor.CreateResolved(this.context);
		}

		// Token: 0x040000C3 RID: 195
		protected new readonly IUnresolvedMember unresolved;

		// Token: 0x040000C4 RID: 196
		protected readonly ITypeResolveContext context;

		// Token: 0x040000C5 RID: 197
		private volatile IType returnType;

		// Token: 0x040000C6 RID: 198
		private IList<IMember> implementedInterfaceMembers;
	}
}
