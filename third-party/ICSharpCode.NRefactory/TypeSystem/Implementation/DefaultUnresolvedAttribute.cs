using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedAttribute" />.
	/// </summary>
	// Token: 0x020000BF RID: 191
	[Serializable]
	public sealed class DefaultUnresolvedAttribute : AbstractFreezable, IUnresolvedAttribute, IFreezable, ISupportsInterning
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x00011500 File Offset: 0x00010500
		public DefaultUnresolvedAttribute(ITypeReference attributeType)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.attributeType = attributeType;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001151D File Offset: 0x0001051D
		public DefaultUnresolvedAttribute(ITypeReference attributeType, IEnumerable<ITypeReference> constructorParameterTypes)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.attributeType = attributeType;
			this.ConstructorParameterTypes.AddRange(constructorParameterTypes);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00011548 File Offset: 0x00010548
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.constructorParameterTypes = FreezableHelper.FreezeList<ITypeReference>(this.constructorParameterTypes);
			this.positionalArguments = FreezableHelper.FreezeListAndElements<IConstantValue>(this.positionalArguments);
			this.namedArguments = FreezableHelper.FreezeList<KeyValuePair<IMemberReference, IConstantValue>>(this.namedArguments);
			foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.namedArguments)
			{
				FreezableHelper.Freeze(keyValuePair.Key);
				FreezableHelper.Freeze(keyValuePair.Value);
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x000115E0 File Offset: 0x000105E0
		public ITypeReference AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000115E8 File Offset: 0x000105E8
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x000115F0 File Offset: 0x000105F0
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

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000115FF File Offset: 0x000105FF
		public IList<ITypeReference> ConstructorParameterTypes
		{
			get
			{
				if (this.constructorParameterTypes == null)
				{
					this.constructorParameterTypes = new List<ITypeReference>();
				}
				return this.constructorParameterTypes;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001161A File Offset: 0x0001061A
		public IList<IConstantValue> PositionalArguments
		{
			get
			{
				if (this.positionalArguments == null)
				{
					this.positionalArguments = new List<IConstantValue>();
				}
				return this.positionalArguments;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00011635 File Offset: 0x00010635
		public IList<KeyValuePair<IMemberReference, IConstantValue>> NamedArguments
		{
			get
			{
				if (this.namedArguments == null)
				{
					this.namedArguments = new List<KeyValuePair<IMemberReference, IConstantValue>>();
				}
				return this.namedArguments;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00011650 File Offset: 0x00010650
		public void AddNamedFieldArgument(string fieldName, IConstantValue value)
		{
			this.NamedArguments.Add(new KeyValuePair<IMemberReference, IConstantValue>(new DefaultMemberReference(SymbolKind.Field, this.attributeType, fieldName, 0, null), value));
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00011672 File Offset: 0x00010672
		public void AddNamedPropertyArgument(string propertyName, IConstantValue value)
		{
			this.NamedArguments.Add(new KeyValuePair<IMemberReference, IConstantValue>(new DefaultMemberReference(SymbolKind.Property, this.attributeType, propertyName, 0, null), value));
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00011694 File Offset: 0x00010694
		public IAttribute CreateResolvedAttribute(ITypeResolveContext context)
		{
			return new DefaultUnresolvedAttribute.DefaultResolvedAttribute(this, context);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000116A0 File Offset: 0x000106A0
		int ISupportsInterning.GetHashCodeForInterning()
		{
			int num = this.attributeType.GetHashCode() ^ this.constructorParameterTypes.GetHashCode();
			if (this.constructorParameterTypes != null)
			{
				foreach (ITypeReference typeReference in this.constructorParameterTypes)
				{
					num *= 27;
					num += typeReference.GetHashCode();
				}
			}
			if (this.positionalArguments != null)
			{
				foreach (IConstantValue constantValue in this.positionalArguments)
				{
					num *= 31;
					num += constantValue.GetHashCode();
				}
			}
			if (this.namedArguments != null)
			{
				foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.namedArguments)
				{
					num *= 71;
					num += keyValuePair.Key.GetHashCode() + keyValuePair.Value.GetHashCode() * 73;
				}
			}
			return num;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000117D0 File Offset: 0x000107D0
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultUnresolvedAttribute defaultUnresolvedAttribute = other as DefaultUnresolvedAttribute;
			return defaultUnresolvedAttribute != null && this.attributeType == defaultUnresolvedAttribute.attributeType && DefaultUnresolvedAttribute.ListEquals<ITypeReference>(this.constructorParameterTypes, defaultUnresolvedAttribute.constructorParameterTypes) && DefaultUnresolvedAttribute.ListEquals<IConstantValue>(this.positionalArguments, defaultUnresolvedAttribute.positionalArguments) && DefaultUnresolvedAttribute.ListEquals(this.namedArguments ?? EmptyList<KeyValuePair<IMemberReference, IConstantValue>>.Instance, defaultUnresolvedAttribute.namedArguments ?? EmptyList<KeyValuePair<IMemberReference, IConstantValue>>.Instance);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00011840 File Offset: 0x00010840
		private static bool ListEquals<T>(IList<T> list1, IList<T> list2) where T : class
		{
			if (list1 == null)
			{
				list1 = EmptyList<T>.Instance;
			}
			if (list2 == null)
			{
				list2 = EmptyList<T>.Instance;
			}
			if (list1 == list2)
			{
				return true;
			}
			if (list1.Count != list2.Count)
			{
				return false;
			}
			for (int i = 0; i < list1.Count; i++)
			{
				if (list1[i] != list2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000118A8 File Offset: 0x000108A8
		private static bool ListEquals(IList<KeyValuePair<IMemberReference, IConstantValue>> list1, IList<KeyValuePair<IMemberReference, IConstantValue>> list2)
		{
			if (list1 == list2)
			{
				return true;
			}
			if (list1.Count != list2.Count)
			{
				return false;
			}
			for (int i = 0; i < list1.Count; i++)
			{
				KeyValuePair<IMemberReference, IConstantValue> keyValuePair = list1[i];
				KeyValuePair<IMemberReference, IConstantValue> keyValuePair2 = list2[i];
				if (keyValuePair.Key != keyValuePair2.Key || keyValuePair.Value != keyValuePair2.Value)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040001F2 RID: 498
		private ITypeReference attributeType;

		// Token: 0x040001F3 RID: 499
		private DomRegion region;

		// Token: 0x040001F4 RID: 500
		private IList<ITypeReference> constructorParameterTypes;

		// Token: 0x040001F5 RID: 501
		private IList<IConstantValue> positionalArguments;

		// Token: 0x040001F6 RID: 502
		private IList<KeyValuePair<IMemberReference, IConstantValue>> namedArguments;

		// Token: 0x020000C0 RID: 192
		private sealed class DefaultResolvedAttribute : IAttribute, ICompilationProvider
		{
			// Token: 0x060006A9 RID: 1705 RVA: 0x0001190F File Offset: 0x0001090F
			public DefaultResolvedAttribute(DefaultUnresolvedAttribute unresolved, ITypeResolveContext context)
			{
				this.unresolved = unresolved;
				this.context = context;
				this.attributeType = unresolved.AttributeType.Resolve(context);
				this.positionalArguments = unresolved.PositionalArguments.Resolve(context);
			}

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x060006AA RID: 1706 RVA: 0x00011949 File Offset: 0x00010949
			public IType AttributeType
			{
				get
				{
					return this.attributeType;
				}
			}

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x060006AB RID: 1707 RVA: 0x00011951 File Offset: 0x00010951
			public DomRegion Region
			{
				get
				{
					return this.unresolved.Region;
				}
			}

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001195E File Offset: 0x0001095E
			public IMethod Constructor
			{
				get
				{
					if (!this.constructorResolved)
					{
						this.constructor = this.ResolveConstructor();
						this.constructorResolved = true;
					}
					return this.constructor;
				}
			}

			// Token: 0x060006AD RID: 1709 RVA: 0x000119A8 File Offset: 0x000109A8
			private IMethod ResolveConstructor()
			{
				IList<IType> parameterTypes = this.unresolved.ConstructorParameterTypes.Resolve(this.context);
				foreach (IMethod method in this.attributeType.GetConstructors((IUnresolvedMethod m) => m.Parameters.Count == parameterTypes.Count, GetMemberOptions.IgnoreInheritedMembers))
				{
					bool flag = true;
					for (int i = 0; i < parameterTypes.Count; i++)
					{
						if (!method.Parameters[i].Type.Equals(parameterTypes[i]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return method;
					}
				}
				return null;
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x060006AE RID: 1710 RVA: 0x00011A74 File Offset: 0x00010A74
			public IList<ResolveResult> PositionalArguments
			{
				get
				{
					return this.positionalArguments;
				}
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x060006AF RID: 1711 RVA: 0x00011A7C File Offset: 0x00010A7C
			public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
			{
				get
				{
					IList<KeyValuePair<IMember, ResolveResult>> list = LazyInit.VolatileRead<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments);
					if (list != null)
					{
						return list;
					}
					list = new List<KeyValuePair<IMember, ResolveResult>>();
					foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.unresolved.NamedArguments)
					{
						IMember member = keyValuePair.Key.Resolve(this.context);
						if (member != null)
						{
							ResolveResult value = keyValuePair.Value.Resolve(this.context);
							list.Add(new KeyValuePair<IMember, ResolveResult>(member, value));
						}
					}
					return LazyInit.GetOrSet<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments, list);
				}
			}

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00011B28 File Offset: 0x00010B28
			public ICompilation Compilation
			{
				get
				{
					return this.context.Compilation;
				}
			}

			// Token: 0x060006B1 RID: 1713 RVA: 0x00011B38 File Offset: 0x00010B38
			public override string ToString()
			{
				if (this.positionalArguments.Count == 0)
				{
					return "[" + this.attributeType.ToString() + "]";
				}
				return "[" + this.attributeType.ToString() + "(...)]";
			}

			// Token: 0x040001F7 RID: 503
			private readonly DefaultUnresolvedAttribute unresolved;

			// Token: 0x040001F8 RID: 504
			private readonly ITypeResolveContext context;

			// Token: 0x040001F9 RID: 505
			private readonly IType attributeType;

			// Token: 0x040001FA RID: 506
			private readonly IList<ResolveResult> positionalArguments;

			// Token: 0x040001FB RID: 507
			private IList<KeyValuePair<IMember, ResolveResult>> namedArguments;

			// Token: 0x040001FC RID: 508
			private IMethod constructor;

			// Token: 0x040001FD RID: 509
			private volatile bool constructorResolved;
		}
	}
}
