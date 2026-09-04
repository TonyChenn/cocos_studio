using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000B0 RID: 176
	public class DefaultResolvedEvent : AbstractResolvedMember, IEvent, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0000DC56 File Offset: 0x0000CC56
		public DefaultResolvedEvent(IUnresolvedEvent unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0000DC67 File Offset: 0x0000CC67
		public bool CanAdd
		{
			get
			{
				return this.unresolved.CanAdd;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0000DC74 File Offset: 0x0000CC74
		public bool CanRemove
		{
			get
			{
				return this.unresolved.CanRemove;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0000DC81 File Offset: 0x0000CC81
		public bool CanInvoke
		{
			get
			{
				return this.unresolved.CanInvoke;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0000DC8E File Offset: 0x0000CC8E
		public IMethod AddAccessor
		{
			get
			{
				return base.GetAccessor(ref this.addAccessor, this.unresolved.AddAccessor);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0000DCA7 File Offset: 0x0000CCA7
		public IMethod RemoveAccessor
		{
			get
			{
				return base.GetAccessor(ref this.removeAccessor, this.unresolved.RemoveAccessor);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0000DCC0 File Offset: 0x0000CCC0
		public IMethod InvokeAccessor
		{
			get
			{
				return base.GetAccessor(ref this.invokeAccessor, this.unresolved.InvokeAccessor);
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000DCD9 File Offset: 0x0000CCD9
		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedEvent(this, substitution);
		}

		// Token: 0x040001A5 RID: 421
		protected new readonly IUnresolvedEvent unresolved;

		// Token: 0x040001A6 RID: 422
		private IMethod addAccessor;

		// Token: 0x040001A7 RID: 423
		private IMethod removeAccessor;

		// Token: 0x040001A8 RID: 424
		private IMethod invokeAccessor;
	}
}
