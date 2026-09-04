using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMethod" /> interface.
	/// </summary>
	// Token: 0x020000C3 RID: 195
	[Serializable]
	public class DefaultUnresolvedMethod : AbstractUnresolvedMember, IUnresolvedMethod, IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x060006CE RID: 1742 RVA: 0x00011D83 File Offset: 0x00010D83
		protected override void FreezeInternal()
		{
			this.returnTypeAttributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.returnTypeAttributes);
			this.typeParameters = FreezableHelper.FreezeListAndElements<IUnresolvedTypeParameter>(this.typeParameters);
			this.parameters = FreezableHelper.FreezeListAndElements<IUnresolvedParameter>(this.parameters);
			base.FreezeInternal();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00011DC0 File Offset: 0x00010DC0
		public override object Clone()
		{
			DefaultUnresolvedMethod defaultUnresolvedMethod = (DefaultUnresolvedMethod)base.Clone();
			if (this.returnTypeAttributes != null)
			{
				defaultUnresolvedMethod.returnTypeAttributes = new List<IUnresolvedAttribute>(this.returnTypeAttributes);
			}
			if (this.typeParameters != null)
			{
				defaultUnresolvedMethod.typeParameters = new List<IUnresolvedTypeParameter>(this.typeParameters);
			}
			if (this.parameters != null)
			{
				defaultUnresolvedMethod.parameters = new List<IUnresolvedParameter>(this.parameters);
			}
			return defaultUnresolvedMethod;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00011E28 File Offset: 0x00010E28
		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			if (provider != null)
			{
				this.returnTypeAttributes = provider.InternList<IUnresolvedAttribute>(this.returnTypeAttributes);
				this.typeParameters = provider.InternList<IUnresolvedTypeParameter>(this.typeParameters);
				this.parameters = provider.InternList<IUnresolvedParameter>(this.parameters);
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00011E75 File Offset: 0x00010E75
		public DefaultUnresolvedMethod()
		{
			base.SymbolKind = SymbolKind.Method;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00011E84 File Offset: 0x00010E84
		public DefaultUnresolvedMethod(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Method;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00011EB0 File Offset: 0x00010EB0
		public IList<IUnresolvedAttribute> ReturnTypeAttributes
		{
			get
			{
				if (this.returnTypeAttributes == null)
				{
					this.returnTypeAttributes = new List<IUnresolvedAttribute>();
				}
				return this.returnTypeAttributes;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00011ECB File Offset: 0x00010ECB
		public IList<IUnresolvedTypeParameter> TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new List<IUnresolvedTypeParameter>();
				}
				return this.typeParameters;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00011EE6 File Offset: 0x00010EE6
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00011EF8 File Offset: 0x00010EF8
		public bool IsExtensionMethod
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

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00011F11 File Offset: 0x00010F11
		public bool IsConstructor
		{
			get
			{
				return base.SymbolKind == SymbolKind.Constructor;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00011F1C File Offset: 0x00010F1C
		public bool IsDestructor
		{
			get
			{
				return base.SymbolKind == SymbolKind.Destructor;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00011F28 File Offset: 0x00010F28
		public bool IsOperator
		{
			get
			{
				return base.SymbolKind == SymbolKind.Operator;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00011F33 File Offset: 0x00010F33
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x00011F45 File Offset: 0x00010F45
		public bool IsPartial
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x00011F5E File Offset: 0x00010F5E
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x00011F70 File Offset: 0x00010F70
		public bool IsAsync
		{
			get
			{
				return this.flags[32768];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[32768] = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00011F89 File Offset: 0x00010F89
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x00011F9B File Offset: 0x00010F9B
		public bool HasBody
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00011FB4 File Offset: 0x00010FB4
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x00011FC9 File Offset: 0x00010FC9
		[Obsolete]
		public bool IsPartialMethodDeclaration
		{
			get
			{
				return this.IsPartial && !this.HasBody;
			}
			set
			{
				if (value)
				{
					this.IsPartial = true;
					this.HasBody = false;
					return;
				}
				if (!value && this.IsPartial && !this.HasBody)
				{
					this.IsPartial = false;
				}
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00011FF7 File Offset: 0x00010FF7
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00012009 File Offset: 0x00011009
		[Obsolete]
		public bool IsPartialMethodImplementation
		{
			get
			{
				return this.IsPartial && this.HasBody;
			}
			set
			{
				if (value)
				{
					this.IsPartial = true;
					this.HasBody = true;
					return;
				}
				if (!value && this.IsPartial && this.HasBody)
				{
					this.IsPartial = false;
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00012037 File Offset: 0x00011037
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

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00012052 File Offset: 0x00011052
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001205A File Offset: 0x0001105A
		public IUnresolvedMember AccessorOwner
		{
			get
			{
				return this.accessorOwner;
			}
			set
			{
				base.ThrowIfFrozen();
				this.accessorOwner = value;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001206C File Offset: 0x0001106C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.SymbolKind.ToString());
			stringBuilder.Append(' ');
			if (base.DeclaringTypeDefinition != null)
			{
				stringBuilder.Append(base.DeclaringTypeDefinition.Name);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(base.Name);
			stringBuilder.Append('(');
			stringBuilder.Append(string.Join<IUnresolvedParameter>(", ", this.Parameters));
			stringBuilder.Append("):");
			stringBuilder.Append(base.ReturnType.ToString());
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00012121 File Offset: 0x00011121
		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedMethod(this, context);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001213C File Offset: 0x0001113C
		public override IMember Resolve(ITypeResolveContext context)
		{
			if (this.accessorOwner != null)
			{
				IMember member = this.accessorOwner.Resolve(context);
				if (member != null)
				{
					IProperty property = member as IProperty;
					if (property != null)
					{
						if (property.CanGet && property.Getter.Name == base.Name)
						{
							return property.Getter;
						}
						if (property.CanSet && property.Setter.Name == base.Name)
						{
							return property.Setter;
						}
					}
					IEvent @event = member as IEvent;
					if (@event != null)
					{
						if (@event.CanAdd && @event.AddAccessor.Name == base.Name)
						{
							return @event.AddAccessor;
						}
						if (@event.CanRemove && @event.RemoveAccessor.Name == base.Name)
						{
							return @event.RemoveAccessor;
						}
						if (@event.CanInvoke && @event.InvokeAccessor.Name == base.Name)
						{
							return @event.InvokeAccessor;
						}
					}
				}
				return null;
			}
			ITypeReference explicitInterfaceTypeReference = null;
			if (base.IsExplicitInterfaceImplementation && base.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = base.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, (from tp in this.TypeParameters
			select tp.Name).ToList<string>(), (from p in this.Parameters
			select p.Type).ToList<ITypeReference>());
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000122E2 File Offset: 0x000112E2
		IMethod IUnresolvedMethod.Resolve(ITypeResolveContext context)
		{
			return (IMethod)this.Resolve(context);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000122F0 File Offset: 0x000112F0
		public static DefaultUnresolvedMethod CreateDefaultConstructor(IUnresolvedTypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException("typeDefinition");
			}
			DomRegion region = typeDefinition.Region;
			region = new DomRegion(region.FileName, region.BeginLine, region.BeginColumn);
			return new DefaultUnresolvedMethod(typeDefinition, ".ctor")
			{
				SymbolKind = SymbolKind.Constructor,
				Accessibility = (typeDefinition.IsAbstract ? Accessibility.Protected : Accessibility.Public),
				IsSynthetic = true,
				HasBody = true,
				Region = region,
				BodyRegion = region,
				ReturnType = KnownTypeReference.Void
			};
		}

		/// <summary>
		/// Returns a dummy constructor instance:
		/// </summary>
		/// <returns>
		/// A public instance constructor with IsSynthetic=true and no declaring type.
		/// </returns>
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001237B File Offset: 0x0001137B
		public static IUnresolvedMethod DummyConstructor
		{
			get
			{
				return DefaultUnresolvedMethod.dummyConstructor;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00012384 File Offset: 0x00011384
		private static IUnresolvedMethod CreateDummyConstructor()
		{
			DefaultUnresolvedMethod defaultUnresolvedMethod = new DefaultUnresolvedMethod
			{
				SymbolKind = SymbolKind.Constructor,
				Name = ".ctor",
				Accessibility = Accessibility.Public,
				IsSynthetic = true,
				ReturnType = KnownTypeReference.Void
			};
			defaultUnresolvedMethod.Freeze();
			return defaultUnresolvedMethod;
		}

		// Token: 0x04000202 RID: 514
		private IList<IUnresolvedAttribute> returnTypeAttributes;

		// Token: 0x04000203 RID: 515
		private IList<IUnresolvedTypeParameter> typeParameters;

		// Token: 0x04000204 RID: 516
		private IList<IUnresolvedParameter> parameters;

		// Token: 0x04000205 RID: 517
		private IUnresolvedMember accessorOwner;

		// Token: 0x04000206 RID: 518
		private static readonly IUnresolvedMethod dummyConstructor = DefaultUnresolvedMethod.CreateDummyConstructor();
	}
}
