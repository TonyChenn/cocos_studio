using System;
using System.Collections.Generic;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IParameter" />.
	/// </summary>
	// Token: 0x020000AD RID: 173
	public sealed class DefaultParameter : IParameter, IVariable, ISymbol
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0000D8EA File Offset: 0x0000C8EA
		public DefaultParameter(IType type, string name)
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

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000D91C File Offset: 0x0000C91C
		public DefaultParameter(IType type, string name, IParameterizedMember owner = null, DomRegion region = default(DomRegion), IList<IAttribute> attributes = null, bool isRef = false, bool isOut = false, bool isParams = false, bool isOptional = false, object defaultValue = null)
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
			this.owner = owner;
			this.region = region;
			this.attributes = attributes;
			this.isRef = isRef;
			this.isOut = isOut;
			this.isParams = isParams;
			this.isOptional = isOptional;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0000D998 File Offset: 0x0000C998
		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.Parameter;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0000D99C File Offset: 0x0000C99C
		public IParameterizedMember Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000D9A4 File Offset: 0x0000C9A4
		public IList<IAttribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0000D9AC File Offset: 0x0000C9AC
		public bool IsRef
		{
			get
			{
				return this.isRef;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000D9B4 File Offset: 0x0000C9B4
		public bool IsOut
		{
			get
			{
				return this.isOut;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0000D9BC File Offset: 0x0000C9BC
		public bool IsParams
		{
			get
			{
				return this.isParams;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000D9C4 File Offset: 0x0000C9C4
		public bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0000D9CC File Offset: 0x0000C9CC
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000D9D4 File Offset: 0x0000C9D4
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0000D9DC File Offset: 0x0000C9DC
		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0000D9E4 File Offset: 0x0000C9E4
		bool IVariable.IsConst
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0000D9E7 File Offset: 0x0000C9E7
		public object ConstantValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000D9EF File Offset: 0x0000C9EF
		public override string ToString()
		{
			return DefaultParameter.ToString(this);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000D9F8 File Offset: 0x0000C9F8
		public static string ToString(IParameter parameter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (parameter.IsRef)
			{
				stringBuilder.Append("ref ");
			}
			if (parameter.IsOut)
			{
				stringBuilder.Append("out ");
			}
			if (parameter.IsParams)
			{
				stringBuilder.Append("params ");
			}
			stringBuilder.Append(parameter.Name);
			stringBuilder.Append(':');
			stringBuilder.Append(parameter.Type.ReflectionName);
			if (parameter.IsOptional)
			{
				stringBuilder.Append(" = ");
				if (parameter.ConstantValue != null)
				{
					stringBuilder.Append(parameter.ConstantValue.ToString());
				}
				else
				{
					stringBuilder.Append("null");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000DAB4 File Offset: 0x0000CAB4
		public ISymbolReference ToReference()
		{
			if (this.owner == null)
			{
				return new ParameterReference(this.type.ToTypeReference(), this.name, this.region, this.isRef, this.isOut, this.isParams, this.isOptional, this.defaultValue);
			}
			return new OwnedParameterReference(this.owner.ToReference(), this.owner.Parameters.IndexOf(this));
		}

		// Token: 0x04000191 RID: 401
		private readonly IType type;

		// Token: 0x04000192 RID: 402
		private readonly string name;

		// Token: 0x04000193 RID: 403
		private readonly DomRegion region;

		// Token: 0x04000194 RID: 404
		private readonly IList<IAttribute> attributes;

		// Token: 0x04000195 RID: 405
		private readonly bool isRef;

		// Token: 0x04000196 RID: 406
		private readonly bool isOut;

		// Token: 0x04000197 RID: 407
		private readonly bool isParams;

		// Token: 0x04000198 RID: 408
		private readonly bool isOptional;

		// Token: 0x04000199 RID: 409
		private readonly object defaultValue;

		// Token: 0x0400019A RID: 410
		private readonly IParameterizedMember owner;
	}
}
