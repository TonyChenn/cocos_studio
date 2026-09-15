using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a method or property.
	/// </summary>
	public interface IParameterizedMember : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		IList<IParameter> Parameters { get; }
	}
}
