using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public interface ISymbolReference
	{
		ISymbol Resolve(ITypeResolveContext context);
	}
}
