using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000074 RID: 116
	[Obsolete("Use SymbolKind instead")]
	public enum EntityType : byte
	{
		// Token: 0x040000EB RID: 235
		None,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" />
		// Token: 0x040000EC RID: 236
		TypeDefinition,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IField" />
		// Token: 0x040000ED RID: 237
		Field,
		/// <summary>
		/// The symbol is a property, but not an indexer.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		// Token: 0x040000EE RID: 238
		Property,
		/// <summary>
		/// The symbol is an indexer, not a regular property.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		// Token: 0x040000EF RID: 239
		Indexer,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IEvent" />
		// Token: 0x040000F0 RID: 240
		Event,
		/// <summary>
		/// The symbol is a method which is not an operator/constructor/destructor or accessor.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x040000F1 RID: 241
		Method,
		/// <summary>
		/// The symbol is a user-defined operator.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x040000F2 RID: 242
		Operator,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x040000F3 RID: 243
		Constructor,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x040000F4 RID: 244
		Destructor,
		/// <summary>
		/// The accessor method for a property getter/setter or event add/remove.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x040000F5 RID: 245
		Accessor
	}
}
