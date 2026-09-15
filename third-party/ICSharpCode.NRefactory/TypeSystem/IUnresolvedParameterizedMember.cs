using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a method or property.
	/// </summary>
	public interface IUnresolvedParameterizedMember : IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		IList<IUnresolvedParameter> Parameters { get; }
	}
}
