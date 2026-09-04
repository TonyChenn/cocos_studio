using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x0200007B RID: 123
	public sealed class VariableReference : ISymbolReference
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x00009AC0 File Offset: 0x00008AC0
		public VariableReference(ITypeReference variableTypeReference, string name, DomRegion region, bool isConst, object constantValue)
		{
			if (variableTypeReference == null)
			{
				throw new ArgumentNullException("variableTypeReference");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.variableTypeReference = variableTypeReference;
			this.name = name;
			this.region = region;
			this.isConst = isConst;
			this.constantValue = constantValue;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00009B14 File Offset: 0x00008B14
		public ISymbol Resolve(ITypeResolveContext context)
		{
			return new DefaultVariable(this.variableTypeReference.Resolve(context), this.name, this.region, this.isConst, this.constantValue);
		}

		// Token: 0x040000FE RID: 254
		private ITypeReference variableTypeReference;

		// Token: 0x040000FF RID: 255
		private string name;

		// Token: 0x04000100 RID: 256
		private DomRegion region;

		// Token: 0x04000101 RID: 257
		private bool isConst;

		// Token: 0x04000102 RID: 258
		private object constantValue;
	}
}
