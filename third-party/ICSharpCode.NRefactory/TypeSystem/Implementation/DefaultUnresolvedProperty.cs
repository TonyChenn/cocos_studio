using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedProperty" />.
	/// </summary>
	// Token: 0x020000C8 RID: 200
	[Serializable]
	public class DefaultUnresolvedProperty : AbstractUnresolvedMember, IUnresolvedProperty, IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x00012A24 File Offset: 0x00011A24
		protected override void FreezeInternal()
		{
			this.parameters = FreezableHelper.FreezeListAndElements<IUnresolvedParameter>(this.parameters);
			FreezableHelper.Freeze(this.getter);
			FreezableHelper.Freeze(this.setter);
			base.FreezeInternal();
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00012A54 File Offset: 0x00011A54
		public override object Clone()
		{
			DefaultUnresolvedProperty defaultUnresolvedProperty = (DefaultUnresolvedProperty)base.Clone();
			if (this.parameters != null)
			{
				defaultUnresolvedProperty.parameters = new List<IUnresolvedParameter>(this.parameters);
			}
			return defaultUnresolvedProperty;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00012A87 File Offset: 0x00011A87
		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			this.parameters = provider.InternList<IUnresolvedParameter>(this.parameters);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00012AA2 File Offset: 0x00011AA2
		public DefaultUnresolvedProperty()
		{
			base.SymbolKind = SymbolKind.Property;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00012AB1 File Offset: 0x00011AB1
		public DefaultUnresolvedProperty(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Property;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00012ADD File Offset: 0x00011ADD
		public bool IsIndexer
		{
			get
			{
				return base.SymbolKind == SymbolKind.Indexer;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00012AE8 File Offset: 0x00011AE8
		public IList<IUnresolvedParameter> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new List<IUnresolvedParameter>();
				}
				return this.parameters;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00012B03 File Offset: 0x00011B03
		public bool CanGet
		{
			get
			{
				return this.getter != null;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x00012B11 File Offset: 0x00011B11
		public bool CanSet
		{
			get
			{
				return this.setter != null;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00012B1F File Offset: 0x00011B1F
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x00012B27 File Offset: 0x00011B27
		public IUnresolvedMethod Getter
		{
			get
			{
				return this.getter;
			}
			set
			{
				base.ThrowIfFrozen();
				this.getter = value;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00012B36 File Offset: 0x00011B36
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x00012B3E File Offset: 0x00011B3E
		public IUnresolvedMethod Setter
		{
			get
			{
				return this.setter;
			}
			set
			{
				base.ThrowIfFrozen();
				this.setter = value;
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00012B4D File Offset: 0x00011B4D
		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedProperty(this, context);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00012B60 File Offset: 0x00011B60
		public override IMember Resolve(ITypeResolveContext context)
		{
			ITypeReference explicitInterfaceTypeReference = null;
			if (base.IsExplicitInterfaceImplementation && base.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = base.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, null, (from p in this.Parameters
			select p.Type).ToList<ITypeReference>());
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00012BE3 File Offset: 0x00011BE3
		IProperty IUnresolvedProperty.Resolve(ITypeResolveContext context)
		{
			return (IProperty)this.Resolve(context);
		}

		// Token: 0x0400021A RID: 538
		private IUnresolvedMethod getter;

		// Token: 0x0400021B RID: 539
		private IUnresolvedMethod setter;

		// Token: 0x0400021C RID: 540
		private IList<IUnresolvedParameter> parameters;
	}
}
