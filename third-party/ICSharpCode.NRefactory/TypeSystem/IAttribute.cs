using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an attribute.
	/// </summary>
	public interface IAttribute
	{
		/// <summary>
		/// Gets the code region of this attribute.
		/// </summary>
		DomRegion Region { get; }

		/// <summary>
		/// Gets the type of the attribute.
		/// </summary>
		IType AttributeType { get; }

		/// <summary>
		/// Gets the constructor being used.
		/// This property may return null if no matching constructor was found.
		/// </summary>
		IMethod Constructor { get; }

		/// <summary>
		/// Gets the positional arguments.
		/// </summary>
		IList<ResolveResult> PositionalArguments { get; }

		/// <summary>
		/// Gets the named arguments passed to the attribute.
		/// </summary>
		IList<KeyValuePair<IMember, ResolveResult>> NamedArguments { get; }
	}
}
