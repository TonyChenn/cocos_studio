using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Obsolete("Use SymbolKind instead")]
	public enum EntityType : byte
	{
		None,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" />
		TypeDefinition,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IField" />
		Field,
		/// <summary>
		/// The symbol is a property, but not an indexer.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		Property,
		/// <summary>
		/// The symbol is an indexer, not a regular property.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		Indexer,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IEvent" />
		Event,
		/// <summary>
		/// The symbol is a method which is not an operator/constructor/destructor or accessor.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		Method,
		/// <summary>
		/// The symbol is a user-defined operator.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		Operator,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		Constructor,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		Destructor,
		/// <summary>
		/// The accessor method for a property getter/setter or event add/remove.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		Accessor
	}
}
