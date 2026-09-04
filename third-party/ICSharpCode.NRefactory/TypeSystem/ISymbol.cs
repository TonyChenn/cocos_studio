using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Interface for type system symbols.
	/// </summary>
	// Token: 0x0200005A RID: 90
	public interface ISymbol
	{
		/// <summary>
		/// This property returns an enum specifying which kind of symbol this is
		/// (which derived interfaces of ISymbol are implemented)
		/// </summary>
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002B5 RID: 693
		SymbolKind SymbolKind { get; }

		/// <summary>
		/// Gets the short name of the symbol.
		/// </summary>
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002B6 RID: 694
		string Name { get; }

		/// <summary>
		/// Creates a symbol reference that can be used to rediscover this symbol in another compilation.
		/// </summary>
		// Token: 0x060002B7 RID: 695
		ISymbolReference ToReference();
	}
}
