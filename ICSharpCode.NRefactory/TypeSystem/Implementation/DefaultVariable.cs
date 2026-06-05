using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IVariable" />.
	/// </summary>
	// Token: 0x02000079 RID: 121
	public sealed class DefaultVariable : IVariable, ISymbol
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x00009A15 File Offset: 0x00008A15
		public DefaultVariable(IType type, string name)
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

		// Token: 0x060003E6 RID: 998 RVA: 0x00009A47 File Offset: 0x00008A47
		public DefaultVariable(IType type, string name, DomRegion region = default(DomRegion), bool isConst = false, object constantValue = null) : this(type, name)
		{
			this.region = region;
			this.isConst = isConst;
			this.constantValue = constantValue;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00009A68 File Offset: 0x00008A68
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00009A70 File Offset: 0x00008A70
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00009A78 File Offset: 0x00008A78
		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00009A80 File Offset: 0x00008A80
		public bool IsConst
		{
			get
			{
				return this.isConst;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00009A88 File Offset: 0x00008A88
		public object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00009A90 File Offset: 0x00008A90
		public SymbolKind SymbolKind
		{
			get
			{
				return SymbolKind.Variable;
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00009A94 File Offset: 0x00008A94
		public ISymbolReference ToReference()
		{
			return new VariableReference(this.type.ToTypeReference(), this.name, this.region, this.isConst, this.constantValue);
		}

		// Token: 0x040000F9 RID: 249
		private readonly string name;

		// Token: 0x040000FA RID: 250
		private readonly DomRegion region;

		// Token: 0x040000FB RID: 251
		private readonly IType type;

		// Token: 0x040000FC RID: 252
		private readonly object constantValue;

		// Token: 0x040000FD RID: 253
		private readonly bool isConst;
	}
}
