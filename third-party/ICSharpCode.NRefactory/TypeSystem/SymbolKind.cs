using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000082 RID: 130
	public enum SymbolKind : byte
	{
		// Token: 0x04000116 RID: 278
		None,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeDefinition" />
		// Token: 0x04000117 RID: 279
		TypeDefinition,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IField" />
		// Token: 0x04000118 RID: 280
		Field,
		/// <summary>
		/// The symbol is a property, but not an indexer.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		// Token: 0x04000119 RID: 281
		Property,
		/// <summary>
		/// The symbol is an indexer, not a regular property.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IProperty" />
		// Token: 0x0400011A RID: 282
		Indexer,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IEvent" />
		// Token: 0x0400011B RID: 283
		Event,
		/// <summary>
		/// The symbol is a method which is not an operator/constructor/destructor or accessor.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x0400011C RID: 284
		Method,
		/// <summary>
		/// The symbol is a user-defined operator.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x0400011D RID: 285
		Operator,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x0400011E RID: 286
		Constructor,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x0400011F RID: 287
		Destructor,
		/// <summary>
		/// The accessor method for a property getter/setter or event add/remove.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" />
		// Token: 0x04000120 RID: 288
		Accessor,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.INamespace" />
		// Token: 0x04000121 RID: 289
		Namespace,
		/// <summary>
		/// The symbol is a variable, but not a parameter.
		/// </summary>
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IVariable" />
		// Token: 0x04000122 RID: 290
		Variable,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.IParameter" />
		// Token: 0x04000123 RID: 291
		Parameter,
		/// <seealso cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeParameter" />
		// Token: 0x04000124 RID: 292
		TypeParameter
	}
}
