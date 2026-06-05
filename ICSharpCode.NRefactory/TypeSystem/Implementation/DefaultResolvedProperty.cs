using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x02000062 RID: 98
	public class DefaultResolvedProperty : AbstractResolvedMember, IProperty, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x0600030B RID: 779 RVA: 0x00007B6A File Offset: 0x00006B6A
		public DefaultResolvedProperty(IUnresolvedProperty unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
			this.parameters = unresolved.Parameters.CreateResolvedParameters(this.context);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00007B92 File Offset: 0x00006B92
		public IList<IParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00007B9A File Offset: 0x00006B9A
		public bool CanGet
		{
			get
			{
				return this.unresolved.CanGet;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00007BA7 File Offset: 0x00006BA7
		public bool CanSet
		{
			get
			{
				return this.unresolved.CanSet;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00007BB4 File Offset: 0x00006BB4
		public IMethod Getter
		{
			get
			{
				return base.GetAccessor(ref this.getter, this.unresolved.Getter);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00007BCD File Offset: 0x00006BCD
		public IMethod Setter
		{
			get
			{
				return base.GetAccessor(ref this.setter, this.unresolved.Setter);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00007BE6 File Offset: 0x00006BE6
		public bool IsIndexer
		{
			get
			{
				return this.unresolved.IsIndexer;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00007C00 File Offset: 0x00006C00
		public override ISymbolReference ToReference()
		{
			ITypeReference typeReference = this.DeclaringType.ToTypeReference();
			if (base.IsExplicitInterfaceImplementation && base.ImplementedInterfaceMembers.Count == 1)
			{
				return new ExplicitInterfaceImplementationMemberReference(typeReference, base.ImplementedInterfaceMembers[0].ToReference());
			}
			return new DefaultMemberReference(base.SymbolKind, typeReference, base.Name, 0, (from p in this.Parameters
			select p.Type.ToTypeReference()).ToList<ITypeReference>());
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00007C87 File Offset: 0x00006C87
		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedProperty(this, substitution);
		}

		// Token: 0x040000C8 RID: 200
		protected new readonly IUnresolvedProperty unresolved;

		// Token: 0x040000C9 RID: 201
		private readonly IList<IParameter> parameters;

		// Token: 0x040000CA RID: 202
		private IMethod getter;

		// Token: 0x040000CB RID: 203
		private IMethod setter;
	}
}
