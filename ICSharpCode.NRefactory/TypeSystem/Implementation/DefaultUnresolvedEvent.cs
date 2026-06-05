using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedEvent" />.
	/// </summary>
	// Token: 0x020000C1 RID: 193
	[Serializable]
	public class DefaultUnresolvedEvent : AbstractUnresolvedMember, IUnresolvedEvent, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x00011B87 File Offset: 0x00010B87
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			FreezableHelper.Freeze(this.addAccessor);
			FreezableHelper.Freeze(this.removeAccessor);
			FreezableHelper.Freeze(this.invokeAccessor);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00011BB0 File Offset: 0x00010BB0
		public DefaultUnresolvedEvent()
		{
			base.SymbolKind = SymbolKind.Event;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00011BBF File Offset: 0x00010BBF
		public DefaultUnresolvedEvent(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Event;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00011BEB File Offset: 0x00010BEB
		public bool CanAdd
		{
			get
			{
				return this.addAccessor != null;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00011BF9 File Offset: 0x00010BF9
		public bool CanRemove
		{
			get
			{
				return this.removeAccessor != null;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00011C07 File Offset: 0x00010C07
		public bool CanInvoke
		{
			get
			{
				return this.invokeAccessor != null;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00011C15 File Offset: 0x00010C15
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00011C1D File Offset: 0x00010C1D
		public IUnresolvedMethod AddAccessor
		{
			get
			{
				return this.addAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.addAccessor = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00011C2C File Offset: 0x00010C2C
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00011C34 File Offset: 0x00010C34
		public IUnresolvedMethod RemoveAccessor
		{
			get
			{
				return this.removeAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.removeAccessor = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00011C43 File Offset: 0x00010C43
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00011C4B File Offset: 0x00010C4B
		public IUnresolvedMethod InvokeAccessor
		{
			get
			{
				return this.invokeAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.invokeAccessor = value;
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00011C5A File Offset: 0x00010C5A
		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedEvent(this, context);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00011C63 File Offset: 0x00010C63
		IEvent IUnresolvedEvent.Resolve(ITypeResolveContext context)
		{
			return (IEvent)this.Resolve(context);
		}

		// Token: 0x040001FE RID: 510
		private IUnresolvedMethod addAccessor;

		// Token: 0x040001FF RID: 511
		private IUnresolvedMethod removeAccessor;

		// Token: 0x04000200 RID: 512
		private IUnresolvedMethod invokeAccessor;
	}
}
