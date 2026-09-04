using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200007A RID: 122
	public interface ISymbolReference
	{
		// Token: 0x060003EE RID: 1006
		ISymbol Resolve(ITypeResolveContext context);
	}
}
