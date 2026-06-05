using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Base class for <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMember" /> implementations.
	/// </summary>
	// Token: 0x020000A5 RID: 165
	[Serializable]
	public abstract class AbstractUnresolvedMember : AbstractUnresolvedEntity, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x0000CC51 File Offset: 0x0000BC51
		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			this.interfaceImplementations = provider.InternList<IMemberReference>(this.interfaceImplementations);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0000CC6C File Offset: 0x0000BC6C
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.interfaceImplementations = FreezableHelper.FreezeList<IMemberReference>(this.interfaceImplementations);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0000CC88 File Offset: 0x0000BC88
		public override object Clone()
		{
			AbstractUnresolvedMember abstractUnresolvedMember = (AbstractUnresolvedMember)base.Clone();
			if (this.interfaceImplementations != null)
			{
				abstractUnresolvedMember.interfaceImplementations = new List<IMemberReference>(this.interfaceImplementations);
			}
			return abstractUnresolvedMember;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000CCBB File Offset: 0x0000BCBB
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0000CCC3 File Offset: 0x0000BCC3
		public ITypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.ThrowIfFrozen();
				this.returnType = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000CCE0 File Offset: 0x0000BCE0
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0000CCEF File Offset: 0x0000BCEF
		public bool IsExplicitInterfaceImplementation
		{
			get
			{
				return this.flags[64];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[64] = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0000CD05 File Offset: 0x0000BD05
		public IList<IMemberReference> ExplicitInterfaceImplementations
		{
			get
			{
				if (this.interfaceImplementations == null)
				{
					this.interfaceImplementations = new List<IMemberReference>();
				}
				return this.interfaceImplementations;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0000CD20 File Offset: 0x0000BD20
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0000CD32 File Offset: 0x0000BD32
		public bool IsVirtual
		{
			get
			{
				return this.flags[128];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[128] = value;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0000CD4B File Offset: 0x0000BD4B
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0000CD5D File Offset: 0x0000BD5D
		public bool IsOverride
		{
			get
			{
				return this.flags[256];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[256] = value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0000CD76 File Offset: 0x0000BD76
		public bool IsOverridable
		{
			get
			{
				return (this.flags.Data & 388) != 0 && !base.IsSealed;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0000CD96 File Offset: 0x0000BD96
		ITypeReference IMemberReference.DeclaringTypeReference
		{
			get
			{
				return base.DeclaringTypeDefinition;
			}
		}

		// Token: 0x06000578 RID: 1400
		public abstract IMember CreateResolved(ITypeResolveContext context);

		// Token: 0x06000579 RID: 1401 RVA: 0x0000CDA0 File Offset: 0x0000BDA0
		public virtual IMember Resolve(ITypeResolveContext context)
		{
			ITypeReference explicitInterfaceTypeReference = null;
			if (this.IsExplicitInterfaceImplementation && this.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = this.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, null, null);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000CDF7 File Offset: 0x0000BDF7
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return ((IUnresolvedMember)this).Resolve(context);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000CE00 File Offset: 0x0000BE00
		protected static ITypeResolveContext ExtendContextForType(ITypeResolveContext assemblyContext, IUnresolvedTypeDefinition typeDef)
		{
			if (typeDef == null)
			{
				return assemblyContext;
			}
			ITypeResolveContext parentContext;
			if (typeDef.DeclaringTypeDefinition != null)
			{
				parentContext = AbstractUnresolvedMember.ExtendContextForType(assemblyContext, typeDef.DeclaringTypeDefinition);
			}
			else
			{
				parentContext = assemblyContext;
			}
			ITypeDefinition definition = typeDef.Resolve(assemblyContext).GetDefinition();
			return typeDef.CreateResolveContext(parentContext).WithCurrentTypeDefinition(definition);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000CE50 File Offset: 0x0000BE50
		public static IMember Resolve(ITypeResolveContext context, SymbolKind symbolKind, string name, ITypeReference explicitInterfaceTypeReference = null, IList<string> typeParameterNames = null, IList<ITypeReference> parameterTypeReferences = null)
		{
			if (context.CurrentTypeDefinition == null)
			{
				return null;
			}
			if (parameterTypeReferences == null)
			{
				parameterTypeReferences = EmptyList<ITypeReference>.Instance;
			}
			if (typeParameterNames == null || typeParameterNames.Count == 0)
			{
				IList<IType> parameterTypes = parameterTypeReferences.Resolve(context);
				if (explicitInterfaceTypeReference == null)
				{
					using (IEnumerator<IMember> enumerator = context.CurrentTypeDefinition.Members.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IMember member = enumerator.Current;
							if (!member.IsExplicitInterfaceImplementation && AbstractUnresolvedMember.IsNonGenericMatch(member, symbolKind, name, parameterTypes))
							{
								return member;
							}
						}
						goto IL_21A;
					}
				}
				IType type = explicitInterfaceTypeReference.Resolve(context);
				using (IEnumerator<IMember> enumerator2 = context.CurrentTypeDefinition.Members.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IMember member2 = enumerator2.Current;
						if (member2.IsExplicitInterfaceImplementation && member2.ImplementedInterfaceMembers.Count == 1 && AbstractUnresolvedMember.IsNonGenericMatch(member2, symbolKind, name, parameterTypes) && type.Equals(member2.ImplementedInterfaceMembers[0].DeclaringType))
						{
							return member2;
						}
					}
					goto IL_21A;
				}
			}
			foreach (IMethod method in context.CurrentTypeDefinition.Methods)
			{
				if (method.SymbolKind == symbolKind && !(method.Name != name) && method.Parameters.Count == parameterTypeReferences.Count)
				{
					if (typeParameterNames.SequenceEqual(from tp in method.TypeParameters
					select tp.Name))
					{
						ITypeResolveContext context2 = context.WithCurrentMember(method);
						IList<IType> parameterTypes2 = parameterTypeReferences.Resolve(context2);
						if (AbstractUnresolvedMember.IsParameterTypeMatch(method, parameterTypes2))
						{
							if (explicitInterfaceTypeReference == null)
							{
								if (!method.IsExplicitInterfaceImplementation)
								{
									return method;
								}
							}
							else if (method.IsExplicitInterfaceImplementation && method.ImplementedInterfaceMembers.Count == 1)
							{
								IType type2 = explicitInterfaceTypeReference.Resolve(context2);
								if (type2.Equals(method.ImplementedInterfaceMembers[0].DeclaringType))
								{
									return method;
								}
							}
						}
					}
				}
			}
			IL_21A:
			return null;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000D0A4 File Offset: 0x0000C0A4
		private static bool IsNonGenericMatch(IMember member, SymbolKind symbolKind, string name, IList<IType> parameterTypes)
		{
			if (member.SymbolKind != symbolKind)
			{
				return false;
			}
			if (member.Name != name)
			{
				return false;
			}
			IMethod method = member as IMethod;
			return (method == null || method.TypeParameters.Count <= 0) && AbstractUnresolvedMember.IsParameterTypeMatch(member, parameterTypes);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000D0F0 File Offset: 0x0000C0F0
		private static bool IsParameterTypeMatch(IMember member, IList<IType> parameterTypes)
		{
			IParameterizedMember parameterizedMember = member as IParameterizedMember;
			if (parameterizedMember == null)
			{
				return parameterTypes.Count == 0;
			}
			if (parameterTypes.Count == parameterizedMember.Parameters.Count)
			{
				for (int i = 0; i < parameterTypes.Count; i++)
				{
					IType type = parameterTypes[i];
					IType type2 = parameterizedMember.Parameters[i].Type;
					if (!type.Equals(type2))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400017C RID: 380
		private ITypeReference returnType = SpecialType.UnknownType;

		// Token: 0x0400017D RID: 381
		private IList<IMemberReference> interfaceImplementations;
	}
}
