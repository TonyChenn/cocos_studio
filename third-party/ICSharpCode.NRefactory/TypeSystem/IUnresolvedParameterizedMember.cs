using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a method or property.
	/// </summary>
	// Token: 0x0200009C RID: 156
	public interface IUnresolvedParameterizedMember : IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060004E1 RID: 1249
		IList<IUnresolvedParameter> Parameters { get; }
	}
}
