using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200008B RID: 139
	[Flags]
	public enum ConversionFlags
	{
		/// <summary>
		/// Convert only the name.
		/// </summary>
		// Token: 0x04000141 RID: 321
		None = 0,
		/// <summary>
		/// Show the parameter list
		/// </summary>
		// Token: 0x04000142 RID: 322
		ShowParameterList = 1,
		/// <summary>
		/// Show names for parameters
		/// </summary>
		// Token: 0x04000143 RID: 323
		ShowParameterNames = 2,
		/// <summary>
		/// Show the accessibility (private, public, etc.)
		/// </summary>
		// Token: 0x04000144 RID: 324
		ShowAccessibility = 4,
		/// <summary>
		/// Show the definition key word (class, struct, Sub, Function, etc.)
		/// </summary>
		// Token: 0x04000145 RID: 325
		ShowDefinitionKeyword = 8,
		/// <summary>
		/// Show the declaring type for the member
		/// </summary>
		// Token: 0x04000146 RID: 326
		ShowDeclaringType = 16,
		/// <summary>
		/// Show modifiers (virtual, override, etc.)
		/// </summary>
		// Token: 0x04000147 RID: 327
		ShowModifiers = 32,
		/// <summary>
		/// Show the return type
		/// </summary>
		// Token: 0x04000148 RID: 328
		ShowReturnType = 64,
		/// <summary>
		/// Use fully qualified names for types.
		/// </summary>
		// Token: 0x04000149 RID: 329
		UseFullyQualifiedTypeNames = 128,
		/// <summary>
		/// Show the list of type parameters on method and class declarations.
		/// Type arguments for parameter/return types are always shown.
		/// </summary>
		// Token: 0x0400014A RID: 330
		ShowTypeParameterList = 256,
		/// <summary>
		/// For fields, events and methods: adds a semicolon at the end.
		/// For properties: shows "{ get; }" or similar.
		/// </summary>
		// Token: 0x0400014B RID: 331
		ShowBody = 512,
		/// <summary>
		/// Use fully qualified names for members.
		/// </summary>
		// Token: 0x0400014C RID: 332
		UseFullyQualifiedEntityNames = 1024,
		// Token: 0x0400014D RID: 333
		StandardConversionFlags = 879,
		// Token: 0x0400014E RID: 334
		All = 2047
	}
}
