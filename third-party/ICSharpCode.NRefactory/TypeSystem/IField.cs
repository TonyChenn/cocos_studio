using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a field or constant.
	/// </summary>
	public interface IField : IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the region where the field is declared.
		/// </summary>
		DomRegion Region { get; }

		/// <summary>
		/// Gets whether this field is readonly.
		/// </summary>
		bool IsReadOnly { get; }

		/// <summary>
		/// Gets whether this field is volatile.
		/// </summary>
		bool IsVolatile { get; }

		/// <summary>
		/// Gets whether this field is a fixed size buffer (C#-like fixed).
		/// If this is true, then ConstantValue contains the size of the buffer.
		/// </summary>
		bool IsFixed { get; }

		IMemberReference ToReference();
	}
}
