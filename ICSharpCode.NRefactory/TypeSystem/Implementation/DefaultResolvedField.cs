using System;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000B1 RID: 177
	public class DefaultResolvedField : AbstractResolvedMember, IField, IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x0000DCF1 File Offset: 0x0000CCF1
		public DefaultResolvedField(IUnresolvedField unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0000DCFB File Offset: 0x0000CCFB
		public bool IsReadOnly
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsReadOnly;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0000DD0D File Offset: 0x0000CD0D
		public bool IsVolatile
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsVolatile;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000DD1F File Offset: 0x0000CD1F
		IType IVariable.Type
		{
			get
			{
				return base.ReturnType;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0000DD27 File Offset: 0x0000CD27
		public bool IsConst
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsConst;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0000DD39 File Offset: 0x0000CD39
		public bool IsFixed
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsFixed;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0000DD4C File Offset: 0x0000CD4C
		public object ConstantValue
		{
			get
			{
				ResolveResult resolveResult = this.constantValue;
				if (resolveResult == null)
				{
					using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
					{
						if (!busyLock.Success)
						{
							return null;
						}
						IConstantValue constantValue = ((IUnresolvedField)this.unresolved).ConstantValue;
						if (constantValue != null)
						{
							resolveResult = constantValue.Resolve(this.context);
						}
						else
						{
							resolveResult = ErrorResolveResult.UnknownError;
						}
						this.constantValue = resolveResult;
					}
				}
				return resolveResult.ConstantValue;
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000DDD4 File Offset: 0x0000CDD4
		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedField(this, substitution);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000DDEC File Offset: 0x0000CDEC
		IMemberReference IField.ToReference()
		{
			return (IMemberReference)this.ToReference();
		}

		// Token: 0x040001A9 RID: 425
		private volatile ResolveResult constantValue;
	}
}
