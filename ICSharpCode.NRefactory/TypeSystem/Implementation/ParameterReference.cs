using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000AF RID: 175
	public sealed class ParameterReference : ISymbolReference
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x0000DBA0 File Offset: 0x0000CBA0
		public ParameterReference(ITypeReference type, string name, DomRegion region, bool isRef, bool isOut, bool isParams, bool isOptional, object defaultValue)
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
			this.region = region;
			this.isRef = isRef;
			this.isOut = isOut;
			this.isParams = isParams;
			this.isOptional = isOptional;
			this.defaultValue = defaultValue;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0000DC0C File Offset: 0x0000CC0C
		public ISymbol Resolve(ITypeResolveContext context)
		{
			return new DefaultParameter(this.type.Resolve(context), this.name, null, this.region, null, this.isRef, this.isOut, this.isParams, this.isOptional, this.defaultValue);
		}

		// Token: 0x0400019D RID: 413
		private readonly ITypeReference type;

		// Token: 0x0400019E RID: 414
		private readonly string name;

		// Token: 0x0400019F RID: 415
		private readonly DomRegion region;

		// Token: 0x040001A0 RID: 416
		private readonly bool isRef;

		// Token: 0x040001A1 RID: 417
		private readonly bool isOut;

		// Token: 0x040001A2 RID: 418
		private readonly bool isParams;

		// Token: 0x040001A3 RID: 419
		private readonly bool isOptional;

		// Token: 0x040001A4 RID: 420
		private readonly object defaultValue;
	}
}
