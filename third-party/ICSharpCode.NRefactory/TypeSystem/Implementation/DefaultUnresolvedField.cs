using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedField" />.
	/// </summary>
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public class DefaultUnresolvedField : AbstractUnresolvedMember, IUnresolvedField, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x00011C71 File Offset: 0x00010C71
		protected override void FreezeInternal()
		{
			FreezableHelper.Freeze(this.constantValue);
			base.FreezeInternal();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00011C84 File Offset: 0x00010C84
		public DefaultUnresolvedField()
		{
			base.SymbolKind = SymbolKind.Field;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00011C93 File Offset: 0x00010C93
		public DefaultUnresolvedField(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Field;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00011CBF File Offset: 0x00010CBF
		public bool IsConst
		{
			get
			{
				return this.constantValue != null && !this.IsFixed;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00011CD4 File Offset: 0x00010CD4
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00011CE6 File Offset: 0x00010CE6
		public bool IsReadOnly
		{
			get
			{
				return this.flags[4096];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[4096] = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00011CFF File Offset: 0x00010CFF
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x00011D11 File Offset: 0x00010D11
		public bool IsVolatile
		{
			get
			{
				return this.flags[8192];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[8192] = value;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00011D2A File Offset: 0x00010D2A
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x00011D3C File Offset: 0x00010D3C
		public bool IsFixed
		{
			get
			{
				return this.flags[16384];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[16384] = value;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00011D55 File Offset: 0x00010D55
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x00011D5D File Offset: 0x00010D5D
		public IConstantValue ConstantValue
		{
			get
			{
				return this.constantValue;
			}
			set
			{
				base.ThrowIfFrozen();
				this.constantValue = value;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00011D6C File Offset: 0x00010D6C
		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedField(this, context);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00011D75 File Offset: 0x00010D75
		IField IUnresolvedField.Resolve(ITypeResolveContext context)
		{
			return (IField)this.Resolve(context);
		}

		// Token: 0x04000201 RID: 513
		private IConstantValue constantValue;
	}
}
