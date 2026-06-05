using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation for IUnresolvedParameter.
	/// </summary>
	// Token: 0x020000C5 RID: 197
	[Serializable]
	public sealed class DefaultUnresolvedParameter : IUnresolvedParameter, IFreezable, ISupportsInterning
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x000123D7 File Offset: 0x000113D7
		public DefaultUnresolvedParameter()
		{
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000123F8 File Offset: 0x000113F8
		public DefaultUnresolvedParameter(ITypeReference type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.type = type;
			this.name = name;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001244B File Offset: 0x0001144B
		private void FreezeInternal()
		{
			this.attributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.attributes);
			FreezableHelper.Freeze(this.defaultValue);
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00012469 File Offset: 0x00011469
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x00012471 File Offset: 0x00011471
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.name = value;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001248E File Offset: 0x0001148E
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00012496 File Offset: 0x00011496
		public ITypeReference Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.type = value;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000124B3 File Offset: 0x000114B3
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

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000124CE File Offset: 0x000114CE
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x000124D6 File Offset: 0x000114D6
		public IConstantValue DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.defaultValue = value;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x000124E5 File Offset: 0x000114E5
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x000124ED File Offset: 0x000114ED
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

		// Token: 0x06000706 RID: 1798 RVA: 0x000124FC File Offset: 0x000114FC
		private bool HasFlag(byte flag)
		{
			return (this.flags & flag) != 0;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001250C File Offset: 0x0001150C
		private void SetFlag(byte flag, bool value)
		{
			FreezableHelper.ThrowIfFrozen(this);
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00012538 File Offset: 0x00011538
		public bool IsFrozen
		{
			get
			{
				return this.HasFlag(1);
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00012541 File Offset: 0x00011541
		public void Freeze()
		{
			if (!this.IsFrozen)
			{
				this.FreezeInternal();
				this.flags |= 1;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00012560 File Offset: 0x00011560
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x00012569 File Offset: 0x00011569
		public bool IsRef
		{
			get
			{
				return this.HasFlag(2);
			}
			set
			{
				this.SetFlag(2, value);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00012573 File Offset: 0x00011573
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x0001257C File Offset: 0x0001157C
		public bool IsOut
		{
			get
			{
				return this.HasFlag(4);
			}
			set
			{
				this.SetFlag(4, value);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00012586 File Offset: 0x00011586
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0001258F File Offset: 0x0001158F
		public bool IsParams
		{
			get
			{
				return this.HasFlag(8);
			}
			set
			{
				this.SetFlag(8, value);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00012599 File Offset: 0x00011599
		public bool IsOptional
		{
			get
			{
				return this.DefaultValue != null;
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000125A8 File Offset: 0x000115A8
		int ISupportsInterning.GetHashCodeForInterning()
		{
			int num = 1919191 ^ ((int)this.flags & -2);
			num *= 31;
			num += this.type.GetHashCode();
			num *= 31;
			num += this.name.GetHashCode();
			if (this.attributes != null)
			{
				foreach (IUnresolvedAttribute unresolvedAttribute in this.attributes)
				{
					num ^= unresolvedAttribute.GetHashCode();
				}
			}
			if (this.defaultValue != null)
			{
				num ^= this.defaultValue.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001264C File Offset: 0x0001164C
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultUnresolvedParameter defaultUnresolvedParameter = other as DefaultUnresolvedParameter;
			return defaultUnresolvedParameter != null && this.type == defaultUnresolvedParameter.type && this.name == defaultUnresolvedParameter.name && this.defaultValue == defaultUnresolvedParameter.defaultValue && this.region == defaultUnresolvedParameter.region && ((int)this.flags & -2) == ((int)defaultUnresolvedParameter.flags & -2) && DefaultUnresolvedParameter.ListEquals(this.attributes, defaultUnresolvedParameter.attributes);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000126CC File Offset: 0x000116CC
		private static bool ListEquals(IList<IUnresolvedAttribute> list1, IList<IUnresolvedAttribute> list2)
		{
			return (list1 ?? EmptyList<IUnresolvedAttribute>.Instance).SequenceEqual(list2 ?? EmptyList<IUnresolvedAttribute>.Instance);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000126E8 File Offset: 0x000116E8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsRef)
			{
				stringBuilder.Append("ref ");
			}
			if (this.IsOut)
			{
				stringBuilder.Append("out ");
			}
			if (this.IsParams)
			{
				stringBuilder.Append("params ");
			}
			stringBuilder.Append(this.name);
			stringBuilder.Append(':');
			stringBuilder.Append(this.type.ToString());
			if (this.defaultValue != null)
			{
				stringBuilder.Append(" = ");
				stringBuilder.Append(this.defaultValue.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001278B File Offset: 0x0001178B
		private static bool IsOptionalAttribute(IType attributeType)
		{
			return attributeType.Name == "OptionalAttribute" && attributeType.Namespace == "System.Runtime.InteropServices";
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000127C0 File Offset: 0x000117C0
		public IParameter CreateResolvedParameter(ITypeResolveContext context)
		{
			this.Freeze();
			if (this.defaultValue != null)
			{
				return new DefaultUnresolvedParameter.ResolvedParameterWithDefaultValue(this.defaultValue, context)
				{
					Type = this.type.Resolve(context),
					Name = this.name,
					Region = this.region,
					Attributes = this.attributes.CreateResolvedAttributes(context),
					IsRef = this.IsRef,
					IsOut = this.IsOut,
					IsParams = this.IsParams
				};
			}
			IParameterizedMember owner = context.CurrentMember as IParameterizedMember;
			IList<IAttribute> list = this.attributes.CreateResolvedAttributes(context);
			bool flag;
			if (list != null)
			{
				flag = list.Any((IAttribute a) => DefaultUnresolvedParameter.IsOptionalAttribute(a.AttributeType));
			}
			else
			{
				flag = false;
			}
			bool isOptional = flag;
			return new DefaultParameter(this.type.Resolve(context), this.name, owner, this.region, list, this.IsRef, this.IsOut, this.IsParams, isOptional, null);
		}

		// Token: 0x04000209 RID: 521
		private string name = string.Empty;

		// Token: 0x0400020A RID: 522
		private ITypeReference type = SpecialType.UnknownType;

		// Token: 0x0400020B RID: 523
		private IList<IUnresolvedAttribute> attributes;

		// Token: 0x0400020C RID: 524
		private IConstantValue defaultValue;

		// Token: 0x0400020D RID: 525
		private DomRegion region;

		// Token: 0x0400020E RID: 526
		private byte flags;

		// Token: 0x020000C6 RID: 198
		private sealed class ResolvedParameterWithDefaultValue : IParameter, IVariable, ISymbol
		{
			// Token: 0x06000718 RID: 1816 RVA: 0x000128C0 File Offset: 0x000118C0
			public ResolvedParameterWithDefaultValue(IConstantValue defaultValue, ITypeResolveContext context)
			{
				this.defaultValue = defaultValue;
				this.context = context;
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000719 RID: 1817 RVA: 0x000128D6 File Offset: 0x000118D6
			SymbolKind ISymbol.SymbolKind
			{
				get
				{
					return SymbolKind.Parameter;
				}
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x000128DA File Offset: 0x000118DA
			public IParameterizedMember Owner
			{
				get
				{
					return this.context.CurrentMember as IParameterizedMember;
				}
			}

			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x0600071B RID: 1819 RVA: 0x000128EC File Offset: 0x000118EC
			// (set) Token: 0x0600071C RID: 1820 RVA: 0x000128F4 File Offset: 0x000118F4
			public IType Type { get; internal set; }

			// Token: 0x170002E6 RID: 742
			// (get) Token: 0x0600071D RID: 1821 RVA: 0x000128FD File Offset: 0x000118FD
			// (set) Token: 0x0600071E RID: 1822 RVA: 0x00012905 File Offset: 0x00011905
			public string Name { get; internal set; }

			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001290E File Offset: 0x0001190E
			// (set) Token: 0x06000720 RID: 1824 RVA: 0x00012916 File Offset: 0x00011916
			public DomRegion Region { get; internal set; }

			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001291F File Offset: 0x0001191F
			// (set) Token: 0x06000722 RID: 1826 RVA: 0x00012927 File Offset: 0x00011927
			public IList<IAttribute> Attributes { get; internal set; }

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000723 RID: 1827 RVA: 0x00012930 File Offset: 0x00011930
			// (set) Token: 0x06000724 RID: 1828 RVA: 0x00012938 File Offset: 0x00011938
			public bool IsRef { get; internal set; }

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000725 RID: 1829 RVA: 0x00012941 File Offset: 0x00011941
			// (set) Token: 0x06000726 RID: 1830 RVA: 0x00012949 File Offset: 0x00011949
			public bool IsOut { get; internal set; }

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000727 RID: 1831 RVA: 0x00012952 File Offset: 0x00011952
			// (set) Token: 0x06000728 RID: 1832 RVA: 0x0001295A File Offset: 0x0001195A
			public bool IsParams { get; internal set; }

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000729 RID: 1833 RVA: 0x00012963 File Offset: 0x00011963
			public bool IsOptional
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x0600072A RID: 1834 RVA: 0x00012966 File Offset: 0x00011966
			bool IVariable.IsConst
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001296C File Offset: 0x0001196C
			public object ConstantValue
			{
				get
				{
					ResolveResult resolveResult = LazyInit.VolatileRead<ResolveResult>(ref this.resolvedDefaultValue);
					if (resolveResult == null)
					{
						resolveResult = this.defaultValue.Resolve(this.context);
						LazyInit.GetOrSet<ResolveResult>(ref this.resolvedDefaultValue, resolveResult);
					}
					return resolveResult.ConstantValue;
				}
			}

			// Token: 0x0600072C RID: 1836 RVA: 0x000129AD File Offset: 0x000119AD
			public override string ToString()
			{
				return DefaultParameter.ToString(this);
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x000129B8 File Offset: 0x000119B8
			public ISymbolReference ToReference()
			{
				if (this.Owner == null)
				{
					return new ParameterReference(this.Type.ToTypeReference(), this.Name, this.Region, this.IsRef, this.IsOut, this.IsParams, true, this.ConstantValue);
				}
				return new OwnedParameterReference(this.Owner.ToReference(), this.Owner.Parameters.IndexOf(this));
			}

			// Token: 0x04000210 RID: 528
			private readonly IConstantValue defaultValue;

			// Token: 0x04000211 RID: 529
			private readonly ITypeResolveContext context;

			// Token: 0x04000212 RID: 530
			private ResolveResult resolvedDefaultValue;
		}
	}
}
