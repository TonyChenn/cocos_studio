using System;
using System.Collections.Generic;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedTypeParameter" />.
	/// </summary>
	// Token: 0x020000CC RID: 204
	[Serializable]
	public class DefaultUnresolvedTypeParameter : IUnresolvedTypeParameter, INamedElement, IFreezable
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00013052 File Offset: 0x00012052
		public void Freeze()
		{
			if (!this.flags[1])
			{
				this.FreezeInternal();
				this.flags[1] = true;
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00013075 File Offset: 0x00012075
		protected virtual void FreezeInternal()
		{
			this.attributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.attributes);
			this.constraints = FreezableHelper.FreezeList<ITypeReference>(this.constraints);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001309C File Offset: 0x0001209C
		public DefaultUnresolvedTypeParameter(SymbolKind ownerType, int index, string name = null)
		{
			this.ownerType = ownerType;
			this.index = index;
			this.name = (name ?? (((ownerType == SymbolKind.Method) ? "!!" : "!") + index.ToString(CultureInfo.InvariantCulture)));
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000130E9 File Offset: 0x000120E9
		public SymbolKind OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000130F1 File Offset: 0x000120F1
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x000130F9 File Offset: 0x000120F9
		public bool IsFrozen
		{
			get
			{
				return this.flags[1];
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00013107 File Offset: 0x00012107
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x0001310F File Offset: 0x0001210F
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.name = value;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001311E File Offset: 0x0001211E
		string INamedElement.FullName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00013126 File Offset: 0x00012126
		string INamedElement.Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00013130 File Offset: 0x00012130
		string INamedElement.ReflectionName
		{
			get
			{
				if (this.ownerType == SymbolKind.Method)
				{
					return "``" + this.index.ToString(CultureInfo.InvariantCulture);
				}
				return "`" + this.index.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00013181 File Offset: 0x00012181
		public IList<IUnresolvedAttribute> Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new List<IUnresolvedAttribute>();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0001319C File Offset: 0x0001219C
		public IList<ITypeReference> Constraints
		{
			get
			{
				if (this.constraints == null)
				{
					this.constraints = new List<ITypeReference>();
				}
				return this.constraints;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000131B7 File Offset: 0x000121B7
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x000131BF File Offset: 0x000121BF
		public VarianceModifier Variance
		{
			get
			{
				return this.variance;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.variance = value;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000131CE File Offset: 0x000121CE
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x000131D6 File Offset: 0x000121D6
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.region = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x000131E5 File Offset: 0x000121E5
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x000131F3 File Offset: 0x000121F3
		public bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.flags[8];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[8] = value;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00013208 File Offset: 0x00012208
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x00013216 File Offset: 0x00012216
		public bool HasReferenceTypeConstraint
		{
			get
			{
				return this.flags[2];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[2] = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0001322B File Offset: 0x0001222B
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x00013239 File Offset: 0x00012239
		public bool HasValueTypeConstraint
		{
			get
			{
				return this.flags[4];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[4] = value;
			}
		}

		/// <summary>
		/// Uses the specified interning provider to intern
		/// strings and lists in this entity.
		/// This method does not test arbitrary objects to see if they implement ISupportsInterning;
		/// instead we assume that those are interned immediately when they are created (before they are added to this entity).
		/// </summary>
		// Token: 0x0600078D RID: 1933 RVA: 0x00013250 File Offset: 0x00012250
		public virtual void ApplyInterningProvider(InterningProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			FreezableHelper.ThrowIfFrozen(this);
			this.name = provider.Intern(this.name);
			this.attributes = provider.InternList<IUnresolvedAttribute>(this.attributes);
			this.constraints = provider.InternList<ITypeReference>(this.constraints);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000132A8 File Offset: 0x000122A8
		public virtual ITypeParameter CreateResolvedTypeParameter(ITypeResolveContext context)
		{
			IEntity entity = null;
			if (this.OwnerType == SymbolKind.Method)
			{
				entity = (context.CurrentMember as IMethod);
			}
			else if (this.OwnerType == SymbolKind.TypeDefinition)
			{
				entity = context.CurrentTypeDefinition;
			}
			if (entity == null)
			{
				throw new InvalidOperationException("Could not determine the type parameter's owner.");
			}
			return new DefaultTypeParameter(entity, this.index, this.name, this.variance, this.Attributes.CreateResolvedAttributes(context), this.Region, this.HasValueTypeConstraint, this.HasReferenceTypeConstraint, this.HasDefaultConstructorConstraint, this.Constraints.Resolve(context));
		}

		// Token: 0x04000224 RID: 548
		private const ushort FlagFrozen = 1;

		// Token: 0x04000225 RID: 549
		private const ushort FlagReferenceTypeConstraint = 2;

		// Token: 0x04000226 RID: 550
		private const ushort FlagValueTypeConstraint = 4;

		// Token: 0x04000227 RID: 551
		private const ushort FlagDefaultConstructorConstraint = 8;

		// Token: 0x04000228 RID: 552
		private readonly int index;

		// Token: 0x04000229 RID: 553
		private IList<IUnresolvedAttribute> attributes;

		// Token: 0x0400022A RID: 554
		private IList<ITypeReference> constraints;

		// Token: 0x0400022B RID: 555
		private string name;

		// Token: 0x0400022C RID: 556
		private DomRegion region;

		// Token: 0x0400022D RID: 557
		private SymbolKind ownerType;

		// Token: 0x0400022E RID: 558
		private VarianceModifier variance;

		// Token: 0x0400022F RID: 559
		private BitVector16 flags;
	}
}
