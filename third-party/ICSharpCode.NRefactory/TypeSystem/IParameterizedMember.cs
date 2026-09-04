using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a method or property.
	/// </summary>
	// Token: 0x02000060 RID: 96
	public interface IParameterizedMember : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000305 RID: 773
		IList<IParameter> Parameters { get; }
	}
}
